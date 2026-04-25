# MCP 获取 tools 的配置与实现说明

## 1. 先说结论

当前系统里，MCP 工具列表有两条不同路径：

- `查询工具`
  从控制面数据库读取已经落库的工具快照。
- `获取工具`
  实时连接真正的 MCP Server，调用 `tools/list`，然后把结果回写数据库。

通用工具执行器也不是直接打数据库，它会：

1. 先根据 `toolId` 读数据库，拿到工具所属连接和工具名。
2. 再实时启动该连接对应的 MCP Server。
3. 调用 `tools/call` 执行工具。
4. 把执行记录写回数据库。

## 2. 配置存在哪里

### 2.1 MCP 连接配置

MCP 连接配置持久化在控制面数据库的 `McpConnections` 表中。

核心字段：

- `Name`
  连接内部名。
- `Engine`
  目标数据库类型，目前是 PostgreSQL / MySQL。
- `DisplayName`
  前端展示名。
- `ServerCommand`
  启动 MCP Server 的真实命令。
- `ServerArgumentsJson`
  启动参数 JSON。
- `EnvironmentJson`
  启动 MCP Server 时要注入的环境变量 JSON。
- `DatabaseConnectionString`
  目标数据库连接串。
- `DatabaseName`
  目标数据库名。
- `Status`
  当前连接状态。
- `LastDiscoveredAt`
  最近一次成功发现工具的时间。

对应源码：

- `src/AIDbOptimize.Infrastructure/Persistence/Entities/McpConnectionEntity.cs`
- `src/AIDbOptimize.Application/Abstractions/Persistence/IMcpConnectionRepository.cs`
- `src/AIDbOptimize.Infrastructure/Persistence/Repositories/McpConnectionRepository.cs`

### 2.2 MCP 工具配置

工具快照持久化在 `McpTools` 表中。

核心字段：

- `ToolName`
  MCP 返回的原始工具名。
- `DisplayName`
  前端展示名。
- `Description`
  工具描述。
- `InputSchemaJson`
  `tools/list` 返回的输入参数 schema 快照。
- `ApprovalMode`
  审批模式。
- `IsEnabled`
  是否启用。
- `IsWriteTool`
  是否推断为写工具。

对应源码：

- `src/AIDbOptimize.Infrastructure/Persistence/Entities/McpToolEntity.cs`
- `src/AIDbOptimize.Application/Abstractions/Persistence/IMcpToolRepository.cs`
- `src/AIDbOptimize.Infrastructure/Persistence/Repositories/McpToolRepository.cs`

## 3. 默认连接现在如何初始化

系统启动时，`ControlPlaneDefaultSeedHostedService` 会检查并补齐默认连接。

### PostgreSQL 默认连接

默认 PostgreSQL MCP 已回退到 npm 无状态实现：

- `ServerCommand`
  `npx`
- `ServerArgumentsJson`
  `["-y","@modelcontextprotocol/server-postgres","postgresql://..."]`
- `EnvironmentJson`
  `{}`，不依赖额外环境变量

这样做的原因：

- 与当前“短生命周期 MCP 会话”架构兼容
- 不依赖会话内先执行 `connect_database`
- 对 Windows、macOS、Linux、Docker 的心智更统一
- 不需要维护单独的 launcher 脚本

### MySQL 默认连接

默认 MySQL MCP 仍然使用：

- `ServerCommand`
  `npx`
- `ServerArgumentsJson`
  `["-y","mysql-mcp-server"]`
- `EnvironmentJson`
  注入 `MYSQL_HOST`、`MYSQL_PORT`、`MYSQL_USER`、`MYSQL_PASSWORD`、`MYSQL_DATABASE`

对应源码：

- `src/AIDbOptimize.ApiService/DatabaseMigrations/ControlPlaneDefaultSeedHostedService.cs`

## 4. 获取 tools 的后端实现

### 4.1 API 路由

MCP 相关路由统一定义在：

- `src/AIDbOptimize.ApiService/Api/McpApi.cs`

当前关键接口：

- `GET /api/mcp/connections`
  查询连接列表。
- `GET /api/mcp/connections/{connectionId}/tools`
  读取数据库中的工具快照。
- `POST /api/mcp/connections/{connectionId}/discover-tools`
  实时连接 MCP Server 发现工具，并回写数据库。
- `PUT /api/mcp/tools/{toolId}/approval-mode`
  保存审批模式。
- `POST /api/mcp/tools/{toolId}/execute`
  执行指定工具。

### 4.2 查询工具：读数据库

调用链：

```text
Web -> /api/mcp/connections/{id}/tools
    -> McpApi.HandleGetToolsAsync
    -> McpDiscoveryAppService.GetToolsAsync
    -> IMcpToolRepository.ListByConnectionAsync
    -> ControlPlane Db / McpTools
```

关键结论：

- 不连 MCP Server
- 不走 `tools/list`
- 只是读最近一次发现后保存在数据库里的快照

### 4.3 获取工具：实时调用 MCP

调用链：

```text
Web -> /api/mcp/connections/{id}/discover-tools
    -> McpApi.HandleDiscoverToolsAsync
    -> McpDiscoveryAppService.DiscoverToolsAsync
    -> IMcpConnectionRepository.GetByIdAsync
    -> IMcpDiscoveryService.DiscoverToolsAsync
    -> McpClientFactory.CreateAsync
    -> 启动真实 MCP Server
    -> session.Client.ListToolsAsync()
    -> IMcpToolRepository.UpsertManyAsync
    -> IMcpConnectionRepository.UpdateAsync(LastDiscoveredAt / Status)
```

关键实现点：

- `McpClientFactory` 负责根据 `ServerCommand`、`ServerArgumentsJson`、`EnvironmentJson` 启动 stdio MCP Server。
- `McpDiscoveryService` 调用 `ListToolsAsync()`，把协议层返回值转换成系统内部的 `McpToolDefinition`。
- `McpDiscoveryAppService` 把结果 upsert 到数据库。

## 5. 为什么当前默认 PostgreSQL MCP 选择 npm 无状态实现

之前切到原生二进制型 `sgaunet/postgresql-mcp` 后，暴露出两个结构性问题：

1. 它依赖先执行 `connect_database` 一类状态化步骤。
2. 当前系统的工具执行链路是“每次调用新建一个短生命周期 MCP 会话”。

这两者天然不匹配。

而 `@modelcontextprotocol/server-postgres` 的优点是：

- 启动简单
- npm 生态天然跨平台
- 与当前无状态、短生命周期会话模型兼容

因此当前默认方案回退为 npm 无状态实现。

## 6. 前端现在如何理解“查询工具”和“获取工具”

当前前端约定如下：

- `查询工具`
  读取数据库中的工具快照，用于稳定查看和管理。
- `获取工具`
  实时访问 MCP Server，用于发现最新工具并刷新数据库。
- 审批模式修改
  改下拉框只是修改当前草稿，点击“保存”才会真正调用后端落库。
- 真实连接命令
  连接列表展示“命令行 + 环境变量折叠”，而不是把全部内容压成一条细列。

对应前端文件：

- `src/AIDbOptimize.Web/src/App.vue`
- `src/AIDbOptimize.Web/src/components/mcp/McpConnectionTable.vue`
- `src/AIDbOptimize.Web/src/components/mcp/McpToolTable.vue`

## 7. 真实连接命令现在怎么展示

后端会把连接配置拆成三部分返回前端：

- `CommandPreview`
  供摘要或日志使用的完整预览
- `CommandLine`
  只包含真正的启动命令和参数
- `EnvironmentEntries`
  单独列出的环境变量项

这样前端可以把“真实命令”显示得更可读：

- 主体显示 `CommandLine`
- 环境变量收纳到折叠区域

生成位置：

- `src/AIDbOptimize.Application/Mcp/Dtos/McpConnectionDto.cs`

## 8. 为什么“刚获取后保存审批”以前会报错

之前的根因是：

- discover 时前端拿到的是新生成的临时 `Id`
- 数据库 upsert 同名工具时复用了旧记录，不会换主键
- 前端立刻保存时，带着临时 `Id` 去更新，自然会报“未找到 MCP 工具”

现在修正后：

- `UpsertManyAsync` 返回真实落库后的记录
- `discover-tools` 接口返回的也是落库后的真实 `Id`
- 重新发现工具时，不会把已配置的审批模式覆盖回默认值

对应源码：

- `src/AIDbOptimize.Application/Abstractions/Persistence/IMcpToolRepository.cs`
- `src/AIDbOptimize.Infrastructure/Persistence/Repositories/McpToolRepository.cs`
- `src/AIDbOptimize.Application/Mcp/Services/McpDiscoveryAppService.cs`

## 9. 通用工具执行器默认参数如何工作

执行器会读取数据库里保存的 `InputSchemaJson`，根据 schema 自动生成默认 JSON。

生成规则：

- 优先使用 schema 自带的 `const`、`default`、`example`、`examples`、`enum`
- 常见字段自动填充
  - `sql` / `query` / `statement` -> `SELECT 1 AS value;`
  - `schema` -> `public`
  - `database` -> 当前连接的数据库名
  - `table` / `objectName` / `name` -> `orders`
  - `limit` / `size` / `count` / `top` -> `20`
- 推断不出的字符串保留为空字符串

对应文件：

- `src/AIDbOptimize.Web/src/components/mcp/McpToolExecutor.vue`

## 10. 工具执行的真实调用链

```text
Web -> /api/mcp/tools/{toolId}/execute
    -> McpApi.HandleExecuteToolAsync
    -> McpToolExecutionService.ExecuteAsync
    -> 先查 McpTools / McpConnections
    -> McpClientFactory.CreateAsync
    -> 启动真实 MCP Server
    -> session.Client.CallToolAsync(tool.ToolName, payload)
    -> 把执行结果写入 McpToolExecutions
```

对应文件：

- `src/AIDbOptimize.Infrastructure/Mcp/McpToolExecutionService.cs`

## 11. 关键源码地图

```text
src/
├── AIDbOptimize.ApiService/
│   ├── Api/McpApi.cs
│   └── DatabaseMigrations/ControlPlaneDefaultSeedHostedService.cs
├── AIDbOptimize.Application/
│   └── Mcp/
│       ├── Dtos/
│       │   ├── McpConnectionDto.cs
│       │   └── McpToolDto.cs
│       └── Services/McpDiscoveryAppService.cs
├── AIDbOptimize.Infrastructure/
│   ├── Mcp/
│   │   ├── McpClientFactory.cs
│   │   ├── McpDiscoveryService.cs
│   │   └── McpToolExecutionService.cs
│   └── Persistence/
│       ├── Entities/
│       │   ├── McpConnectionEntity.cs
│       │   └── McpToolEntity.cs
│       └── Repositories/
│           ├── McpConnectionRepository.cs
│           └── McpToolRepository.cs
└── AIDbOptimize.Web/
    └── src/
        ├── App.vue
        ├── api/mcp.ts
        ├── models/mcp.ts
        └── components/mcp/
            ├── McpConnectionTable.vue
            ├── McpToolExecutor.vue
            └── McpToolTable.vue
```

## 12. 当前最重要的事实

- 查询工具：读数据库
- 获取工具：实时连 MCP，再回写数据库
- 默认 PostgreSQL MCP：`@modelcontextprotocol/server-postgres`
- 默认 PostgreSQL 启动方式：`npx -y @modelcontextprotocol/server-postgres <postgres-url>`
- 默认 MySQL MCP：`mysql-mcp-server`

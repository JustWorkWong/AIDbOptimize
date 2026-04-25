# AIDbOptimize DDD + MCP + 测试数据初始化详细设计方案

## 1. 当前实现状态

### 1.1 已完成

- 解决方案已拆分为 `Domain / Application / Infrastructure / DataInit`
- 控制面数据库 `DbContext`、实体、仓储已存在
- API 已暴露 MCP 连接、工具、审批模式、执行记录、初始化状态等接口
- 前端已有最小 MCP 管理视图
- 真实 MCP 发现与执行链路已接入
- Agent 工具装配已接入真实 `AIFunction`

### 1.2 当前默认 MCP 连接

- PostgreSQL 默认连接使用 `@modelcontextprotocol/server-postgres`
- MySQL 默认连接使用 `mysql-mcp-server`

## 2. 项目职责

### `AIDbOptimize.Domain`

只放纯领域对象，不依赖 EF Core、MCP、Web。

### `AIDbOptimize.Application`

负责用例编排，不直接关心数据库驱动或 MCP 具体实现。

### `AIDbOptimize.Infrastructure`

负责：

- EF Core `DbContext`
- 实体映射
- 迁移
- 仓储实现
- 真实 MCP client / session / tool 调用适配

### `AIDbOptimize.ApiService`

负责：

- HTTP API 暴露
- 控制面数据库自动迁移
- 默认 MCP 连接种子

### `AIDbOptimize.DataInit`

负责：

- 业务测试库 `MigrateAsync`
- 初始化状态可视化
- 触发业务测试库迁移中的数据种子执行

### `AIDbOptimize.Web`

负责：

- 资源概览
- MCP 管理视图

## 3. MCP 详细设计

### 3.1 当前真实实现

当前已接入：

- PostgreSQL 默认连接：`@modelcontextprotocol/server-postgres`
- MySQL 默认连接：`mysql-mcp-server`
- stdio MCP 会话工厂：`McpClientFactory`
- 短生命周期会话包装：`McpProcessSession`
- 工具发现：真实 `tools/list`
- 工具执行：真实 `tools/call`

### 3.2 为什么 PostgreSQL 当前选择 npm 无状态实现

当前系统的 MCP 调用模型是“短生命周期会话”：

- 获取工具时创建一次会话
- 执行工具时再次创建一次会话

因此默认 PostgreSQL MCP server 必须尽量满足：

- 无状态
- 单次调用即可完成

`@modelcontextprotocol/server-postgres` 更符合这个模型。

### 3.3 当前实现边界

- PostgreSQL 当前默认工具主要是只读 `query`
- MySQL 默认 server 仍然是社区 `mysql-mcp-server`
- 前端已支持真实调用，但还不是多连接、多策略、多租户级别的管理台

## 4. 关键文件

```text
src/
├── AIDbOptimize.ApiService/
│   ├── Program.cs
│   ├── Api/McpApi.cs
│   └── DatabaseMigrations/ControlPlaneDefaultSeedHostedService.cs
├── AIDbOptimize.Application/
│   └── Mcp/
│       ├── Dtos/
│       └── Services/
├── AIDbOptimize.Infrastructure/
│   ├── Mcp/
│   └── Persistence/
└── AIDbOptimize.Web/
    └── src/
        └── components/mcp/
```

## 5. 文档关系

实现前后请按顺序阅读：

1. [总体设计](./design.md)
2. [本详细设计](./detailed-design.md)
3. [任务清单](./tasks.md)

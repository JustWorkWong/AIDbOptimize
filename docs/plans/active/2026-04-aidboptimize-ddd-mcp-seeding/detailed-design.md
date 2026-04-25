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

- PostgreSQL 默认连接使用 `sgaunet/postgresql-mcp`
  通过 `postgresql-mcp-launcher.ps1` 自动下载并缓存 Windows 二进制
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
- PostgreSQL MCP launcher 脚本随输出目录复制

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

- PostgreSQL 默认连接：`sgaunet/postgresql-mcp`
- MySQL 默认连接：`mysql-mcp-server`
- stdio MCP 会话工厂：`McpClientFactory`
- 短生命周期会话包装：`McpProcessSession`
- 工具发现：真实 `tools/list`
- 工具执行：真实 `tools/call`

### 3.2 PostgreSQL 默认连接设计

默认 PostgreSQL 连接不再直接调用 npm 包，而是走 launcher：

```text
powershell
  -NoProfile
  -ExecutionPolicy Bypass
  -File <output>/postgresql-mcp-launcher.ps1
```

环境变量：

- `POSTGRES_URL`

launcher 负责：

1. 检查 `%LOCALAPPDATA%\AIDbOptimize\tools\postgresql-mcp`
2. 若不存在目标版本二进制，则从 GitHub Releases 下载
3. 启动 `postgresql-mcp_0.3.0_windows_amd64.exe`

这样做的目的：

- 避免要求开发机预装 Go
- 避免要求手工下载 exe
- 保持默认连接可直接工作

### 3.3 当前实现边界

- PostgreSQL 当前默认 server 仍然是只读导向
- MySQL 默认 server 仍然是社区 `mysql-mcp-server`
- 前端已支持真实调用，但还不是多连接、多策略、多租户级别的管理台

## 4. 关键文件

```text
src/
├── AIDbOptimize.ApiService/
│   ├── Program.cs
│   ├── postgresql-mcp-launcher.ps1
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

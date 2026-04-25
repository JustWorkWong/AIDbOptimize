# AIDbOptimize.ApiService 目录说明

## 目录职责

`AIDbOptimize.ApiService/` 负责对外暴露 HTTP API，并在启动阶段完成控制面数据库迁移与默认连接种子。

## 当前结构

```text
AIDbOptimize.ApiService/
├── CLAUDE.md
├── AIDbOptimize.ApiService.csproj
├── Program.cs
├── postgresql-mcp-launcher.ps1
├── Api/
│   └── McpApi.cs
└── DatabaseMigrations/
    └── ControlPlaneDefaultSeedHostedService.cs
```

## 文件用途

- `Program.cs`
  API 启动入口，负责依赖注入、Swagger、CORS、基础设施总览接口和 MCP API 注册。
- `Api/McpApi.cs`
  MCP 管理相关 HTTP 接口。
- `DatabaseMigrations/ControlPlaneDefaultSeedHostedService.cs`
  控制面数据库默认 MCP 连接种子，当前在这里定义 PostgreSQL / MySQL 默认连接。
- `postgresql-mcp-launcher.ps1`
  PostgreSQL 默认 MCP launcher。首次运行时自动下载并缓存 `sgaunet/postgresql-mcp` Windows 二进制，再用 `POSTGRES_URL` 启动。
- `AIDbOptimize.ApiService.csproj`
  项目依赖和内容文件复制规则，当前负责把 `postgresql-mcp-launcher.ps1` 复制到输出目录。

## 依赖关系

- 依赖 `AIDbOptimize.Application/`
  提供应用服务与抽象。
- 依赖 `AIDbOptimize.Infrastructure/`
  提供仓储、数据库、MCP client 与执行实现。
- 对 `AIDbOptimize.Web/` 暴露 HTTP API。

## 架构约束

- 默认 PostgreSQL MCP 连接不依赖开发机预装 Go。
- 如需切换默认 PostgreSQL MCP server，必须同时更新：
  - `ControlPlaneDefaultSeedHostedService.cs`
  - `postgresql-mcp-launcher.ps1`
  - `docs/mcp/README.md`
  - 本文件

# AIDbOptimize

一个面向数据库配置优化 workflow 的本地开发项目，当前技术栈为 `Aspire + ASP.NET Core + Vue 3 + Vite`。

当前版本已经具备这些核心能力：

- 使用 Aspire 统一编排后端 API、前端 Vite 应用和基础设施容器
- 提供 `PostgreSQL`、`MySQL`、`Redis`、`RabbitMQ`
- 数据库配置优化 workflow 的发起、review、resume、recovery、history 和 SSE 事件流
- MCP 连接管理、工具发现、工具执行和 workflow 只读采集链路
- 控制面持久化、agent session 审计和前端工作台
- 所有资源使用固定端口，并保留本地管理入口

## 设计与计划文档

长期设计方案、执行计划和任务清单统一维护在：

- [docs/README.md](./docs/README.md)
- [Workflow 后端代码流程说明](./docs/workflow/README.md)

当前唯一执行计划：

- [数据库配置优化 workflow 总体方案](./docs/plans/active/2026-04-aidboptimize-db-config-workflow/design.md)
- [数据库配置优化 workflow 详细设计](./docs/plans/active/2026-04-aidboptimize-db-config-workflow/detailed-design.md)
- [数据库配置优化 workflow 任务清单](./docs/plans/active/2026-04-aidboptimize-db-config-workflow/tasks.md)

## 启动方式

在仓库根目录执行：

```powershell
dotnet run --project .\src\AIDbOptimize.AppHost\AIDbOptimize.AppHost.csproj
```

启动后可通过 Aspire Dashboard 查看整体运行状态。

## 固定端口清单

| 资源 | 端口 | 说明 |
| --- | ---: | --- |
| Aspire AppHost / Dashboard | `17200` | Aspire 本地入口 |
| API | `17100` | ASP.NET Core API |
| Web | `17101` | Vue 3 + Vite 前端 |
| PostgreSQL | `15432` | PostgreSQL 数据库 |
| pgAdmin | `15050` | PostgreSQL 管理面板 |
| MySQL | `13306` | MySQL 数据库 |
| phpMyAdmin | `15051` | MySQL 管理面板 |
| Redis | `16379` | Redis 缓存 |
| Redis Insight | `15540` | Redis 管理面板 |
| RabbitMQ | `15672` | RabbitMQ AMQP 端口 |
| RabbitMQ Management | `15673` | RabbitMQ 管理后台 |

## 当前状态

当前仓库已完成：

- 基础设施编排与固定端口本地开发环境
- db-config workflow 后端 runtime、review / resume、history / replay、SSE 事件流
- MCP 控制面、工具执行审计与 agent 持久化
- 对应测试工程与前端工作台

后续执行顺序仍以仓库根 [CLAUDE.md](./CLAUDE.md) 和当前唯一任务清单为准。

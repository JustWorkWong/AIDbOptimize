# AIDbOptimize

一个最小可运行的 `Aspire + ASP.NET Core + Vue 3 + Vite` 示例项目。

当前版本聚焦本地开发骨架：

- 使用 Aspire 统一编排后端 API、前端 Vite 应用和基础设施容器
- 提供 `PostgreSQL`、`MySQL`、`Redis`、`RabbitMQ`
- 所有资源使用固定端口
- 每个资源都带管理页面
- 基础设施容器启用持久化卷，重启后数据仍然保留

## 设计与计划文档

长期设计方案、执行计划和任务清单统一维护在：

- [docs/README.md](./docs/README.md)

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

- Aspire 基础编排骨架
- API / Web 最小可运行项目
- 文档目录与长期计划目录

后续执行顺序以仓库根 [CLAUDE.md](./CLAUDE.md) 和当前唯一任务清单为准。

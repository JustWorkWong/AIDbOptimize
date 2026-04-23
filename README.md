# AIDbOptimize

一个最小可运行的 `Aspire + ASP.NET Core + Vue 3 + Vite` 示例项目。

当前版本聚焦本地开发骨架：

- 使用 Aspire 统一编排后端 API、前端 Vite 应用和基础设施容器
- 提供 `PostgreSQL`、`MySQL`、`Redis`、`RabbitMQ`
- 所有资源都使用固定端口
- 每个资源都带管理页面
- 基础设施容器启用持久化卷，重启应用后数据仍然保留

## 设计与计划文档

长期设计方案、执行计划和任务清单统一维护在：

- [docs/README.md](./docs/README.md)

当前活跃计划：

- [DDD + MCP + 测试数据初始化设计方案](./docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/design.md)
- [DDD + MCP + 测试数据初始化详细设计方案](./docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/detailed-design.md)
- [DDD + MCP + 测试数据初始化任务清单](./docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/tasks.md)

## 启动方式

在仓库根目录执行：

```powershell
dotnet run --project .\src\AIDbOptimize.AppHost\AIDbOptimize.AppHost.csproj
```

启动后可以通过 Aspire Dashboard 查看整体运行状态。

## 固定端口清单

| 资源 | 端口 | 说明 |
| --- | ---: | --- |
| Aspire AppHost / Dashboard | `17200` | Aspire 本地入口 |
| API | `17100` | ASP.NET Core 最小 API |
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

下一阶段的详细设计和执行顺序，请直接参考 `docs` 目录中的计划文档。

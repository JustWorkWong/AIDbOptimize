# AIDbOptimize

一个面向数据库配置优化 workflow 的本地开发项目，当前技术栈为 `Aspire + ASP.NET Core + Vue 3 + Vite`。

当前版本已经具备这些核心能力：

- 使用 Aspire 统一编排后端 API、前端 Vite 应用和基础设施容器
- 提供 `PostgreSQL`、`MySQL`、`Redis`、`RabbitMQ`
- 数据库配置优化 workflow 的发起、review、resume、recovery、history 和 SSE 事件流
- `workflow skills v1` 主链：bundle/version 解析、investigation / diagnosis skill、planner、collection subworkflow、policy gate
- MCP 连接管理、工具发现、工具执行和 workflow 只读采集链路
- 控制面持久化、agent session 审计和前端工作台
- 所有资源使用固定端口，并保留本地管理入口

## 设计与计划文档

长期设计方案、实现说明和执行计划统一维护在：

- [docs/README.md](./docs/README.md)
- [Workflow 后端代码流程说明](./docs/workflow/README.md)

最近完成计划（当前默认基线）：

- [workflow skills v1 方案](./docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/design.md)
- [workflow skills v1 详细方案](./docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/detailed-design.md)
- [workflow skills v1 任务清单](./docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/tasks.md)

当前没有新的未完成 active plan；上面这组文档用于描述最近一次已经落地的实现基线。

## 当前验证快照（2026-05-07）

- `workflow skills v1` 已进入真实 workflow 主链，`RAG` 仍然只保留协议与扩展位
- `dotnet test .\AIDbOptimize.slnx --no-restore` 通过，当前共 `84` 个测试
- `npm run build` (`src/AIDbOptimize.Web`) 通过

## 启动方式

在仓库根目录执行：

```powershell
dotnet run --project .\src\AIDbOptimize.AppHost\AIDbOptimize.AppHost.csproj
```

启动后可通过 Aspire Dashboard 查看整体运行状态。

如果要跑完整的 diagnosis / review 主链，还需要先配置 diagnosis agent：

- 共享占位文件：`src/AIDbOptimize.ApiService/appsettings.json`
- 本地覆盖文件：`src/AIDbOptimize.ApiService/appsettings.Local.json`
- 配置键：`AIDbOptimize:Agent:Diagnosis:Endpoint`、`Model`、`ApiKey`
- 约定：`appsettings.json` 中的 `ApiKey` 只保留占位值 `xxx`，真实值优先放在 `appsettings.Local.json`

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
- workflow skills v1：bundle/version 解析、skill 驱动的采集规划、policy gate、`recommendationType`
- MCP 控制面、工具执行审计与 agent 持久化
- 对应测试工程与前端工作台
- `RAG` 仅预留未来插入位，尚未接入真实 retrieval

后续如进入新一轮中大型需求，以仓库根 [CLAUDE.md](./CLAUDE.md) 为准：先建立新 plan，再切换默认执行指针。

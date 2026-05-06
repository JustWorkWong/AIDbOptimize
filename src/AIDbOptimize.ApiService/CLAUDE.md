# AIDbOptimize.ApiService 目录说明

## 目录职责

`AIDbOptimize.ApiService/` 负责暴露 HTTP API、承接最薄的一层请求绑定与结果返回，并在启动阶段完成控制面数据库迁移和默认种子初始化。

## 当前结构

```text
AIDbOptimize.ApiService/
├── CLAUDE.md
├── AIDbOptimize.ApiService.csproj
├── Program.cs
├── appsettings.json
├── appsettings.Local.json (local only, gitignored)
├── Api/
│   ├── DataInitializationApi.cs
│   ├── HistoryApi.cs
│   ├── McpApi.cs
│   ├── ReviewsApi.cs
│   ├── WorkflowEventsApi.cs
│   └── WorkflowsApi.cs
├── DatabaseMigrations/
│   ├── ControlPlaneDefaultSeedHostedService.cs
│   └── ControlPlaneMigrationHostedService.cs
└── Properties/
    └── launchSettings.json
```

## 文件用途

- `Program.cs`: 组合宿主、基础设施注册、中间件与 endpoint 映射入口。
- `appsettings.json`: 共享占位配置，保留 diagnosis agent 的 Endpoint / Model / ApiKey 键路径，默认使用占位值 `xxx`。
- `appsettings.Local.json`: 本地私有配置，当前用于注入 DashScope/Qwen diagnosis agent 配置，不提交远程。
- `Api/DataInitializationApi.cs`: 暴露数据初始化状态查询。
- `Api/McpApi.cs`: 暴露 MCP 连接、工具发现、执行与审批模式更新接口。
- `Api/WorkflowsApi.cs`: 暴露数据库配置优化 workflow 启动、查询、列表与取消接口。
- `Api/WorkflowEventsApi.cs`: 暴露 workflow 基于持久化事件的 SSE replay 接口。
- `Api/ReviewsApi.cs`: 暴露 workflow review 任务查询与提交接口。
- `Api/HistoryApi.cs`: 暴露 workflow 历史列表与详情接口。
- `DatabaseMigrations/`: 启动期控制面数据库迁移与默认数据种子。

## 依赖关系

- `Api/* -> Application/*`: API 层只做参数绑定和结果封装，业务逻辑下沉到应用层接口。
- `Program.cs -> Infrastructure/*`: 运行时注册和启动任务集中在组合根，不扩散到 route builder。
- `appsettings.Local.json -> appsettings.json`: 本地私有配置覆盖共享占位配置；diagnosis agent 的真实 `ApiKey` 应优先放在 `appsettings.Local.json`。

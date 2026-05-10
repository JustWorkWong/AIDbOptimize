# AIDbOptimize.DataInit 目录说明

## 目录职责

`AIDbOptimize.DataInit/` 负责一次性初始化任务的宿主编排，当前覆盖业务测试库迁移/种子，以及显式开关控制下的 RAG seed 语料预下载与入库。

## 当前结构

```text
AIDbOptimize.DataInit/
├── CLAUDE.md
├── AIDbOptimize.DataInit.csproj
├── Program.cs
├── appsettings.json
├── Abstractions/
│   └── IDataInitializer.cs
├── HostedServices/
│   └── DataInitializationHostedService.cs
├── Options/
│   ├── DataInitializationOptions.cs
│   └── RagSeedOptions.cs
└── Services/
    ├── InitializationStateService.cs
    ├── MySqlLabInitializer.cs
    ├── PostgreSqlLabInitializer.cs
    └── RagSeedCorpusInitializer.cs
```

## 文件用途

- `Program.cs`
  一次性作业宿主入口，负责连接串解析、EF 注册、初始化器注册、`appsettings.Local.json` 覆盖加载，以及 `SeedPreloadCommand` 的 HTTP client 接线。
- `appsettings.json`
  共享占位配置，当前用于 `AIDbOptimize:Agent:RagEmbedding` 的 Endpoint / Model / ApiKey 占位值。
- `Abstractions/IDataInitializer.cs`
  约束数据库初始化器的统一接口。
- `HostedServices/DataInitializationHostedService.cs`
  顺序执行初始化器并在作业完成后主动退出宿主。
- `Options/DataInitializationOptions.cs`
  业务测试库初始化选项。
- `Options/RagSeedOptions.cs`
  RAG seed preload 的显式开关与语料根目录配置。
- `Services/RagSeedCorpusInitializer.cs`
  在 `DataInit` 宿主内桥接 `SeedPreloadCommand` 与 `RagKnowledgeIngestionService`，默认关闭，只有显式启用时才下载 seed 文档并写入控制面 document/chunk 骨架。

## 依赖关系

- `DataInit -> Infrastructure.Persistence`
  复用控制面和业务测试库 `DbContext`。
- `DataInit -> Infrastructure.Rag.Corpus`
  复用 `SeedPreloadCommand` 与固定 seed catalog，不在宿主层重写下载逻辑。
- `DataInit -> Infrastructure.Rag.Ingestion`
  复用 `RagKnowledgeIngestionService`，把 `docs/rag` 原文目录写入控制面 PostgreSQL 骨架表。

## 维护原则

1. `DataInit` 只承接一次性离线准备任务，不要把它扩成常驻 API 后台服务。
2. `RagSeedCorpusInitializer` 默认必须保持显式开关关闭，避免每次开发启动都产生外部下载副作用。
3. 真正的 seed 下载逻辑留在 `Infrastructure/Rag/Corpus`，宿主层只做编排与配置注入。
4. 如果要在本地启用真实 embedding key，优先通过 `appsettings.Local.json` 覆盖，不要改共享 `appsettings.json` 的占位值。

# tests 目录说明

## 目录职责

`tests/` 保存独立测试工程，负责验证 API 契约、控制面流转和后续新增功能的回归行为。

## 当前结构

```text
tests/
├── CLAUDE.md
├── AIDbOptimize.ApiService.Tests/
│   ├── AIDbOptimize.ApiService.Tests.csproj
│   ├── DbConfigPipelineTests.cs
│   ├── WorkflowApiTests.cs
│   └── WorkflowTelemetryTests.cs
└── AIDbOptimize.Infrastructure.Tests/
    ├── AIDbOptimize.Infrastructure.Tests.csproj
    ├── AgentPersistenceTests.cs
    ├── CorpusChunkerTests.cs
    ├── CorpusChunkMetadataExtractorTests.cs
    ├── ControlPlaneRagMigrationTests.cs
    ├── CorpusPreprocessorTests.cs
    ├── DbConfigCollectorTests.cs
    ├── DbConfigQualityGateTests.cs
    ├── HistoricalOptimizationCaseProjectorTests.cs
    ├── DiagnosisRuleContractEvaluatorTests.cs
    ├── EvidenceCapabilityCatalogTests.cs
    ├── InvestigationPlannerTests.cs
    ├── MarkdownSkillParserTests.cs
    ├── RagCorpusFileNamerTests.cs
    ├── RagDocumentChunkRepositoryTests.cs
    ├── RagEmbeddingServiceTests.cs
    ├── RagKnowledgeQueryServiceTests.cs
    ├── RagKnowledgeIngestionServiceTests.cs
    ├── SeedPreloadCommandTests.cs
    ├── SeedPreloadDocumentCatalogTests.cs
    └── SecurityPrimitivesTests.cs
```

## 文件用途

- `AIDbOptimize.ApiService.Tests.csproj`: API 集成测试工程，使用 `WebApplicationFactory` 启动测试宿主。
- `DbConfigPipelineTests.cs`: 覆盖 workflow 管道的 API 视角回归，包括 result DTO、history/replay 和前端依赖的结构化字段。
- `WorkflowApiTests.cs`: 当前覆盖 workflow 列表、history、events、checkpoint、no-review、review/adjust 流转，以及 agent session/message/summary 落库。
- `WorkflowTelemetryTests.cs`: 当前覆盖 `start -> review.submit -> resume -> complete` 这条 OTel activity/counter 链路。
- `AIDbOptimize.Infrastructure.Tests.csproj`: Infrastructure 单元测试工程。
- `AgentPersistenceTests.cs`: 当前覆盖 agent session、summary、message 的持久化服务行为。
- `CorpusChunkerTests.cs`: 覆盖章节优先 chunk 切分与稳定 chunk 元数据。
- `CorpusChunkMetadataExtractorTests.cs`: 覆盖 `parameter_names / keywords / product_version / applies_to` 的最小抽取规则。
- `ControlPlaneRagMigrationTests.cs`: 覆盖控制面 migration script 已包含 RAG 表、embedding 列与 `vector` 扩展。
- `CorpusPreprocessorTests.cs`: 覆盖 HTML seed 原文清洗、导航/页脚剔除与正文提取。
- `DbConfigCollectorTests.cs`: 当前覆盖 planner 驱动采集请求、只读工具优先采集、fallback 采集与 capability 结果映射。
- `DbConfigQualityGateTests.cs`: 当前覆盖 recommendation schema、grounding、review adjustment、human review gate 与 `recommendationType` 传输。
- `HistoricalOptimizationCaseProjectorTests.cs`: 覆盖已完成 workflow 向 `RagCaseRecordEntity / RagCaseEvidenceLinkEntity` 的最小 case projection。
- `DiagnosisRuleContractEvaluatorTests.cs`: 当前覆盖 diagnosis skill 输出契约、forbidden pattern 与缺失上下文规则。
- `EvidenceCapabilityCatalogTests.cs`: 当前覆盖 capability 映射、baseline refs 与跨引擎元数据对齐。
- `InvestigationPlannerTests.cs`: 当前覆盖 skill ref 到 capability plan 的稳定映射与 missing-context policy。
- `MarkdownSkillParserTests.cs`: 当前覆盖 front matter、固定章节和 evidence ref 归一化契约。
- `RagCorpusFileNamerTests.cs`: 覆盖 RAG facts / cases 文件命名、路径解析与非法命名拦截。
- `RagDocumentChunkRepositoryTests.cs`: 覆盖 document chunk repository 的 engine/vendor/topic/parameter_names 过滤。
- `RagEmbeddingServiceTests.cs`: 覆盖 embedding 适配层在占位配置下的安全退化行为。
- `RagKnowledgeQueryServiceTests.cs`: 覆盖基于控制面 chunk 骨架的 facts 检索、metadata 状态和 evidence-ref 匹配打分。
- `RagKnowledgeIngestionServiceTests.cs`: 覆盖 `docs/rag` 原文目录到控制面 document/chunk 骨架的最小入库链路。
- `SeedPreloadCommandTests.cs`: 覆盖固定 seed URL 清单的最小下载落盘与 front matter 写入。
- `SeedPreloadDocumentCatalogTests.cs`: 覆盖固定 seed URL 清单、目标相对路径和最少 metadata 完整性。
- `WorkflowRagContextAssemblerTests.cs`: 覆盖 pre-diagnosis RAG 插槽在未配置检索后端时的 no-op 元数据语义。
- `SecurityPrimitivesTests.cs`: 当前覆盖脱敏、工具白名单和 prompt 输入拼装的安全原语行为。

## 依赖关系

- `AIDbOptimize.ApiService.Tests -> ApiService`: 通过 HTTP 层验证最小 API 闭环。
- `AIDbOptimize.ApiService.Tests -> Infrastructure`: 使用测试专用 `IDbContextFactory` 替换真实数据库依赖。
- `AIDbOptimize.Infrastructure.Tests -> Infrastructure`: 直接验证安全原语等无宿主依赖的底层组件。

## 维护原则

1. 新增 workflow skills、planner、policy gate 或 recommendation contract 相关测试时，必须同步更新本文件。
2. 如果后端 graph 节点或 DTO 语义变化，优先检查 `WorkflowApiTests.cs`、`DbConfigPipelineTests.cs` 和对应 Infrastructure 单测是否仍覆盖新链路。
3. 新增 RAG corpus、seed preload 或语料命名契约测试时，优先落到 `AIDbOptimize.Infrastructure.Tests/`，并保持“先写失败测试，再补最小实现”。

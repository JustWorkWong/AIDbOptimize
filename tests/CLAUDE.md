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
    ├── DbConfigCollectorTests.cs
    ├── DbConfigQualityGateTests.cs
    ├── DiagnosisRuleContractEvaluatorTests.cs
    ├── EvidenceCapabilityCatalogTests.cs
    ├── InvestigationPlannerTests.cs
    ├── MarkdownSkillParserTests.cs
    └── SecurityPrimitivesTests.cs
```

## 文件用途

- `AIDbOptimize.ApiService.Tests.csproj`: API 集成测试工程，使用 `WebApplicationFactory` 启动测试宿主。
- `DbConfigPipelineTests.cs`: 覆盖 workflow 管道的 API 视角回归，包括 result DTO、history/replay 和前端依赖的结构化字段。
- `WorkflowApiTests.cs`: 当前覆盖 workflow 列表、history、events、checkpoint、no-review、review/adjust 流转，以及 agent session/message/summary 落库。
- `WorkflowTelemetryTests.cs`: 当前覆盖 `start -> review.submit -> resume -> complete` 这条 OTel activity/counter 链路。
- `AIDbOptimize.Infrastructure.Tests.csproj`: Infrastructure 单元测试工程。
- `AgentPersistenceTests.cs`: 当前覆盖 agent session、summary、message 的持久化服务行为。
- `DbConfigCollectorTests.cs`: 当前覆盖 planner 驱动采集请求、只读工具优先采集、fallback 采集与 capability 结果映射。
- `DbConfigQualityGateTests.cs`: 当前覆盖 recommendation schema、grounding、review adjustment、human review gate 与 `recommendationType` 传输。
- `DiagnosisRuleContractEvaluatorTests.cs`: 当前覆盖 diagnosis skill 输出契约、forbidden pattern 与缺失上下文规则。
- `EvidenceCapabilityCatalogTests.cs`: 当前覆盖 capability 映射、baseline refs 与跨引擎元数据对齐。
- `InvestigationPlannerTests.cs`: 当前覆盖 skill ref 到 capability plan 的稳定映射与 missing-context policy。
- `MarkdownSkillParserTests.cs`: 当前覆盖 front matter、固定章节和 evidence ref 归一化契约。
- `SecurityPrimitivesTests.cs`: 当前覆盖脱敏、工具白名单和 prompt 输入拼装的安全原语行为。

## 依赖关系

- `AIDbOptimize.ApiService.Tests -> ApiService`: 通过 HTTP 层验证最小 API 闭环。
- `AIDbOptimize.ApiService.Tests -> Infrastructure`: 使用测试专用 `IDbContextFactory` 替换真实数据库依赖。
- `AIDbOptimize.Infrastructure.Tests -> Infrastructure`: 直接验证安全原语等无宿主依赖的底层组件。

## 维护原则

1. 新增 workflow skills、planner、policy gate 或 recommendation contract 相关测试时，必须同步更新本文件。
2. 如果后端 graph 节点或 DTO 语义变化，优先检查 `WorkflowApiTests.cs`、`DbConfigPipelineTests.cs` 和对应 Infrastructure 单测是否仍覆盖新链路。

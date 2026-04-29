# tests 目录说明

## 目录职责

`tests/` 保存独立测试工程，负责验证 API 契约、控制面流转和后续新增功能的回归行为。

## 当前结构

```text
tests/
├── CLAUDE.md
├── AIDbOptimize.ApiService.Tests/
│   ├── AIDbOptimize.ApiService.Tests.csproj
│   ├── WorkflowApiTests.cs
│   └── WorkflowTelemetryTests.cs
└── AIDbOptimize.Infrastructure.Tests/
    ├── AIDbOptimize.Infrastructure.Tests.csproj
    ├── AgentPersistenceTests.cs
    ├── DbConfigCollectorTests.cs
    ├── DbConfigQualityGateTests.cs
    └── SecurityPrimitivesTests.cs
```

## 文件用途

- `AIDbOptimize.ApiService.Tests.csproj`: API 集成测试工程，使用 `WebApplicationFactory` 启动测试宿主。
- `WorkflowApiTests.cs`: 当前覆盖 workflow 列表、history、events、checkpoint、no-review、review/adjust 流转，以及 agent session/message/summary 落库。
- `WorkflowTelemetryTests.cs`: 当前覆盖 `start -> review.submit -> resume -> complete` 这条 OTel activity/counter 链路。
- `AIDbOptimize.Infrastructure.Tests.csproj`: Infrastructure 单元测试工程。
- `AgentPersistenceTests.cs`: 当前覆盖 agent session、summary、message 的持久化服务行为。
- `DbConfigCollectorTests.cs`: 当前覆盖只读工具优先采集、fallback 采集和 diagnosis executor 外壳。
- `DbConfigQualityGateTests.cs`: 当前覆盖 recommendation schema、grounding、review adjustment 和 human review gate。
- `SecurityPrimitivesTests.cs`: 当前覆盖脱敏、工具白名单和 prompt 输入拼装的安全原语行为。

## 依赖关系

- `AIDbOptimize.ApiService.Tests -> ApiService`: 通过 HTTP 层验证最小 API 闭环。
- `AIDbOptimize.ApiService.Tests -> Infrastructure`: 使用测试专用 `IDbContextFactory` 替换真实数据库依赖。
- `AIDbOptimize.Infrastructure.Tests -> Infrastructure`: 直接验证安全原语等无宿主依赖的底层组件。

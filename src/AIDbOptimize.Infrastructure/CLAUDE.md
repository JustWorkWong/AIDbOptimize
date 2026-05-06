# AIDbOptimize.Infrastructure 目录说明

## 目录职责

`AIDbOptimize.Infrastructure/` 负责持久化、MCP 调用适配、workflow 控制面实现，以及与外部运行时相关的真实技术实现。

## 当前结构

```text
AIDbOptimize.Infrastructure/
├── CLAUDE.md
├── Agents/
├── Mcp/
├── Observability/
├── Persistence/
│   ├── DesignTime/
│   ├── Entities/
│   ├── Lab/
│   ├── Migrations/
│   └── Repositories/
├── Security/
└── Workflows/
    ├── Pipeline/
    ├── Runtime/
    ├── Services/
    └── Skills/
```

## 文件用途

- `Agents/`
  agent session 持久化与 rolling summary 服务，实现 `agent_sessions / agent_summaries / agent_messages` 的控制面落库。
- `Mcp/`
  MCP client、session、tool execution 与 agent tool assembly 的真实实现。
- `Observability/`
  统一维护 workflow / agent / review / mcp 的 `ActivitySource`、`Meter` 与核心计数器/直方图。
- `Persistence/Entities/`
  控制面数据库实体，包含 workflow session、checkpoint、review、node execution、event 和 MCP 执行记录。
- `Persistence/Repositories/`
  `Mcp*` 与 `Workflow*` 持久化抽象的 EF Core 实现。
- `Persistence/Migrations/ControlPlane/`
  控制面数据库 schema 迁移。
- `Security/`
  prompt 输入拼装、工具输出脱敏、只读工具白名单和通用敏感信息脱敏原语。
- `Workflows/Pipeline/`
  workflow 主链上的业务执行骨架，负责 input validation、planner 驱动的 evidence collection subworkflow、policy gate、diagnosis、grounding 与 review gate。
- `Workflows/Runtime/`
  runtime seam、workflow command、skill selection 持久化、checkpoint/recovery、执行注册表与进度计算。
- `Workflows/Services/`
  workflow、review、history 的控制面服务实现。
- `Workflows/Skills/`
  workflow skills 相关的受控文档协议解析、bundle 解析、capability catalog 与 diagnosis 契约校验实现。

## 依赖关系

- `ApiService -> Infrastructure.Workflows.Services`
  API 层组合 workflow 服务。
- `Infrastructure.Workflows.Services -> Persistence/*`
  workflow 控制面实现直接依赖控制面数据库。
- `Infrastructure.Mcp -> Persistence.Entities.Mcp*`
  MCP 执行记录通过 `WorkflowSessionId` 与 workflow 审计关联。
- `Infrastructure.Workflows.Skills -> docs/workflow/skills/*`
  受控解析器消费长期规则资产文档，但解析规则必须由当前源码定义并验证。

## 维护原则

1. 在 `Workflows/` 下新增子目录时，必须同步更新本文件。
2. `Workflows/Skills/` 中的代码优先实现受控协议解析，不要把它扩展成通用 Markdown 引擎。
3. 如果某次修改同时影响 `docs/workflow/skills/` 的目录边界与 `Workflows/Skills/` 的解析行为，必须同时更新两侧文档。
4. `Workflows/Pipeline/` 与 `Workflows/Runtime/` 的文档描述必须与当前 graph 一致；当前真实主链是 `InputValidation -> InvestigationPlanner -> EvidenceCollectionSubworkflow -> SkillPolicyGate -> Diagnosis -> Grounding -> Review/Complete`。

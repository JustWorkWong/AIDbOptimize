# AIDbOptimize.Infrastructure 目录说明

## 目录职责

`AIDbOptimize.Infrastructure/` 负责持久化、MCP 调用适配、workflow 控制面实现以及与外部运行时相关的真实技术实现。

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
    └── Services/
```

## 文件用途

- `Agents/`: agent session 持久化与 rolling summary 服务，实现 `agent_sessions / agent_summaries / agent_messages` 的控制面落库。
- `Mcp/`: MCP client、session、tool execution 与 agent tool assembly 的真实实现。
- `Observability/`: 统一维护 workflow / agent / review / mcp 的 `ActivitySource`、`Meter` 与核心计数器/直方图。
- `Persistence/Entities/`: 控制面数据库实体，现已包含 workflow session、checkpoint、review、node execution、event 和 MCP 执行记录。
- `Persistence/Repositories/`: `Mcp*` 与 `Workflow*` 持久化抽象的 EF Core 实现。
- `Persistence/Migrations/ControlPlane/`: 控制面数据库 schema 迁移，已新增 workflow 控制面相关表和 `workflow_checkpoints`。
- `Security/`: prompt 输入拼装、工具输出脱敏、只读工具白名单和通用敏感信息脱敏原语。
- `Workflows/Pipeline/`: 阶段 3/4 的执行骨架，负责 input validation、真实只读工具优先/metadata fallback snapshot、rule analysis、diagnosis executor、report builder，以及 grounding / schema / review adjustment / human review gate。
- `Workflows/Runtime/`: runtime seam、workflow command 和恢复 hosted service；当前先收口 control-plane v1，并已接入 persisted checkpoint/resume，后续再切换到真正的 MAF graph runtime。
- `Workflows/Services/`: 当前数据库配置优化 workflow、review、history 的最小控制面实现，已通过 runtime seam 发起/取消 workflow。

## 依赖关系

- `ApiService -> Infrastructure.Workflows.Services`: 组合根在 API 层注册 workflow 服务实现。
- `Infrastructure.Workflows.Services -> Persistence/*`: workflow 控制面实现直接依赖控制面数据库。
- `Infrastructure.Mcp -> Persistence.Entities.Mcp*`: MCP 执行记录与 workflow session 通过 `WorkflowSessionId` 形成审计关联。

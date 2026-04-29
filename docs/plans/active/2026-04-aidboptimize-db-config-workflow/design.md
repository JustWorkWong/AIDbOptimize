# AIDbOptimize 数据库配置优化 workflow 总体方案

## 1. 目标

在当前 `MCP 连接管理 + 工具发现 + 工具执行 + Agent 工具装配 + 前端管理台` 基线之上，新增一条可审计、可恢复、可审核的数据库配置优化 workflow，用于支撑：

1. 基于现有 `mcp_connections` 选择目标数据库实例，而不是新造一套连接模型。
2. 基于 MAF workflow 统一管理 run、checkpoint、resume、review、history、SSE 与观测。
3. 在 workflow 中引入最小必要的 LLM agent，但把数据库采集和高风险动作保持为确定性、可审计路径。
4. 为前端提供“发起优化、查看进度、审核建议、恢复结果、追踪事件”的完整闭环。

## 2. 第一版边界

第一版明确只做“生成配置优化建议”，不做“自动下发配置变更”。

第一版不做：

1. 自动执行 `SET`、`ALTER SYSTEM`、重启实例等写操作。
2. 多租户、复杂 RBAC、外部 SSO。
3. Redis 热缓存、MQ 解耦、离线批处理等二期能力。
4. 通用 agent 对话工作台。
5. 任意 workflow 编排器；本轮只聚焦 `DbConfigOptimization`。

## 3. 总体设计结论

### 3.1 编排内核

统一采用 `Microsoft Agent Framework Workflow` 作为 run / checkpoint / review / resume 的执行内核。

原因：

1. 用户这次关心的不只是“跑起来”，而是“如何保存、如何摘要、如何恢复、如何审核、如何统一 run 和 resume”。
2. 参考仓库 `E:\wfcodes\DbOptimizer` 已经验证了 `workflow_sessions + review_tasks + SSE + projection` 这一套模型可落地。
3. 当前 `E:\Db` 已经有 OpenTelemetry 基线，MAF 更容易把 workflow 执行链路和现有 API / EF Core 链路接起来。

### 3.2 运行模式

采用“确定性采集 + 规则分析 + 单 agent 解释/归纳 + 人工审核”的简化模型。

含义：

1. 数据库配置采集不交给 agent 自由调用工具，而是走后端确定性工具执行。
2. agent 只消费结构化 evidence pack，负责把规则分析结果整理为可读、可审计的优化报告。
3. `run` 和 `resume` 共用同一套 runtime、checkpoint、projection、audit、OTel 管线。

### 3.3 当前仓库的复用点

直接复用：

1. `mcp_connections` 作为目标数据库配置来源。
2. `mcp_tools` 作为配置采集工具目录。
3. `IMcpToolExecutionService` 作为确定性采集执行入口。
4. `IAgentToolAssemblyService` 作为后续扩展到“人工 drill-down / 互动分析”时的现成基线。
5. `ControlPlaneDbContext` 作为 workflow 控制面数据库。
6. 现有 `AIDbOptimize.Web` 的单页工作台模式。

需要新增：

1. workflow session / checkpoint / review / event / agent session 持久化模型。
2. workflow application service 与 runtime。
3. review 提交触发 resume 的后端闭环。
4. SSE 事件流与历史查询。
5. 安全脱敏、prompt injection 防护、grounding 校验。

## 4. 端到端业务流程

```mermaid
flowchart LR
    A["Web: 选择现有 MCP 连接"] --> B["POST /api/workflows/db-config-optimization"]
    B --> C["Workflow Session 初始化"]
    C --> D["确定性配置采集"]
    D --> E["规则分析 + 证据归一化"]
    E --> F["Config Diagnosis Agent"]
    F --> G["Grounding / 结构校验"]
    G --> H{"需要人工审核?"}
    H -- "是" --> I["创建 Review Task + 挂起"]
    I --> J["审核提交"]
    J --> K["同一 run 恢复"]
    H -- "否" --> L["完成"]
    K --> L
    L --> M["状态查询 / 事件流 / 历史回放"]
```

## 5. 状态分层

必须分成 4 层，不能混成一个 JSON：

1. `workflow checkpoint`
   负责恢复 MAF 执行现场。
2. `business state`
   负责前端状态、当前节点、结果快照、审核关联。
3. `agent session`
   负责恢复 agent 运行态与摘要上下文。
4. `review task / pending request`
   负责前后端分离场景下的审核回传和 resume 关联。

## 6. start / resume 复用原则

这次方案的核心原则不是“把 resume 另外补一套逻辑”，而是：

1. `StartAsync` 负责创建 session 和初始 command。
2. `ResumeAsync` 负责加载 checkpoint 和外部 response。
3. 两者都进入同一个 `RunInternalAsync(...)` 主执行管线。
4. checkpoint 保存、agent 持久化、summary 生成、event projection、OTel 记录都复用同一套中间层。

公开接口层面：

1. 保留 `cancel`。
2. 不暴露通用 `/resume` 给前端。
3. `WaitingForReview` 状态下只能通过 review submit 触发恢复。
4. 服务重启后的 `Running / Recovering` 会话由后台恢复服务自动续跑。

## 7. 审核模型

本轮明确区分两类审核：

1. `tool approval`
   当前已有 `ApprovalRequiredAIFunction` 基线，属于工具调用审批。
2. `workflow review`
   本轮新增 `workflow_review_tasks`，属于建议结果审批。

数据库配置优化 workflow 第一版默认只允许只读采集工具进入主链路，因此：

1. workflow 主路径不依赖写工具审批。
2. 高风险写工具继续只保留在人工工具执行台，不进入自动优化流程。
3. workflow 结果必须经过结构校验和人工审核后才能标记为“可执行建议”。

## 8. 安全结论

第一版必须把以下四条写成硬约束：

1. agent 不直接看到原始凭证、连接串、环境变量。
2. tool / MCP 输出一律视为不可信输入，只能进入规范化 evidence pack。
3. workflow 不执行用户生成 SQL，不允许通过 prompt 或 review comment 拼接可执行命令。
4. `mcp_connections.DatabaseConnectionString` 与 `EnvironmentJson` 在进入 workflow 前必须经过脱敏；中长期应落到加密存储。

## 9. OTel 结论

不另造观测系统，直接扩展现有 `Program.cs` OpenTelemetry 配置。

新增统一观测面：

1. `AIDbOptimize.Workflow`
2. `AIDbOptimize.Agent`
3. `AIDbOptimize.Mcp`
4. `AIDbOptimize.Review`

至少覆盖：

1. start / resume / cancel / recovery
2. checkpoint save / load
3. agent run / summary
4. tool execution
5. review pending / submitted / resumed

## 10. 前端结论

前端继续沿用当前单应用工作台模式，但不能再把所有新逻辑堆进现有 `App.vue`。

本轮建议最少新增 4 个工作区：

1. `db-config`
2. `review`
3. `history`
4. `replay`

## 11. 交付物

本轮方案文档拆成三层：

1. `design.md`
   说明方向、边界和核心决策。
2. `detailed-design.md`
   说明模块拆分、数据模型、API、状态机、恢复、OTel 与安全细节。
3. `tasks.md`
   拆成可以直接进入开发排期的任务清单。

# Workflow 后端代码流程说明

本文档描述当前 `DbConfigOptimization` workflow 在后端的真实代码链路，重点说明：

- 请求如何从 API 进入 Application / Runtime
- `skills v1` 主链如何驱动规划、采集、gate、diagnosis 和 completion
- review / resume / recovery / cancel 的真实落点
- 哪些状态落在控制面数据库，哪些状态只是 graph 编排结果
- SSE、history、checkpoint 为什么能稳定回放当前与历史状态

如果要看本轮范围和执行拆解，请回到：

- [workflow skills v1 方案](../plans/active/2026-05-aidboptimize-workflow-skills-v1/design.md)
- [workflow skills v1 详细方案](../plans/active/2026-05-aidboptimize-workflow-skills-v1/detailed-design.md)
- [workflow skills v1 任务清单](../plans/active/2026-05-aidboptimize-workflow-skills-v1/tasks.md)

## 当前验证快照（2026-05-07）

- `workflow skills v1` 已经进入真实 start / resume / recovery 主链，不再只是设计稿
- `RAG` 仍然只保留扩展位，没有接入真实 retrieval、知识注入或外部裁决
- `dotnet test .\AIDbOptimize.slnx --no-restore` 通过，当前共 `84` 个测试
- `npm run build` (`src/AIDbOptimize.Web`) 通过

## 1. skills 与 RAG 边界

`docs/workflow/skills/` 已经是 workflow 规则资产的长期落点，当前仓库已经落地：

- MySQL / PostgreSQL investigation skill
- MySQL / PostgreSQL diagnosis skill
- bundle 级组合说明
- 受控 markdown 契约解析器与 bundle resolver

当前仍要明确区分两件事：

1. `skills`
   已经接入 runtime 主链，不再只是文档规划。
2. `RAG`
   仍然只有协议预留位，没有接入真实 retrieval、知识注入或外部知识裁决。

因此，后续所有文档都必须遵守同一条边界：

- skill 可以决定“查什么、怎么降级、怎么输出”
- RAG 未来只能提供 diagnosis 附加上下文，不能补齐 required evidence，也不能改变 gate 判定

未来如果接入 `FactRagExecutor / CaseRagExecutor`，插入位置也已经固定：

```text
SkillPolicyGate
  -> FactRagExecutor / CaseRagExecutor (future, optional)
  -> DiagnosisAgent
  -> Grounding
```

也就是说，RAG 只能增强 diagnosis 的上下文，不能回流修改 planner、collection 或 gate 结果。

## 2. 后端分层

当前 workflow 相关后端代码主要分成 4 层：

```text
ApiService
  -> Application Services
    -> Infrastructure Workflow Runtime
      -> MAF Graph + Persistence + Projection
```

各层职责如下：

- `ApiService`
  负责 HTTP 路由、请求绑定和返回码，不承担 workflow 状态机语义。
- `Application / Services`
  把前端请求转成 workflow command，决定是 start、detail、cancel 还是 review submit。
- `Infrastructure / Runtime`
  负责 session 状态、history、checkpoint、review task、cancel、防重入、recovery 和投影。
- `MAF graph`
  只负责节点编排、分支和 review request / response 的连接。

当前真正的业务控制中心是 `ControlPlaneWorkflowRuntime`，不是 API，也不是 graph 本身。

## 3. start 链路

发起入口：

- `POST /api/workflows/db-config-optimization`

主调用链路：

```text
WorkflowsApi
  -> IDbConfigOptimizationWorkflowService.StartAsync
    -> WorkflowSkillResolver.Resolve(...)
    -> IWorkflowRuntime.QueueStartDbConfigAsync
    -> IWorkflowExecutionRegistry.StartOrReplace
      -> IWorkflowRuntime.ContinueStartAsync
        -> ExecuteStartSessionAsync
          -> ExecuteMafStartGraphAsync
```

关键点：

1. `DbConfigOptimizationWorkflowService.StartAsync`
   会先读取连接引擎，再用 `WorkflowSkillResolver` 解析 bundle、investigation skill、diagnosis skill。
2. `DbConfigWorkflowCommand`
   从 start 开始就显式带上：
   `bundleId`、`bundleVersion`、`investigationSkillId`、`investigationSkillVersion`、`diagnosisSkillId`、`diagnosisSkillVersion`。
3. `QueueStartDbConfigAsync`
   先创建 `workflow_sessions` 记录，把 command 持久化到 `InputPayloadJson`，但不在当前请求线程里跑 graph。
4. `WorkflowExecutionRegistry`
   维护 `sessionId -> CancellationTokenSource / background task` 的运行时映射，用于 cancel 和防止旧后台任务覆盖新状态。
5. `ContinueStartAsync`
   在后台继续执行 graph，并把中间状态持续投影到控制面表。

## 4. 当前真实 graph

当前 `BuildDbConfigMafWorkflow()` 的真实节点顺序如下：

```text
DbConfigInputValidationExecutor
  -> InvestigationPlanner
  -> EvidenceCollectionSubworkflow
  -> SkillPolicyGate
  -> DbConfigDiagnosisAgentExecutor
  -> DbConfigGroundingExecutor
  -> DbConfigHumanReviewGateExecutor or DbConfigCompletionDirectExecutor

SkillPolicyGate(blocked)
  -> DbConfigPolicyBlockedCompletionExecutor

DbConfigHumanReviewGateExecutor
  -> RequestPort<MafReviewRequest, MafReviewResponse>
  -> DbConfigReviewResponseExecutor
```

这条链路和旧版“固定 snapshot -> rule analysis -> diagnosis”已经不同：

1. planner 先把 skill ref 映射成 capability 计划。
2. collection subworkflow 按计划做采集，而不是无条件全量采集。
3. policy gate 在 diagnosis 前做 `pass / degraded / blocked` 判定。
4. blocked 路径直接完成，避免缺证据时继续生成看似确定的建议。

## 5. graph 与 runtime 的分工

### 5.1 graph 负责什么

graph 只负责：

- 节点顺序
- blocked / direct completion / human review 分支
- `RequestPort<MafReviewRequest, MafReviewResponse>`
- resume 后从 pending request 继续跑到 completion

### 5.2 runtime 负责什么

`ControlPlaneWorkflowRuntime` 负责：

- 创建和更新 `workflow_sessions`
- 记录 `workflow_node_executions`
- 追加 `workflow_events`
- 创建 `workflow_checkpoints`
- 落库 review task
- 持久化 agent session / summary
- cancel 后阻止旧任务覆写状态
- recovery 时判断“从 checkpoint 续跑”还是“重启 start”

判断标准很简单：

- “节点怎么连”是 graph 的事
- “数据库怎么记、前端怎么查、取消后怎么保护状态”是 runtime 的事

## 6. planner、collection 与 gate

### 6.1 SkillResolver

`WorkflowSkillResolver` 的职责：

1. 根据 `engine + workflowType + requested bundle/version` 解析最终 bundle。
2. 加载 investigation skill 与 diagnosis skill。
3. 在解析阶段阻断 bundle/version 不匹配、skill 缺失、bundle 内 skill 不一致等错误。

解析结果从 start 开始进入 command，并在后续 resume / recovery 中通过已持久化 command 复用，不允许同一 session 漂移到另一套 bundle/version。

### 6.2 InvestigationPlanner

`InvestigationPlanner` 不输出 SQL，只输出 `InvestigationPlan`：

- `planId`
- `bundleId / bundleVersion`
- `skillId / skillVersion`
- capability steps
- required / optional refs
- baseline refs
- retrieval hints
- missing context policy

planner 通过 `EvidenceCapabilityCatalog` 把逻辑 evidence ref 映射成 capability，而不是直接把 skill 内容喂给 collector。

### 6.3 EvidenceCollectionSubworkflow

`EvidenceCollectionSubworkflow` 做三件事：

1. 根据 plan 构造 `DbConfigSnapshotCollectionRequest`
   只请求本次计划需要的 collector key、value key 和 host context。
2. 补齐跨引擎 baseline 元数据
   例如：
   `engine.version`
   `deployment.flavor`
   `parameter.source`
   `parameter.apply_scope`
   `parameter.normalized_unit`
3. 产出 `CollectionExecutionSummary`
   记录 planned / collected 数量、missing refs 和 `CapabilityCollectionResult[]`。

它还会把以下关键信息写进 `collectionMetadata`：

- `bundle_id`
- `bundle_version`
- `investigation_skill_id`
- `investigation_skill_version`
- `plan_id`
- `planned_evidence_count`
- `collected_evidence_count`
- `planned_capability_ids_json`
- `planned_skill_references_json`
- `missing_required_evidence_refs`
- `missing_optional_evidence_refs`
- `capability_results_json`
- `missing_context_policy`
- `retrieval_hints_json`

### 6.4 SkillPolicyGate

`SkillPolicyGate` 负责把采集结果翻译成稳定的 gate 语义：

- `pass`
  必要证据满足，允许进入 diagnosis。
- `degraded`
  有缺口，但不命中 blocking rule，允许继续 diagnosis，但结果必须带降级语义。
- `blocked`
  缺失命中 blocking rule 的必要证据，workflow 直接停止在 diagnosis 前。

gate 会继续把这些字段写回 `collectionMetadata`：

- `skill_id`
- `skill_version`
- `plan_id`
- `collection_completeness`
- `gate_status`

blocked 路径会生成结构化 report JSON，而不是只返回一个异常字符串。

## 7. diagnosis、grounding 与 recommendationType

`DbConfigDiagnosisAgentExecutor` 现在消费的是 `DiagnosisSkillDefinition` 约束后的输入，而不是自由 prompt。

诊断 agent 配置约定：

- 基础键路径固定为 `AIDbOptimize:Agent:Diagnosis`。
- 仓库内 `src/AIDbOptimize.ApiService/appsettings.json` 只保留占位配置，`ApiKey` 默认写成 `xxx`，避免把共享配置误当成可直接运行的真实密钥。
- 本地真实密钥应写在 `src/AIDbOptimize.ApiService/appsettings.Local.json`，`Program.cs` 会在默认配置之上额外加载它，因此本地值优先级更高。
- 如果 `ApiKey` 为空或仍是占位值，workflow 不会兜底跳过 diagnosis，而会直接报出显式配置错误。

后续结果至少经过这些约束点：

1. `RecommendationSchemaValidator`
   保证输出结构稳定。
2. `DiagnosisRuleEvaluator`
   对缺失 host context、低置信度、forbidden pattern 等规则做二次校验。
3. `DbConfigGroundingExecutor`
   保证每条 actionable recommendation 都能回指到 evidence。

`DbConfigRecommendation` 以及 API / 前端 DTO 已经显式暴露 `recommendationType`：

- `actionableRecommendation`
- `requestMoreContext`

因此“缺更多上下文”不再伪装成普通可执行调优建议。

## 8. review / resume 链路

review 入口：

- `GET /api/reviews`
- `GET /api/reviews/{taskId}`
- `POST /api/reviews/{taskId}/submit`

调用链：

```text
ReviewsApi
  -> WorkflowReviewService.SubmitAsync
    -> 更新 workflow_review_tasks
    -> IWorkflowRuntime.ResumeAsync(trigger=review-submit)
      -> ExecuteMafResumeGraphAsync
      -> DbConfigReviewResponseExecutor
      -> ExecuteCompletionAsync
```

关键点：

1. review submit 不是单纯改一条 review 状态。
2. runtime 会校验 session 当前是否仍允许恢复。
3. 它会把 review 决策喂回 MAF `RequestPort`，再统一走 completion、checkpoint 和 history 收尾。
4. 对已经 cancelled / failed / completed 的 session，review submit 会返回冲突，而不是伪造恢复成功。

## 9. recovery 链路

recovery 入口来自：

- `WorkflowRecoveryHostedService`

调用链：

```text
WorkflowRecoveryHostedService
  -> IWorkflowRuntime.ResumeAsync(trigger=recovery)
    -> RunRecoveryAsync
```

### 9.1 有 checkpoint 的恢复

如果 `workflow_sessions.EngineCheckpointRef` 存在：

1. session 先标记为 `Recovering`
2. 记录 `workflow.recovered`
3. 从 `ExecuteMafRecoveryGraphAsync(...)` 恢复 graph
4. 根据输出回到 `WaitingForReview`、`Completed` 或 `Cancelled`

### 9.2 无 checkpoint 的启动期恢复

如果 session 已是 `Running / Recovering`，但还没有 checkpoint：

1. runtime 从 `InputPayloadJson` 反序列化 `DbConfigWorkflowCommand`
2. 复用原 command 中已持久化的 bundle/version/skill selection
3. 记录 `workflow.recovered`，标记 `recoveryMode = restart-start`
4. 重新进入 `ExecuteStartSessionAsync(...)`

这条分支用于覆盖：

- session 已创建
- 后台 graph 还没走到首个 checkpoint
- 进程重启或崩溃

## 10. cancel 语义

cancel 入口：

- `POST /api/workflows/{sessionId}/cancel`

调用链：

```text
WorkflowsApi
  -> IDbConfigOptimizationWorkflowService.CancelAsync
    -> IWorkflowRuntime.CancelAsync
      -> IWorkflowExecutionRegistry.TryCancel
      -> 更新 session / review / node execution / checkpoint
```

cancel 现在同时做两层保护：

1. 运行时级别
   触发该 session 对应后台任务的 `CancellationTokenSource`。
2. 控制面级别
   把 session 持久化为 `Cancelled`，关闭 pending review，补 checkpoint，并阻止后续结果把状态覆写回 `Completed` 或 `WaitingForReview`。

这就是“最佳努力停止”的真实含义：

- 已经发出的外部调用不一定能被立刻杀掉
- 但返回后的结果不能再把 session 状态污染回成功态

## 11. 持久化投影

workflow 相关表的职责如下：

- `workflow_sessions`
  面向前端查询的主会话表。
- `workflow_checkpoints`
  保存 MAF checkpoint 快照与恢复引用。
- `workflow_review_tasks`
  保存 review 任务、决策与 checkpoint 关联。
- `workflow_node_executions`
  保存每个节点的输入、输出、状态与错误。
- `workflow_events`
  保存 timeline 事件，SSE 和 replay 依赖它。
- `mcp_tool_executions`
  保存真实 MCP 工具调用审计。
- `agent_sessions / agent_messages / agent_summaries`
  保存 diagnosis agent 的上下文与摘要。

## 12. state、checkpoint 与 skill selection

当前 runtime 会把显式 skill 选择持续写入多个持久化面：

- `workflow_sessions.InputPayloadJson`
- `workflow_sessions.StateJson`
- `workflow_checkpoints.SnapshotJson`

其中至少包含：

- `bundleId`
- `bundleVersion`
- `investigationSkillId`
- `investigationSkillVersion`
- `diagnosisSkillId`
- `diagnosisSkillVersion`
- `skillSelection`

因此 start、resume、recovery、history 和 checkpoint 都能追溯“本次到底用了哪套规则”。

## 13. SSE 与 history 为什么能稳定回放

### 13.1 SSE

SSE 入口：

- `GET /api/workflows/{sessionId}/events`

SSE 读取的是控制面事件投影，而不是直接偷窥 graph 内存状态，所以：

- 前端能在断线后重新拉历史事件
- 恢复后仍能看到同一 session 的完整 timeline
- review / recovery / cancel 也能和 start 阶段使用同一套事件来源

### 13.2 history detail

history detail 来自：

- `workflow_sessions`
- `workflow_node_executions`
- `mcp_tool_executions`
- `workflow_review_tasks`

因此它能同时展示：

- 当前状态和最终结果
- 每个节点的执行输入 / 输出
- MCP 工具调用审计
- review 决策记录

换句话说，前端看到的“状态、回放、历史详情”都来自持久化投影，而不是临时内存对象。

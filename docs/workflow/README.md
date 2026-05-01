# Workflow 后端代码流程说明

本文档描述当前 `DbConfigOptimization` workflow 在后端的真实代码链路，重点解释：

- 请求如何从 API 进入 Application / Runtime
- MAF graph 和控制面 runtime 各自负责什么
- review / resume / recovery / cancel 的实际落点
- 控制面持久化表分别承担什么职责
- SSE 与 history 为什么能拿到当前状态和历史明细

如果要看需求边界和排期，请回到：

- [当前 workflow 总体方案](../plans/active/2026-04-aidboptimize-db-config-workflow/design.md)
- [当前 workflow 详细设计](../plans/active/2026-04-aidboptimize-db-config-workflow/detailed-design.md)

## 1. 后端模块分层

当前 workflow 相关后端代码主要分成 4 层：

```text
ApiService
  -> Application Services
    -> Infrastructure Workflow Runtime
      -> MAF Graph + Persistence + Projection
```

对应职责：

- `ApiService`
  负责 HTTP 路由、请求绑定和响应返回。
- `Application / Services`
  负责把前端请求转成 workflow command，决定是排队启动、查询、取消还是提交 review。
- `Infrastructure / Runtime`
  负责 workflow 会话状态机、checkpoint、review task、history event、node execution、恢复和取消保护。
- `MAF graph`
  负责节点顺序、request port、resume 后的 completion 输出。

这 4 层里，真正的业务语义控制中心是 `ControlPlaneWorkflowRuntime`，而不是 API，也不是 MAF graph 本身。

## 2. workflow 发起链路

发起入口：

- `POST /api/workflows/db-config-optimization`

主要调用链：

```text
WorkflowsApi
  -> IDbConfigOptimizationWorkflowService.StartAsync
    -> IWorkflowRuntime.QueueStartDbConfigAsync
    -> IWorkflowExecutionRegistry.StartOrReplace
      -> IWorkflowRuntime.ContinueStartAsync
        -> ExecuteStartSessionAsync
          -> ExecuteMafStartGraphAsync
```

代码职责拆分：

- `WorkflowsApi`
  只负责收请求并返回 `202 Accepted`。
- `DbConfigOptimizationWorkflowService.StartAsync`
  先创建一条 `workflow_sessions` 记录，再把后台启动任务注册到 `IWorkflowExecutionRegistry`。
- `WorkflowExecutionRegistry`
  维护 `sessionId -> CancellationTokenSource / background task` 的运行时映射，供 cancel 最佳努力停止使用。
- `ControlPlaneWorkflowRuntime.QueueStartDbConfigAsync`
  只创建 session，不真正执行 graph。
- `ControlPlaneWorkflowRuntime.ContinueStartAsync`
  在后台继续执行，负责打点和失败统计。

### 2.1 启动期 session 长什么样

`QueueStartDbConfigAsync(...)` 会先创建一条 `workflow_sessions`：

- `Status = Running`
- `CurrentNodeKey = DbConfigInputValidationExecutor`
- `InputPayloadJson` 保存完整 `DbConfigWorkflowCommand`
- `EngineRunId` 先生成
- `ResultPayloadJson = {}`

这样做的意义是：

- API 能立即返回一个 `sessionId`
- recovery 可以在启动未完成时依靠 `InputPayloadJson` 重新拉起

## 3. MAF graph 和 runtime 的分工

当前 graph 逻辑固定为：

```text
DbConfigInputValidationExecutor
  -> DbConfigSnapshotCollectorExecutor
  -> DbConfigRuleAnalysisExecutor
  -> DbConfigDiagnosisAgentExecutor
  -> DbConfigGroundingExecutor
  -> DbConfigHumanReviewGateExecutor 或 DbConfigCompletionDirectExecutor
  -> RequestPort / ReviewResponseExecutor
```

关键点：

- `start / resume / recovery` 共享同一张 `BuildDbConfigMafWorkflow()` 生成的 graph
- runtime 决定“从哪里开始”和“如何处理输出”
- MAF 只负责编排节点与 request/response 续跑

### 3.1 graph 负责什么

MAF graph 负责：

- 节点顺序
- `RequireHumanReview` 分支
- `RequestPort<MafReviewRequest, MafReviewResponse>`
- `Run.ResumeAsync(...)` 后产生 `MafCompletionMessage`

### 3.2 runtime 负责什么

`ControlPlaneWorkflowRuntime` 负责：

- session 状态变化
- review task 创建 / 更新
- checkpoint 落库
- node execution 审计
- workflow event 投影
- agent session / summary 持久化
- cancel 防脏写
- recovery 分支判断

一个简单判断标准是：

- “节点怎么连”是 graph 的事
- “数据库里怎么记、前端怎么看、取消时怎么保护”是 runtime 的事

## 4. 节点执行真实链路

`ExecuteMafStartGraphAsync(...)` 和 `BuildDbConfigMafWorkflow()` 内部通过 `FunctionExecutor` 把节点映射到 runtime handler：

- `HandleValidationMessageAsync`
- `HandleSnapshotMessageAsync`
- `HandleRuleAnalysisMessageAsync`
- `HandleDiagnosisMessageAsync`
- `HandleGroundingMessageAsync`
- `HandleReviewRequestMessageAsync`

这些 handler 的共同模式是：

1. 从控制面库加载当前 session
2. 检查 workflow 是否已被取消
3. 执行对应业务节点
4. 更新 `workflow_node_executions`
5. 追加 `workflow_events`
6. 更新 `workflow_sessions.CurrentNodeKey / StateJson / ResultPayloadJson`

### 4.1 Snapshot 节点

`DbConfigSnapshotCollectorExecutor` 不让 agent 自由调工具，而是通过固定模板走：

```text
IMcpToolExecutionService
  -> McpClientFactory
  -> real MCP server
  -> CallToolAsync
  -> McpToolExecutions audit
```

所以 workflow 主路径的采集是“确定性 + 可审计”的，不是 agent 随机选工具。

### 4.2 Diagnosis 节点

`DbConfigDiagnosisAgentExecutor` 的输出会被 runtime 进一步做 3 件事：

- `RecommendationSchemaValidator` 校验结构
- `AgentSessionPersistenceService` 保存 agent session / messages
- `AgentSummaryService` 生成滚动 summary

因此 agent 结果不是只存在内存里，而是同步沉淀到控制面库。

## 5. review / resume 链路

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
      -> RunResumeAsync
        -> ExecuteMafResumeGraphAsync
        -> ExecuteCompletionAsync 或 Cancelled
```

### 5.1 review task 是怎么创建的

当 graph 进入 review 分支后：

- `HandleReviewRequestMessageAsync(...)` 先把 session 节点推进到 `DbConfigHumanReviewGateExecutor`
- `ExecuteReviewGateAsync(...)` 再把 runtime 侧审计和 review task 持久化补齐

创建时会写入：

- `workflow_review_tasks`
- `workflow_node_executions` 中一个 `WaitingForReview` 的 gate 节点
- `workflow_events.review.requested`
- `workflow_sessions.ActiveReviewTaskId`
- `workflow_sessions.EngineCheckpointRef`

### 5.2 review submit 为什么不能直接跳过 runtime

因为 review submit 不只是“改个状态”，还要：

- 校验当前 session 仍允许恢复
- 把 review 决策喂回 MAF `RequestPort`
- 生成最终 completion / cancel
- 收敛 node execution / event / checkpoint / final result

所以 `POST /api/reviews/{taskId}/submit` 一定会再进 runtime。

### 5.3 已取消 workflow 为什么不能再提交 review

cancel 后 runtime 会：

- 关闭活跃 review task
- 清空 `ActiveReviewTaskId`
- 把 session 置为 `Cancelled`

`WorkflowReviewService.SubmitAsync(...)` 会再次检查 session 状态，`Cancelled / Failed / Completed` 一律拒绝提交，避免 stale review 再次驱动恢复。

## 6. recovery 链路

recovery 入口不是前端 API，而是：

- `WorkflowRecoveryHostedService`

调用链：

```text
WorkflowRecoveryHostedService
  -> IWorkflowRuntime.ResumeAsync(trigger=recovery)
    -> RunRecoveryAsync
```

### 6.1 有 checkpoint 的恢复

如果 `workflow_sessions.EngineCheckpointRef` 存在：

- runtime 先把 session 标成 `Recovering`
- 写 `workflow.recovered`
- 调 `ExecuteMafRecoveryGraphAsync(...)`
- 根据输出回到：
  - `WaitingForReview`
  - `Completed`
  - `Cancelled`

### 6.2 无 checkpoint 的启动期恢复

如果 session 是 `Running / Recovering`，但还没有 `EngineCheckpointRef`：

- runtime 从 `InputPayloadJson` 反序列化出 `DbConfigWorkflowCommand`
- 记录一次 `workflow.recovered`，标记 `recoveryMode = restart-start`
- 重新进入 `ExecuteStartSessionAsync(...)`

这条分支是为了覆盖：

- session 已建好
- 后台 graph 还没跑到首个 checkpoint
- 进程就重启 / 崩溃

以前这里会直接失败；现在可以从启动阶段继续。

## 7. cancel 链路

cancel 入口：

- `POST /api/workflows/{sessionId}/cancel`

调用链：

```text
WorkflowsApi
  -> IDbConfigOptimizationWorkflowService.CancelAsync
    -> IWorkflowRuntime.CancelAsync
      -> IWorkflowExecutionRegistry.TryCancel
      -> session / review / node execution / checkpoint update
```

### 7.1 cancel 现在做了什么

`CancelAsync(...)` 现在会做两类动作：

1. 运行时级别

- 找到该 `sessionId` 对应的后台执行
- 触发其 `CancellationTokenSource`

2. 控制面级别

- `workflow_sessions.Status = Cancelled`
- 关闭 pending review task
- 收敛 `WaitingForReview` 节点
- 写 `workflow.cancelled`
- 补一条 checkpoint

### 7.2 为什么叫“最佳努力停止”

因为已经发出去的单次外部调用不一定能被立刻强杀，例如：

- 某个 MCP tool 已经开始执行
- 某次模型调用已经发出

当前实现保证的是：

- 尽量中断后续节点
- 即使外部调用自然返回，也不能再把 session 覆盖成 `Completed / WaitingForReview`

这靠两层保护实现：

- active execution token
- runtime 写终态前的 cancel guard

## 8. 控制面持久化表职责

workflow 相关表不要混着理解：

- `workflow_sessions`
  主会话表，对前端状态查询最重要。
- `workflow_checkpoints`
  保存 checkpoint 快照与恢复引用。
- `workflow_review_tasks`
  保存人审任务、恢复关联和最终决定。
- `workflow_node_executions`
  保存每个节点的输入输出和执行状态。
- `workflow_events`
  保存时间线事件，SSE 和 replay 主要依赖它。
- `mcp_tool_executions`
  保存 workflow 采集过程中真实发生的 MCP 工具调用审计。
- `agent_sessions / agent_messages / agent_summaries`
  保存 diagnosis agent 的恢复上下文和可读摘要。

一个实用记法：

- “当前状态”看 `workflow_sessions`
- “怎么恢复”看 `workflow_checkpoints`
- “为什么卡住”看 `workflow_review_tasks`
- “哪个节点做了什么”看 `workflow_node_executions`
- “时间线怎么播”看 `workflow_events`

## 9. SSE 与 history 为什么能拿到数据

### 9.1 SSE

SSE 入口：

- `GET /api/workflows/{sessionId}/events`

它不是直接订阅 graph，而是轮询控制面库中的 `workflow_events`，并在首帧拼一个 session snapshot。

所以 SSE 的事实来源是：

- snapshot：`workflow_sessions`
- backlog / heartbeat / replay：`workflow_events`

### 9.2 history

history 入口：

- `GET /api/history`
- `GET /api/history/{sessionId}`

history detail 之所以能同时看到结果、node、tool、review，是因为 runtime 在每条主路径上都把这些投影写回控制面库，而不是只把最终结果存一份。

## 10. 与前端协作时最重要的几个语义

前端最依赖下面这些稳定语义：

- workflow 创建后会立即拿到 `sessionId`
- 真实进度状态来自 `workflow_sessions + workflow_events`
- `WaitingForReview` 一定伴随 `ActiveReviewTaskId`
- cancel 后 session 不会再回写成 `Completed`
- 已取消 workflow 的 review 不能再次提交
- recovery 可以覆盖 checkpoint 恢复和启动阶段重试恢复两条路径

如果后续要改 runtime，最需要保护的也是这几条。

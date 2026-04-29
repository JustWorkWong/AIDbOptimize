# AIDbOptimize 数据库配置优化 workflow 任务拆解

## 里程碑

本轮继续沿用 6 个阶段推进，并以本文件作为唯一执行清单。  
完成标记规则：

- `[x]` 代表代码、测试、前端接线、文档注记都已经满足验收条件。
- `[ ]` 代表仍有实现缺口，或仍保留为显式技术债。
- 对“已知部分完成”的项继续拆细，避免一个勾选同时覆盖 v1 和目标态两种语义。

## 阶段 1：控制面 schema 与契约

- [x] 为 `workflow_sessions` 补齐目标态字段
- [x] 为 `workflow_checkpoints` 升级为目标态 checkpoint 结构
- [x] 为 `workflow_review_tasks` 补齐目标态关联字段
- [x] 为 `workflow_node_executions` 补齐节点审计字段
- [x] 为 `workflow_events` 补齐稳定顺序与追踪字段
- [x] 为现有 `mcp_tool_executions` 增加 `workflow_session_id / workflow_node_name / execution_scope / trace_id`
- [x] 在 `Application` 层升级 workflow / review / history DTO
- [x] 生成控制面迁移并更新 snapshot

验收：

- [x] `dotnet ef migrations add ...` 可生成并通过
- [x] `ControlPlaneDbContext` 能完成 build 并带上最新 snapshot

## 阶段 2：workflow runtime 骨架

- [x] 引入 MAF workflow 依赖并锁定版本
- [x] 固化 `IWorkflowRuntime` 为 `StartDbConfigAsync / ResumeAsync / CancelAsync`
- [x] 新增 `DbConfigWorkflowCommand / WorkflowResumeRequest / WorkflowRunDescriptor`
- [x] 新增统一 `RunInternalAsync(...)` 主执行入口
- [x] 打通 `StartAsync`
- [x] 打通 `CancelAsync`
- [x] 打通 `ResumeAsync`
- [x] 新增 `WorkflowRecoveryHostedService` 并在启动时扫描 `Running / Recovering` 会话
- [x] 将 control-plane runtime 替换为真正的 `Microsoft Agent Framework Workflow graph` 执行内核
  当前 `start / review resume / recovery` 已统一基于 `WorkflowBuilder + FunctionExecutor + InProcessExecution + CheckpointManager + RequestPort + Run.ResumeAsync(...)`。

验收：

- [x] 能创建 session 并进入 `Running / WaitingForReview / Completed`
- [x] `Start / Resume / Cancel / Recovery` 都走统一 runtime 语义

## 阶段 3：配置采集、规则分析与 agent 持久化

- [x] `DbConfigInputValidationExecutor` 接入节点审计
- [x] `DbConfigSnapshotCollectorExecutor` 接入 workflow 上下文
- [x] 基于 `IMcpToolExecutionService` 的固定模板采集接入 `workflowSessionId / workflowNodeName / executionScope / traceId`
- [x] workflow 主路径保持只读工具白名单
- [x] `DbConfigRuleAnalysisExecutor` 接入节点审计
- [x] `DbConfigDiagnosisAgentExecutor` 升级为返回结构化执行结果
- [x] `IAgentSessionPersistenceService`、`IAgentSummaryService` 与 runtime 打通
- [x] 在 workflow 中保存 `serialized session / summary / messages`
- [x] 记录 agent prompt version、model id、token usage
- [x] 适配真实 MySQL `execute_query` 工具返回，提取结构化配置值而不是只保留 MCP 文本块

验收：

- [x] 能得到结构化 evidence pack
- [x] 能生成 `db-config-optimization-report`
- [x] agent session / summary / messages 可落库
- [x] history detail 能看到 diagnosis 节点与 agent session 关联
- [x] MySQL 真实采集结果可落到 `max_connections / innodb_buffer_pool_size` 等 evidence 项

## 阶段 4：grounding、review、resume

- [x] `DbConfigGroundingExecutor` 接入节点审计
- [x] `RecommendationSchemaValidator` 接入主路径
- [x] `ReviewAdjustmentValidator` 切到目标态结构
- [x] `DbConfigHumanReviewGateExecutor` 写入 `requestId + runId + checkpointRef + checkpointId`
- [x] 实现 `GET /api/reviews`
- [x] 实现 `GET /api/reviews/{taskId}`
- [x] 实现 `POST /api/reviews/{taskId}/submit`
- [x] 让 review submit 驱动同一 run 恢复
- [x] `adjust` 会真正修改 recommendation risk level，而不是只拼接 summary 文案
- [x] `review gate / review resume` 走原生 `RequestPort / RequestInfoEvent / ExternalResponse / Run.ResumeAsync(...)`

验收：

- [x] `requireHumanReview = false` 能直接完成
- [x] `requireHumanReview = true` 能挂起并恢复
- [x] `adjust` 能产生修正后的最终报告
- [x] `reject` 能让 workflow 进入 `Cancelled`

## 阶段 5：事件流、历史与前端工作台

- [x] `workflow_events` 改为由 runtime / executor 直接写入稳定事件流
- [x] 实现 `GET /api/workflows/{sessionId}/events`
- [x] SSE 首帧返回 `snapshot`
- [x] SSE 支持 `Last-Event-ID` backlog replay
- [x] SSE 保持 `heartbeat`
- [x] 实现 `GET /api/workflows`
- [x] 实现 `GET /api/workflows/{sessionId}`
- [x] 实现 `GET /api/history`
- [x] 实现 `GET /api/history/{sessionId}`
- [x] `history detail` 返回 node executions / tool executions / reviews / result / summary / error
- [x] Web 拆分 `workflow.ts / review.ts / history.ts`
- [x] Web 新增独立 `WorkflowWorkspace`
- [x] `App.vue` 不再承载 workflow 状态编排逻辑
- [x] 前端改为基于 `EventSource` 的实时事件订阅
- [x] Web 保留 `db-config / review / history / replay` 工作区

验收：

- [x] 前端能发起 workflow
- [x] 前端能实时看到状态变化
- [x] 前端能处理 review
- [x] 前端能回放事件
- [x] 前端构建通过

## 阶段 6：安全、OTel、测试与收尾

- [x] `PromptInputBuilder`、`ToolOutputSanitizer`、`ToolAllowlistPolicy`、`SensitiveDataMasker` 已接入主链路
- [x] workflow / agent / review / mcp 的 `ActivitySource` 与 `Meter` 持续保留
- [x] 补齐 checkpoint / resume / review / failure / tool execution 指标
- [x] 补齐单元测试
- [x] 补齐集成测试
- [x] 补齐前端构建验证
- [x] API 响应、event payload、agent message、tool execution 审计不回传连接串明文
- [x] 将 `mcp_connections.DatabaseConnectionString / EnvironmentJson` 改为加密存储
- [x] 将 `ServerArgumentsJson` 一并纳入加密保护
- [x] 删除旧的 `/api/workflows/db-config*` 兼容入口
- [x] 删除轮询式 workflow 工作区刷新逻辑
- [x] 增加 MAF graph 最小运行测试
- [x] 增加 MAF request/resume 最小运行测试

验收：

- [x] OTel 中能看到 `start -> review -> resume -> complete` 链路
- [x] `dotnet test` 全部通过
- [x] `npm run build` 通过
- [x] 敏感字段加密存储完成
- [x] 控制面库中可查到完整的 workflow / checkpoint / review / node execution / event / tool execution / agent session 记录

## 明确暂不做

- [ ] 自动应用配置变更
- [ ] Redis checkpoint 热缓存
- [ ] 多租户 / SSO / 复杂权限系统

## 评审重点

- [x] `run / resume / recovery` 是否统一走同一 runtime 主入口
- [x] `workflow checkpoint / business state / agent session / review pending state` 是否已分层
- [x] workflow 主路径是否保持只读工具
- [x] 前端是否改为独立 workflow 工作区与 SSE 实时订阅
- [x] runtime 内核是否已切到真正的 MAF native graph
- [x] 敏感字段是否已经加密存储
- [x] 建议结果是否能基于真实采集值生成，而不是仅依赖 metadata fallback

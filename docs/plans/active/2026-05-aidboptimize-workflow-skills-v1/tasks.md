# AIDbOptimize workflow skills v1 任务清单

## 里程碑

本轮只交付 `skills v1`；`RAG` 只预留协议与扩展位，不接真实检索与知识注入。

完成标记规则：

- `[x]` 代表代码、测试、文档与任务状态已经对齐
- `[ ]` 代表仍有实现缺口或验证缺口

最终验证：

- [x] `dotnet test .\AIDbOptimize.slnx --no-restore`
- [x] `npm run build` (`src/AIDbOptimize.Web`)

## 阶段 1：规划与文档基线

- [x] 创建 `docs/workflow/skills/` 目录规划，并同步更新对应 `CLAUDE.md`
- [x] 在 `docs/workflow/README.md` 增加 workflow 消费 skills 的长期说明入口
- [x] 在 `docs/README.md` 与仓库根 `README.md` 切换当前唯一执行计划指针
- [x] 确认本轮 `RAG` 仅预留不实现，并在长期文档中明确说明

验收：

- [x] 仓库根 `CLAUDE.md` 与 `README.md` 指向本计划
- [x] `docs/README.md` 清晰区分当前计划与历史基线

## 阶段 2：预研先行

- [x] 完成 `SKILL.md contract spike`，验证 front matter + 固定章节解析策略
- [x] 完成 `Evidence ref normalization spike`，验证 `skill ref -> capability ref -> evidence pack reference -> grounding reference`
- [x] 完成 `No-op node insertion spike`，验证在 diagnosis 前插入空节点后 history / progress / resume / recovery 仍然正确
- [x] 完成 `Degraded semantics spike`，验证 `actionable recommendation` 与 `request-more-context` 的分流语义
- [x] 完成 `Diagnosis rule evaluator spike`，验证缺 host context 禁止目标值、low-confidence 必须 `requiresMoreContext=true`

验收：

- [x] 每个 spike 都有对应测试或最小验证代码
- [x] spike 结论回填到 `detailed-design.md`

## 阶段 3：skill 协议与 bundle 解析

- [x] 新增 `mysql-investigation/SKILL.md`
- [x] 新增 `mysql-diagnosis/SKILL.md`
- [x] 新增 `postgresql-investigation/SKILL.md`
- [x] 新增 `postgresql-diagnosis/SKILL.md`
- [x] 新增 MySQL 默认 bundle
- [x] 新增 PostgreSQL 默认 bundle
- [x] 定义 `bundle.md` 文件格式与必填元数据
- [x] 固化 investigation skill markdown 章节协议
- [x] 固化 diagnosis skill markdown 章节协议
- [x] 固化 `SKILL.md` 结构化元数据协议
- [x] 实现 bundle parser
- [x] 实现 `MarkdownSkillParser`
- [x] 实现 `WorkflowSkillCatalog`
- [x] 实现 `WorkflowSkillResolver`
- [x] 定义 `InvestigationSkillDefinition`
- [x] 定义 `DiagnosisSkillDefinition`
- [x] 定义 `SkillBundleDefinition`
- [x] 实现 bundle 内 skill 元数据一致性校验
- [x] 定义 bundle/version 解析来源策略：请求、默认配置、持久化恢复
- [x] 更新 `StartDbConfigOptimizationWorkflowRequest`，支持显式 bundle/version 输入
- [x] 更新 workflow start API 请求模型与校验逻辑，支持显式 bundle/version 或默认解析
- [x] 更新前端 workflow 启动请求模型，允许传入 bundle/version
- [x] 让 start 路径支持显式 bundle/version 或默认 bundle 解析
- [x] 让 resume/recovery 路径强制复用已持久化 bundle/version

验收：

- [x] 代码能够按 `engine + workflowType + version` 解析并加载正确 bundle
- [x] 非法或缺失元数据、缺失章节、bundle 不兼容、bundle 内 skill 不一致会返回明确错误
- [x] 同一个 workflow 的 start / resume / recovery 不会漂移到不同 bundle/version
- [x] 显式 bundle/version 能从前端请求一路传到 start 入口并被 resolver 正确消费

## 阶段 4：capability 目录与 planner

- [x] 定义 `EvidenceCapabilityCatalog`
- [x] 为 MySQL 建立最小 capability 映射
- [x] 为 PostgreSQL 建立最小 capability 映射
- [x] 定义 `InvestigationPlan`
- [x] 定义 `CollectionExecutionSummary`
- [x] 实现 `InvestigationPlanner`
- [x] 确认 planner 输出消费的是 capability，而不是直接驱动 SQL
- [x] 将跨引擎基础元证据纳入第一版 baseline：`engine.version`、`deployment.flavor`、`parameter.source`、`parameter.apply_scope`、`parameter.normalized_unit`
- [x] 为跨引擎基础元证据定义 capability 采集与归一化规则
- [x] 将跨引擎基础元证据写入 evidence pack
- [x] 将跨引擎基础元证据写入 collection metadata
- [x] 让 diagnosis 输入显式消费这些基础元证据

验收：

- [x] workflow 前半段能够显式产出采集计划
- [x] plan 中每个 evidence ref 都能映射到 capability
- [x] 跨引擎基础元证据能够从 capability 一路进入 evidence pack、collection metadata 与 diagnosis 输入

## 阶段 5：采集子流程与运行时插入

- [x] 将现有采集逻辑收敛为 `EvidenceCollectionSubworkflow`
- [x] 让 subworkflow 消费 planner 输出，而不是继续按固定顺序全量采集
- [x] 为 capability result 输出结构化执行结果：成功、归一化、错误分类、时效、来源质量
- [x] 在 runtime 中接入 `SkillResolver -> InvestigationPlanner -> EvidenceCollectionSubworkflow`
- [x] 持久化 `bundle_id / bundle_version / investigation_skill_version / diagnosis_skill_version`
- [x] 将这些版本信息写入 session、checkpoint、history detail 与 collection metadata
- [x] 更新 `WorkflowProgressCalculator`，为新节点定义稳定进度语义
- [x] 更新 history 节点展示语义，明确新节点的人类可读名称和职责
- [x] 更新前端节点状态映射，确保 workflow 详情与 replay 能正确展示新节点

验收：

- [x] history 中能看出计划证据数量与实际采集数量
- [x] no-op 节点预研结论在真实实现下仍然成立
- [x] 新节点接入后 progress / history / replay 展示不退化

## 阶段 6：policy gate 与降级语义

- [x] 实现 `SkillPolicyGate`
- [x] 固化 `pass / degraded / blocked` 三档语义
- [x] 让 `BlockingRules` 升级为规则求值，而不是只检查“字段有没有拿到”
- [x] 将 required evidence 缺口写入 `MissingContextItems`
- [x] 将 `skill_id / skill_version / plan_id / collection_completeness` 写入 `CollectionMetadata`
- [x] 对 blocked 路径补充统一错误说明

验收：

- [x] 缺失必需证据时 workflow 不会伪造确定性结论
- [x] degraded 路径能够继续进入 diagnosis
- [x] gate 判定不受未来 `RAG` 数据影响

## 阶段 7：diagnosis skill 驱动输出

- [x] 改造 `DbConfigDiagnosisAgentExecutor`，使其消费 `DiagnosisSkillDefinition`
- [x] 固化 output contract 到 prompt 输入构造
- [x] 固化 recommendation rules、confidence rules、forbidden patterns
- [x] 新增 `DiagnosisRuleEvaluator`
- [x] 缺失 host context 时禁止输出具体目标值
- [x] 区分 `actionable recommendation` 与 `request-more-context`
- [x] 保持 `DbConfigGroundingExecutor` 继续校验 evidence grounding
- [x] 为 `DbConfigRecommendation` 增加 `recommendationType` 领域语义
- [x] 更新 workflow 结果 DTO 与 history detail DTO，显式暴露 `recommendationType`
- [x] 更新 API 序列化与解析逻辑，确保 `actionable recommendation` 与 `request-more-context` 都能稳定传输
- [x] 更新前端 workflow 结果展示与 history 展示，显式区分两类 recommendation

验收：

- [x] diagnosis 输出结构稳定
- [x] 每条 actionable recommendation 都带正向 `evidenceReferences`
- [x] missing-context-only 的结果不会伪装成 actionable recommendation
- [x] forbidden pattern 会被识别并阻断
- [x] API 与前端都能稳定展示 `recommendationType`

## 阶段 8：RAG 预留位

- [x] 为 investigation skill 增加 `Retrieval Hints` 协议
- [x] 为 diagnosis skill 增加 `Citation Rules` 协议
- [x] 为 planner 保留 retrieval hints 透传字段
- [x] 为后续 knowledge injection 保留 evidence pack 扩展位
- [x] 在长期文档中说明未来 `FactRagExecutor / CaseRagExecutor` 的插入位置
- [x] 明确 `RAG` 只能作为 diagnosis 附加上下文，不能补齐 required evidence

验收：

- [x] 第一版代码中没有真实 RAG 依赖
- [x] 第二版接 RAG 时不需要重构主链

## 阶段 9：测试与验证

- [x] 补 skill parser 单元测试
- [x] 补 bundle resolver 单元测试
- [x] 补 capability catalog 单元测试
- [x] 补 planner 单元测试
- [x] 补 policy gate 单元测试
- [x] 补 diagnosis rule evaluator 单元测试
- [x] 补 workflow 集成测试：MySQL pass 路径
- [x] 补 workflow 集成测试：PostgreSQL pass 路径
- [x] 补 workflow 集成测试：缺失必需证据时 degraded / blocked 路径
- [x] 补 workflow 集成测试：skill 缺失或不匹配时 blocked 路径
- [x] 补 workflow 集成测试：插入新节点后 progress / resume / recovery 不退化

验收：

- [x] `dotnet test` 覆盖 parser、bundle、planner、policy gate、diagnosis rule evaluator
- [x] 至少一条 MySQL 与一条 PostgreSQL workflow 集成路径通过

## 评审重点

- [x] skill 真正驱动采集规划，而不是只驱动 prompt
- [x] investigation / diagnosis 通过 bundle 绑定，而不是运行时混搭
- [x] capability catalog 已把 skill ref 与 collector 执行真实接上
- [x] diagnosis agent 同时受 diagnosis skill 与 rule evaluator 约束
- [x] 采集缺口能够显式进入 `MissingContextItems`
- [x] 当前主链为后续 `RAG` 保留了稳定扩展位
- [x] 目录结构与职责边界保持简洁，没有把 skill、采集、diagnosis 混成一层

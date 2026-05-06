# AIDbOptimize workflow skills v1 详细方案

## 1. 当前事实与缺口

### 1.1 当前可复用基线

当前仓库已经具备以下可直接复用能力：

1. `DbConfigOptimization` workflow 主链、review、resume、recovery、SSE、history。
2. `DbConfigEvidencePack`、`DbConfigEvidenceItem`、`DbConfigMissingContextItem`、`DbConfigRecommendation` 等结构化模型。
3. `DbConfigSnapshotCollectorExecutor` 的只读采集路径。
4. `DbConfigDiagnosisAgentExecutor` 与 `DbConfigGroundingExecutor` 的现有诊断与 grounding 骨架。

### 1.2 当前缺口

当前 workflow 仍然缺少：

1. 前置“该查什么”的规范层。
2. 采集计划与采集执行之间的稳定机器契约。
3. `skill -> capability -> evidence reference -> grounding reference` 的统一映射。
4. diagnosis skill 的硬约束执行点。
5. 对未来 `RAG` 的稳定扩展位与硬边界。

## 2. 目标结构

### 2.1 文档与 skill 目录

规划文档保存在：

```text
docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/
  design.md
  detailed-design.md
  tasks.md
```

后续实现资产保存在：

```text
docs/workflow/skills/
  CLAUDE.md
  bundles/
    mysql-default/
      bundle.md
    postgresql-default/
      bundle.md
  mysql-investigation/
    SKILL.md
  mysql-diagnosis/
    SKILL.md
  postgresql-investigation/
    SKILL.md
  postgresql-diagnosis/
    SKILL.md
```

### 2.2 代码模块

建议新增：

```text
src/AIDbOptimize.Infrastructure/Workflows/
  Skills/
    WorkflowSkillResolver.cs
    WorkflowSkillCatalog.cs
    MarkdownSkillParser.cs
    InvestigationSkillDefinition.cs
    DiagnosisSkillDefinition.cs
    SkillBundleDefinition.cs
    InvestigationPlan.cs
    CollectionExecutionSummary.cs
    EvidenceCapabilityCatalog.cs
  Pipeline/
    InvestigationPlanner.cs
    SkillPolicyGate.cs
    DiagnosisRuleEvaluator.cs
```

## 3. SKILL.md 机器契约

### 3.1 总原则

`SKILL.md` 是主源，但必须同时满足：

1. 人可读。
2. 机器可稳定解析。
3. 后续版本可演进。

因此 parser 不能直接依赖自由文本段落，而必须依赖“结构化元数据 + 固定章节”的双层契约。

### 3.2 必要元数据

每个 `SKILL.md` 顶部都必须带结构化元数据，至少包含：

1. `skill_id`
2. `skill_type`
3. `engine`
4. `skill_version`
5. `schema_version`
6. `bundle_id`
7. `bundle_version`

解析规则：

1. 元数据缺失时，直接阻断解析。
2. 未知字段默认允许保留，但不能 silently 改变运行时语义。
3. 章节缺失要分级报错，不能全部退化成同一种失败。
4. parser 只消费结构化元数据和受控章节，不依赖自然语言解释段落。

### 3.3 investigation skill 固定章节

每个 `investigation skill` 至少包含：

1. `## Scope`
2. `## Required Evidence`
3. `## Optional Evidence`
4. `## Blocking Rules`
5. `## Investigation Questions`
6. `## Collection Hints`
7. `## Retrieval Hints`

### 3.4 diagnosis skill 固定章节

每个 `diagnosis skill` 至少包含：

1. `## Scope`
2. `## Output Contract`
3. `## Recommendation Rules`
4. `## Confidence Rules`
5. `## Forbidden Patterns`
6. `## Citation Rules`

## 4. skill bundle / profile

`SkillResolver` 不直接返回两份松散 skill，而是先解析 `skill bundle/profile`。

bundle 至少定义：

1. `bundle_id`
2. `bundle_version`
3. `workflow_type`
4. `engine`
5. `investigation_skill_id`
6. `investigation_skill_version`
7. `diagnosis_skill_id`
8. `diagnosis_skill_version`

bundle 的意义是：

1. 保证 investigation 与 diagnosis 使用兼容的一组定义。
2. 允许未来分别升级单个 skill，但仍通过 bundle 做组合兼容。
3. 让 workflow session、checkpoint、history 能明确追溯“本次到底用了哪套规则”。

## 5. 内部定义模型

### 5.1 InvestigationSkillDefinition

建议包含：

1. `SkillId`
2. `Engine`
3. `Version`
4. `RequiredEvidence`
5. `OptionalEvidence`
6. `BlockingRules`
7. `InvestigationQuestions`
8. `CollectionHints`
9. `RetrievalHints`

### 5.2 DiagnosisSkillDefinition

建议包含：

1. `SkillId`
2. `Engine`
3. `Version`
4. `OutputFields`
5. `RecommendationRules`
6. `ConfidenceRules`
7. `ForbiddenPatterns`
8. `CitationRules`

### 5.3 InvestigationPlan

planner 输出不是 SQL，而是采集计划。建议包含：

1. `PlanId`
2. `BundleId`
3. `BundleVersion`
4. `SkillId`
5. `Engine`
6. `Steps`
7. `RequiredEvidenceRefs`
8. `OptionalEvidenceRefs`
9. `MissingContextPolicy`
10. `RetrievalHints`

### 5.4 CollectionExecutionSummary

建议包含：

1. `PlannedEvidenceCount`
2. `CollectedEvidenceCount`
3. `MissingRequiredEvidenceRefs`
4. `MissingOptionalEvidenceRefs`
5. `Warnings`
6. `CapabilityResults`

### 5.5 EvidenceCapabilityCatalog

`InvestigationPlan` 里的 ref 不是直接给 collector 执行，而是先映射到 capability。

每条 capability 至少定义：

1. `CapabilityId`
2. `EvidenceRef`
3. `Engine`
4. `CollectorKey`
5. `NormalizerKey`
6. `NormalizedUnit`
7. `VersionConstraint`
8. `FailureClassification`
9. `SourceScope`

职责边界：

1. skill 只声明“要什么证据”。
2. capability catalog 负责“谁能采、怎么归一化、失败时怎么分类”。
3. subworkflow 只编排 capability，不自己解释 skill ref。

## 6. workflow 主链调整

### 6.1 新主链

```text
InputValidation
  -> SkillResolver
  -> InvestigationPlanner
  -> EvidenceCollectionSubworkflow
  -> SkillPolicyGate
  -> DiagnosisAgent
  -> DiagnosisRuleEvaluator
  -> Grounding
  -> Review / Complete
```

### 6.2 SkillResolver

职责：

1. 根据 `engine + workflowType + version` 解析对应 bundle。
2. 从 bundle 中解析一份 `InvestigationSkillDefinition` 与一份 `DiagnosisSkillDefinition`。
3. 把 bundle/version 信息注入 workflow 上下文。
4. 对 skill 缺失、版本不匹配、bundle 不兼容给出明确失败原因。

bundle/version 解析策略需要提前定死：

1. `start`
   优先使用显式请求的 bundle/version；如果请求未指定，则按配置中的默认 bundle 策略解析。
2. `resume`
   必须复用 session / checkpoint 中已经持久化的 bundle/version，不允许重新解析最新默认值。
3. `recovery`
   必须复用 session / checkpoint 中已经持久化的 bundle/version，不允许漂移到新规则集。

这样做的目的只有一个：同一个 workflow 的 start、resume、recovery 必须使用同一套规则，结果才可复现、可审计。

### 6.3 InvestigationPlanner

职责：

1. 根据 investigation skill 生成采集计划。
2. 将 required/optional evidence refs 显式化。
3. 将 skill ref 绑定为 capability 执行单元，而不是只产出抽象字段名。

不负责：

1. 不直接执行 MCP tool。
2. 不输出 diagnosis 建议。

### 6.4 EvidenceCollectionSubworkflow

职责：

1. 按采集计划组织现有 collector 的执行顺序。
2. 产出 `DbConfigEvidencePack` 与 `CollectionExecutionSummary`。
3. 将 skill 计划与实际采集结果关联起来。
4. 输出可供 gate 求值的结构化执行结果，而不只是 count 和 warning。

原则：

1. 保持只读采集。
2. 不让 skill 直接绑定 SQL。
3. 引擎差异在 subworkflow 实现层吸收。

每个 capability result 至少保留：

1. 是否命中 collector
2. 是否采集成功
3. 是否归一化成功
4. 原始错误分类
5. 数据时效与来源质量

### 6.5 SkillPolicyGate

职责：

1. 校验 required evidence 是否齐全。
2. 计算 `pass / degraded / blocked` 三档结果。
3. 把缺口写入 `MissingContextItems`。
4. 把完整度写入 `CollectionMetadata`。
5. 执行 `BlockingRules`，而不只是检查“字段有没有拿到”。

建议语义：

1. `pass`
   必需证据齐全，正常继续。
2. `degraded`
   缺部分关键上下文，允许继续，但必须降低置信度或设置 `requiresMoreContext`。
3. `blocked`
   核心采集完全失败或连接前提不成立，直接终止。

`BlockingRules` 需要从“完整度检查”升级为“规则求值”，至少支持：

1. evidence 缺失
2. evidence 过期
3. unit 无法归一化
4. source 不可信
5. engine/version 不受支持
6. host context 不足以支撑目标值建议

### 6.6 DiagnosisAgent

职责：

1. 消费 `DiagnosisSkillDefinition`。
2. 基于 evidence pack、missing context、collection metadata 输出结构化建议。
3. 遵守 skill 规定的输出字段、禁止模式、置信度规则。

必须保证：

1. 每条 recommendation 都带 `evidenceReferences`。
2. 缺失 host context 时禁止输出具体目标值。
3. 禁止输出无证据结论。

### 6.7 DiagnosisRuleEvaluator

这一层专门把 diagnosis skill 从 prompt 约定变成运行时硬规则。

第一版至少硬校验：

1. 缺失 host context 时，禁止输出具体目标值。
2. low-confidence recommendation 必须显式 `requiresMoreContext = true`。
3. forbidden patterns 不能出现在 `actionable recommendation` 中。

### 6.8 recommendation 语义拆分

recommendation 语义要拆成两类，避免 degraded 场景混淆：

1. `actionable recommendation`
   必须引用正向 evidence，才能被视为可执行建议。
2. `request-more-context`
   允许只引用 missing-context，用于表达“还需要补什么信息”，不能伪装成执行建议。

这两类建议必须在 DTO、history 和前端展示上显式区分。

### 6.9 Grounding

第一版继续承担：

1. schema 校验。
2. evidence references 校验。
3. recommendation 与实时证据的一致性校验。

但要补一个明确前提：grounding 校验使用的是“归一化后的最终引用协议”，而不是 planner 原始 ref。

因此必须先完成一条唯一映射链：

`skill evidence ref -> capability ref -> evidence pack reference -> grounding reference`

如果不是 1:1，就必须明确由哪一层负责归一化。

## 7. 与现有领域模型的对齐

### 7.1 DbConfigEvidencePack

第一版继续复用当前模型，不新增并行大对象。

新增语义约定：

1. `EvidenceItems`
   存实时采集到的事实。
2. `MissingContextItems`
   存 skill gate 识别出的缺口。
3. `CollectionMetadata`
   至少补 `bundle_id`、`bundle_version`、`investigation_skill_id`、`investigation_skill_version`、`diagnosis_skill_id`、`diagnosis_skill_version`、`plan_id`、`collection_completeness`。

同时先写死一条语义边界：

1. `ObservedEvidence`
   当前实例实时采集到的证据。
2. `RetrievedKnowledge`
   未来 `RAG` 注入的外部知识。

第一版不一定立刻拆成新对象，但外部知识不能回填为 observed evidence。

### 7.2 DbConfigRecommendation

继续复用当前结构，但 diagnosis skill 和 rule evaluator 要统一约束：

1. `confidence`
2. `requiresMoreContext`
3. `recommendationClass`
4. `evidenceReferences`
5. `ruleId / ruleVersion`
6. `recommendationType`

## 8. MySQL 与 PostgreSQL 第一版最小证据集

### 8.1 跨引擎基础元证据

所有引擎第一版都必须补齐以下元证据，否则 diagnosis 很容易给出不可执行建议：

1. `engine.version`
2. `deployment.flavor`
3. `parameter.source`
4. `parameter.apply_scope`
5. `parameter.normalized_unit`

这些元证据不是只存在 planner 层的概念，它们必须贯穿整条链路：

1. 在 capability catalog 中定义其采集与归一化责任。
2. 在 evidence pack 中作为可追溯 evidence / metadata 落库。
3. 在 collection metadata 中保留汇总值，便于 history 与前端展示。
4. 在 diagnosis 输入中显式可见，供 recommendation 规则使用。

### 8.2 MySQL investigation skill

第一版最小必查：

1. `config.max_connections`
2. `config.innodb_buffer_pool_size`
3. `config.innodb_log_file_size` 或等价 redo 配置
4. `config.table_open_cache`
5. `metric.threads_connected`
6. `metric.max_used_connections`
7. `host.memory.total`
8. `host.memory.available`

### 8.3 PostgreSQL investigation skill

第一版最小必查：

1. `config.max_connections`
2. `config.shared_buffers`
3. `config.work_mem`
4. `config.effective_cache_size`
5. `config.maintenance_work_mem`
6. `metric.numbackends`
7. `host.memory.total`
8. `host.memory.available`

## 9. RAG 预留位

本轮只定义协议，不实现功能。预留点如下：

1. `InvestigationSkillDefinition.RetrievalHints`
2. `InvestigationPlan.RetrievalHints`
3. `DiagnosisSkillDefinition.CitationRules`
4. `DbConfigEvidencePack` 的外部知识扩展位

后续接入 `RAG` 时，插入位置定为：

```text
SkillPolicyGate
  -> FactRagExecutor
  -> CaseRagExecutor
  -> DiagnosisAgent
```

并明确一条硬边界：

1. `SkillPolicyGate` 只根据 observed evidence 判定完整度。
2. `FactRagExecutor / CaseRagExecutor` 不能补齐 required evidence。
3. `RAG` 只能作为 diagnosis 附加上下文。

## 10. 文档同步要求

本轮真正实施时，必须同步更新：

1. `docs/workflow/README.md`
2. `docs/workflow/CLAUDE.md`
3. `docs/workflow/skills/CLAUDE.md`
4. 仓库根 `CLAUDE.md`
5. `docs/README.md`

## 11. 必要预研

在正式实现前，先做以下小型预研，避免直接写错主链：

1. `SKILL.md contract spike`
   验证 front matter + 固定章节的解析策略。
2. `Evidence ref normalization spike`
   验证 skill ref、capability、grounding ref 的映射是否稳定。
3. `No-op node insertion spike`
   在 diagnosis 前插一个空节点，验证 history / progress / resume / recovery。
4. `Degraded semantics spike`
   验证 actionable recommendation 与 request-more-context 的分流。
5. `Diagnosis rule evaluator spike`
   先验证两条最关键的硬规则能否稳定执行。

### 11.1 SKILL.md contract spike 结论

本轮已经完成 `SKILL.md contract spike`，结论如下：

1. 第一版不引入额外 YAML / Markdown 解析依赖，使用受限 front matter + 固定 `##` 章节的手写解析器即可满足需求。
2. spike 放在 `AIDbOptimize.Infrastructure.Tests` 最合适，采用现有 “纯内存输入 + xUnit + 直接断言结构化结果或异常” 的测试风格。
3. parser 必须强制校验结构化元数据，最少包含：
   `skill_id`、`skill_type`、`engine`、`skill_version`、`schema_version`、`bundle_id`、`bundle_version`。
4. parser 只依赖固定二级标题名称，不依赖 section 顺序。
5. section 内允许保留自由文本说明，但这些说明不参与机器语义判断。
6. 元数据缺失、front matter 非法、缺少必需 section 时必须立即失败，不能默默降级。

### 11.2 Evidence ref normalization spike 结论

本轮已经完成 `Evidence ref normalization spike`，结论如下：

1. 第一版必须显式区分 3 套名字：
   `skill reference`、`capability id`、`grounding/evidence reference`。
2. `skill reference -> grounding reference` 不能靠运行时猜测，必须经过显式归一化。
3. 同一个 `skill reference` 映射到多个 capability 时，必须立即失败，不能静默选择其一。
4. required skill reference 找不到 capability 映射时，必须立即失败。
5. missing-context reference 可以参与归一化，但必须带显式标记，不能和正向 evidence 混为同一语义。

### 11.3 No-op node insertion spike 结论

本轮已经完成 `No-op node insertion spike`，结论如下：

1. 在 diagnosis 前插入 pass-through 节点是可行的，不需要先重构整条 runtime。
2. 最小插入点应放在 `DbConfigRuleAnalysisExecutor` 与 `DbConfigDiagnosisAgentExecutor` 之间。
3. 新节点必须同时更新 3 处：
   graph 边、runtime handler、`WorkflowProgressCalculator`。
4. 只要节点继续走统一的 node execution / workflow events / state update 模式，history 与 recovery 语义可以保持稳定。

### 11.4 Degraded semantics spike 结论

本轮已经完成 `Degraded semantics spike`，结论如下：

1. `degraded` 场景下必须显式区分两类 recommendation：
   `actionable recommendation` 与 `request-more-context`。
2. 只要 recommendation 引用了至少一个正向 evidence ref，就可以归类为 actionable。
3. 如果 recommendation 只引用 missing-context ref，则必须归类为 request-more-context，不能伪装成执行建议。
4. recommendation 引用无法解析时必须立即失败，不能默默降级。

### 11.5 Diagnosis rule evaluator spike 结论

本轮已经完成 `Diagnosis rule evaluator spike`，结论如下：

1. 缺少 host context 时，diagnosis 阶段必须显式阻断具体目标值建议。
2. `confidence = low` 的 recommendation 必须同时设置 `requiresMoreContext = true`。
3. 这两条规则可以先在独立 evaluator 中验证，不必一开始就接入 runtime 主链。
4. 后续正式实现时，`DiagnosisRuleEvaluator` 应该消费结构化 recommendation，而不是直接操作原始文本 prompt。

## 12. 自审结论

本方案满足以下约束：

1. 先规范排查，再生成建议，职责边界清晰。
2. skill 保持标准 `SKILL.md` 形态，但机器契约也被明确下来。
3. diagnosis agent 也受 skill 约束，而且有运行时硬规则兜底。
4. `RAG` 被明确延后，同时其边界提前定死，不会回写完整度语义。

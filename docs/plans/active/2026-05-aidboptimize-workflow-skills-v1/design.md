# AIDbOptimize workflow skills v1 方案

## 1. 目标

在现有 `DbConfigOptimization` workflow 基线之上，为数据库配置排查增加第一版 `skills` 能力，使 workflow 先按规范排查，再基于证据生成建议，而不是先固定采集、后由 agent 自由发挥。

本轮目标只覆盖：

1. 用标准 `SKILL.md` 规范 MySQL 与 PostgreSQL 的排查规则与输出规则。
2. 让 skill 真正驱动 workflow 的前半段采集规划与后半段诊断输出。
3. 为后续 `RAG` 预留协议与扩展点，但本轮不接真实知识库或案例库。

## 2. 边界

本轮做：

1. 新增 workflow skills 的规划文档与执行任务清单。
2. 设计 `investigation skill` 与 `diagnosis skill` 两类标准 skill。
3. 设计 `SkillResolver -> InvestigationPlanner -> EvidenceCollectionSubworkflow -> SkillPolicyGate -> DiagnosisAgent -> DiagnosisRuleEvaluator` 主链。
4. 设计 skill 对现有 `DbConfigEvidencePack / MissingContextItems / CollectionMetadata` 的映射方式。
5. 预留 `RAG` 所需的 retrieval hints 与 citation rules 协议字段。

本轮不做：

1. 不接真实 `Fact RAG`。
2. 不接真实 `Case RAG`。
3. 不做向量库、文档切片、召回排序。
4. 不扩展到 SQL 调优、索引设计、慢查询分析等更大主题。
5. 不把 skill 设计成可执行脚本或直接绑定 SQL。

## 3. 总体结论

### 3.1 设计原则

1. `skill` 定义规范，不定义具体 SQL 或 MCP tool 名称。
2. 采集规范与输出规范分离，避免一个大 skill 同时承载两种职责。
3. 缺失上下文时允许 workflow 继续，但必须降级，不能伪造确定性结论。
4. 所有建议都必须可追溯到实时证据；后续知识引用也必须显式可追溯。
5. `SKILL.md` 必须既能给人读，也必须能被机器稳定解析，不能只靠章节标题碰运气。
6. 逻辑 evidence ref、collector capability、最终 grounding 引用必须收敛到同一套可归一化语义。

### 3.2 skill 形态

统一采用仓库内标准 `SKILL.md` 目录结构，但真正解析单元不是“单个 skill 文件”，而是一个 `skill bundle/profile`。bundle 负责把 investigation skill 与 diagnosis skill 绑定成一套兼容组合，避免后续版本混搭。

第一版至少覆盖 4 个 skill：

1. `mysql-investigation`
2. `mysql-diagnosis`
3. `postgresql-investigation`
4. `postgresql-diagnosis`

### 3.3 workflow 形态

workflow 主链从当前的“固定采集 + diagnosis + grounding”演进为：

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

为避免 planner 输出变成“半结构化愿望清单”，主链中需要显式引入一层 `EvidenceCapabilityCatalog` 语义，用于把 skill 中的逻辑 ref 映射到实际 collector、normalizer、unit 和失败语义。

### 3.4 RAG 预留策略

本轮只定义：

1. `Retrieval Hints`
2. `Citation Rules`
3. `externalKnowledge` 相关预留语义

但不实现真实召回与知识注入。

同时提前定死一条边界：后续 `Fact RAG / Case RAG` 只能作为 diagnosis 附加上下文，不能补齐 `required evidence`，也不能改变 `pass / degraded / blocked` 的 gate 判定。

## 4. 核心收益

1. 排查规范从 prompt 提示升级为 workflow 控制面规则。
2. diagnosis agent 不再决定“该查什么”，只负责基于结构化证据出建议。
3. MySQL 与 PostgreSQL 的差异收敛到 skill 与 subworkflow，而不是散落在 diagnosis prompt 和分支里。
4. 后续接入 `RAG` 时，只需在 skill gate 后插入检索层，不用推翻主链。
5. 通过 skill bundle、capability catalog 和统一引用协议，把实现风险前移到可预研、可测试的契约层。

## 5. 交付物

本轮文档交付拆成三层：

1. `design.md`
   说明目标、边界与总体方向。
2. `detailed-design.md`
   说明 skill 协议、类职责、数据模型、subworkflow 与降级语义。
3. `tasks.md`
   拆成可直接进入实施排期的任务清单。

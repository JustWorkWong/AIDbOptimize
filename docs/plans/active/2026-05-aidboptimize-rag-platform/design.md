# AIDbOptimize RAG 平台接入方案

## 1. 目标

在现有 `DbConfigOptimization` workflow skills v1 基线之上，接入一套统一 `RAG` 平台，用于为 diagnosis 阶段补充权威事实型上下文，同时为后续案例型检索保留稳定扩展位。

本轮目标只覆盖：

1. 设计统一 `RAG` 平台，而不是只做单点 prompt 拼接。
2. 首批接入 `MySQL / PostgreSQL 官方文档 + 云厂商数据库文档`。
3. 提供一个最小预下载方法，把首批 seed 文档拉取到 `docs/` 目录，后续再允许人工补充文档。
4. 让 `RAG` 的索引、检索元数据与 retrieval snapshot 默认落在控制面 PostgreSQL。
5. 让 workflow 在 `SkillPolicyGate` 之后、`DiagnosisAgent` 之前通过 MAF 提供的上下文注入机制消费 `RAG` 结果。
6. 让模型适配层默认对接阿里百炼，并保持供应商可替换。
7. 让历史优化记录形成独立的案例知识流，并与事实文档流分层。
8. 让 PostgreSQL 实体、EF Core migration、后端主流程与前端范围在方案阶段就提前定型。

## 2. 边界

本轮做：

1. 设计 `Corpus -> Preprocess -> Index -> Retrieval -> Workflow Adapter` 五段式平台结构。
2. 设计首批 seed 文档的预下载方法，以及后续人工补充文档的目录、命名、清洗、切片、索引与检索契约。
3. 设计事实文档流与历史优化案例流两条知识通道。
4. 设计 `RAG` 结果在 PostgreSQL + workflow 中的持久化语义、注入方式与 citation 约束。
5. 设计基于 MAF `AIContextProvider / VectorStore` 抽象的接入方式，而不是重复发明一套 `RAG` 方法层。
6. 设计阿里百炼在 `embedding / generation` 适配层中的职责边界。
7. 输出可直接进入实施的 `design.md / detailed-design.md / tasks.md`。

本轮不做：

1. 不实现复杂爬虫或全站抓取器，只实现最小 seed 文档预下载方法。
2. 不先拆独立知识服务，不引入新微服务部署面。
3. 不把“后续人工怎么持续搜集文档”写成程序职责；程序只负责首批 seed 文档预下载和后续处理。
4. 不让 `RAG` 结果参与 `required evidence` 判定。
5. 不绕开 MAF 单独设计一套并行检索主流程。
6. 不把博客、社区帖子、issue、论坛内容纳入第一批事实语料。

## 3. 总体结论

### 3.1 架构原则

1. `RAG` 是独立平台能力，但 workflow 注入方式优先复用 MAF 现有抽象，不再平行发明另一套 agent 检索方法。
2. 原始文档、清洗结果、索引结果职责分离，避免后续难以追溯。
3. `ObservedEvidence` 与 `RetrievedKnowledge` 必须分开建模，不能混成同一种证据。
4. 同一 session 的 retrieval 结果必须可持久化、可恢复、可回放，不能在 resume / recovery 时悄悄漂移到另一批文档结果。
5. 索引、chunk metadata、retrieval snapshot 默认进入控制面 PostgreSQL；`docs/` 保留原始语料和清洗产物。
6. workflow 只消费 `RAG` 输出，不控制语料生命周期；语料生命周期由平台负责。
7. 事实文档知识与历史优化案例必须分层建模、分层召回，但共用统一 retrieval 出口。
8. 供应商模型能力可以替换，但语料与检索契约必须由仓库自己掌控。

### 3.2 workflow 插入位置

统一插入位置固定为：

```text
InvestigationPlanner
  -> EvidenceCollectionSubworkflow
  -> SkillPolicyGate
  -> WorkflowRagContextAssembler
  -> DbConfigDiagnosisAgentExecutor
  -> DbConfigGroundingExecutor
```

这条边界不允许改成：

```text
RAG
  -> 回填 required evidence
  -> 改写 SkillPolicyGate
```

也就是说：

1. gate 只对实时采集到的 observed evidence 负责。
2. `RAG` 只能补 diagnosis 上下文，不补采集缺口，不改变 `pass / degraded / blocked`。

### 3.3 平台形态

统一 `RAG` 平台先作为当前仓库内能力落地，不先拆独立服务。平台分成 5 层：

1. `Corpus`
   管理人工整理后的原始文档、案例原文和命名规则。
2. `Preprocess`
   管理正文提取、清洗和 chunk 切分。
3. `Index`
   管理 embedding、chunk metadata 和 PostgreSQL 向量索引。
4. `MAF Context`
   优先复用 MAF / `Microsoft.Extensions.AI` 当前版本可直接提供的上下文与向量检索抽象；`AIContextProvider` 是否存在待预研验证，如不存在则改由仓库内适配层直接承接 query、filter、召回与上下文注入。
5. `Workflow Adapter`
   管理 `RetrievalHints -> MAF Context Scope -> Diagnosis Context` 的转换。

### 3.4 语料范围

第一阶段事实型语料只覆盖：

1. `MySQL` 官方文档
2. `PostgreSQL` 官方文档
3. `阿里云 RDS` 相关参数与运维文档
4. `AWS RDS`
5. `Azure Database`
6. `GCP Cloud SQL`

选择这组来源的原因只有两个：

1. 权威性高，适合支撑事实型 citation。
2. 与当前数据库配置优化 workflow 的问题空间直接相关。

首批文档获取方式分成两段：

1. 程序提供一个最小预下载方法，把固定 seed URL 下载到 `docs/rag/facts/...`。
2. 后续新增文档允许人工直接放入相应目录，不要求继续走程序化下载流程。

### 3.5 历史优化记录流

历史优化记录不能只作为“以后再说”的概念，需要从第一版开始留正式入口。

统一原则：

1. 历史优化记录作为 `case knowledge`，不与事实文档混在同一来源类型下。
2. 案例来源优先取当前系统内已经完成并可审计的 workflow 结果，而不是外部零散笔记。
3. 只有满足质量门槛的历史记录才进入 case 索引，判断基于以下字段：
   - `workflow_sessions.Status = Completed`
   - `ResultPayloadJson` 包含非空 recommendation
   - `workflow_review_tasks.Decision != Rejected`，或 review task 不存在（即无否决）
   - `ResultPayloadJson / InputPayloadJson` 中可提取出 `engine / problem_type / recommendation_type`
4. case knowledge 只作为 diagnosis 附加参考，不参与 gate。

这里的“历史优化记录”默认就是从当前控制面 PostgreSQL 读取并投影，不是另外维护一套外部案例库。

### 3.6 阿里百炼职责

阿里百炼只承担模型能力，不承担语料资产管理。

默认职责如下：

1. `Embedding`
   为清洗后的 chunk 生成向量。
2. `Generation`
   继续作为 diagnosis agent 的底层模型来源。

模型接入必须通过 MAF / `Microsoft.Extensions.AI` 兼容适配层完成，不能把供应商 SDK 直接散落到 retrieval、workflow 和语料模块里。

### 3.7 版本管理与仓库体积控制

文档语料必须先落在 `docs/` 下，但这不等于所有原始语料和快照都应直接进入 git。

统一原则如下：

1. 目录说明、命名规则、少量示例数据进入版本控制。
2. 首批 seed 文档由最小预下载方法拉取到 `docs/rag/`；后续人工补充文档直接按文件名和类型放入对应目录。
3. 大体积原始语料、prepared chunks 是否入 git 由目录边界单独控制，但检索主存储仍然是 PostgreSQL。
4. 真正参与检索的 chunk metadata、embedding 和 retrieval snapshot 默认进入 PostgreSQL，不以平面文件作为主存储。
5. 如果后续需要沉淀可复现的最小测试样本，只单独挑选小样本入库，不把整批原始语料都提交。

这样既满足“文档数据放到 `docs/` 下”，也避免仓库体积和 diff 噪声失控。

## 4. 核心收益

1. `RAG` 接入后，diagnosis 可以引用权威参数说明、厂商约束与运行建议，而不是只依赖实时观测。
2. 语料先落 `docs/`，后续审计、追溯、重建索引都更稳定。
3. workflow 与知识平台解耦，后续可接 `Case RAG` 而不推翻当前事实检索基线。
4. 阿里百炼只作为模型提供方接入，平台结构不会被供应商绑定死。
5. 通过人工整理目录、类型命名和案例准入规则，把知识资产边界前移到可审查、可测试的资产层。

## 5. 交付物

本轮规划文档交付拆成三层：

1. `design.md`
   说明目标、边界、总体结构和接入原则。
2. `detailed-design.md`
   说明目录、模块职责、数据契约、workflow 注入点和模型适配策略。
3. `tasks.md`
   拆成可直接进入实施的任务清单。

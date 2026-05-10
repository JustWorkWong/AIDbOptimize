# AIDbOptimize RAG 平台接入详细方案

## 1. 当前事实与缺口

### 1.1 当前可复用基线

当前仓库已经具备以下可直接复用能力：

1. `DbConfigOptimization` workflow 主链、review、resume、recovery、SSE、history。
2. `InvestigationPlanner` 已输出 `RetrievalHints`。
3. `SkillPolicyGate` 已经稳定固化 `pass / degraded / blocked` 三档语义。
4. `DbConfigDiagnosisAgentExecutor` 已经是 diagnosis prompt 的统一输入出口，并已形成基于 `Microsoft.Extensions.AI` / `ChatClient` 的现有 generation 接入路径。
5. MAF `AIContextProvider` 是否可在当前版本直接复用仍待验证，现阶段只能把它视为候选抽象，而不是既有稳定基线。
6. `DbConfigEvidencePack` 已经保留 `ExternalKnowledgeItems` 扩展位。
7. `docs/workflow/README.md` 已经明确 `RAG` 只能插在 gate 后、diagnosis 前。

### 1.2 当前缺口

当前仓库仍然缺少：

1. 统一的语料目录、最小预下载方法与命名规则。
2. 原始文档、清洗结果、PostgreSQL 索引结果的生命周期管理。
3. `RetrievalHints -> MAF Context Query -> RetrievedKnowledge` 的稳定转换层。
4. 基于 MAF 可用抽象或 `Microsoft.Extensions.AI` 直接适配层的统一接入方式。
5. 阿里百炼在 embedding / generation 两处的适配边界。
6. 用于 workflow 注入的事实型 citation 契约。
7. 历史优化记录进入 case knowledge 的筛选、清洗与索引流程。
8. PostgreSQL 实体、EF Core migration、后端服务和前端展示边界。

## 2. 目录与文档结构

### 2.1 规划文档目录

本轮规划文档保存在：

```text
docs/plans/active/2026-05-aidboptimize-rag-platform/
  design.md
  detailed-design.md
  tasks.md
```

### 2.2 语料目录

正式实施后，文档语料先落在：

```text
docs/rag/
  CLAUDE.md
  README.md
  facts/
    mysql/
    postgresql/
    cloud/
  cases/
    mysql/
    postgresql/
  prepared/
    facts/
    cases/
```

目录边界必须保持：

1. `CLAUDE.md`
   说明 `docs/rag/` 的目录边界、哪些内容进入版本控制、哪些内容仅本地保留。
2. `facts/`
   保存人工整理的事实型原始文档，按数据库类型和文件名区分。
3. `cases/`
   保存人工整理或程序导出的历史优化案例原文，按数据库类型和文件名区分。
4. `prepared/`
   保存清洗后的 chunk 与 metadata，不回写原始内容。

### 2.3 首批文档获取方式

第一版不是“纯人工整理”，也不是“复杂抓站器”，而是两段式：

1. `SeedPreloadCommand`
   程序内置一小批固定 seed URL，负责把首批官方文档和云厂商文档下载到 `docs/rag/facts/...`。
2. `ManualDropIn`
   后续新增文档允许人工直接放入 `docs/rag/facts/...` 或 `docs/rag/cases/...`，再走统一预处理。

边界要求：

1. `SeedPreloadCommand` 只支持固定 URL 清单，不支持站点递归抓取。
2. 下载结果直接按命名规则落文件，不额外维护复杂来源树。
3. 人工补充文档与程序预下载文档必须进入同一套目录和后续预处理链路。

### 2.4 版本控制策略

`docs/rag/` 放在仓库内，但不是所有内容都直接纳入版本控制。

默认规则：

1. 提交到 git：
   `docs/rag/README.md`
   `docs/rag/CLAUDE.md`
   小体积样例数据与测试夹具
2. 默认通过 `.gitignore` 排除：
   `docs/rag/prepared/**`
   本地缓存产物
3. PostgreSQL 中的向量索引、chunk metadata、retrieval snapshot 是主检索存储，不以 `docs/` 平面文件作为主索引存储。
4. 如需保留可复现样本，只保留最小样本，不提交整批原始语料。

## 3. 代码模块

建议新增：

```text
src/AIDbOptimize.Infrastructure/
  Rag/
    Corpus/
      RagSourceDocument.cs
      RagDocumentType.cs
      RagCorpusFileNamer.cs
      SeedPreloadDocumentCatalog.cs
    Preprocess/
      CorpusPreprocessor.cs
      CorpusChunker.cs
    Storage/
      RagDocumentEntity.cs
      RagDocumentChunkEntity.cs
      RagCaseRecordEntity.cs
      RagCaseEvidenceLinkEntity.cs
      RagRetrievalSnapshotEntity.cs
      RagDocumentChunkRepository.cs
      RagCaseRecordRepository.cs
      RagRetrievalSnapshotRepository.cs
      PostgreSqlVectorStoreFactory.cs
    Maff/
      WorkflowDocumentContextProvider.cs
      WorkflowRagContextScope.cs
      RagCitationFormatter.cs
    Workflow/
      WorkflowRagContextAssembler.cs
      RetrievedKnowledgeItem.cs
      HistoricalOptimizationCaseProjector.cs
      RagKnowledgeQueryService.cs
      RagKnowledgeIngestionService.cs
```

新增上述目录与文件时，必须同步更新：

1. `src/AIDbOptimize.Infrastructure/CLAUDE.md`
2. `docs/rag/CLAUDE.md`
3. `docs/workflow/CLAUDE.md`

### 3.1 Corpus 层

职责：

1. 定义事实文档与案例文档的目录边界。
2. 定义文件命名规则、类型规则和最小 metadata 规则。
3. 管理程序预下载和人工补充进仓库的原始文档资产。
4. 维护首批 seed 文档清单。

不负责：

1. 不做复杂站点抓取。
2. 不做 embedding。
3. 不做 workflow 拼接。

### 3.2 Preprocess 层

职责：

1. 读取程序预下载或人工补充的原始文档。
2. 提取正文、移除导航和页脚噪声。
3. 按章节切分 chunk。
4. 对历史优化案例做结构化抽取。

不负责：

1. 不做模型调用。
2. 不做 query 召回。
3. 不直接感知 workflow skill。

### 3.3 Storage / MAF Context 层

职责：

1. 将 chunk metadata、embedding 和检索记录落 PostgreSQL。
2. 通过 `VectorStore / VectorStoreCollection` 暴露统一向量检索入口。
3. 优先通过当前 MAF 版本可直接提供的上下文抽象暴露 workflow 可消费的检索上下文；若 `AIContextProvider` 不存在，则由仓库内适配层直接承接该职责。

设计约束：

1. 不单独设计一套并行 `IRagRetriever` 主流程。
2. workflow 侧只依赖 MAF / `Microsoft.Extensions.AI` 可用抽象或仓库内适配层，不直接拼供应商 SDK。
3. PostgreSQL 是主检索存储，`docs/` 只保留原始语料和 prepared 产物。

### 3.4 PostgreSQL 实体与 EF Core 边界

这部分必须在实施前提前定型，避免后面边做边长。

建议新增控制面实体：

1. `RagDocumentEntity`
   保存原始文档记录。
   核心字段：
   - `Id`
   - `DocumentType` (`fact` / `case`)
   - `Engine`
   - `Vendor`
   - `Topic`
   - `SourcePath`
   - `SourceUrl`
   - `ContentHash`
   - `CapturedAt`
   - `IsActive`
2. `RagDocumentChunkEntity`
   保存 chunk 文本、metadata 和向量。
   核心字段：
   - `Id`
   - `DocumentId`
   - `ChunkKey`
   - `Title`
   - `SectionPath`
   - `Text`
   - `ParameterNamesJson`
   - `KeywordsJson`
   - `Embedding`
   - `ProductVersion`
   - `AppliesTo`
3. `RagCaseRecordEntity`
   保存由 workflow 投影出来的案例记录。
   核心字段：
   - `Id`
   - `WorkflowSessionId`
   - `Engine`
   - `ProblemType`
   - `Outcome`
   - `ReviewStatus`
   - `RecommendationType`
   - `Summary`
   - `CreatedAt`
4. `RagCaseEvidenceLinkEntity`
   保存案例与 evidence / recommendation 的引用关系。
5. `RagRetrievalSnapshotEntity`
   保存某次 workflow 注入时实际用到的 knowledge snapshot。
   核心字段：
   - `Id`
   - `WorkflowSessionId`
   - `NodeExecutionId`
   - `SnapshotTypeJson`
   - `RetrievedItemsJson`
   - `CreatedAt`

实现约束：

1. 所有实体都走 EF Core。
2. migration 必须和控制面 schema 一起提交。
3. 不允许把 RAG 主数据只藏在 JSON blob 里；聚合根和查询主键要有显式表结构。
4. 当前 AppHost 需要将 PostgreSQL 容器镜像切换到 `pgvector/pgvector:pg17`（或等价版本），并在实施前验证 `CREATE EXTENSION vector` 可以成功执行。

### 3.5 后端主体逻辑

后端主流程也要在方案阶段写死，避免执行时各写各的。

建议服务边界：

1. `RagKnowledgeIngestionService`
   负责从 `docs/rag/facts` 和 `docs/rag/cases` 读取原始文件，执行预处理并写入 `RagDocumentEntity / RagDocumentChunkEntity`。
2. `HistoricalOptimizationCaseProjector`
   负责从控制面 PostgreSQL 中读取：
   - `workflow_sessions`
   - `workflow_review_tasks`
   - `workflow_node_executions`
   - 必要的 recommendation / evidence payload
   然后筛选满足质量门槛的记录，投影为 `RagCaseRecordEntity` 和案例 chunk。
3. `RagKnowledgeQueryService`
   负责组合 facts / cases 查询、metadata filter、top-k 和融合规则。
4. `WorkflowDocumentContextProvider`
   负责把 query 结果按 MAF context provider 注入 workflow diagnosis。
5. `RagRetrievalSnapshotRepository`
   负责 retrieval snapshot 的持久化与回放。

### 3.6 前端范围

前端不应缺席，至少要把可观测性和审计入口补上。

第一版前端范围建议只做 3 块：

1. `RAG 资产状态面板`
   展示 facts / cases 文档数量、最后一次入库时间、最后一次索引时间。
   依赖新增 API：`GET /api/rag/assets/status`，返回 facts / cases 文档数、最后入库时间、最后索引时间。
2. `workflow external knowledge 展示`
   在 workflow 结果 / history detail 中展示：
   - fact citation
   - case citation
   - 来源 URL / session id
   复用现有 `GET /api/history/{sessionId}`，在返回结构中补充 `externalKnowledgeItems` 字段。
3. `case audit 视图`
   展示哪些历史 workflow 被投影成 case knowledge，以及其 review / outcome 状态。
   依赖新增 API：`GET /api/rag/cases/audit`，返回已投影的 case record 列表及其 review / outcome 状态。

不在第一版做：

1. 在线编辑文档内容
2. 在线改写 case 规则
3. 通用知识库后台

### 3.7 Workflow 层

职责：

1. 消费 `InvestigationPlan.RetrievalHints`。
2. 构造 workflow 级的 context scope 与 filter。
3. 调用 MAF context provider 取回 retrieved knowledge。
4. 将 knowledge items 注入 diagnosis 输入。

不负责：

1. 不改变 `MissingContextItems`。
2. 不改变 `CollectionMetadata` 中 gate 相关字段。
3. 不把 retrieved knowledge 伪装成 observed evidence。

## 4. 语料目录与命名契约

### 4.1 文件命名规则

第一版不做复杂下载流程，只做最小 seed 预下载和统一文件命名契约。

建议命名格式：

1. 事实文档：
   `<engine>__<vendor>__<topic>__<short-title>.md`
2. 历史案例：
   `<engine>__case__<problem-type>__<session-or-ticket>.md`

最少要能从路径 + 文件名推断：

1. `engine`
2. `source_type`
3. `vendor`
4. `topic` 或 `problem_type`

### 4.2 Seed 预下载方法

程序提供一个最小预下载方法，职责固定为：

1. 维护少量固定 seed URL 清单。
2. 下载对应正文或 markdown 页面。
3. 按命名规则落到：
   - `docs/rag/facts/mysql/`
   - `docs/rag/facts/postgresql/`
   - `docs/rag/facts/cloud/`
4. 为每个下载文件补齐最少 metadata 与来源 URL。

首批 seed URL 示例清单如下：

1. `MySQL`
   `https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html`
2. `PostgreSQL`
   `https://www.postgresql.org/docs/current/runtime-config-resource.html`
3. `阿里云`
   `https://help.aliyun.com/zh/rds/apsaradb-rds-for-mysql/use-parameters-to-tune-an-apsaradb-rds-for-mysql-instance`
4. `AWS RDS MySQL`
   `https://docs.aws.amazon.com/AmazonRDS/latest/UserGuide/Appendix.MySQL.Parameters.html`
5. `Azure MySQL`
   `https://learn.microsoft.com/azure/mysql/flexible-server/concepts-server-parameters`
6. `GCP Cloud SQL MySQL`
   `https://cloud.google.com/sql/docs/mysql/flags`

以上只是最小示例集，实施时按同样格式继续扩充。

明确不做：

1. 不支持递归抓站。
2. 不支持动态发现新页面。
3. 不支持自动增量同步整个文档站。

### 4.3 文件类型规则

必须遵守：

1. `facts/` 下只放权威事实型文档。
2. `cases/` 下只放历史优化记录或由程序导出的结构化案例。
3. 每个文件都要保留来源 URL 或来源说明。
4. 每个文件都要能补齐最少 metadata。

明确不允许：

1. 混放 facts 和 cases。
2. 只靠人工记忆判断文件类型，不在路径和文件名上表达。
3. 因清洗失败而覆盖或丢弃原始文档。

### 4.4 来源范围

第一版事实型来源固定为：

1. `mysql.com` / `dev.mysql.com`
2. `postgresql.org`
3. `aliyun.com` 下 RDS / PolarDB 相关数据库参数与运维文档
4. `aws.amazon.com` 下 RDS PostgreSQL / MySQL 相关文档
5. `learn.microsoft.com` 下 Azure Database 相关文档
6. `cloud.google.com` 下 Cloud SQL 相关文档

### 4.5 历史优化案例来源

历史优化案例优先来自当前系统内部已完成 workflow，而不是仓库外零散文档。

建议来源：

1. `workflow_sessions.ResultPayloadJson`
2. `workflow_sessions.InputPayloadJson`
3. `workflow_node_executions`
4. `workflow_review_tasks`
5. 必要时补充人工整理的结案总结文档到 `docs/rag/cases/`

案例入库前至少满足：

1. `workflow_sessions.Status = Completed`
2. `ResultPayloadJson` 包含非空 recommendation
3. `workflow_review_tasks.Status = Approved`，或 review task 不存在，即无否决
4. `ResultPayloadJson / InputPayloadJson` 中可提取出 `engine / problem_type / recommendation_type`

默认读取来源就是控制面 PostgreSQL 中的 workflow 表与持久化 payload，而不是文件系统扫描。

## 5. chunk 与 metadata 契约

### 5.1 chunk 原则

第一版以“章节切分优先”而不是“固定 token 粗切”。

切分原则：

1. 小章节单 chunk。
2. 大章节按二级小标题或语义段落组切分。
3. 保留少量 overlap，但不能让 chunk 大量重复。
4. chunk 必须保留原文标题路径，方便 citation。
5. 目标 chunk 大小为 400-800 token，按 `tiktoken` 或 `SharpToken` 估算。
6. overlap 控制在 50-100 token。
7. 单章节超过 1200 token 时，按二级小标题强制分割。
8. 小于 100 token 的短节与上一节合并。

### 5.2 chunk metadata 必填字段

每个 chunk 至少包含：

1. `chunk_id`
2. `document_id`
3. `source_id`
4. `source_type`
5. `engine`
6. `vendor`
7. `topic`
8. `title`
9. `section_path`
10. `url`
11. `captured_at`
12. `content_hash`
13. `product_version`
14. `applies_to`
15. `parameter_names`
16. `keywords`

这些字段最终除了写入 prepared metadata 外，还需要映射到 PostgreSQL 中的 chunk 记录与向量检索 filter 字段。
对于案例文档，还需要额外补齐：

1. `problem_type`
2. `outcome`
3. `session_id` 或 `case_id`
4. `review_status`

### 5.3 topic 受控集合

第一版先限制为：

1. `memory`
2. `connections`
3. `cache`
4. `checkpoint`
5. `logging`
6. `replication`
7. `storage`
8. `timeout`
9. `locking`
10. `observability`

这样做的目的是：

1. 让 `RetrievalHints` 能落到稳定 filter。
2. 避免只靠语义相似度在全库乱搜。

## 6. workflow 注入设计

本节默认优先使用当前 MAF 版本可直接提供的抽象。`AIContextProvider` 是否存在待通过预研确认；如不存在，则由 `WorkflowDocumentContextProvider` 直接基于 `Microsoft.Extensions.AI` 实现等价适配层。

### 6.1 插入位置

统一插入位置固定为：

```text
InvestigationPlanner
  -> EvidenceCollectionSubworkflow
  -> SkillPolicyGate
  -> WorkflowRagContextAssembler
  -> DbConfigDiagnosisAgentExecutor
  -> DbConfigGroundingExecutor
```

### 6.2 Query 输入

`WorkflowRagContextAssembler` 至少消费：

1. `engine`
2. `bundleId / bundleVersion`
3. `investigationSkillId / diagnosisSkillId`
4. `RetrievalHints`
5. `MissingContextItems`
6. `CollectionMetadata` 中的基础元证据
7. 用户提交的优化目标
8. facts / cases 的召回配额策略

`RetrievalHints` 当前由 `InvestigationPlanner` 输出，契约结构在本方案中锁定为包含 `topics`（对应 topic 受控集合）、`parameter_names`（相关参数名）、`keywords`。如果实际字段名与此不符，实施时以代码为准，但结构语义以此处为准。

### 6.3 Query 输出

统一返回 `RetrievedKnowledgeItem[]`，每条至少包含：

1. `knowledge_id`
2. `source_id`
3. `source_type`
4. `title`
5. `summary`
6. `snippet`
7. `url`
8. `section_path`
9. `topic`
10. `parameter_names`
11. `retrieval_score`
12. `rerank_score`
13. `captured_at`
14. `content_hash`
15. `snapshot_id`
16. `knowledge_kind` (`fact` / `case`)
17. `citation`

### 6.4 EvidencePack 语义

第一版不新增并行大对象，但必须明确两种语义分层：

1. `ObservedEvidence`
   当前实例实时采集到的事实。
2. `RetrievedKnowledge`
   外部文档或厂商知识片段。

实现要求：

1. `RetrievedKnowledge` 只能进入 `ExternalKnowledgeItems`。
2. 不允许把 `RetrievedKnowledge` 回填成 `EvidenceItems`。
3. `Grounding` 校验时，事实证据与外部知识引用必须分开处理。

### 6.5 facts 与 cases 融合策略

统一出口仍然是 `RetrievedKnowledgeItem[]`，但内部必须分两条召回通道：

1. `fact retrieval`
   面向官方文档和云厂商文档，解决“参数是什么、限制是什么、版本差异是什么”。
2. `case retrieval`
   面向历史优化记录，解决“类似问题以前怎么处理、在什么前提下有效”。

融合规则：

1. facts 优先回答约束和定义。
2. cases 只补“相似场景经验”，不能覆盖事实约束。
3. 当 facts 与 cases 冲突时，以 facts 为准。
4. citation 输出必须标明是 `fact` 还是 `case`。

### 6.6 持久化与恢复语义

这一层必须先设计清楚，否则 resume / recovery / history 会失真。

统一规则：

1. `start` 阶段首次完成 retrieval 后，retrieval snapshot 唯一写入 `RagRetrievalSnapshotEntity`，并通过 `NodeExecutionId` 关联到对应 `workflow_node_executions`。
2. `workflow_sessions.StateJson` 和 `workflow_checkpoints.SnapshotJson` 只存 `snapshot_id` 引用，不重复写完整 JSON。
3. `resume / recovery` 默认复用已持久化的 retrieval snapshot，不重新查“最新”文档。
4. 只有新的 workflow session 或显式“refresh knowledge”动作，才允许重新检索。
5. `history detail` 必须能展示当时实际注入的 knowledge items，而不是事后重新查出来的一批近似结果。

这样可以保证：

1. 同一 session 的 diagnosis 可复现。
2. 文档源更新后，不会污染旧 session 的回放与审计结果。
3. review、resume、recovery 与 history 都共享同一份 retrieval 事实快照。
4. facts 和 cases 的召回结果都要分别保留类型标记与来源标记。

### 6.7 Citation 格式

统一 citation 字符串格式为：

`[{knowledge_kind}:{engine}:{topic}] {title} > {section_path} | {url}`

例如：

`[fact:mysql:memory] InnoDB Buffer Pool > innodb_buffer_pool_size | https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html`

citation 字符串由 `RagCitationFormatter` 生成，diagnosis prompt 和前端展示共用同一格式。

## 7. 阿里百炼适配策略

### 7.1 适配边界

阿里百炼默认承担：

1. `Embedding`
2. `Generation`

必须通过接口隔离：

1. MAF / `Microsoft.Extensions.AI` 兼容 embedding 适配层
2. diagnosis agent 现有配置适配层

### 7.2 配置原则

第一版保持：

1. 统一从配置读取 endpoint、api key、model id。
2. embedding 默认模型使用 `text-embedding-v3`，generation 默认模型使用 `qwen3.6-plus`，两者都允许通过配置覆盖，不硬编码进业务逻辑。
3. `appsettings.json` 只保留占位值，真实 key 进入本地覆盖配置。

设计上不把具体模型名称硬编码进业务逻辑或文档主叙述里，避免供应商可用模型变化导致规划文档过时。
同时不在 `RAG` 主流程里直接散落使用百炼 SDK，而是优先复用 MAF / `Microsoft.Extensions.AI` 可接入的模型能力。

## 8. 检索与融合策略

### 8.1 第一版检索

第一版只要求：

1. facts 和 cases 两条通道都支持 embedding 召回
2. `engine / vendor / topic / parameter_names / product_version` filter
3. facts / cases 分配独立 top-k
4. 基于 MAF context provider 的上下文注入
5. facts 优先、cases 补充的融合规则
6. 默认配额为 facts top-k = 5，cases top-k = 3，合并后最多注入 8 条到 diagnosis context，可通过配置覆盖。

### 8.2 不在第一版强做的能力

1. query rewrite 自动重写
2. hybrid BM25 + vector 检索
3. 多路召回融合
4. 在线学习排序
5. 案例型知识打分模型
6. 自定义并行 rerank 主流程

原因很简单：

1. 先把事实语料质量做对，比先堆检索花活更重要。

## 9. 文档与实施顺序

### 9.1 第一阶段：Corpus 基线

先交付：

1. `docs/rag/` 目录规划
2. 原始 facts / cases 文档命名规则
3. `SeedPreloadCommand` 与 seed URL 清单
4. 原始文档目录样例
5. prepared chunks / metadata

### 9.2 第二阶段：Index / Retrieval 基线

交付：

1. 阿里百炼 embedding 适配
2. PostgreSQL 向量索引
3. `VectorStore / VectorStoreCollection`
4. metadata filter
5. facts / cases 双通道检索
6. EF Core entities + migration

### 9.3 第三阶段：Workflow Adapter

交付：

1. `WorkflowRagContextAssembler`
2. `RetrievedKnowledgeItem`
3. `WorkflowDocumentContextProvider`
4. diagnosis prompt 中的 citation 注入
5. retrieval snapshot 持久化到 session / checkpoint / node execution
6. history / detail 中的 external knowledge 展示语义
7. `WorkflowProgressCalculator`、history 节点展示和前端 replay 节点映射同步更新
8. case audit 视图与 RAG 资产状态面板

### 9.4 第四阶段：质量与运维

交付：

1. 文档入库校验命令
2. 索引重建命令
3. freshness / coverage 指标
4. 为未来 `Case RAG` 预留统一出口
5. 同步更新 `src/AIDbOptimize.Infrastructure/CLAUDE.md`、`docs/rag/CLAUDE.md`、`docs/workflow/CLAUDE.md`

## 10. 风险与对应策略

### 10.1 文档下载噪声高

原因：

1. 厂商站点导航、页脚、营销内容多。

应对：

1. 原始文档目录规则
2. 正文提取
3. 原始文档保留
4. chunk 前清洗而不是召回后再猜

### 10.2 RAG 污染 evidence 语义

原因：

1. 如果 retrieved knowledge 与 observed evidence 不分层，gate 与 grounding 都会变脏。

应对：

1. `ExternalKnowledgeItems` 单独建模
2. gate 不读 retrieval 结果
3. grounding 分开校验两类引用
4. retrieval snapshot 持久化，避免 resume / recovery 时语义漂移

### 10.3 供应商耦合过深

原因：

1. 如果下载、索引、workflow 都直接引用百炼 SDK，后面替换会很痛。

应对：

1. embedding / rerank / generation 统一走 adapter 接口
2. 文档资产和索引结构完全由仓库自己管理

### 10.4 后续 Case RAG 接不进来

原因：

1. 如果第一版 retrieval 出口只适配“事实文档段落”，未来案例检索会再起一套体系。

应对：

1. 统一返回 `RetrievedKnowledgeItem`
2. `source_type` 区分 `official-doc / cloud-doc / case`
3. 事实与案例共用 retrieval 出口，不共用完整度语义
4. retrieval snapshot 与 citation 契约从第一版就统一，不为 facts / cases 分别起两套持久化格式

## 11. 必要预研

正式实施前，先做以下预研：

1. `Corpus naming spike`
   验证按路径 + 文件名即可稳定识别 facts / cases / engine / vendor。
2. `Seed preload spike`
   验证固定 seed URL 能稳定落盘到 `docs/rag/facts/...`。
3. `MAF abstraction availability spike`
   验证 `AIContextProvider` 是否在当前 MAF 版本中存在；如不存在，则改用 `Microsoft.Extensions.AI` 直接实现适配层。
4. `Content extraction spike`
   验证正文抽取后导航噪声是否可控。
5. `Chunk metadata spike`
   验证章节切分后是否能稳定产生 `topic / parameter_names / section_path`。
6. `Embedding adapter spike`
   验证阿里百炼 embedding 接口、限流与批量索引策略。
7. `DashScope model id availability spike`
   验证 `text-embedding-v3` 与 `qwen3.6-plus` 这两个具体模型 ID 在当前账号与区域下可用。
8. `Pgvector image spike`
   验证 `pgvector/pgvector:pg17` 容器镜像可用，且 `CREATE EXTENSION vector` 成功。
9. `Historical case projection spike`
   验证已完成 workflow 结果能否稳定投影成 case knowledge。
10. `Workflow injection spike`
   验证 `ExternalKnowledgeItems` 注入 diagnosis 后不会影响 gate 与 grounding 现有语义。
11. `Persistence replay spike`
   验证 retrieval snapshot 写入 checkpoint 后，resume / recovery / history 仍然稳定。

## 12. 自审结论

本方案满足以下约束：

1. 统一平台先于单点拼接，方向正确。
2. 文档语料先落 `docs/`，可审计、可回溯。
3. `RAG` 只增强 diagnosis，不污染 observed evidence 和 gate 判定。
4. 阿里百炼被限制在模型能力层，不会绑死平台结构。
5. 后续 `Case RAG` 可以在同一平台上增量扩展，而不需要推翻第一版事实检索。

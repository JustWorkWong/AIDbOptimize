# AIDbOptimize RAG 平台接入任务清单

## 里程碑

本轮目标是落地统一 `RAG` 平台，并先完成事实型语料基线：

1. `官方文档 + 云厂商文档`
2. 程序先预下载首批 seed 文档到 `docs/`，后续人工可继续补充
3. 索引、chunk metadata 与 retrieval snapshot 默认落控制面 PostgreSQL
4. workflow 在 `SkillPolicyGate` 后、`DiagnosisAgent` 前通过 MAF context provider 接入 retrieval context
5. 模型能力默认走阿里百炼适配层
6. 历史优化记录形成独立 case knowledge 流，并与 facts 融合

完成标记规则：

- `[x]` 代表代码、测试、文档与任务状态已经对齐
- `[ ]` 代表仍有实现缺口或验证缺口

最终验证（完成时应满足）：

- [x] `dotnet test .\AIDbOptimize.slnx --no-restore`
- [x] `npm run build` (`src/AIDbOptimize.Web`)

## 阶段 1：规划与文档基线

- [x] 明确 `RAG` 统一平台方向，而不是单点 prompt 拼接
- [x] 明确首批语料范围：官方文档 + 云厂商文档
- [x] 明确程序提供最小 seed 预下载方法，后续人工可继续补充文档
- [x] 明确索引、chunk metadata 与 retrieval snapshot 默认落控制面 PostgreSQL
- [x] 明确 `RAG` 相关方法优先使用 MAF 提供的抽象，不再平行设计一套主流程
- [x] 明确历史优化记录必须进入 case knowledge 流，而不是只留模糊预留位
- [x] 明确阿里百炼只承担模型能力，不承担语料资产管理
- [x] 在 `docs/plans/active/2026-05-aidboptimize-rag-platform/` 下完成 `design.md / detailed-design.md / tasks.md`
- [x] 在仓库根 `CLAUDE.md`、`README.md`、`docs/README.md` 切换当前执行计划指针到本计划

验收：

- [x] 方案、详细方案、任务清单齐全
- [x] 仓库入口文档清晰指向本轮 `RAG` 计划

## 阶段 2：语料目录与命名契约

- [x] 完成 `Corpus naming spike`
- [x] 完成 `Seed preload spike`
- [x] 创建 `docs/rag/` 目录规划，并同步更新对应 `CLAUDE.md`
- [x] 为 `docs/rag/` 定义版本控制边界与 `.gitignore` 规则
- [x] 创建 `docs/rag/facts/mysql/`
- [x] 创建 `docs/rag/facts/postgresql/`
- [x] 创建 `docs/rag/facts/cloud/`
- [x] 创建 `docs/rag/cases/mysql/`
- [x] 创建 `docs/rag/cases/postgresql/`
- [x] 定义 facts 文件命名规则
- [x] 定义 cases 文件命名规则
- [x] 定义最少 metadata 规则：`engine / source_type / vendor / topic`
- [x] 定义固定 seed URL 清单
- [x] 为人工整理文档补齐来源 URL 或来源说明字段

验收：

- [x] 仅靠路径 + 文件名 + 最少 metadata 就能稳定识别文档类型
- [x] 大体积原始语料与 prepared 数据默认不直接进入 git

## 阶段 3：预处理与案例投影预研

- [x] 完成 `Content extraction spike`
- [x] 完成 `Chunk metadata spike`
- [x] 完成 `Historical case projection spike`
- [x] 完成 `Persistence replay spike`
- [x] 实现 `RagSourceDocument`
- [x] 实现 `RagDocumentType`
- [x] 实现 `RagCorpusFileNamer`
- [x] 实现 `SeedPreloadDocumentCatalog`
- [x] 实现最小 `SeedPreloadCommand`
- [x] 将首批 seed 文档落盘到 `docs/rag/facts/...`
- [x] 实现 `CorpusPreprocessor`
- [x] 实现去导航、页脚、无关营销块的正文提取规则
- [x] 实现 `CorpusChunker`
- [x] 实现章节优先的 chunk 切分
- [x] 实现 `HistoricalOptimizationCaseProjector`
- [x] 从已完成 workflow 中提取 case knowledge 原始记录
- [x] 将 prepared facts / cases 落盘到 `docs/rag/prepared/`
- [x] 新增 `src/AIDbOptimize.Infrastructure/Rag/` 目录后，同步更新 `src/AIDbOptimize.Infrastructure/CLAUDE.md`
- [x] 为 PostgreSQL 向量存储设计控制面 schema 与 migration
- [x] 为 `RagDocumentEntity / RagDocumentChunkEntity / RagCaseRecordEntity / RagRetrievalSnapshotEntity` 设计 EF Core 实体

验收：

- [x] 至少一批 facts 文档可稳定清洗
- [x] 首批 seed 文档可以由程序一次性准备好
- [x] 至少一批内部历史优化记录可稳定投影为 case knowledge
- [x] 原始文档、prepared 数据、PostgreSQL 索引三层分离

## 阶段 4：chunk metadata 与索引契约

- [x] 定义 chunk metadata 结构
- [x] 固化 `engine / vendor / topic / parameter_names / section_path` 字段
- [x] 固化 topic 受控集合
- [x] 为 `product_version / applies_to / keywords` 定义抽取规则
- [x] 实现 metadata 校验
- [x] 确保 chunk 与原始 URL、标题路径可双向追溯

验收：

- [x] 每个 chunk 都能稳定回溯到来源页面和章节路径
- [x] 每个 case chunk 都能稳定回溯到 workflow session 或 case id
- [x] retrieval 阶段可以只靠 metadata 做基础过滤

## 阶段 5：阿里百炼模型适配层

- [x] 完成 `Embedding adapter spike`
- [x] 确认阿里百炼模型能力通过 MAF / `Microsoft.Extensions.AI` 可接入路径使用
- [x] 实现 embedding 适配，不在业务层散落直连 SDK
- [x] 将 endpoint / api key / model id 配置接入本地配置体系
- [x] 保持 `appsettings.json` 只存占位值
- [x] 保持本地真实 key 走本地覆盖配置

验收：

- [x] MAF / `Microsoft.Extensions.AI` 是模型接入主路径
- [x] 无业务模块直接硬编码百炼模型能力

## 阶段 6：索引与检索基线

- [x] 实现 `RagDocumentEntity`
- [x] 定义 PostgreSQL chunk / embedding / retrieval snapshot 存储结构
- [x] 实现 `RagDocumentChunkEntity`
- [x] 实现 `RagCaseRecordEntity`
- [x] 实现 `RagCaseEvidenceLinkEntity`
- [x] 实现 `RagRetrievalSnapshotEntity`
- [x] 验证并切换 AppHost PostgreSQL 镜像到 `pgvector/pgvector:pg17` 或等价 pgvector 支持版本
- [x] 为 PostgreSQL 接入 `pgvector` 或等价向量存储能力
- [x] 实现 EF Core migration
- [x] 实现 `RagDocumentChunkRepository`
- [x] 实现 `RagCaseRecordRepository`
- [x] 实现 `RagRetrievalSnapshotRepository`
- [x] 实现索引构建与重建命令
- [x] 实现 `VectorStore / VectorStoreCollection`
- [x] 实现 metadata filter
- [x] 实现 facts 通道 top-k retrieval
- [x] 实现 cases 通道 top-k retrieval
- [x] 验证 MAF context provider 可基于 PostgreSQL vector store 工作

验收：

- [x] 检索可按 `engine / vendor / topic / parameter_names` 过滤
- [x] 检索结果可输出稳定 citation 字段
- [x] PostgreSQL 成为主检索存储，而不是平面文件索引
- [x] facts 与 cases 共用 retrieval 出口，但召回配额可独立控制

## 阶段 7：workflow 注入层

- [x] 完成 `Workflow injection spike`
- [x] 定义 `RetrievedKnowledgeItem`
- [x] 实现 `WorkflowDocumentContextProvider`
- [x] 实现 `WorkflowRagContextAssembler`
- [x] 实现 `RagKnowledgeQueryService`
- [x] 消费 `InvestigationPlan.RetrievalHints`
- [x] 在 `SkillPolicyGate` 后插入基于 MAF context provider 的 workflow rag 节点
- [x] 为 facts / cases 定义独立召回配额与融合顺序
- [x] 将 retrieval 结果写入 `ExternalKnowledgeItems`
- [x] 将 retrieval snapshot 唯一写入 `RagRetrievalSnapshotEntity` 并关联对应 `workflow_node_executions`
- [x] 将 `snapshot_id` 写入 `workflow_sessions.StateJson` 作为引用
- [x] 将 `snapshot_id` 写入 `workflow_checkpoints.SnapshotJson` 作为引用
- [x] 保持 `SkillPolicyGate` 不读取 retrieval 结果
- [x] 保持 `DbConfigGroundingExecutor` 不把 retrieval 结果当 observed evidence
- [x] 更新 `WorkflowProgressCalculator`、history 节点文案和前端 replay 节点映射

验收：

- [x] workflow 能注入 facts context，但 `pass / degraded / blocked` 不受影响
- [x] workflow 能注入 case context，但 case 不能覆盖 facts 约束
- [x] external knowledge 与 observed evidence 语义分层清晰
- [x] resume / recovery / history 复用同一份 retrieval snapshot，不漂移到更新后的文档结果

## 阶段 8：diagnosis 与 citation 集成

- [x] 扩展 diagnosis prompt 输入，使其显式消费 retrieved knowledge
- [x] 让 diagnosis 输出稳定引用外部知识 citation
- [x] 固化 citation 展示格式
- [x] 为 history detail 暴露 retrieval context 摘要
- [x] 为前端结果展示补 external knowledge 引用区域
- [x] 为前端增加 RAG 资产状态面板
- [x] 为前端增加 case audit 视图
- [x] 为 grounding 增加 external knowledge citation 校验

验收：

- [x] diagnosis 能同时引用实时证据与外部文档
- [x] diagnosis 能明确区分 `fact citation` 与 `case citation`
- [x] citation 可追溯到具体来源 URL 与章节路径
- [x] external knowledge citation 与 observed evidence grounding 语义分开校验

## 阶段 9：质量与可运维性

- [x] 增加文档入库校验命令
- [x] 增加索引重建命令
- [x] 增加 freshness 指标
- [x] 增加语料覆盖率检查
- [x] 为未来 `Case RAG` 预留统一 retrieval 出口
- [x] 在长期文档中更新 `docs/workflow/README.md` 与 `docs/rag/README.md`
- [x] 同步更新 `docs/workflow/CLAUDE.md`、`docs/rag/CLAUDE.md`、`src/AIDbOptimize.Infrastructure/CLAUDE.md`

验收：

- [x] 文档更新、索引重建、workflow 注入都具备明确运维入口
- [x] 第二阶段接 `Case RAG` 时不需要推翻 facts 基线

测试数据策略：单元测试使用 `tests/AIDbOptimize.Infrastructure.Tests/Fixtures/Rag/` 下的最小样本夹具文件；集成测试使用本地 PostgreSQL（Aspire 编排）；embedding adapter 测试使用 mock embedding 接口，不依赖真实阿里百炼 API key。

## 测试与验证

- [x] corpus naming 规则测试
- [x] seed preload 测试
- [x] 正文提取测试
- [x] chunk metadata 测试
- [x] historical case projection 测试
- [x] embedding adapter 测试
- [x] retrieval filter 测试
- [x] facts / cases 融合测试
- [x] EF Core migration 测试
- [x] case projector 数据来源测试（从 workflow 控制面表读取）
- [x] workflow rag 注入测试
- [x] workflow rag resume 测试
- [x] workflow rag recovery 测试
- [x] workflow rag history replay 测试
- [x] PostgreSQL vector store 集成测试
- [x] MAF context provider 集成测试
- [x] diagnosis citation 集成测试
- [x] `dotnet test .\AIDbOptimize.slnx --no-restore`
- [x] `npm run build` (`src/AIDbOptimize.Web`)

## 评审重点

- [x] `RAG` 是否仍然严格位于 `SkillPolicyGate` 后、`DiagnosisAgent` 前
- [x] 文档语料是否先落 `docs/` 并保留原始文件
- [x] `RetrievedKnowledge` 是否与 `ObservedEvidence` 严格分层
- [x] RAG 主流程是否优先复用 MAF 提供的抽象，而不是重复发明一套
- [x] 历史优化记录是否已经形成明确 case knowledge 流，而不是停留在概念层
- [x] 阿里百炼是否被限制在模型适配层
- [x] 统一平台结构是否足以承接未来 `Case RAG`

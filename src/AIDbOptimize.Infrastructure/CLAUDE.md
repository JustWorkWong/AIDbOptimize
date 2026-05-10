# AIDbOptimize.Infrastructure 目录说明

## 目录职责

`AIDbOptimize.Infrastructure/` 负责持久化、MCP 调用适配、workflow 控制面实现，以及与外部运行时相关的真实技术实现。

## 当前结构

```text
AIDbOptimize.Infrastructure/
├── CLAUDE.md
├── Agents/
├── Mcp/
├── Observability/
├── Persistence/
│   ├── DesignTime/
│   ├── Entities/
│   ├── Lab/
│   ├── Migrations/
│   └── Repositories/
├── Rag/
│   ├── Corpus/
│   ├── Embeddings/
│   ├── Ingestion/
│   ├── Preprocess/
│   └── Workflow/
├── Security/
└── Workflows/
    ├── Pipeline/
    ├── Runtime/
    ├── Services/
    └── Skills/
```

## 文件用途

- `Agents/`
  agent session 持久化与 rolling summary 服务，实现 `agent_sessions / agent_summaries / agent_messages` 的控制面落库。
- `Mcp/`
  MCP client、session、tool execution 与 agent tool assembly 的真实实现。
- `Observability/`
  统一维护 workflow / agent / review / mcp 的 `ActivitySource`、`Meter` 与核心计数器/直方图。
- `Persistence/Entities/`
  控制面数据库实体，包含 workflow session、checkpoint、review、node execution、event 和 MCP 执行记录。
- `Persistence/Repositories/`
  `Mcp*` 与 `Workflow*` 持久化抽象的 EF Core 实现。
- `Persistence/Migrations/ControlPlane/`
  控制面数据库 schema 迁移。
- `Rag/Corpus/`
  RAG 原始语料层的最小契约，负责 facts / cases 文件命名、路径解析、类型边界与首批固定 seed 文档目录。
- `Rag/Preprocess/`
  RAG 预处理层的最小契约，负责 seed 原文清洗、正文提取、章节优先 chunk 切分和基础 chunk metadata 提取，不掺入检索语义。
- `Rag/Ingestion/`
  RAG 入库层的最小契约，负责扫描 `docs/rag` 原文目录并把 document/chunk 骨架写入控制面 PostgreSQL。
- `Rag/Embeddings/`
  RAG embedding 适配层，负责占位配置、OpenAI-compatible embedding 调用和向量生成。
- `Rag/VectorData/`
  RAG vector store 适配层，负责把 facts 检索统一暴露为 `VectorStore / VectorStoreCollection`，让 workflow 查询主路径复用同一套向量检索入口，而不是在不同调用点重复拼 EF 查询。
- `Rag/Workflow/`
  RAG 与 workflow 历史数据的桥接层，负责把已完成 workflow 投影成 case knowledge 骨架，不承担 diagnosis prompt 或 API 编排。
- `Security/`
  prompt 输入拼装、工具输出脱敏、只读工具白名单和通用敏感信息脱敏原语。
- `Workflows/Pipeline/`
  workflow 主链上的业务执行骨架，负责 input validation、planner 驱动的 evidence collection subworkflow、policy gate、pre-diagnosis RAG context assembler、diagnosis、grounding 与 review gate；其中 diagnosis 侧还通过 MAF `AIContextProvider` 注入已检索的外部知识。
- `Workflows/Runtime/`
  runtime seam、workflow command、skill selection 持久化、checkpoint/recovery、执行注册表与进度计算。
- `Workflows/Services/`
  workflow、review、history 的控制面服务实现。
- `Workflows/Skills/`
  workflow skills 相关的受控文档协议解析、bundle 解析、capability catalog 与 diagnosis 契约校验实现。

## 依赖关系

- `ApiService -> Infrastructure.Workflows.Services`
  API 层组合 workflow 服务。
- `Infrastructure.Workflows.Services -> Persistence/*`
  workflow 控制面实现直接依赖控制面数据库。
- `Infrastructure.Mcp -> Persistence.Entities.Mcp*`
  MCP 执行记录通过 `WorkflowSessionId` 与 workflow 审计关联。
- `Infrastructure.Workflows.Skills -> docs/workflow/skills/*`
  受控解析器消费长期规则资产文档，但解析规则必须由当前源码定义并验证。
- `Infrastructure.Rag.Corpus -> docs/plans/active/2026-05-aidboptimize-rag-platform/*`
  当前 corpus 骨架直接落实现行 RAG active plan 中的命名契约与 seed preload 清单。

## Rag 子树

```text
Rag/
|- Corpus/
|- Embeddings/
|- Ingestion/
|- Preprocess/
|- VectorData/
`- Workflow/
```

## 维护原则

1. 在 `Workflows/` 下新增子目录时，必须同步更新本文件。
2. `Workflows/Skills/` 中的代码优先实现受控协议解析，不要把它扩展成通用 Markdown 引擎。
3. 如果某次修改同时影响 `docs/workflow/skills/` 的目录边界与 `Workflows/Skills/` 的解析行为，必须同时更新两侧文档。
4. `Workflows/Pipeline/` 与 `Workflows/Runtime/` 的文档描述必须与当前 graph 一致；当前真实主链是 `InputValidation -> InvestigationPlanner -> EvidenceCollectionSubworkflow -> SkillPolicyGate -> WorkflowRagContextAssembler -> Diagnosis -> Grounding -> Review/Complete`。
5. `Rag/Corpus/` 只维护原始语料命名、解析与 seed catalog，不在这里掺入下载器、切片器或 retrieval 逻辑。
6. `Rag/Preprocess/` 只负责“原文 -> 清洗文本 -> chunk”的本地转换，不直接依赖 workflow runtime 或控制面数据库。
7. `Rag/Ingestion/` 可以依赖 `ControlPlaneDbContext`，但只负责 document/chunk 入库，不承担 retrieval、workflow 注入或诊断语义。
8. `Rag/Embeddings/` 负责向量生成适配，不把 provider SDK 散落到 ingestion、query 或 runtime 多处。
9. `Rag/VectorData/` 只负责 facts vector retrieval 抽象和 `VectorStore` 适配，不承担 case 排序、workflow snapshot 或 diagnosis prompt 组装。
10. `Rag/Workflow/` 只负责“workflow 历史 -> case record/evidence link”的投影，不在这里做 case retrieval 排序或前端 DTO 拼装。

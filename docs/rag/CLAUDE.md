# docs/rag 目录约束

## 目录定位

`docs/rag/` 是 RAG 语料资产的长期边界说明目录，负责约束原始事实文档、历史案例文档和预处理产物的落点，不承担向量索引、数据库快照或运行时配置。

## 目录结构

```text
docs/rag/
├── CLAUDE.md
├── README.md
├── facts/
│   ├── README.md
│   ├── mysql/
│   │   └── README.md
│   ├── postgresql/
│   │   └── README.md
│   └── cloud/
│       └── README.md
├── cases/
│   ├── README.md
│   ├── mysql/
│   │   └── README.md
│   └── postgresql/
│       └── README.md
└── prepared/
    ├── README.md
    ├── facts/
    │   └── README.md
    └── cases/
        └── README.md
```

## 文件职责

- `README.md`
  说明 `facts/cases/prepared` 的长期职责、命名约定、版本控制边界和人工补充规则。
- `facts/README.md`
  说明事实型语料总入口和放置规则。
- `facts/mysql/`、`facts/postgresql/`、`facts/cloud/`
  分别承接 MySQL、PostgreSQL、云厂商事实型文档。
- `cases/README.md`
  说明案例型语料总入口和准入规则。
- `cases/mysql/`、`cases/postgresql/`
  按数据库引擎承接历史优化案例。
- `prepared/README.md`
  说明清洗产物、chunk 和 metadata 的边界。
- `prepared/facts/`、`prepared/cases/`
  分别承接 facts 与 cases 的预处理产物说明或最小样例。
  当前 rebuild 会在这两个目录下写入 chunk markdown 与 `__metadata.json`。

## 依赖关系

- `docs/rag/README.md` 依赖 `docs/plans/active/2026-05-aidboptimize-rag-platform/` 作为当前规划来源。
- `facts/` 与 `cases/` 依赖后续 `SeedPreloadCommand`、人工补充流程和 workflow case 投影流程写入内容。
- `prepared/` 依赖后续预处理链路生成清洗产物。
- 运行时检索、chunk metadata 主存储和 retrieval snapshot 不依赖本目录持久化，而依赖控制面 PostgreSQL。
 - 当前运维入口为 `/api/rag/assets/status`、`/api/rag/cases/audit`、`/api/rag/validate`、`/api/rag/rebuild`，相关长期说明应同步反映到 [README.md](/E:/Db/docs/rag/README.md)。

## 维护规则

1. 新增语料子类型时，先增加子目录说明，再放文档内容。
2. `facts/` 只放权威事实文档，`cases/` 只放案例，不允许混放。
3. `prepared/` 只放预处理结果和最小样例，不放人工编辑后的事实正文。
4. 大体积语料、批量下载和本地缓存统一放到 `_bulk/`、`_cache/`、`_downloads/` 这类忽略目录。
5. 修改目录结构、类型边界或命名契约时，必须同步更新本文件和 [README.md](/E:/Db/docs/rag/README.md)。
6. 如果 workflow 投影新增了 case 原文导出或 prepared case 产物，必须继续保持 `cases/` 与 `prepared/cases/` 的职责分离，不要让 projected raw case docs 和 prepared artifacts 混在一个目录。

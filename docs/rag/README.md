# RAG 语料目录说明

`docs/rag/` 只放长期保留的 RAG 语料边界说明、少量可审计样例和人工整理后的原始文档入口，不放数据库索引、向量和大批量本地缓存。

## 目录职责

- `facts/`
  权威事实型语料入口。只收官方文档、云厂商数据库参数与运维说明，不混入案例总结。
- `cases/`
  历史优化案例入口。只收已完成 workflow 投影出的案例原文，或人工整理的结案总结，不混入参数事实文档。
- `prepared/`
  预处理产物入口。只放清洗后的 chunk、metadata、少量测试样例和目录说明，不回写原始正文。

## 当前结构

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

## 命名约定

- facts 文件名：
  `<engine>__<vendor>__<topic>__<short-title>.md`
- cases 文件名：
  `<engine>__case__<problem-type>__<session-or-ticket>.md`
- prepared 文件名：
  `<source-stem>__chunk-<nnnn>.md` 或 `<source-stem>__metadata.json`

最少要能从路径、文件名和伴随 metadata 推断：

- `engine`
- `source_type`
- `vendor`
- `topic` 或 `problem_type`

## 版本控制边界

- 默认纳入 git：
  目录说明、规则文档、`README.md`、`CLAUDE.md`、少量样例。
- 默认不纳入 git：
  `prepared/` 下批量 chunk、metadata、缓存文件。
- 约定放入忽略目录：
  `_bulk/`、`_cache/`、`_downloads/`，用于大体积语料、临时下载和本地导入。
- 需要保留可复现样例时：
  只提交最小样本，不提交整批语料。

## 人工补充规则

1. 人工补充只落到 `facts/` 或 `cases/`，不要直接写进 `prepared/`。
2. 每个文件都要保留来源 URL 或来源说明。
3. 路径先表达类型，再由文件名表达 `engine/vendor/topic`，不要靠正文猜类型。
4. 同一主题先补事实，再补案例；facts 与 cases 冲突时，以 facts 为准。
5. 清洗失败时保留原始文档，禁止用 prepared 产物覆盖原文。

## 使用顺序

1. 首批 seed 文档下载到 `facts/...`。
2. 人工补充文档继续落 `facts/...` 或 `cases/...`。
3. 统一预处理后输出到 `prepared/...`。
4. PostgreSQL 保存检索主数据；`docs/rag/` 只保留可审计资产边界。

## 当前运维入口

- `GET /api/rag/assets/status`
  查看 facts / cases / chunks / snapshots 数量与 freshness 时间戳。
- `GET /api/rag/cases/audit`
  查看当前 case knowledge 投影结果。
- `POST /api/rag/validate`
  校验 corpus 路径、front matter 和 coverage。
- `POST /api/rag/rebuild`
  触发 document/chunk 重建与 case projection。

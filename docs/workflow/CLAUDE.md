# docs/workflow 目录说明

## 目录职责

`docs/workflow/` 用于保存 workflow 相关的长期有效说明，重点覆盖：

1. 当前后端实现链路与控制面语义。
2. workflow 相关规则资产的目录边界。
3. 后续 skills / RAG 演进时的文档归档位置。

这里不保存一次性的设计讨论或排期，那些内容统一进入 `docs/plans/active/...`。

## 当前结构

```text
docs/workflow/
├── CLAUDE.md
├── README.md
└── skills/
    ├── CLAUDE.md
    ├── bundles/
    │   ├── mysql-default/
    │   │   └── bundle.md
    │   └── postgresql-default/
    │       └── bundle.md
    ├── mysql-investigation/
    │   └── SKILL.md
    ├── mysql-diagnosis/
    │   └── SKILL.md
    ├── postgresql-investigation/
    │   └── SKILL.md
    └── postgresql-diagnosis/
        └── SKILL.md
```

## 文件约定

- `README.md`
  workflow 后端代码流程主文档，描述 API、Application、Runtime、skills-v1 graph、持久化与投影之间的关系。
- `skills/`
  workflow skills 资产目录。后续 MySQL/PostgreSQL investigation skill、diagnosis skill，以及 bundle 说明都放在这里。
- `skills/CLAUDE.md`
  `docs/workflow/skills/` 子目录的边界、结构与维护规则。

## 与 plans 的边界

- `docs/plans/active/...`
  保存当前阶段的方案、详细方案、任务清单和预研结果。
- `docs/workflow/...`
  只保存沉淀后的长期规则与实现说明。

换句话说：

1. “准备怎么做”去 `plans`
2. “系统现在怎么工作 / 规则资产放哪里”留在 `workflow`

## 维护原则

1. 新增 workflow 规则资产目录时，必须同步更新本文件与对应子目录的 `CLAUDE.md`。
2. `README.md` 中关于 skills 与 RAG 的描述必须区分“当前已实现”和“仅规划预留”。
3. `README.md` 中的主链节点名必须与当前真实 graph 保持一致，尤其是 `InvestigationPlanner`、`EvidenceCollectionSubworkflow`、`SkillPolicyGate` 这类中间节点。
4. 如果 skills 或 RAG 方案尚未真正落地到代码，不要在这里把规划内容写成既成事实。

# docs 目录说明

## 目录职责

`docs/` 用于保存长期有效的设计文档、实现说明和执行计划，不放临时草稿。

## 当前结构

```text
docs/
├── CLAUDE.md
├── README.md
├── mcp/
│   ├── CLAUDE.md
│   └── README.md
├── workflow/
│   ├── CLAUDE.md
│   └── README.md
└── plans/
    ├── README.md
    └── active/
        ├── 2026-04-aidboptimize-db-config-workflow/
        │   ├── design.md
        │   ├── detailed-design.md
        │   └── tasks.md
        └── 2026-04-aidboptimize-ddd-mcp-seeding/
            ├── design.md
            ├── detailed-design.md
            └── tasks.md
```

## 文件用途

- `README.md`
  docs 总索引，放文档导航，不承载某个子系统的细节实现。
- `mcp/README.md`
  MCP 工具获取、持久化、前端交互、工具执行链路的详细说明。
- `mcp/CLAUDE.md`
  `docs/mcp/` 子目录的边界、文件用途和后续扩展规则。
- `workflow/README.md`
  workflow 后端代码流程主文档，覆盖 API、Application、Runtime、MAF、review、recovery、cancel 和控制面表职责。
- `workflow/CLAUDE.md`
  `docs/workflow/` 子目录的边界和维护约束。
- `plans/README.md`
  计划类文档的约束说明。
- `plans/active/2026-04-aidboptimize-db-config-workflow/`
  数据库配置优化 workflow 的总体方案、详细设计和任务拆解，覆盖 MAF workflow、agent 持久化、审核恢复、OTel、安全与前后端契约。
- `plans/active/...`
  正在推进的方案设计、细化设计和任务拆解。

## 依赖关系

- `docs/README.md` 依赖各子目录 README 作为详情入口。
- `docs/mcp/README.md` 依赖源码中的 API、Application、Infrastructure、Web 实现作为事实来源。
- `docs/workflow/README.md` 依赖 workflow 相关 API、Application、Infrastructure runtime 和控制面实体实现作为事实来源。
- `docs/plans/*` 依赖当前架构和业务目标，但不反向约束运行时代码。

## 维护原则

- 新增一个长期主题文档时，优先在 `docs/` 下创建独立子目录，不要把所有说明都堆在根目录。
- 发生目录级调整时，先改文档树，再同步更新本文件。

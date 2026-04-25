# docs/mcp 目录说明

## 目录职责

`docs/mcp/` 专门记录 MCP 子系统相关的长期说明文档，包括配置来源、调用链路、前端交互约定、方案对比和问题排查。

## 当前结构

```text
docs/mcp/
├── CLAUDE.md
├── README.md
└── postgresql-mcp-options.md
```

## 文件用途

- `README.md`
  MCP 的主说明文档，回答“如何配置”“如何获取工具”“执行器为什么会失败”“前后端职责如何划分”等问题。
- `postgresql-mcp-options.md`
  PostgreSQL MCP 方案对比与当前选择，说明为什么当前默认方案是 npm 无状态实现。
- `CLAUDE.md`
  维护本目录边界，避免后续把运行计划、一次性排查记录和长期说明混写。

## 依赖关系

- 本目录文档依赖 `src/AIDbOptimize.ApiService/`、`src/AIDbOptimize.Application/`、`src/AIDbOptimize.Infrastructure/`、`src/AIDbOptimize.Web/` 中的 MCP 相关实现。
- 本目录不依赖 `docs/plans/`，计划文档和事实文档分离维护。

## 后续扩展建议

- 如果以后增加“接入新 MCP Server 指南”“故障排查手册”“安全约束”，直接在本目录新增并列 Markdown 文件。
- 一个问题域一篇文档，避免单个文件无限膨胀。

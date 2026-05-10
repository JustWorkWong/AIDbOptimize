# 文档索引

`docs/` 用于保存 AIDbOptimize 的长期设计文档、实现说明和执行计划。

## 当前目录约定

- `docs/rag/`
  RAG 语料长期边界文档，包括 facts / cases / prepared 的职责、命名和版本控制约束。
- `docs/mcp/`
  MCP 相关长期说明文档，包括配置、发现、执行和前端交互约定。
- `docs/workflow/`
  workflow 后端代码流程、控制面 runtime、review / recovery / cancel 语义说明。
- `docs/plans/`
  分阶段计划与执行清单。
- `docs/plans/active/`
  存放最近完成计划、当前未完成计划和仍保留参考价值的历史基线计划。

## 主题文档

- [RAG 语料目录说明](./rag/README.md)
- [MCP 获取 tools 的配置与实现说明](./mcp/README.md)
- [Workflow 后端代码流程说明](./workflow/README.md)
- [PostgreSQL MCP 方案对比与建议](./mcp/postgresql-mcp-options.md)

## 当前状态（2026-05-09）

- `workflow skills v1` 已从方案进入真实主链
- `docs/rag/` 长期目录骨架与版本控制边界已建立
- `Rag/Corpus` 命名契约与固定 seed catalog 已落代码并有单测覆盖
- workflow 已新增 `WorkflowRagContextAssembler` 显式注入节点，并具备 facts/cases 检索基线、embedding 适配层、pgvector 存储接线，以及 `validate/rebuild/case audit` 运维入口
- 当前已有新的 `RAG` active plan，后续实施以该计划为准

## 当前执行计划

- [RAG 平台接入方案](./plans/active/2026-05-aidboptimize-rag-platform/design.md)
- [RAG 平台接入详细方案](./plans/active/2026-05-aidboptimize-rag-platform/detailed-design.md)
- [RAG 平台接入任务清单](./plans/active/2026-05-aidboptimize-rag-platform/tasks.md)

## 当前默认基线

- [workflow skills v1 方案](./plans/active/2026-05-aidboptimize-workflow-skills-v1/design.md)
- [workflow skills v1 详细方案](./plans/active/2026-05-aidboptimize-workflow-skills-v1/detailed-design.md)
- [workflow skills v1 任务清单](./plans/active/2026-05-aidboptimize-workflow-skills-v1/tasks.md)

说明：
这组文档描述最近一次已经完成的实现基线，不代表当前还有未完成任务。

## 历史基线

- [数据库配置优化 workflow 总体方案](./plans/active/2026-04-aidboptimize-db-config-workflow/design.md)
- [数据库配置优化 workflow 详细设计](./plans/active/2026-04-aidboptimize-db-config-workflow/detailed-design.md)
- [数据库配置优化 workflow 任务拆解](./plans/active/2026-04-aidboptimize-db-config-workflow/tasks.md)
- [DDD + MCP + 测试数据初始化总体设计](./plans/active/2026-04-aidboptimize-ddd-mcp-seeding/design.md)
- [DDD + MCP + 测试数据初始化详细设计](./plans/active/2026-04-aidboptimize-ddd-mcp-seeding/detailed-design.md)
- [DDD + MCP + 测试数据初始化任务清单](./plans/active/2026-04-aidboptimize-ddd-mcp-seeding/tasks.md)

如果要看当前系统真实 workflow 语义，优先读 [docs/workflow/README.md](./workflow/README.md)，不要只看计划文档。

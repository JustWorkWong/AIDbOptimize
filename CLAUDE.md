# E:\Db 执行约束

## 当前执行计划

- 当前 active plan：`docs/plans/active/2026-05-aidboptimize-rag-platform/`
- 当前任务清单：`docs/plans/active/2026-05-aidboptimize-rag-platform/tasks.md`

## 当前默认基线

- 最近完成计划：`docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/`
- 默认基线任务清单：`docs/plans/active/2026-05-aidboptimize-workflow-skills-v1/tasks.md`

## 执行协议

1. 任何实现工作都必须先读取当前 active plan 的 `tasks.md`，并且只处理未勾选任务。
2. `workflow skills v1` 是默认实现基线，不再作为当前待办清单。
3. 不允许因为“顺手”而扩展到当前任务清单之外的模块、接口或前端区域。
4. 如果发现设计、依赖或上下文不足，先更新当前 plan 的 `tasks.md` 记录阻塞或缺口，再停止编码并汇报。
5. 完成当前 plan 后，再把它切换为默认基线，并重新指定新的 active plan。

## 文档入口

- `README.md` 只展示当前执行计划、当前验证快照、默认基线和启动入口。
- `docs/README.md` 可以保留历史基线链接，但必须明确区分“当前未完成计划（若有）”、“默认基线”和“历史基线”。
- `docs/plans/README.md` 只说明计划目录规则，不能替代本文档的执行约束。

## 架构同步

- 创建、删除、移动文件或目录时，必须同步更新对应目录下的 `CLAUDE.md`。
- 如果本次修改影响仓库根级规则，以本文档为准。

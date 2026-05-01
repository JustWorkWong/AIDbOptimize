# docs/workflow 目录说明

## 目录职责

`docs/workflow/` 用于保存与 workflow 后端实现相关的长期说明文档，重点覆盖真实代码链路、运行时职责边界和控制面语义。

## 文件约定

- `README.md`
  workflow 后端代码流程主文档，描述 API、Application、Runtime、MAF、持久化和投影之间的关系。

## 维护原则

- 这里放“长期有效的实现说明”，不放一次性的任务计划。
- 文档内容应以当前源码行为为准，避免重复 `plans/active/...` 中的需求和排期表述。
- 如果后续新增 workflow 主题文档，优先继续放在本目录下，而不是把细节堆回 `docs/README.md`。

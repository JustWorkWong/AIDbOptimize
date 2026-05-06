# docs/workflow/skills 目录说明

## 目录职责

`docs/workflow/skills/` 用于保存 workflow skill 相关的长期有效文档约束，重点说明：
1. skill 资产目录的边界与组织方式。
2. 各数据库 skill 文档的承载位置与最小文件约定。
3. bundle 级说明与单个 skill 说明之间的职责分层。

这里不保存一次性的调研记录、设计推演或任务拆解；这类内容统一进入 `docs/plans/active/...`。

## 当前预期结构

下面是当前已经落地的目录结构；后续新增数据库时继续沿用同构布局：

```text
docs/workflow/skills/
|-- CLAUDE.md
|-- bundles/
|   |-- mysql-default/
|   |   `-- bundle.md
|   `-- postgresql-default/
|       `-- bundle.md
|-- mysql-investigation/
|   `-- SKILL.md
|-- mysql-diagnosis/
|   `-- SKILL.md
|-- postgresql-investigation/
|   `-- SKILL.md
`-- postgresql-diagnosis/
    `-- SKILL.md
```

如果后续扩展其他数据库或 workflow skill，继续沿用 `bundles/<bundle-id>/bundle.md` 与 `<engine>-<skill-type>/SKILL.md` 的同构布局，不要为单个 skill 发明额外层级。

## 文件用途

- `CLAUDE.md`
  `docs/workflow/skills/` 的边界说明，定义目录职责、结构约束、与其他文档域的分工，以及维护原则。
- `bundles/<bundle-id>/bundle.md`
  bundle 级组合说明，描述 investigation / diagnosis skill 的绑定关系、版本兼容契约、适用 workflow 与 RAG 预留边界；不承载单个 skill 的完整执行细节。
- `<engine>-investigation/SKILL.md`
  investigation skill 的主说明文件，负责描述排查范围、证据要求、阻断规则、采集提示与 retrieval 预留协议。
- `<engine>-diagnosis/SKILL.md`
  diagnosis skill 的主说明文件，负责描述输出契约、建议规则、置信度语义、禁用模式与 citation 预留协议。

## 与 docs/plans 的边界

- `docs/plans/active/...`
  保存某一阶段的方案设计、详细设计、任务清单、调研结论和推进记录，服务于“准备怎么做”。
- `docs/workflow/skills/...`
  只保存沉淀后的长期结构约束与 skill 说明，服务于“这个目录承载什么、skill 应该如何组织”。

换句话说：
1. 还在讨论、验证、拆任务的内容去 `plans`
2. 已经稳定下来的目录边界、bundle 约定和 skill 说明模板留在 `workflow/skills`

## 维护原则

1. 新增、删除或移动 `docs/workflow/skills/` 下的目录结构时，必须同步更新本文件。
2. 新增数据库 skill 时，优先创建独立目录，并在目录内放置唯一入口 `SKILL.md`，不要把多个 skill 混写到同一文件。
3. investigation 与 diagnosis 必须拆成独立 skill 目录，避免把采集约束与诊断规则混写到同一文件。
4. `bundles/` 只描述组合关系，不复制各个 skill 的正文内容，避免文档分叉。
5. `SKILL.md` 必须带结构化 front matter，并严格保留各自固定章节；`bundle.md` 也必须带结构化 front matter，显式声明 skill 对应关系。
6. `RAG` 相关内容当前只能作为协议预留位出现，不能写成已接入 runtime、collector 或 gate 的既成事实。
7. 如果 skill 边界变化影响 `docs/workflow/` 的整体结构认知，必须同步检查并更新 `docs/workflow/CLAUDE.md`。

# AIDbOptimize.Web/src

## 目录职责

```text
src
├─ api
│  ├─ mcp.ts
│  ├─ rag.ts
│  └─ workflow.ts
├─ components
│  ├─ mcp
│  └─ workflow
├─ models
│  ├─ mcp.ts
│  ├─ rag.ts
│  └─ workflow.ts
├─ App.vue
├─ main.ts
└─ style.css
```

- `App.vue`：页面总装配点，负责 overview、MCP、workflow 三个视图的状态编排，并管理 db-config history/review/replay 的交互状态。
- `api/*.ts`：前端契约层，隔离 `fetch` 细节；`workflow.ts` 现在同时承接发起、history、review、replay 和 review submit 的最小封装，`rag.ts` 承接 RAG 资产状态查询。
- `models/*.ts`：页面使用的数据模型与契约状态，避免组件直接依赖裸 JSON。
- `components/mcp/*`：MCP 管理相关的展示与操作面板。
- `components/workflow/*`：db-config workflow 的最小发起、历史、review、replay 面板；`RagAssetStatusPanel.vue` 负责展示 RAG 资产概览。
- `style.css`：全局页面样式与复用布局类。

## 依赖关系

- `App.vue` 依赖 `api/*` 拉取数据，并把 `models/*` 中的结构传入 `components/*`。
- `components/workflow/*` 只接收 props / emits，不直接访问接口，保持最小展示职责。
- `api/workflow.ts` 通过 `WorkflowContractState<T>` 把“接口已就绪”和“仅占位回退”统一表达，减少页面分支。

# AIDbOptimize.Domain 目录说明

## 目录职责

`AIDbOptimize.Domain/` 保存纯领域模型、枚举和值对象，不依赖 EF Core、HTTP、MCP 客户端或前端实现。

## 当前结构

```text
AIDbOptimize.Domain/
├── CLAUDE.md
├── Common/
├── DbConfigOptimization/
│   ├── Enums/
│   └── Models/
├── Mcp/
├── Orders/
└── Seed/
```

## 文件用途

- `Common/`: 通用领域异常与基础类型。
- `DbConfigOptimization/Enums/`: workflow session、review、node execution、event 的领域状态枚举。
- `DbConfigOptimization/Models/`: 配置采集快照、recommendation、evidence item、evidence pack 等结构化领域模型。
- `Mcp/`: MCP 连接和工具的领域定义。
- `Orders/`: 示例订单领域模型。
- `Seed/`: 测试数据初始化相关的领域状态。

## 依赖关系

- `Application/* -> Domain/*`: 应用层只读取领域模型和枚举，不在领域层反向引用基础设施。
- `Infrastructure/Persistence/* -> Domain/DbConfigOptimization/Enums`: 持久化映射把 workflow 状态枚举落库，但状态语义仍归领域层。

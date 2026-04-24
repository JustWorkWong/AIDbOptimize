# AIDbOptimize DDD + MCP + 测试数据初始化总体设计

## 1. 目标

在当前 `Aspire + API + Web + PostgreSQL + MySQL + Redis + RabbitMQ` 骨架基础上，逐步演进为一套可持续扩展的正式架构，用于支撑：

1. 基于 DDD 的订单域后端结构
2. PostgreSQL / MySQL 双业务测试库
3. Aspire 启动时自动执行的 EF Core 迁移与幂等测试数据初始化
4. PostgreSQL / MySQL MCP 连接配置、工具发现与工具执行
5. 前端最小可用 MCP 管理界面
6. Agent 场景下基于 `ApprovalRequiredAIFunction` 的工具审核能力

## 2. 总体架构

建议解决方案维持以下分层：

- `AIDbOptimize.Domain`
- `AIDbOptimize.Application`
- `AIDbOptimize.Infrastructure`
- `AIDbOptimize.ApiService`
- `AIDbOptimize.DataInit`
- `AIDbOptimize.AppHost`
- `AIDbOptimize.Web`

职责划分：

- `Domain`：订单域模型、值对象、规则、仓储接口
- `Application`：应用服务、查询/命令编排、MCP 管理编排
- `Infrastructure`：EF Core、仓储实现、MCP 调用、初始化执行
- `ApiService`：HTTP API 与控制面自动迁移
- `DataInit`：业务测试库迁移与测试数据初始化
- `AppHost`：Aspire 编排入口
- `Web`：MCP 管理与状态展示前端

## 3. 当前约束

- PostgreSQL 与 MySQL 使用相同的订单 / 订单明细业务模型
- 测试数据初始化的目标规模为 `10w` 级订单与明细，不再按 `1000w` 规划
- 订单测试数据通过 **EF Core 迁移文件中的 SQL** 生成
- 幂等性由 **EF Core migration history** 天然保证
- `data_initialization_runs` 主要用于前端状态可视化，不是幂等主判据
- `ApprovalRequired` 仅在 Agent 工具装配阶段生效
- 前端当前采用“嵌入首页的 MCP 管理视图”，不是独立 `/mcp` 路由
- 当前 MCP 仍是最小骨架：
  - 工具发现不是实时 `tools/list`
  - 工具执行不是实时 `tools/call`

## 4. 下一阶段重点

下一轮实现重点固定为：

1. 修正 `DataInit` 状态逻辑，避免未造数就被标记为完成
2. 将 `10w` 级测试数据下沉到 EF Core 迁移 SQL
3. 用真实 MCP `tools/list / tools/call` 替换当前最小骨架
4. 将 MCP 工具转换为 Agent 所需的 `Tools`
5. 对 `ApprovalRequired` 工具接入 `ApprovalRequiredAIFunction`

## 5. 文档关系

本文件只描述总体目标与边界。

实现前请继续阅读：

- [详细设计](./detailed-design.md)
- [任务清单](./tasks.md)

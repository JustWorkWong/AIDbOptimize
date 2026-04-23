# AIDbOptimize DDD + MCP + 测试数据初始化总体设计

## 1. 目标

在当前 `Aspire + API + Web + PostgreSQL + MySQL + Redis + RabbitMQ` 骨架基础上，升级为一套可持续演进的正式架构，用于支撑：

1. 基于 DDD 的订单域后端结构
2. PostgreSQL / MySQL 双业务测试库
3. Aspire 启动时自动执行的 EF Core 迁移与幂等测试数据初始化
4. PostgreSQL / MySQL MCP 连接配置、工具发现与工具执行
5. 前端最小可用 MCP 管理页
6. Agent 场景下基于 `ApprovalRequiredAIFunction` 的工具审核能力

## 2. 总体架构

建议解决方案扩展为：

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
- `DataInit`：业务测试库迁移与大数据初始化
- `AppHost`：Aspire 编排入口
- `Web`：MCP 管理与状态展示前端

## 3. 核心约束

- PostgreSQL 与 MySQL 使用相同的订单 / 订单明细业务模型
- 测试数据初始化使用 SQL 批量生成
- 数据规模为 1000 万级订单及明细
- 幂等语义固定为“已初始化则跳过”
- DDD 采用标准分层，不做过重模式堆叠
- 工具权限枚举在前端可配置
- `ApprovalRequired` 仅在 Agent 工具装配阶段生效
- 前端插入能力采用通用工具执行器，不做专用订单表单

## 4. 文档关系

本文件只描述总体目标与边界。

实现前请继续阅读：

- [详细设计](./detailed-design.md)
- [任务清单](./tasks.md)

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

解决方案当前维持以下分层：

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
- 测试数据初始化目标规模固定为 `10w` 级订单与明细
- 订单测试数据通过 **EF Core 迁移文件中的 SQL** 生成
- 幂等性由 **EF Core migration history** 天然保证
- `data_initialization_runs` 主要用于前端状态可视化，不作为幂等主判据
- 前端当前采用“嵌入首页的 MCP 管理视图”，不是独立 `/mcp` 路由

## 4. 当前进展

已经完成：

- DDD 基础分层与订单域骨架
- 控制面数据库、业务测试库和三套迁移
- `10w` 级测试数据迁移
- 控制面默认 MCP 连接种子
- 真实 MCP `tools/list / tools/call` 最小链路
- Agent 工具装配到真实 `AIFunction`
- `ApprovalRequiredAIFunction` 已按工具配置生效
- 前端 MCP 管理视图第一版

仍未完成：

- 更丰富的前端写入型工具管理体验
- MySQL 写入型 MCP 工具的长期方案统一

## 5. 下一阶段重点

下一轮重点固定为：

1. 完善前端对真实写入型工具的管理体验
2. 明确 MySQL 写入型 MCP 工具的长期方案

## 6. 文档关系

本文件只描述总体目标与边界。

实现前请继续阅读：

- [详细设计](./detailed-design.md)
- [任务清单](./tasks.md)

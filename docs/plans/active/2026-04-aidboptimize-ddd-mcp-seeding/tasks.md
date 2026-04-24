# AIDbOptimize DDD + MCP + 测试数据初始化原子任务清单

## 阶段 0：文档基线

- [x] T0.1 建立 `docs` 目录结构
- [x] T0.2 输出总体设计文档
- [x] T0.3 输出详细设计文档
- [x] T0.4 输出原子任务清单

## 阶段 1：解决方案分层重构

- [x] T1.1 新增 `AIDbOptimize.Domain` 项目
- [x] T1.2 新增 `AIDbOptimize.Application` 项目
- [x] T1.3 新增 `AIDbOptimize.Infrastructure` 项目
- [x] T1.4 新增 `AIDbOptimize.DataInit` 项目
- [x] T1.5 配置项目引用关系
- [x] T1.6 调整 `ApiService` 引用到新分层
- [x] T1.7 调整 `AppHost` 引用到新分层与 `DataInit`

## 阶段 2：订单域 DDD 模型

- [x] T2.1 新增订单状态枚举 `OrderStatus`
- [x] T2.2 新增值对象 `OrderNumber`
- [x] T2.3 新增值对象 `Money`
- [x] T2.4 新增实体 `OrderItem`
- [x] T2.5 新增聚合根 `Order`
- [x] T2.6 新增仓储接口 `IOrderRepository`

## 阶段 3：控制面领域模型

- [x] T3.1 新增数据库引擎枚举 `DatabaseEngine`
- [x] T3.2 新增工具权限枚举 `ToolApprovalMode`
- [x] T3.3 新增 MCP 连接状态枚举 `McpConnectionStatus`
- [x] T3.4 新增数据初始化状态枚举 `DataInitializationState`

## 阶段 4：控制面数据库模型与迁移

- [x] T4.1 新增控制面实体 `McpConnectionEntity`
- [x] T4.2 新增控制面实体 `McpToolEntity`
- [x] T4.3 新增控制面实体 `McpToolExecutionEntity`
- [x] T4.4 新增控制面实体 `DataInitializationRunEntity`
- [x] T4.5 新增 `ControlPlaneDbContext`
- [x] T4.6 新增控制面设计时工厂
- [x] T4.7 生成控制面首个迁移
- [x] T4.8 在 `ApiService` 中注册 `ControlPlaneDbContext`
- [x] T4.9 新增控制面自动迁移 HostedService

## 阶段 5：业务测试库 EF Core 模型

- [x] T5.1 新增 PostgreSQL 业务测试库实体映射
- [x] T5.2 新增 MySQL 业务测试库实体映射
- [x] T5.3 新增 `PostgreSqlLabDbContext`
- [x] T5.4 新增 `MySqlLabDbContext`
- [x] T5.5 新增 PostgreSQL 设计时工厂
- [x] T5.6 新增 MySQL 设计时工厂
- [x] T5.7 生成 PostgreSQL 业务测试库首个迁移
- [x] T5.8 生成 MySQL 业务测试库首个迁移

## 阶段 6：AppHost 数据库编排

- [x] T6.1 在 AppHost 中新增 PostgreSQL 控制面库
- [x] T6.2 在 AppHost 中新增 PostgreSQL 业务测试库
- [x] T6.3 在 AppHost 中新增 MySQL 业务测试库
- [x] T6.4 在 AppHost 中接入 `DataInit` 项目

## 阶段 7：DataInit 项目骨架

- [x] T7.1 新增 `IDataInitializer` 抽象
- [x] T7.2 新增 `InitializationStateService`
- [x] T7.3 新增 `PostgreSqlLabInitializer`
- [x] T7.4 新增 `MySqlLabInitializer`
- [x] T7.5 新增 `DataInitializationHostedService`

## 阶段 8：10w 级测试数据初始化

- [x] T8.1 将 PostgreSQL 造数下沉到 EF Core 迁移 SQL
- [x] T8.2 将 MySQL 造数下沉到 EF Core 迁移 SQL
- [x] T8.3 将目标规模统一为 `10w`
- [x] T8.4 修正 `DataInit` 状态逻辑
- [x] T8.5 首次启动完成 PostgreSQL 10w 数据灌库
- [x] T8.6 首次启动完成 MySQL 10w 数据灌库
- [x] T8.7 二次启动验证幂等跳过

## 阶段 9：MCP 持久化与发现

- [x] T9.1 新增 `IMcpConnectionRepository`
- [x] T9.2 新增 `IMcpToolRepository`
- [x] T9.3 实现 `McpConnectionRepository`
- [x] T9.4 实现 `McpToolRepository`
- [x] T9.5 新增真实 MCP client/session factory
- [x] T9.6 用真实 `tools/list` 实现工具发现
- [x] T9.7 写入默认 PostgreSQL / MySQL MCP 连接

## 阶段 10：工具权限与执行

- [x] T10.1 新增工具执行服务接口
- [x] T10.2 用真实 `tools/call` 实现通用工具执行
- [x] T10.3 落库真实工具执行记录
- [x] T10.4 实现工具权限更新逻辑
- [x] T10.5 实现 Agent 工具装配到真实 Agent Tools
- [x] T10.6 对 `ApprovalRequired` 工具接入 `ApprovalRequiredAIFunction`

## 阶段 11：API 落地

- [x] T11.1 新增 MCP 连接查询接口
- [x] T11.2 新增 MCP 连接新增接口
- [x] T11.3 新增 MCP 连接更新接口
- [x] T11.4 新增工具发现接口
- [x] T11.5 新增工具列表接口
- [x] T11.6 新增工具权限更新接口
- [x] T11.7 新增真实工具执行接口
- [x] T11.8 新增真实执行记录查询接口
- [x] T11.9 新增初始化状态接口

## 阶段 12：前端 MCP 管理视图

- [x] T12.1 新增前端 MCP 数据模型
- [x] T12.2 新增前端 MCP API 封装
- [x] T12.3 新增连接表格组件
- [x] T12.4 新增工具表格组件
- [x] T12.5 新增通用工具执行器组件
- [x] T12.6 新增初始化状态面板组件
- [x] T12.7 在 `App.vue` 中实现 MCP 管理视图
- [x] T12.8 在首页加入 MCP 管理视图入口

## 阶段 13：集成验证

- [x] T13.1 解决方案编译通过
- [x] T13.2 前端编译通过
- [x] T13.3 Aspire 启动后控制面库迁移成功
- [x] T13.4 Aspire 启动后 PostgreSQL / MySQL 业务测试库迁移成功
- [x] T13.5 首次启动完成 10w 数据灌库
- [x] T13.6 二次启动验证幂等跳过
- [x] T13.7 前端默认 MCP 连接可见
- [x] T13.8 点击“获取工具”可拉取真实 PostgreSQL / MySQL 工具
- [x] T13.9 通用工具执行器可执行真实工具
- [x] T13.10 `ApprovalRequiredAIFunction` 包装逻辑验证通过

## 备注

- 当前 PostgreSQL 默认 MCP 连接使用官方 `@modelcontextprotocol/server-postgres`
- 当前 MySQL 默认 MCP 连接使用社区 `mysql-mcp-server`
- 后续如果要支持 MySQL 写入型工具，需要单独确定新的 MySQL MCP 服务器方案

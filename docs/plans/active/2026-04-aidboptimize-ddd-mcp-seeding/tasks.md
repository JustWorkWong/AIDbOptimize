# AIDbOptimize DDD + MCP + 测试数据初始化原子任务清单

说明：

- 本清单用于后续 agent 逐项执行并回填状态。
- 每项任务都尽量保持“单一产出、单一验证目标”。
- 除非明确标注可并行，否则默认串行执行。
- 完成后请直接把对应项从 `[ ]` 改成 `[x]`。
- 实现前必须先阅读：
  - `design.md`
  - `detailed-design.md`

状态约定：

- `[ ]` 未开始
- `[x]` 已完成

字段约定：

- `依赖`：必须先完成的任务
- `产出物`：本任务要新增或修改的核心文件/类
- `验证`：完成后必须执行的命令或检查动作
- `完成标准`：判断该任务是否真正完成

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
  - 依赖：T5.7
  - 产出物：
    - `PostgreSqlLab` 新迁移文件
  - 验证：
    - `dotnet tool run dotnet-ef migrations add ...`
  - 完成标准：
    - PostgreSQL 迁移中包含订单与明细的批量造数 SQL

- [x] T8.2 将 MySQL 造数下沉到 EF Core 迁移 SQL
  - 依赖：T5.8
  - 产出物：
    - `MySqlLab` 新迁移文件
  - 验证：
    - `dotnet tool run dotnet-ef migrations add ...`
  - 完成标准：
    - MySQL 迁移中包含订单与明细的批量造数 SQL

- [x] T8.3 将目标规模统一为 `10w`
  - 依赖：T8.1, T8.2
  - 产出物：
    - 迁移 SQL
    - 相关配置与文档
  - 验证：
    - 迁移代码检查
  - 完成标准：
    - PostgreSQL / MySQL 目标规模都为 10w

- [x] T8.4 修正 `DataInit` 状态逻辑
  - 依赖：T8.1, T8.2
  - 产出物：
    - `DataInitializationHostedService.cs`
    - `PostgreSqlLabInitializer.cs`
    - `MySqlLabInitializer.cs`
  - 验证：
    - 启动后状态与真实造数结果一致
  - 完成标准：
    - 未完成造数时不会被错误标记为 `Completed`

- [x] T8.5 首次启动完成 PostgreSQL 10w 数据灌库
  - 依赖：T8.4
  - 验证：
    - PostgreSQL 订单量检查
  - 完成标准：
    - PostgreSQL 达到目标订单量

- [x] T8.6 首次启动完成 MySQL 10w 数据灌库
  - 依赖：T8.4
  - 验证：
    - MySQL 订单量检查
  - 完成标准：
    - MySQL 达到目标订单量

- [x] T8.7 二次启动验证幂等跳过
  - 依赖：T8.5, T8.6
  - 验证：
    - 二次启动日志
  - 完成标准：
    - 二次启动不会重复执行数据迁移

## 阶段 9：MCP 持久化与发现

- [x] T9.1 新增 `IMcpConnectionRepository`
- [x] T9.2 新增 `IMcpToolRepository`
- [x] T9.3 实现 `McpConnectionRepository`
- [x] T9.4 实现 `McpToolRepository`

- [ ] T9.5 新增真实 MCP client/session factory
  - 依赖：T9.3, T9.4
  - 产出物：
    - `Infrastructure/Mcp/McpClientFactory.cs`
    - `Infrastructure/Mcp/McpProcessSession.cs`
  - 验证：
    - 能建立真实 MCP 会话
  - 完成标准：
    - 后端可根据连接配置启动 MCP server 并拿到 client

- [ ] T9.6 用真实 `tools/list` 实现工具发现
  - 依赖：T9.5
  - 产出物：
    - `Infrastructure/Mcp/McpDiscoveryService.cs`
  - 验证：
    - `POST /api/mcp/connections/{id}/discover-tools`
  - 完成标准：
    - 工具列表来自真实 MCP server，而不是硬编码

- [x] T9.7 写入默认 PostgreSQL / MySQL MCP 连接

## 阶段 10：工具权限与执行

- [x] T10.1 新增工具执行服务接口

- [ ] T10.2 用真实 `tools/call` 实现通用工具执行
  - 依赖：T9.5
  - 产出物：
    - `Infrastructure/Mcp/McpToolExecutionService.cs`
  - 验证：
    - `POST /api/mcp/tools/{toolId}/execute`
  - 完成标准：
    - 执行逻辑通过真实 MCP server 完成

- [ ] T10.3 落库真实工具执行记录
  - 依赖：T10.2
  - 产出物：
    - 执行记录持久化逻辑
  - 验证：
    - `GET /api/mcp/executions`
  - 完成标准：
    - 真实 MCP 执行记录可查询

- [x] T10.4 实现工具权限更新逻辑

- [ ] T10.5 实现 Agent 工具装配到真实 Agent Tools
  - 依赖：T10.4, T10.2
  - 产出物：
    - `AgentToolAssemblyService.cs`
  - 验证：
    - 生成真实 Agent Tools 集合
  - 完成标准：
    - 不再只是返回 `AgentToolDescriptor`

- [ ] T10.6 对 `ApprovalRequired` 工具接入 `ApprovalRequiredAIFunction`
  - 依赖：T10.5
  - 产出物：
    - Agent 工具包装逻辑
  - 验证：
    - 针对 `ApprovalRequired` 工具的装配结果包含审批包装
  - 完成标准：
    - 审核能力在 Agent 场景真实生效

## 阶段 11：API 落地

- [x] T11.1 新增 MCP 连接查询接口
- [x] T11.2 新增 MCP 连接新增接口
- [x] T11.3 新增 MCP 连接更新接口
- [x] T11.4 新增工具发现接口
- [x] T11.5 新增工具列表接口
- [x] T11.6 新增工具权限更新接口

- [ ] T11.7 新增真实工具执行接口
  - 依赖：T10.2
  - 完成标准：
    - 执行接口走真实 MCP `tools/call`

- [ ] T11.8 新增真实执行记录查询接口
  - 依赖：T10.3
  - 完成标准：
    - 返回真实 MCP 调用记录

- [x] T11.9 新增初始化状态接口

## 阶段 12：前端 MCP 管理视图

- [x] T12.1 新增前端 MCP 数据模型
- [x] T12.2 新增前端 MCP API 封装
- [x] T12.3 新增连接表格组件
- [x] T12.4 新增工具表格组件
- [x] T12.5 新增通用工具执行器组件
- [x] T12.6 新增初始化状态面板组件

- [x] T12.7 在 `App.vue` 中实现 MCP 管理视图
  - 依赖：T12.3, T12.4, T12.5, T12.6
  - 产出物：
    - `Web/src/App.vue`
    - `Web/src/components/mcp/*`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 页面包含连接、工具、执行器、初始化状态四个区块

- [x] T12.8 在首页加入 MCP 管理视图入口

## 阶段 13：集成验证

- [x] T13.1 解决方案编译通过
- [x] T13.2 前端编译通过
- [x] T13.3 Aspire 启动后控制面库迁移成功
- [x] T13.4 Aspire 启动后 PostgreSQL / MySQL 业务测试库迁移成功

- [x] T13.5 首次启动完成 10w 数据灌库
- [x] T13.6 二次启动验证幂等跳过

- [ ] T13.7 前端默认 MCP 连接可见
  - 依赖：T13.3, T12.8
  - 完成标准：
    - 在真实页面里确认默认连接可见

- [ ] T13.8 点击“获取工具”可拉取真实 PostgreSQL / MySQL 工具
  - 依赖：T13.7, T9.6
  - 完成标准：
    - 工具来自真实 MCP `tools/list`

- [ ] T13.9 通用工具执行器可执行真实工具
  - 依赖：T13.8, T10.2
  - 完成标准：
    - 前端执行器通过真实 `tools/call` 返回结果

- [ ] T13.10 `ApprovalRequiredAIFunction` 包装逻辑验证通过
  - 依赖：T10.6
  - 完成标准：
    - `ApprovalRequired` 工具被正确包装

## Agent 执行备注

- 如果设计发生变化，先更新：
  1. `design.md`
  2. `detailed-design.md`
  3. `tasks.md`
- 当前代码中的 MCP 与 DataInit 仍有“最小骨架”成分，后续 agent 不应把骨架误判为终态

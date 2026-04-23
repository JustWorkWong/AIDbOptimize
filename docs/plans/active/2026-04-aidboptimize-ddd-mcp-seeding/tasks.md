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
  - 依赖：无
  - 产出物：
    - `docs/`
    - `docs/plans/`
    - `docs/plans/active/`
  - 验证：
    - 目录存在且可访问
  - 完成标准：
    - 后续计划可以在统一目录下持续追加

- [x] T0.2 输出总体设计文档
  - 依赖：T0.1
  - 产出物：
    - `docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/design.md`
  - 验证：
    - 文档可正常打开
  - 完成标准：
    - 文档能够表达总体目标、边界和约束

- [x] T0.3 输出详细设计文档
  - 依赖：T0.2
  - 产出物：
    - `docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/detailed-design.md`
  - 验证：
    - 文档可正常打开
  - 完成标准：
    - 文档包含项目结构、数据结构、接口、类设计、ASCII 草图、伪代码

- [x] T0.4 输出原子任务清单
  - 依赖：T0.3
  - 产出物：
    - `docs/plans/active/2026-04-aidboptimize-ddd-mcp-seeding/tasks.md`
  - 验证：
    - 文档可正常打开
  - 完成标准：
    - 后续 agent 可以按清单逐项推进并回填状态

## 阶段 1：解决方案分层重构

- [ ] T1.1 新增 `AIDbOptimize.Domain` 项目
  - 依赖：T0.4
  - 产出物：
    - `src/AIDbOptimize.Domain/AIDbOptimize.Domain.csproj`
  - 验证：
    - `dotnet sln .\AIDbOptimize.slnx list`
  - 完成标准：
    - `Domain` 项目已存在且已加入解决方案

- [ ] T1.2 新增 `AIDbOptimize.Application` 项目
  - 依赖：T1.1
  - 产出物：
    - `src/AIDbOptimize.Application/AIDbOptimize.Application.csproj`
  - 验证：
    - `dotnet sln .\AIDbOptimize.slnx list`
  - 完成标准：
    - `Application` 项目已存在且已加入解决方案

- [ ] T1.3 新增 `AIDbOptimize.Infrastructure` 项目
  - 依赖：T1.2
  - 产出物：
    - `src/AIDbOptimize.Infrastructure/AIDbOptimize.Infrastructure.csproj`
  - 验证：
    - `dotnet sln .\AIDbOptimize.slnx list`
  - 完成标准：
    - `Infrastructure` 项目已存在且已加入解决方案

- [ ] T1.4 新增 `AIDbOptimize.DataInit` 项目
  - 依赖：T1.3
  - 产出物：
    - `src/AIDbOptimize.DataInit/AIDbOptimize.DataInit.csproj`
  - 验证：
    - `dotnet sln .\AIDbOptimize.slnx list`
  - 完成标准：
    - `DataInit` 项目已存在且已加入解决方案

- [ ] T1.5 配置项目引用关系
  - 依赖：T1.4
  - 产出物：
    - 各项目 `.csproj` 引用关系
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 依赖方向满足 `Domain <- Application <- Infrastructure <- ApiService/AppHost/DataInit`

- [ ] T1.6 调整 `ApiService` 引用到新分层
  - 依赖：T1.5
  - 产出物：
    - `src/AIDbOptimize.ApiService/AIDbOptimize.ApiService.csproj`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - `ApiService` 不再直接承载领域与基础设施细节

- [ ] T1.7 调整 `AppHost` 引用到新分层与 `DataInit`
  - 依赖：T1.6
  - 产出物：
    - `src/AIDbOptimize.AppHost/AIDbOptimize.AppHost.csproj`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - `AppHost` 能引用 `DataInit` 项目并参与编排

## 阶段 2：订单域 DDD 模型

- [ ] T2.1 新增订单状态枚举 `OrderStatus`
  - 依赖：T1.1
  - 产出物：
    - `Domain/Orders/Enums/OrderStatus.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 枚举值与详细设计一致

- [ ] T2.2 新增值对象 `OrderNumber`
  - 依赖：T2.1
  - 产出物：
    - `Domain/Orders/ValueObjects/OrderNumber.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 具备非空与格式校验

- [ ] T2.3 新增值对象 `Money`
  - 依赖：T2.2
  - 产出物：
    - `Domain/Orders/ValueObjects/Money.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 具备金额规范化与基础运算能力

- [ ] T2.4 新增实体 `OrderItem`
  - 依赖：T2.3
  - 产出物：
    - `Domain/Orders/Entities/OrderItem.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 能表达订单明细核心字段和金额规则

- [ ] T2.5 新增聚合根 `Order`
  - 依赖：T2.4
  - 产出物：
    - `Domain/Orders/Aggregates/Order.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 能创建订单、追加明细、汇总总金额、变更状态

- [ ] T2.6 新增仓储接口 `IOrderRepository`
  - 依赖：T2.5
  - 产出物：
    - `Domain/Orders/Repositories/IOrderRepository.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 接口足以支撑订单聚合保存与读取

## 阶段 3：控制面领域模型

- [ ] T3.1 新增数据库引擎枚举 `DatabaseEngine`
  - 依赖：T1.1
  - 产出物：
    - `Domain/Mcp/Enums/DatabaseEngine.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 枚举至少包含 `PostgreSql` 和 `MySql`

- [ ] T3.2 新增工具权限枚举 `ToolApprovalMode`
  - 依赖：T3.1
  - 产出物：
    - `Domain/Mcp/Enums/ToolApprovalMode.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 枚举至少包含 `NoApproval` 和 `ApprovalRequired`

- [ ] T3.3 新增 MCP 连接状态枚举 `McpConnectionStatus`
  - 依赖：T3.2
  - 产出物：
    - `Domain/Mcp/Enums/McpConnectionStatus.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 状态值与详细设计一致

- [ ] T3.4 新增数据初始化状态枚举 `DataInitializationState`
  - 依赖：T3.3
  - 产出物：
    - `Domain/Seed/Enums/DataInitializationState.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 至少包含 `NotStarted` / `InProgress` / `Completed` / `Failed`

## 阶段 4：控制面数据库模型与迁移

- [ ] T4.1 新增控制面实体 `McpConnectionEntity`
  - 依赖：T1.3, T3.3
  - 产出物：
    - `Infrastructure/Persistence/Entities/McpConnectionEntity.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 字段与详细设计中的 `mcp_connections` 一致

- [ ] T4.2 新增控制面实体 `McpToolEntity`
  - 依赖：T4.1, T3.2
  - 产出物：
    - `Infrastructure/Persistence/Entities/McpToolEntity.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 字段与详细设计中的 `mcp_tools` 一致

- [ ] T4.3 新增控制面实体 `McpToolExecutionEntity`
  - 依赖：T4.2
  - 产出物：
    - `Infrastructure/Persistence/Entities/McpToolExecutionEntity.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 字段与详细设计中的 `mcp_tool_executions` 一致

- [ ] T4.4 新增控制面实体 `DataInitializationRunEntity`
  - 依赖：T4.3, T3.4
  - 产出物：
    - `Infrastructure/Persistence/Entities/DataInitializationRunEntity.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 字段与详细设计中的 `data_initialization_runs` 一致

- [ ] T4.5 新增 `ControlPlaneDbContext`
  - 依赖：T4.1, T4.2, T4.3, T4.4
  - 产出物：
    - `Infrastructure/Persistence/ControlPlaneDbContext.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 已注册全部控制面实体与索引/约束

- [ ] T4.6 新增控制面设计时工厂
  - 依赖：T4.5
  - 产出物：
    - `Infrastructure/Persistence/ControlPlaneDesignTimeDbContextFactory.cs`
  - 验证：
    - `dotnet ef migrations list --project .\src\AIDbOptimize.Infrastructure --startup-project .\src\AIDbOptimize.ApiService`
  - 完成标准：
    - 能正常生成和读取迁移

- [ ] T4.7 生成控制面首个迁移
  - 依赖：T4.6
  - 产出物：
    - `Infrastructure/Persistence/Migrations/*`
  - 验证：
    - `dotnet ef migrations add InitialControlPlane ...`
  - 完成标准：
    - 控制面迁移已生成且可编译

- [ ] T4.8 在 `ApiService` 中注册 `ControlPlaneDbContext`
  - 依赖：T4.7
  - 产出物：
    - `ApiService/Program.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - API 启动时可解析控制面库上下文

- [ ] T4.9 新增控制面自动迁移 HostedService
  - 依赖：T4.8
  - 产出物：
    - `ApiService/DatabaseMigrations/ControlPlaneMigrationHostedService.cs`
  - 验证：
    - `dotnet run --project .\src\AIDbOptimize.ApiService\AIDbOptimize.ApiService.csproj`
  - 完成标准：
    - API 启动时自动应用控制面迁移

## 阶段 5：业务测试库 EF Core 模型

- [ ] T5.1 新增 PostgreSQL 业务测试库实体映射
  - 依赖：T2.6, T1.3
  - 产出物：
    - PostgreSQL 订单实体映射文件
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - `orders` / `order_items` 结构与详细设计一致

- [ ] T5.2 新增 MySQL 业务测试库实体映射
  - 依赖：T5.1
  - 产出物：
    - MySQL 订单实体映射文件
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - `orders` / `order_items` 结构与详细设计一致

- [ ] T5.3 新增 `PostgreSqlLabDbContext`
  - 依赖：T5.1
  - 产出物：
    - `Infrastructure/Persistence/PostgreSqlLabDbContext.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - PostgreSQL 业务测试库上下文可用

- [ ] T5.4 新增 `MySqlLabDbContext`
  - 依赖：T5.2
  - 产出物：
    - `Infrastructure/Persistence/MySqlLabDbContext.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - MySQL 业务测试库上下文可用

- [ ] T5.5 新增 PostgreSQL 设计时工厂
  - 依赖：T5.3
  - 产出物：
    - `Infrastructure/Persistence/PostgreSqlLabDesignTimeDbContextFactory.cs`
  - 验证：
    - `dotnet ef migrations list ...`
  - 完成标准：
    - PostgreSQL 业务测试库迁移可生成

- [ ] T5.6 新增 MySQL 设计时工厂
  - 依赖：T5.4
  - 产出物：
    - `Infrastructure/Persistence/MySqlLabDesignTimeDbContextFactory.cs`
  - 验证：
    - `dotnet ef migrations list ...`
  - 完成标准：
    - MySQL 业务测试库迁移可生成

- [ ] T5.7 生成 PostgreSQL 业务测试库首个迁移
  - 依赖：T5.5
  - 产出物：
    - PostgreSQL 业务测试库迁移文件
  - 验证：
    - `dotnet ef migrations add InitialPgLab ...`
  - 完成标准：
    - PostgreSQL 业务测试库迁移已生成

- [ ] T5.8 生成 MySQL 业务测试库首个迁移
  - 依赖：T5.6
  - 产出物：
    - MySQL 业务测试库迁移文件
  - 验证：
    - `dotnet ef migrations add InitialMySqlLab ...`
  - 完成标准：
    - MySQL 业务测试库迁移已生成

## 阶段 6：AppHost 数据库编排

- [ ] T6.1 在 AppHost 中新增 PostgreSQL 控制面库
  - 依赖：T1.7
  - 产出物：
    - `AppHost.cs`
    - `appsettings.json`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - PostgreSQL 控制面库资源已定义

- [ ] T6.2 在 AppHost 中新增 PostgreSQL 业务测试库
  - 依赖：T6.1
  - 产出物：
    - `AppHost.cs`
    - `appsettings.json`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - PostgreSQL 业务测试库资源已定义

- [ ] T6.3 在 AppHost 中新增 MySQL 业务测试库
  - 依赖：T6.2
  - 产出物：
    - `AppHost.cs`
    - `appsettings.json`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - MySQL 业务测试库资源已定义

- [ ] T6.4 在 AppHost 中接入 `DataInit` 项目
  - 依赖：T1.7, T6.3
  - 产出物：
    - `AppHost.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - Aspire 启动时会拉起 `DataInit`

## 阶段 7：DataInit 项目骨架

- [ ] T7.1 新增 `IDataInitializer` 抽象
  - 依赖：T1.4
  - 产出物：
    - `DataInit/Abstractions/IDataInitializer.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - PostgreSQL / MySQL 初始化器可复用统一接口

- [ ] T7.2 新增 `InitializationStateService`
  - 依赖：T4.4, T4.5, T7.1
  - 产出物：
    - `DataInit/Services/InitializationStateService.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 可读写初始化状态

- [ ] T7.3 新增 `PostgreSqlLabInitializer`
  - 依赖：T5.7, T7.2
  - 产出物：
    - `DataInit/Services/PostgreSqlLabInitializer.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - PostgreSQL 初始化器骨架可运行

- [ ] T7.4 新增 `MySqlLabInitializer`
  - 依赖：T5.8, T7.2
  - 产出物：
    - `DataInit/Services/MySqlLabInitializer.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - MySQL 初始化器骨架可运行

- [ ] T7.5 新增 `DataInitializationHostedService`
  - 依赖：T7.3, T7.4
  - 产出物：
    - `DataInit/HostedServices/DataInitializationHostedService.cs`
    - `DataInit/Program.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - `DataInit` 项目能启动并编排两套初始化器

## 阶段 8：大规模测试数据初始化

- [ ] T8.1 实现 PostgreSQL 初始化状态检查
  - 依赖：T7.3
  - 产出物：
    - `PostgreSqlLabInitializer.cs`
  - 验证：
    - 单元或集成验证：已完成状态时直接返回
  - 完成标准：
    - PostgreSQL 已初始化时不重复造数

- [ ] T8.2 实现 MySQL 初始化状态检查
  - 依赖：T7.4
  - 产出物：
    - `MySqlLabInitializer.cs`
  - 验证：
    - 单元或集成验证：已完成状态时直接返回
  - 完成标准：
    - MySQL 已初始化时不重复造数

- [ ] T8.3 实现 PostgreSQL 批量订单 SQL 生成
  - 依赖：T8.1
  - 产出物：
    - PostgreSQL 批量订单 SQL 生成逻辑
  - 验证：
    - 本地小批量试跑
  - 完成标准：
    - 可按批次生成订单数据

- [ ] T8.4 实现 PostgreSQL 批量订单明细 SQL 生成
  - 依赖：T8.3
  - 产出物：
    - PostgreSQL 批量明细 SQL 生成逻辑
  - 验证：
    - 本地小批量试跑
  - 完成标准：
    - 可按批次生成订单明细数据

- [ ] T8.5 实现 PostgreSQL 总金额回填 SQL
  - 依赖：T8.4
  - 产出物：
    - PostgreSQL 汇总更新逻辑
  - 验证：
    - 检查订单总金额与明细汇总一致
  - 完成标准：
    - `orders.total_amount` 正确

- [ ] T8.6 实现 MySQL 批量订单 SQL 生成
  - 依赖：T8.2
  - 产出物：
    - MySQL 批量订单 SQL 生成逻辑
  - 验证：
    - 本地小批量试跑
  - 完成标准：
    - 可按批次生成订单数据

- [ ] T8.7 实现 MySQL 批量订单明细 SQL 生成
  - 依赖：T8.6
  - 产出物：
    - MySQL 批量明细 SQL 生成逻辑
  - 验证：
    - 本地小批量试跑
  - 完成标准：
    - 可按批次生成订单明细数据

- [ ] T8.8 实现 MySQL 总金额回填 SQL
  - 依赖：T8.7
  - 产出物：
    - MySQL 汇总更新逻辑
  - 验证：
    - 检查订单总金额与明细汇总一致
  - 完成标准：
    - `orders.total_amount` 正确

- [ ] T8.9 打通 PostgreSQL 1000 万级初始化全链路
  - 依赖：T8.5
  - 产出物：
    - PostgreSQL 全量初始化流程
  - 验证：
    - 首次启动完成 PostgreSQL 全量初始化
  - 完成标准：
    - PostgreSQL 达到目标订单量并写入完成标记

- [ ] T8.10 打通 MySQL 1000 万级初始化全链路
  - 依赖：T8.8
  - 产出物：
    - MySQL 全量初始化流程
  - 验证：
    - 首次启动完成 MySQL 全量初始化
  - 完成标准：
    - MySQL 达到目标订单量并写入完成标记

- [ ] T8.11 验证初始化失败时状态能正确落为 `Failed`
  - 依赖：T8.9, T8.10
  - 产出物：
    - 初始化失败保护逻辑
  - 验证：
    - 人工制造失败后检查状态记录
  - 完成标准：
    - 不会把失败初始化误判为完成

## 阶段 9：MCP 持久化与发现

- [ ] T9.1 新增 `IMcpConnectionRepository`
  - 依赖：T4.1, T1.2
  - 产出物：
    - `Application/Abstractions/Persistence/IMcpConnectionRepository.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 连接存取接口已定义

- [ ] T9.2 新增 `IMcpToolRepository`
  - 依赖：T4.2, T1.2
  - 产出物：
    - `Application/Abstractions/Persistence/IMcpToolRepository.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 工具存取接口已定义

- [ ] T9.3 实现 `McpConnectionRepository`
  - 依赖：T9.1, T4.5
  - 产出物：
    - `Infrastructure/Persistence/Repositories/McpConnectionRepository.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 连接可持久化读写

- [ ] T9.4 实现 `McpToolRepository`
  - 依赖：T9.2, T4.5
  - 产出物：
    - `Infrastructure/Persistence/Repositories/McpToolRepository.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 工具可持久化读写

- [ ] T9.5 新增 MCP client 工厂
  - 依赖：T9.3, T9.4
  - 产出物：
    - `Infrastructure/Mcp/McpClientFactory.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 可基于连接配置创建 MCP client

- [ ] T9.6 新增工具发现服务
  - 依赖：T9.5
  - 产出物：
    - `Application/Mcp/Services/McpDiscoveryAppService.cs`
    - `Infrastructure/Mcp/McpDiscoveryService.cs`
  - 验证：
    - 可对单条连接执行 `tools/list`
  - 完成标准：
    - 发现结果可映射并持久化

- [ ] T9.7 写入默认 PostgreSQL / MySQL MCP 连接
  - 依赖：T9.3, T6.3
  - 产出物：
    - 默认连接种子逻辑
  - 验证：
    - 启动后连接表存在两条默认连接
  - 完成标准：
    - 前端无需手工录入默认连接

## 阶段 10：工具权限与执行

- [ ] T10.1 新增工具执行服务接口
  - 依赖：T9.4, T1.2
  - 产出物：
    - `Application/Abstractions/Mcp/IMcpToolExecutionService.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 工具执行抽象已定义

- [ ] T10.2 实现工具执行服务
  - 依赖：T10.1, T9.5
  - 产出物：
    - `Infrastructure/Mcp/McpToolExecutionService.cs`
  - 验证：
    - 能执行一次工具调用并拿到返回结果
  - 完成标准：
    - 工具执行结果可返回给上层

- [ ] T10.3 落库工具执行记录
  - 依赖：T10.2, T4.3
  - 产出物：
    - 执行记录持久化逻辑
  - 验证：
    - 执行一次工具后数据库有记录
  - 完成标准：
    - 成功/失败都能记录

- [ ] T10.4 实现工具权限更新逻辑
  - 依赖：T9.4, T3.2
  - 产出物：
    - `Application/Mcp/Services/McpToolPermissionAppService.cs`
  - 验证：
    - 可更新 `approval_mode`
  - 完成标准：
    - 前端修改权限枚举后可持久保存

- [ ] T10.5 实现 Agent 工具装配服务
  - 依赖：T10.4
  - 产出物：
    - `Application/Mcp/Services/AgentToolAssemblyService.cs`
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 可从已配置工具生成 Agent 可用工具集合

- [ ] T10.6 对 `ApprovalRequired` 工具接入 `ApprovalRequiredAIFunction`
  - 依赖：T10.5
  - 产出物：
    - Agent 工具包装逻辑
  - 验证：
    - 针对 `ApprovalRequired` 工具的装配结果包含审批包装
  - 完成标准：
    - 审核能力在 Agent 场景真实生效

## 阶段 11：API 落地

- [ ] T11.1 新增 MCP 连接查询接口
  - 依赖：T9.3, T4.9
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `GET /api/mcp/connections`
  - 完成标准：
    - 可返回连接列表

- [ ] T11.2 新增 MCP 连接新增接口
  - 依赖：T11.1
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `POST /api/mcp/connections`
  - 完成标准：
    - 可创建连接配置

- [ ] T11.3 新增 MCP 连接更新接口
  - 依赖：T11.2
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `PUT /api/mcp/connections/{id}`
  - 完成标准：
    - 可更新连接配置

- [ ] T11.4 新增工具发现接口
  - 依赖：T9.6
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `POST /api/mcp/connections/{id}/discover-tools`
  - 完成标准：
    - 可触发工具发现

- [ ] T11.5 新增工具列表接口
  - 依赖：T11.4
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `GET /api/mcp/connections/{id}/tools`
  - 完成标准：
    - 可返回工具列表

- [ ] T11.6 新增工具权限更新接口
  - 依赖：T10.4
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `PUT /api/mcp/tools/{toolId}/approval-mode`
  - 完成标准：
    - 可更新工具权限

- [ ] T11.7 新增工具执行接口
  - 依赖：T10.3
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `POST /api/mcp/tools/{toolId}/execute`
  - 完成标准：
    - 可执行工具并返回结果

- [ ] T11.8 新增执行记录查询接口
  - 依赖：T10.3
  - 产出物：
    - `Api/McpApi.cs`
  - 验证：
    - `GET /api/mcp/executions`
  - 完成标准：
    - 可查询执行记录

- [ ] T11.9 新增初始化状态接口
  - 依赖：T7.5, T8.11
  - 产出物：
    - `Api/DataInitializationApi.cs`
  - 验证：
    - `GET /api/data-init/status`
  - 完成标准：
    - 可返回 PostgreSQL / MySQL 初始化状态

## 阶段 12：前端 MCP 管理页

- [ ] T12.1 新增前端 MCP 数据模型
  - 依赖：T11.1
  - 产出物：
    - `Web/src/models/mcp.ts`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 前端类型与 API 返回结构对齐

- [ ] T12.2 新增前端 MCP API 封装
  - 依赖：T12.1
  - 产出物：
    - `Web/src/api/mcp.ts`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 前端具备 MCP 页面所需调用封装

- [ ] T12.3 新增连接表格组件
  - 依赖：T12.2
  - 产出物：
    - `Web/src/components/mcp/McpConnectionTable.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 可展示默认 PostgreSQL / MySQL MCP 连接

- [ ] T12.4 新增工具表格组件
  - 依赖：T12.2
  - 产出物：
    - `Web/src/components/mcp/McpToolTable.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 可展示工具列表和权限枚举

- [ ] T12.5 新增通用工具执行器组件
  - 依赖：T12.2
  - 产出物：
    - `Web/src/components/mcp/McpToolExecutor.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 可输入 JSON 参数并执行工具

- [ ] T12.6 新增初始化状态面板组件
  - 依赖：T11.9
  - 产出物：
    - `Web/src/components/mcp/DataInitStatusPanel.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 可展示 PostgreSQL / MySQL 初始化状态

- [ ] T12.7 新增 MCP 管理页
  - 依赖：T12.3, T12.4, T12.5, T12.6
  - 产出物：
    - `Web/src/pages/mcp/McpManagementPage.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 页面包含连接、工具、执行器、初始化状态四个区块

- [ ] T12.8 在首页加入 MCP 管理页入口
  - 依赖：T12.7
  - 产出物：
    - `Web/src/App.vue`
  - 验证：
    - `npm run build`
  - 完成标准：
    - 首页可跳转或切换到 MCP 管理页

## 阶段 13：集成验证

- [ ] T13.1 解决方案编译通过
  - 依赖：T12.8
  - 产出物：无
  - 验证：
    - `dotnet build .\AIDbOptimize.slnx`
  - 完成标准：
    - 所有 .NET 项目编译通过

- [ ] T13.2 前端编译通过
  - 依赖：T12.8
  - 产出物：无
  - 验证：
    - `npm run build`
  - 完成标准：
    - 前端打包通过

- [ ] T13.3 Aspire 启动后控制面库迁移成功
  - 依赖：T13.1
  - 产出物：无
  - 验证：
    - `dotnet run --project .\src\AIDbOptimize.AppHost\AIDbOptimize.AppHost.csproj`
  - 完成标准：
    - 控制面库可自动迁移

- [ ] T13.4 Aspire 启动后 PostgreSQL / MySQL 业务测试库迁移成功
  - 依赖：T13.3
  - 产出物：无
  - 验证：
    - 启动日志 + 数据库验证
  - 完成标准：
    - 两套业务测试库结构正确

- [ ] T13.5 首次启动完成 1000 万级数据灌库
  - 依赖：T13.4
  - 产出物：无
  - 验证：
    - 两套数据库中订单量达到目标值
  - 完成标准：
    - PostgreSQL / MySQL 均完成首轮全量初始化

- [ ] T13.6 二次启动验证幂等跳过
  - 依赖：T13.5
  - 产出物：无
  - 验证：
    - 二次启动日志
  - 完成标准：
    - 不重复生成 1000 万数据

- [ ] T13.7 前端默认 MCP 连接可见
  - 依赖：T13.3, T12.8
  - 产出物：无
  - 验证：
    - 页面检查
  - 完成标准：
    - 默认 PostgreSQL / MySQL 连接均可见

- [ ] T13.8 点击“获取工具”可拉取 PostgreSQL / MySQL 工具
  - 依赖：T13.7
  - 产出物：无
  - 验证：
    - 页面操作 + 控制面数据库结果
  - 完成标准：
    - 工具列表可展示并落库

- [ ] T13.9 通用工具执行器可执行插入类工具
  - 依赖：T13.8
  - 产出物：无
  - 验证：
    - 页面执行一次插入类工具
  - 完成标准：
    - 返回成功结果并写入执行记录

- [ ] T13.10 `ApprovalRequiredAIFunction` 包装逻辑验证通过
  - 依赖：T10.6
  - 产出物：无
  - 验证：
    - 运行时或测试验证装配结果
  - 完成标准：
    - `ApprovalRequired` 工具被正确包装

## 并行建议

可有限并行：

- 阶段 2 与阶段 3 前半段可并行
- 阶段 12 的组件开发可在 API 稳定后局部并行

不建议并行：

- 阶段 8 大规模初始化链路
- 阶段 10 Agent 工具包装
- 阶段 13 集成验证

## Agent 执行备注

- 如果某个任务实现范围明显超出单次提交，可继续拆成 `Tn.x.y`
- 如果设计发生变化，先更新：
  1. `design.md`
  2. `detailed-design.md`
  3. `tasks.md`
- 每次执行后优先回填任务状态，再开启下一个任务

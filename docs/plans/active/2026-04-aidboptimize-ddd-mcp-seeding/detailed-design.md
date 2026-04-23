# AIDbOptimize DDD + MCP + 测试数据初始化详细设计方案

> 说明  
> `design.md` 用于描述总体目标与架构方向。  
> 本文档用于提供后续 agent 可直接落地的详细设计，包括包依赖、项目结构、数据结构、接口、页面草图、类设计和关键伪代码。

## 1. 实现目标

本阶段要交付一套可运行的“控制面 + 业务测试库 + MCP 管理 + Agent 工具审核适配”架构。

核心能力固定为：

1. 使用 DDD 分层承载订单域
2. PostgreSQL 与 MySQL 使用相同的订单 / 订单明细模型
3. 新增测试数据初始化项目，在 Aspire 启动时自动执行
4. 使用 EF Core Code First 自动迁移
5. 首次启动时对 PostgreSQL / MySQL 业务测试库分别生成 1000 万级订单与订单明细数据
6. 数据初始化具备幂等能力：完成后再次启动直接跳过
7. 前端提供 MCP 管理页，默认带好 PostgreSQL / MySQL MCP 连接
8. 前端可点击“获取工具”，拉取并配置工具权限枚举
9. 前端可通过通用工具执行器执行插入类工具
10. Agent 场景下，对标记为 `ApprovalRequired` 的工具使用 `ApprovalRequiredAIFunction`

## 2. 解决方案结构

建议解决方案扩展为：

```text
E:\Db
├─ docs/
│  └─ plans/
│     └─ active/
│        └─ 2026-04-aidboptimize-ddd-mcp-seeding/
│           ├─ design.md
│           ├─ detailed-design.md
│           └─ tasks.md
├─ src/
│  ├─ AIDbOptimize.Domain/
│  ├─ AIDbOptimize.Application/
│  ├─ AIDbOptimize.Infrastructure/
│  ├─ AIDbOptimize.ApiService/
│  ├─ AIDbOptimize.DataInit/
│  ├─ AIDbOptimize.AppHost/
│  └─ AIDbOptimize.Web/
└─ AIDbOptimize.slnx
```

### 2.1 项目职责

#### `AIDbOptimize.Domain`

只放纯领域对象，不依赖 EF Core、MCP、Web。

建议目录：

```text
Domain/
├─ Orders/
│  ├─ Aggregates/
│  │  └─ Order.cs
│  ├─ Entities/
│  │  └─ OrderItem.cs
│  ├─ ValueObjects/
│  │  ├─ Money.cs
│  │  └─ OrderNumber.cs
│  ├─ Enums/
│  │  └─ OrderStatus.cs
│  └─ Repositories/
│     └─ IOrderRepository.cs
├─ Mcp/
│  ├─ Enums/
│  │  ├─ DatabaseEngine.cs
│  │  ├─ ToolApprovalMode.cs
│  │  └─ McpConnectionStatus.cs
│  └─ Models/
│     ├─ McpConnectionDefinition.cs
│     └─ McpToolDefinition.cs
└─ Seed/
   └─ Enums/
      └─ DataInitializationState.cs
```

#### `AIDbOptimize.Application`

负责“用例编排”，不直接关心具体数据库驱动或 MCP 实现细节。

建议目录：

```text
Application/
├─ Orders/
│  ├─ Commands/
│  ├─ Queries/
│  └─ Services/
├─ Mcp/
│  ├─ Dtos/
│  ├─ Commands/
│  ├─ Queries/
│  └─ Services/
├─ DataInitialization/
│  ├─ Dtos/
│  └─ Services/
└─ Abstractions/
   ├─ Persistence/
   ├─ Mcp/
   └─ Agents/
```

#### `AIDbOptimize.Infrastructure`

负责：

- EF Core `DbContext`
- 实体映射
- 迁移
- SQL 批量造数实现
- MCP client / server 启动适配
- 仓储实现

#### `AIDbOptimize.ApiService`

负责：

- HTTP API 暴露
- 控制面数据库自动迁移
- 应用服务注册

#### `AIDbOptimize.DataInit`

负责：

- PostgreSQL / MySQL 业务测试库迁移
- 幂等检测
- 大规模数据初始化
- 初始化状态落库

#### `AIDbOptimize.AppHost`

负责：

- PostgreSQL / MySQL / Redis / RabbitMQ 编排
- 新增业务测试库引用
- 启动 `ApiService`
- 启动 `Web`
- 启动 `DataInit`

#### `AIDbOptimize.Web`

负责：

- 现有首页保留
- 新增 MCP 管理页

## 3. NuGet / npm 包设计

### 3.1 .NET 包

#### `AIDbOptimize.Domain`

不新增第三方包。

#### `AIDbOptimize.Application`

建议包：

- `FluentValidation`

#### `AIDbOptimize.Infrastructure`

建议包：

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Pomelo.EntityFrameworkCore.MySql`
- `MySqlConnector`
- `Npgsql`
- `ModelContextProtocol`
- `Microsoft.Extensions.Hosting.Abstractions`

如果 Agent 包装层放在这里，还需要：

- `Microsoft.Extensions.AI`
- 与 `ApprovalRequiredAIFunction` 所属命名空间一致的相关包

#### `AIDbOptimize.ApiService`

建议包：

- `Swashbuckle.AspNetCore`
- `FluentValidation.DependencyInjectionExtensions`

#### `AIDbOptimize.DataInit`

建议包：

- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging.Console`
- `Microsoft.EntityFrameworkCore`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Pomelo.EntityFrameworkCore.MySql`

#### `AIDbOptimize.AppHost`

保留并扩展：

- `Aspire.Hosting.JavaScript`
- `Aspire.Hosting.PostgreSQL`
- `Aspire.Hosting.MySql`
- `Aspire.Hosting.Redis`
- `Aspire.Hosting.RabbitMQ`

### 3.2 前端包

当前前端保持轻量，建议新增：

- 不强制增加 UI 框架
- 若需要更稳定的 JSON 编辑体验，可加：
  - `zod`
  - `@vueuse/core`

首版不建议引入重型组件库，避免页面管理台复杂化。

## 4. 数据库设计

### 4.1 控制面数据库

控制面数据库使用 PostgreSQL，建议库名：

- `aidbopt_control`

#### 表：`mcp_connections`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `uuid` | 主键 |
| `name` | `varchar(100)` | 连接名称 |
| `engine` | `varchar(20)` | `postgresql` / `mysql` |
| `display_name` | `varchar(100)` | 前端展示名 |
| `server_command` | `varchar(500)` | MCP server 启动命令 |
| `server_arguments_json` | `jsonb` | 启动参数数组 |
| `environment_json` | `jsonb` | 环境变量字典 |
| `database_connection_string` | `text` | 目标数据库连接串 |
| `database_name` | `varchar(100)` | 目标数据库名 |
| `is_default` | `boolean` | 是否默认 |
| `status` | `varchar(20)` | `Draft` / `Ready` / `Unavailable` |
| `last_discovered_at` | `timestamp` | 最后发现工具时间 |
| `created_at` | `timestamp` | 创建时间 |
| `updated_at` | `timestamp` | 更新时间 |

#### 表：`mcp_tools`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `uuid` | 主键 |
| `connection_id` | `uuid` | 所属连接 |
| `tool_name` | `varchar(200)` | 工具名 |
| `display_name` | `varchar(200)` | 展示名 |
| `description` | `text` | 工具描述 |
| `input_schema_json` | `jsonb` | MCP 返回的输入 schema |
| `approval_mode` | `varchar(30)` | `NoApproval` / `ApprovalRequired` |
| `is_enabled` | `boolean` | 是否启用 |
| `is_write_tool` | `boolean` | 是否写入型工具 |
| `created_at` | `timestamp` | 创建时间 |
| `updated_at` | `timestamp` | 更新时间 |

#### 表：`mcp_tool_executions`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `uuid` | 主键 |
| `connection_id` | `uuid` | 连接 |
| `tool_id` | `uuid` | 工具 |
| `requested_by` | `varchar(100)` | 操作者标识 |
| `request_payload_json` | `jsonb` | 入参 |
| `response_payload_json` | `jsonb` | 返回值 |
| `status` | `varchar(20)` | `Succeeded` / `Failed` |
| `error_message` | `text` | 错误信息 |
| `created_at` | `timestamp` | 执行时间 |

#### 表：`data_initialization_runs`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `uuid` | 主键 |
| `engine` | `varchar(20)` | `postgresql` / `mysql` |
| `database_name` | `varchar(100)` | 目标业务库 |
| `seed_version` | `varchar(50)` | 种子版本 |
| `target_order_count` | `bigint` | 目标订单数 |
| `state` | `varchar(20)` | `NotStarted` / `InProgress` / `Completed` / `Failed` |
| `started_at` | `timestamp` | 开始时间 |
| `completed_at` | `timestamp` | 完成时间 |
| `error_message` | `text` | 错误信息 |

### 4.2 PostgreSQL / MySQL 业务测试库

建议数据库名：

- PostgreSQL：`aidbopt_lab_pg`
- MySQL：`aidbopt_lab_mysql`

#### 表：`orders`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `bigint` | 主键 |
| `order_number` | `varchar(50)` | 订单号，唯一 |
| `customer_id` | `bigint` | 客户 ID |
| `status` | `varchar(20)` | 订单状态 |
| `total_amount` | `decimal(18,2)` | 总金额 |
| `created_at` | `timestamp/datetime` | 创建时间 |
| `updated_at` | `timestamp/datetime` | 更新时间 |

#### 表：`order_items`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `bigint` | 主键 |
| `order_id` | `bigint` | 订单主键 |
| `product_id` | `bigint` | 商品 ID |
| `product_name` | `varchar(200)` | 商品名 |
| `quantity` | `int` | 数量 |
| `unit_price` | `decimal(18,2)` | 单价 |
| `line_amount` | `decimal(18,2)` | 小计 |
| `created_at` | `timestamp/datetime` | 创建时间 |

## 5. 领域模型设计

### 5.1 枚举

```csharp
public enum OrderStatus
{
    Pending = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}
```

```csharp
public enum ToolApprovalMode
{
    NoApproval = 1,
    ApprovalRequired = 2
}
```

```csharp
public enum DataInitializationState
{
    NotStarted = 1,
    InProgress = 2,
    Completed = 3,
    Failed = 4
}
```

### 5.2 聚合根：`Order`

建议公开行为：

- `Create(...)`
- `AddItem(...)`
- `ChangeStatus(...)`
- `RecalculateTotal()`

建议签名：

```csharp
public sealed class Order
{
    public long Id { get; }
    public OrderNumber OrderNumber { get; }
    public long CustomerId { get; }
    public OrderStatus Status { get; private set; }
    public Money TotalAmount { get; private set; }
    public DateTimeOffset CreatedAt { get; }
    public DateTimeOffset UpdatedAt { get; private set; }

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public static Order Create(OrderNumber orderNumber, long customerId, DateTimeOffset createdAt);
    public void AddItem(long productId, string productName, int quantity, Money unitPrice, DateTimeOffset createdAt);
    public void ChangeStatus(OrderStatus newStatus, DateTimeOffset updatedAt);
    private void RecalculateTotal();
}
```

### 5.3 实体：`OrderItem`

```csharp
public sealed class OrderItem
{
    public long Id { get; }
    public long ProductId { get; }
    public string ProductName { get; }
    public int Quantity { get; }
    public Money UnitPrice { get; }
    public Money LineAmount { get; }
    public DateTimeOffset CreatedAt { get; }
}
```

## 6. AppHost 详细设计

### 6.1 新增数据库资源

在现有实例基础上新增：

- PostgreSQL 控制面库：`aidbopt_control`
- PostgreSQL 业务测试库：`aidbopt_lab_pg`
- MySQL 业务测试库：`aidbopt_lab_mysql`

### 6.2 新增 DataInit 项目资源

AppHost 中新增：

```csharp
builder.AddProject<Projects.AIDbOptimize_DataInit>("data-init")
```

行为要求：

- 等待控制面库、PostgreSQL 业务测试库、MySQL 业务测试库就绪
- 注入相关连接串

## 7. DataInit 详细设计

### 7.1 建议新增类

- `DataInitializationHostedService`
- `IDataInitializer`
- `PostgreSqlLabInitializer`
- `MySqlLabInitializer`
- `InitializationStateService`

### 7.2 初始化伪代码

```csharp
StartAsync():
    await InitializeEngineAsync(PostgreSql)
    await InitializeEngineAsync(MySql)

InitializeEngineAsync(engine):
    state = await stateService.GetLatestAsync(engine, seedVersion)
    if state == Completed:
        return

    await stateService.MarkInProgressAsync(engine, seedVersion, targetCount)

    try:
        await dbContext.Database.MigrateAsync()
        await initializer.SeedLargeDatasetAsync()
        await stateService.MarkCompletedAsync(engine, seedVersion)
    catch ex:
        await stateService.MarkFailedAsync(engine, seedVersion, ex.Message)
        throw
```

## 8. MCP 管理详细设计

### 8.1 后端接口抽象

建议新增：

- `IMcpConnectionRepository`
- `IMcpToolRepository`
- `IMcpDiscoveryService`
- `IMcpToolExecutionService`
- `IAgentToolAssemblyService`

### 8.2 工具发现伪代码

```csharp
DiscoverToolsAsync(connectionId):
    connection = repository.GetById(connectionId)
    client = mcpClientFactory.Create(connection)
    tools = client.ListTools()
    toolRepository.UpsertMany(mappedTools)
    return mappedTools
```

### 8.3 Agent 工具包装伪代码

```csharp
foreach (var tool in tools)
{
    var aiFunction = ConvertToAIFunction(tool);

    if (tool.ApprovalMode == ApprovalRequired)
        assembledTools.Add(new ApprovalRequiredAIFunction(aiFunction));
    else
        assembledTools.Add(aiFunction);
}
```

## 9. API 详细设计

建议新增：

- `GET /api/mcp/connections`
- `POST /api/mcp/connections`
- `PUT /api/mcp/connections/{id}`
- `POST /api/mcp/connections/{id}/discover-tools`
- `GET /api/mcp/connections/{id}/tools`
- `PUT /api/mcp/tools/{toolId}/approval-mode`
- `POST /api/mcp/tools/{toolId}/execute`
- `GET /api/mcp/executions`
- `GET /api/data-init/status`

### 示例：执行工具请求

```json
{
  "payload": {
    "orderNumber": "ORD-20260423-000000001",
    "customerId": 10001,
    "items": [
      {
        "productId": 20001,
        "productName": "Keyboard",
        "quantity": 2,
        "unitPrice": 199.99
      }
    ]
  }
}
```

## 10. 前端页面设计

### 10.1 页面路由

建议新增：

- `/mcp`

### 10.2 页面 ASCII 草图

```text
+----------------------------------------------------------------------------------+
| MCP 管理台                                                                       |
+----------------------------------------------------------------------------------+
| [连接列表]                                                                       |
| -------------------------------------------------------------------------------- |
| 名称                  引擎        数据库              状态        操作            |
| pgsql-lab-default     PostgreSQL  aidbopt_lab_pg      Ready       [获取工具]     |
| mysql-lab-default     MySQL       aidbopt_lab_mysql   Ready       [获取工具]     |
+----------------------------------------------------------------------------------+
| [工具列表]                                                                       |
| -------------------------------------------------------------------------------- |
| 工具名              类型      权限枚举               启用状态     操作            |
| insert_order        写工具    [不需要审核 v]         启用         [执行]          |
| insert_order_item   写工具    [需要审核 v]           启用         [执行]          |
| list_orders         读工具    [不需要审核 v]         启用         [执行]          |
+----------------------------------------------------------------------------------+
| [通用工具执行器]                                                                 |
| -------------------------------------------------------------------------------- |
| 当前连接: [pgsql-lab-default           v]                                        |
| 当前工具: [insert_order                v]                                        |
| 参数 JSON:                                                                        |
| {                                                                                |
|   "orderNumber": "ORD-20260423-000000001",                                      |
|   "customerId": 10001,                                                           |
|   "items": [ ... ]                                                               |
| }                                                                                |
|                                                                  [执行工具]      |
+----------------------------------------------------------------------------------+
| [执行结果]                                                                       |
| -------------------------------------------------------------------------------- |
| 状态: 成功                                                                        |
| 返回: { "inserted": true, "orderId": 123456 }                                   |
+----------------------------------------------------------------------------------+
| [初始化状态]                                                                     |
| -------------------------------------------------------------------------------- |
| PostgreSQL: Completed   目标订单数: 10000000   完成时间: 2026-04-23 18:45        |
| MySQL:      Completed   目标订单数: 10000000   完成时间: 2026-04-23 19:02        |
+----------------------------------------------------------------------------------+
```

### 10.3 前端模块建议

```text
src/
├─ pages/
│  └─ mcp/
│     └─ McpManagementPage.vue
├─ components/
│  └─ mcp/
│     ├─ McpConnectionTable.vue
│     ├─ McpToolTable.vue
│     ├─ McpToolExecutor.vue
│     └─ DataInitStatusPanel.vue
├─ api/
│  └─ mcp.ts
└─ models/
   └─ mcp.ts
```

## 11. 后端新增 / 修改类清单

### 11.1 新增类

- `AIDbOptimize.Domain`: `Order`, `OrderItem`, `Money`, `OrderNumber`, `OrderStatus`
- `AIDbOptimize.Infrastructure`: `ControlPlaneDbContext`, `PostgreSqlLabDbContext`, `MySqlLabDbContext`
- `AIDbOptimize.Infrastructure`: `McpConnectionEntity`, `McpToolEntity`, `McpToolExecutionEntity`, `DataInitializationRunEntity`
- `AIDbOptimize.Infrastructure`: `McpConnectionRepository`, `McpToolRepository`, `McpDiscoveryService`, `McpToolExecutionService`
- `AIDbOptimize.Infrastructure`: `PostgreSqlLabInitializer`, `MySqlLabInitializer`, `InitializationStateService`
- `AIDbOptimize.ApiService`: `McpApi`, `DataInitializationApi`

### 11.2 修改类

- `AIDbOptimize.AppHost/AppHost.cs`
- `AIDbOptimize.ApiService/Program.cs`
- `AIDbOptimize.Web/src/App.vue`

## 12. 文档关系

实现前请按顺序阅读：

1. [总体设计](./design.md)
2. [本详细设计](./detailed-design.md)
3. [执行清单](./tasks.md)

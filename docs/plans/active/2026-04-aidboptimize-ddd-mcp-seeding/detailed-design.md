# AIDbOptimize DDD + MCP + 测试数据初始化详细设计方案

> 说明  
> `design.md` 用于描述总体目标与架构方向。  
> 本文档用于提供后续 agent 可直接落地的详细设计，包括包依赖、项目结构、数据结构、接口、页面草图、类设计和关键伪代码。

## 1. 当前实现状态与目标边界

### 1.1 已经完成的部分

- 解决方案已拆出 `Domain / Application / Infrastructure / DataInit`
- 订单域基础模型已具备
- 控制面数据库 `DbContext`、实体、仓储骨架已存在
- 控制面、PostgreSQL 业务测试库、MySQL 业务测试库三套迁移已生成
- 前端已有最小 MCP 管理视图
- API 已暴露 MCP 连接、工具、审批模式、执行记录、初始化状态等基础接口

### 1.2 仍未完成的部分

- `DataInit` 仍未真正生成订单测试数据
- 测试数据规模目标已调整为 `10w` 级别
- 测试数据应通过 **EF Core 迁移内 SQL** 完成，而不是由运行时服务手写插入
- 当前 MCP 发现/执行仍是最小骨架，不是真实 `tools/list / tools/call`
- `ApprovalRequiredAIFunction` 还未真正接入 Agent 工具装配

## 2. 解决方案结构

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

## 3. 项目职责

### `AIDbOptimize.Domain`

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

### `AIDbOptimize.Application`

负责“用例编排”，不直接关心具体数据库驱动或 MCP 实现细节。

### `AIDbOptimize.Infrastructure`

负责：

- EF Core `DbContext`
- 实体映射
- 迁移
- 仓储实现
- 真实 MCP client/session 调用适配

### `AIDbOptimize.ApiService`

负责：

- HTTP API 暴露
- 控制面数据库自动迁移
- 默认 MCP 连接种子

### `AIDbOptimize.DataInit`

负责：

- 业务测试库 `MigrateAsync`
- 初始化状态可视化
- 触发业务测试库迁移中的数据种子执行

> 重要约束  
> `DataInit` 本身不应再手写“大批量插入 SQL”。  
> 真实造数 SQL 应写在 EF Core 迁移中，由 `MigrateAsync()` 执行。

### `AIDbOptimize.Web`

负责：

- 资源概览
- MCP 管理视图

当前实现是 `App.vue` 内部的视图切换，不是独立路由页。

## 4. 包设计

### 4.1 .NET 包

#### `AIDbOptimize.Domain`

- 无额外第三方包

#### `AIDbOptimize.Application`

- `FluentValidation`

#### `AIDbOptimize.Infrastructure`

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Pomelo.EntityFrameworkCore.MySql`
- `MySqlConnector`
- `Npgsql`
- `ModelContextProtocol`

#### `AIDbOptimize.ApiService`

- `Swashbuckle.AspNetCore`
- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`

#### `AIDbOptimize.DataInit`

- `Microsoft.Extensions.Hosting`
- `Microsoft.Extensions.Logging.Console`
- `Microsoft.EntityFrameworkCore`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Pomelo.EntityFrameworkCore.MySql`

#### Agent 相关

后续真正接入 `ApprovalRequiredAIFunction` 时，还需要：

- `Microsoft.Extensions.AI`
- `ApprovalRequiredAIFunction` 对应包

## 5. 数据库设计

## 5.1 控制面数据库

控制面数据库使用 PostgreSQL，库名：

- `aidbopt_control`

### 表：`mcp_connections`

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

### 表：`mcp_tools`

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

### 表：`mcp_tool_executions`

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

### 表：`data_initialization_runs`

> 这个表仅用于“展示状态”和“记录结果”，不是幂等主判据。  
> 真正的幂等以 EF Core migration history 为准。

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

## 5.2 PostgreSQL / MySQL 业务测试库

数据库名：

- PostgreSQL：`aidbopt_lab_pg`
- MySQL：`aidbopt_lab_mysql`

### 表：`orders`

| 字段 | 类型 | 说明 |
| --- | --- | --- |
| `id` | `bigint` | 主键 |
| `order_number` | `varchar(50)` | 订单号，唯一 |
| `customer_id` | `bigint` | 客户 ID |
| `status` | `varchar(20)` | 订单状态 |
| `total_amount` | `decimal(18,2)` | 总金额 |
| `created_at` | `timestamp/datetime` | 创建时间 |
| `updated_at` | `timestamp/datetime` | 更新时间 |

### 表：`order_items`

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

## 6. 订单域模型设计

### 6.1 枚举

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

### 6.2 聚合根：`Order`

公开行为：

- `Create(...)`
- `AddItem(...)`
- `ChangeStatus(...)`
- `RecalculateTotal()`

### 6.3 实体：`OrderItem`

负责表达订单明细及金额规则。

## 7. 测试数据初始化设计

### 7.1 目标规模

当前目标规模改为：

- PostgreSQL：`10w` 订单 + 订单明细
- MySQL：`10w` 订单 + 订单明细

### 7.2 幂等策略

测试数据通过 **EF Core 迁移文件中的 SQL** 实现。

因此：

- 首次 `MigrateAsync()` 时执行造数 SQL
- 后续再次 `MigrateAsync()` 时，由 EF Core migration history 自动跳过
- 不再依赖运行时服务自行判断“是否已初始化”

### 7.3 DataInit 责任

`DataInit` 只负责：

1. 调用 PostgreSQL 业务测试库 `MigrateAsync()`
2. 调用 MySQL 业务测试库 `MigrateAsync()`
3. 将迁移结果写入 `data_initialization_runs`

### 7.4 造数位置

真正的订单测试数据 SQL 应放在：

- `PostgreSqlLab` 对应的 EF Core migration 文件
- `MySqlLab` 对应的 EF Core migration 文件

而不放在：

- `PostgreSqlLabInitializer`
- `MySqlLabInitializer`
- `DataInitializationHostedService`

### 7.5 迁移 SQL 设计原则

- 订单与订单明细统一在迁移 SQL 中生成
- 订单号可通过序列批量生成
- 明细数量控制在合理范围内，例如每单 1~3 条
- 总金额在迁移 SQL 中回填
- SQL 规模控制在 `10w` 级别

## 8. MCP 详细设计

### 8.1 当前状态

当前代码里：

- 有 MCP 连接持久化
- 有默认连接种子
- 有工具列表持久化
- 有最小只读工具执行能力

但还**不是真实 MCP 调用**：

- 发现工具不是 `tools/list`
- 执行工具不是 `tools/call`

### 8.2 下一步真实 MCP 方案

目标替换为：

1. 根据 `mcp_connections` 中的配置启动 MCP server 子进程
2. 通过 stdio 建立 MCP client 会话
3. 发现工具时调用真实 `tools/list`
4. 执行工具时调用真实 `tools/call`
5. 将结果写回控制面库

### 8.3 建议新增类

- `McpClientFactory`
- `McpProcessSession`
- `McpConnectionEnvironmentBuilder`

### 8.4 Agent 工具接入

后续要把 `mcp_tools` 转成 Agent 所需的工具描述或 `AIFunction`：

```csharp
foreach (var tool in tools)
{
    var aiFunction = ConvertToAIFunction(tool);

    if (tool.ApprovalMode == ToolApprovalMode.ApprovalRequired)
    {
        assembledTools.Add(new ApprovalRequiredAIFunction(aiFunction));
    }
    else
    {
        assembledTools.Add(aiFunction);
    }
}
```

当前代码只到“返回 `AgentToolDescriptor`”这一步，尚未真正接入 `ApprovalRequiredAIFunction`。

## 9. API 详细设计

已具备或计划具备：

- `GET /api/mcp/connections`
- `POST /api/mcp/connections`
- `PUT /api/mcp/connections/{id}`
- `POST /api/mcp/connections/{id}/discover-tools`
- `GET /api/mcp/connections/{id}/tools`
- `PUT /api/mcp/tools/{toolId}/approval-mode`
- `POST /api/mcp/tools/{toolId}/execute`
- `GET /api/mcp/executions`
- `GET /api/data-init/status`

### 当前执行接口说明

当前版本的 `/api/mcp/tools/{toolId}/execute` 只保证最小链路可用：

- 支持一组只读工具
- 返回执行结果
- 写入执行记录

它还不是最终的通用 `tools/call` 执行器。

## 10. 前端页面设计

### 10.1 当前实现形态

当前不是独立 `/mcp` 路由，而是在 `App.vue` 中通过视图切换显示：

- 资源概览
- MCP 管理

### 10.2 当前页面区块

```text
+----------------------------------------------------------------------------------+
| MCP 管理视图                                                                     |
+----------------------------------------------------------------------------------+
| [连接列表]                                                                       |
| [工具列表]                                                                       |
| [通用工具执行器]                                                                 |
| [初始化状态]                                                                     |
+----------------------------------------------------------------------------------+
```

### 10.3 当前前端模块

```text
src/
├─ api/
│  └─ mcp.ts
├─ components/
│  └─ mcp/
│     ├─ McpConnectionTable.vue
│     ├─ McpToolTable.vue
│     ├─ McpToolExecutor.vue
│     └─ DataInitStatusPanel.vue
├─ models/
│  └─ mcp.ts
└─ App.vue
```

## 11. 后端新增 / 修改类清单

### 已新增

- `ControlPlaneDbContext`
- `PostgreSqlLabDbContext`
- `MySqlLabDbContext`
- `McpConnectionRepository`
- `McpToolRepository`
- `DataInitializationRunRepository`
- `McpApi`
- `DataInitializationApi`
- `ControlPlaneMigrationHostedService`
- `ControlPlaneDefaultSeedHostedService`

### 下一步建议新增

- `McpClientFactory`
- `McpProcessSession`
- 迁移内 10w 造数 SQL

## 12. 文档关系

实现前请按顺序阅读：

1. [总体设计](./design.md)
2. [本详细设计](./detailed-design.md)
3. [执行清单](./tasks.md)

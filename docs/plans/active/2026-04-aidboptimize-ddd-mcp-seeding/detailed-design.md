# AIDbOptimize DDD + MCP + 测试数据初始化详细设计方案

## 1. 当前实现状态

### 1.1 已完成

- 解决方案已拆出 `Domain / Application / Infrastructure / DataInit`
- 订单域基础模型已具备
- 控制面数据库 `DbContext`、实体、仓储骨架已存在
- 控制面、PostgreSQL 业务测试库、MySQL 业务测试库三套迁移已生成
- PostgreSQL / MySQL 的 `10w` 级测试数据迁移已落地
- 前端已有最小 MCP 管理视图
- API 已暴露 MCP 连接、工具、审批模式、执行记录、初始化状态等基础接口
- 真实 MCP 发现与执行链路已接入：
  - PostgreSQL 默认连接使用 `@modelcontextprotocol/server-postgres`
  - MySQL 默认连接当前使用 `mysql-mcp-server`
- Agent 工具装配已接入真实 `AIFunction`
- `ApprovalRequiredAIFunction` 已能根据配置生效

### 1.2 仍未完成

- 更丰富的写入型工具体验
- MySQL 写入型 MCP 工具的长期方案统一

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

### `AIDbOptimize.Application`

负责“用例编排”，不直接关心具体数据库驱动或 MCP 实现细节。

### `AIDbOptimize.Infrastructure`

负责：

- EF Core `DbContext`
- 实体映射
- 迁移
- 仓储实现
- 真实 MCP client / session / tool 调用适配

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
> `DataInit` 不再手写批量插入 SQL。  
> 真实订单测试数据 SQL 已下沉到 EF Core 迁移中，由 `MigrateAsync()` 执行。

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
- `Microsoft.Extensions.AI.Abstractions`

#### `AIDbOptimize.Infrastructure`

- `Microsoft.EntityFrameworkCore`
- `Microsoft.EntityFrameworkCore.Design`
- `Npgsql.EntityFrameworkCore.PostgreSQL`
- `Pomelo.EntityFrameworkCore.MySql`
- `MySqlConnector`
- `Npgsql`
- `ModelContextProtocol.Core`
- `Microsoft.Extensions.AI`

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

## 5. 数据库设计

### 5.1 控制面数据库

控制面数据库使用 PostgreSQL，库名：

- `aidbopt_control`

保存：

- MCP 连接配置
- MCP 工具配置
- MCP 工具执行记录
- 数据初始化状态

### 5.2 PostgreSQL / MySQL 业务测试库

数据库名：

- PostgreSQL：`aidbopt_lab_pg`
- MySQL：`aidbopt_lab_mysql`

表结构：

- `orders`
- `order_items`

## 6. 测试数据初始化设计

### 6.1 目标规模

当前目标规模固定为：

- PostgreSQL：`10w` 订单 + `10w` 明细
- MySQL：`10w` 订单 + `10w` 明细

### 6.2 幂等策略

测试数据通过 **EF Core 迁移文件中的 SQL** 实现，因此：

- 首次 `MigrateAsync()` 时执行造数 SQL
- 后续再次 `MigrateAsync()` 时，由 EF Core migration history 自动跳过
- 运行时状态表不再承担幂等主判据

### 6.3 DataInit 职责

`DataInit` 只负责：

1. 调用 PostgreSQL 业务测试库 `MigrateAsync()`
2. 调用 MySQL 业务测试库 `MigrateAsync()`
3. 将迁移结果写入 `data_initialization_runs`

### 6.4 已落地迁移

- `PostgreSqlLab/SeedPostgreSqlLab10wOrders`
- `MySqlLab/SeedMySqlLab10wOrders`

## 7. MCP 详细设计

### 7.1 当前真实实现

当前已接入：

- PostgreSQL 默认连接：官方 `@modelcontextprotocol/server-postgres`
- MySQL 默认连接：社区 `mysql-mcp-server`
- stdio MCP 会话工厂：`McpClientFactory`
- 短生命周期会话包装：`McpProcessSession`
- 工具发现：真实 `tools/list`
- 工具执行：真实 `tools/call`

### 7.2 当前实现边界

虽然已经是真实 MCP 调用，但仍有边界：

- PostgreSQL 当前默认工具主要是只读 `query`
- MySQL 当前默认工具来自社区只读服务器
- 前端执行器已支持真实调用，但还不是“写入型工具优先”的管理体验

### 7.3 Agent 工具接入

当前实现已经把真实 MCP 工具转成 Agent 所需的 `AIFunction / AITool`，并按审批模式包装：

```csharp
foreach (var tool in tools)
{
    var aiFunction = tool;

    if (configured.ApprovalMode == ToolApprovalMode.ApprovalRequired)
    {
        assembledTools.Add(new ApprovalRequiredAIFunction(aiFunction));
    }
    else
    {
        assembledTools.Add(aiFunction);
    }
}
```

## 8. API 详细设计

已具备：

- `GET /api/mcp/connections`
- `POST /api/mcp/connections`
- `PUT /api/mcp/connections/{id}`
- `POST /api/mcp/connections/{id}/discover-tools`
- `GET /api/mcp/connections/{id}/tools`
- `GET /api/mcp/connections/{id}/agent-tools`
- `PUT /api/mcp/tools/{toolId}/approval-mode`
- `POST /api/mcp/tools/{toolId}/execute`
- `GET /api/mcp/executions`
- `GET /api/data-init/status`

## 9. 前端页面设计

当前不是独立 `/mcp` 路由，而是在 `App.vue` 中通过视图切换显示：

- 资源概览
- MCP 管理

前端已具备区块：

- 连接列表
- 工具列表
- 通用工具执行器
- 初始化状态

## 10. 后端新增 / 修改类清单

### 已落地

- `McpClientFactory`
- `McpProcessSession`
- `McpDiscoveryService`
- `McpToolExecutionService`
- `Infrastructure.Mcp.AgentToolAssemblyService`
- `ControlPlaneDefaultSeedHostedService`

## 11. 文档关系

实现前请按顺序阅读：

1. [总体设计](./design.md)
2. [本详细设计](./detailed-design.md)
3. [执行清单](./tasks.md)

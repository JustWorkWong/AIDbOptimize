# AIDbOptimize.Application 目录说明

## 目录职责

`AIDbOptimize.Application/` 负责定义应用层用例、DTO 与对外契约，承接 API 层与基础设施层之间的业务编排边界，不直接落持久化实现。

## 当前结构

```text
AIDbOptimize.Application/
├── CLAUDE.md
├── AIDbOptimize.Application.csproj
├── Abstractions/
│   ├── Agents/
│   ├── Mcp/
│   └── Persistence/
├── DataInitialization/
│   ├── Dtos/
│   └── Services/
├── Mcp/
│   ├── Commands/
│   ├── Dtos/
│   ├── Queries/
│   └── Services/
├── Orders/
│   ├── Commands/
│   ├── Queries/
│   └── Services/
├── Rag/
│   ├── Dtos/
│   └── Services/
└── Workflows/
    ├── Dtos/
    └── Services/
```

## 文件用途

- `Abstractions/`: 面向基础设施的接口，供应用服务编排调用。
- `DataInitialization/`: 数据初始化状态查询 DTO 与应用服务。
- `Mcp/`: MCP 连接、工具发现、执行与权限相关的应用层模型和服务。
- `Orders/`: 示例订单用例。
- `Rag/`: RAG 资产状态查询 DTO 与应用服务，承接 API 层读取 facts/cases/chunks/snapshots 的聚合视图。
- `Workflows/Dtos/`: 数据库配置优化 workflow、review、history 的最小 API 请求/响应契约。
- `Workflows/Services/`: API 直接依赖的 workflow/review/history 服务接口，等待主线程接入真实实现。

## 依赖关系

- `ApiService -> Application.Workflows.Services`: HTTP 路由只依赖接口和 DTO，不触碰 persistence 细节。
- `Application.* -> Abstractions.Persistence`: 真实实现应继续通过抽象访问持久化，而不是反向依赖 API。

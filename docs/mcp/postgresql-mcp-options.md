# PostgreSQL MCP 方案对比与推荐

## 1. 背景

当前项目已经把默认 PostgreSQL MCP 从 npm 包型参考实现切换到 `sgaunet/postgresql-mcp`，原因是它提供了更丰富的 PostgreSQL 只读工具集合，更适合当前“工具列表 + 审批 + 通用执行器”的前端管理模型。

但切换后也带来了一个现实问题：

- npm 型 MCP server 看起来很省事
- 原生二进制型 MCP server 明显更麻烦

这不是 PostgreSQL 特殊，而是“npm 包”与“原生二进制”在交付方式上的天然差异。

## 2. 为什么 npm 型方案更省事

以 `@modelcontextprotocol/server-postgres` 为例，它本质上是 Node 包。

当前这类方案的启动方式通常只有一条命令：

```text
npx -y <package-name> <args...>
```

它省事的原因是：

1. `npx` 自带下载并执行能力。
2. Node/npm 在多数开发环境里本来就有。
3. 同一个 npm 包通常可以跨 Windows、macOS、Linux 直接运行。
4. 缓存、依赖解析、版本下载都由 npm 生态兜底。

所以“原生看起来不麻烦”并不是因为 MCP 协议本身简单，而是因为 npm 帮你吃掉了分发成本。

## 3. 为什么原生二进制型方案更麻烦

以 `sgaunet/postgresql-mcp` 为例，它是平台相关的原生二进制，不是 npm 包。

这类方案天然要自己处理：

- 不同平台不同文件名
- 不同架构不同文件
- 下载地址管理
- 本地缓存目录
- 可执行权限
- Docker 与本地开发的差异

也就是说，麻烦点不是 Go，而是“非 npm 的原生二进制交付”。

## 4. 可选方案对比

| 方案 | 本地开发复杂度 | mac 兼容 | Docker 兼容 | 首次启动成本 | 维护成本 | 能力上限 | 适合场景 |
|---|---|---|---|---|---|---|---|
| 继续使用 npm 型 PostgreSQL MCP | 低 | 好 | 好 | 低 | 低 | 中 | 优先简单、接受工具能力较弱 |
| 直接要求环境预装原生二进制 | 中 | 中 | 中 | 中 | 中 | 高 | 团队环境可控、可接受手工安装 |
| PowerShell/bash 启动脚本 | 中 | 一般 | 一般 | 中 | 中偏高 | 高 | 临时过渡方案 |
| Docker sidecar 单独跑 MCP | 中偏高 | 好 | 好 | 中 | 中偏高 | 高 | 部署优先、服务化治理 |
| 构建阶段把原生二进制打进镜像/产物 | 中 | 好 | 很好 | 低 | 中 | 高 | 生产部署优先 |
| 统一 .NET launcher | 中 | 好 | 很好 | 中 | 中 | 高 | 长期维护、跨平台统一治理 |

## 5. 每种方案的核心判断

### 5.1 继续使用 npm 型 PostgreSQL MCP

优点：

- 本地开发最省事
- 不需要额外下载逻辑
- 最符合当前已有的 `npx` 心智

缺点：

- 能力往往偏轻量
- 很多实现是参考型，不一定覆盖更完整的 PostgreSQL 工具集

适合：

- 你优先要简单
- 不在意 PostgreSQL 工具能力偏弱

### 5.2 直接要求环境预装原生二进制

优点：

- 启动逻辑最直接
- 运行时不需要再下载

缺点：

- 每台机器都要提前准备
- mac / Windows / Linux 要分别装
- Docker 也得单独处理镜像内容

适合：

- 团队环境高度统一
- 可以接受手工准备运行环境

### 5.3 PowerShell/bash 启动脚本

优点：

- 改造成本低
- 适合作为验证期方案

缺点：

- Windows 和 mac/Linux 很容易分叉成两套脚本
- 路径、权限、下载逻辑都比较脆弱

适合：

- 短期验证
- 不适合长期作为跨平台基础设施方案

### 5.4 Docker sidecar

优点：

- 部署结构清晰
- MCP server 生命周期可独立管理

缺点：

- 本地开发成本会更高
- 编排、网络、健康检查都要补

适合：

- 生产部署优先
- MCP server 需要独立服务化

### 5.5 构建阶段打包原生二进制

优点：

- 运行时最稳
- Docker 场景尤其合适
- 无需线上临时下载

缺点：

- 本地开发和发布流程需要分开考虑
- 多平台构建时要管理不同产物

适合：

- Docker 部署明确
- 追求运行时稳定性

### 5.6 统一 .NET launcher

优点：

- 一套 C# 逻辑覆盖 Windows / macOS / Linux
- 不依赖 PowerShell
- 可以复用到别的原生 MCP server
- 日志、缓存、下载、重试逻辑都可统一治理

缺点：

- 初期工程量比脚本方案高
- 需要额外维护一个 launcher 项目

适合：

- 明确要跨平台
- 本地开发、mac、Docker 都要支持
- 后续还会继续接别的原生 MCP server

## 6. 本项目推荐

结合当前项目的实际约束：

- 本地开发不只在 Windows
- 后面会有 mac 开发机
- 部署后会跑在 Docker
- 你已经明确想要比官方 npm 参考实现更强的 PostgreSQL MCP

推荐路线是：

### 第一阶段：保留当前原生二进制路线

原因：

- `sgaunet/postgresql-mcp` 的能力更符合现在的需求
- 已经完成默认连接切换

### 第二阶段：把当前 PowerShell launcher 升级成统一的 .NET launcher

原因：

- 当前 `postgresql-mcp-launcher.ps1` 还是偏 Windows 过渡方案
- 它不适合长期承载 mac 和 Docker

### 第三阶段：Docker 构建阶段预打包二进制

原因：

- 生产部署不应该依赖运行时临时下载
- 镜像构建时准备好二进制，运行时只负责启动，最稳

## 7. 当前落地建议

如果按优先级排：

1. 短期先让当前 `sgaunet/postgresql-mcp` 路线稳定工作。
2. 中期把 launcher 从 PowerShell 升级成统一的 `.NET` launcher。
3. 部署阶段在 Docker 镜像构建期打包平台对应二进制。

## 8. 最终结论

### 如果你优先要“简单”

回到 npm 型 PostgreSQL MCP server 最省事。

### 如果你优先要“能力更强”

保留 `sgaunet/postgresql-mcp` 是对的，但不应该长期停留在 PowerShell 脚本阶段。

### 对这个项目的最佳长期方案

保留原生二进制 PostgreSQL MCP server，后续演进为：

```text
本地开发：统一 .NET launcher
Docker 部署：镜像构建期预打包二进制
运行时：只负责启动，不做临时下载
```

这条路不是最省事，但对你的跨平台和部署目标最稳。

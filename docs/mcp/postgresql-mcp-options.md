# PostgreSQL MCP 方案对比与当前选择

## 1. 背景

这个项目先后尝试过两类 PostgreSQL MCP 方案：

- npm 包型无状态实现
- 原生二进制型实现

最终当前默认方案已经回退为 npm 包型无状态实现。

## 2. 为什么 npm 型方案看起来更省事

以 `@modelcontextprotocol/server-postgres` 为例，它本质上是 Node 包。

启动方式通常只有一条命令：

```text
npx -y @modelcontextprotocol/server-postgres <postgres-url>
```

它省事的原因是：

1. `npx` 自带下载并执行能力。
2. Node/npm 在多数开发环境里本来就有。
3. 同一个 npm 包通常可以跨 Windows、macOS、Linux 直接运行。
4. 缓存、依赖解析、版本下载都由 npm 生态兜底。

## 3. 为什么原生二进制型方案更麻烦

原生二进制型方案通常要自己处理：

- 不同平台不同文件名
- 不同架构不同文件
- 下载地址
- 本地缓存目录
- 权限
- Docker 与本地开发差异

更重要的是，如果 server 设计成“先连接数据库，再执行后续工具”的状态化模型，而系统本身又是“每次执行新建一个 MCP 会话”，两者会天然冲突。

## 4. 这个项目最终为什么选 npm 无状态实现

当前系统的 MCP 调用模型是：

- 获取工具时新建一个短生命周期 MCP 会话
- 执行工具时再次新建一个短生命周期 MCP 会话

这意味着默认 PostgreSQL MCP server 最好满足两个条件：

1. 无状态
2. 一次调用即可完成

`@modelcontextprotocol/server-postgres` 符合这两个条件，所以它与当前架构最匹配。

而之前尝试的原生二进制型 PostgreSQL MCP server 暴露了两个结构性问题：

- 依赖 `connect_database` 一类状态化步骤
- 与当前短生命周期 MCP 会话模型不兼容

因此当前默认方案已经回退。

## 5. 当前默认方案

### PostgreSQL

- `ServerCommand`
  `npx`
- `ServerArgumentsJson`
  `["-y","@modelcontextprotocol/server-postgres","postgresql://..."]`
- 特征
  npm 包型、无状态、跨平台心智简单

### MySQL

- `ServerCommand`
  `npx`
- `ServerArgumentsJson`
  `["-y","mysql-mcp-server"]`

## 6. 后续演进建议

如果未来真的需要比 `@modelcontextprotocol/server-postgres` 更强的 PostgreSQL MCP 工具集，应该优先二选一：

1. 找到“仍然是 npm 包，但工具更丰富”的 PostgreSQL MCP server。
2. 如果必须使用状态化原生二进制型 server，就要连同架构一起调整：
   - 把 MCP 会话改成按连接复用
   - 不再每次调用都新建进程

在没有改会话模型之前，不建议再次把默认 PostgreSQL MCP 切回状态化原生二进制方案。

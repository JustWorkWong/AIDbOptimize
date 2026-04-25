# AIDbOptimize DDD + MCP + 测试数据初始化任务清单

## 已完成

- [x] 建立 `docs` 目录结构
- [x] 输出总体设计文档
- [x] 输出详细设计文档
- [x] 输出任务清单
- [x] 完成 `Domain / Application / Infrastructure / DataInit` 分层
- [x] 建立控制面数据库模型与迁移
- [x] 建立 PostgreSQL / MySQL 测试库模型与迁移
- [x] 接入真实 `tools/list`
- [x] 接入真实 `tools/call`
- [x] 落地 MCP 前端管理视图
- [x] 修复“刚获取工具后保存审批报未找到工具”的主键错位问题
- [x] 优化真实连接命令展示
- [x] 修复 MCP 相关日志乱码
- [x] 修复 Windows 路径落库时的 JSON 转义问题
- [x] 将默认 PostgreSQL MCP 回退为 npm 无状态实现

## 当前默认基线

- PostgreSQL 默认 MCP：`@modelcontextprotocol/server-postgres`
- PostgreSQL 启动方式：`npx -y @modelcontextprotocol/server-postgres <postgres-url>`
- MySQL 默认 MCP：`mysql-mcp-server`

## 后续可选任务

- [ ] 为 `docs/mcp/README.md` 增加故障排查章节
- [ ] 增加连接命令脱敏开关，避免共享环境直接展示敏感信息
- [ ] 增加 MCP resources 浏览能力，补齐“工具之外的 schema 资源”查看
- [ ] 为默认参数生成逻辑补前端单测
- [ ] 如果未来需要更强 PostgreSQL MCP，优先评估 npm 无状态实现；若必须引入状态化 server，则先改会话复用架构

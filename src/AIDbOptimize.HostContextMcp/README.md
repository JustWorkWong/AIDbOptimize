# AIDbOptimize HostContext MCP

最小只读 MCP server。

当前只服务于 `DbConfigOptimization` workflow 的宿主资源上下文采集，提供：

- `resolve_runtime_target`
- `get_container_limits`
- `get_container_stats`
- `get_disk_usage`
- `get_host_memory`
- `get_host_cpu`
- `get_process_limits`
- `get_managed_service_profile`

运行方式：

```powershell
node E:\Db\src\AIDbOptimize.HostContextMcp\server.mjs
```

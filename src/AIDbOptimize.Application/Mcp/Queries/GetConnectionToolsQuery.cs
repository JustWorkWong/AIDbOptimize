namespace AIDbOptimize.Application.Mcp.Queries;

/// <summary>
/// 按连接读取工具列表的查询对象。
/// </summary>
public sealed record GetConnectionToolsQuery(Guid ConnectionId);

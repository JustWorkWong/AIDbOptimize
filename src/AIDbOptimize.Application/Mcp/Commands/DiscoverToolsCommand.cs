namespace AIDbOptimize.Application.Mcp.Commands;

/// <summary>
/// 触发工具发现的命令对象。
/// </summary>
public sealed record DiscoverToolsCommand(Guid ConnectionId);

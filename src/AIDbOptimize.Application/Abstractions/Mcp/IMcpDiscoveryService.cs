using AIDbOptimize.Domain.Mcp.Models;

namespace AIDbOptimize.Application.Abstractions.Mcp;

/// <summary>
/// MCP 工具发现抽象。
/// </summary>
public interface IMcpDiscoveryService
{
    Task<IReadOnlyCollection<McpToolDefinition>> DiscoverToolsAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default);
}

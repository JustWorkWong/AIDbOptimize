using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;

namespace AIDbOptimize.Application.Mcp.Dtos;

/// <summary>
/// MCP 连接查询结果。
/// </summary>
public sealed record McpConnectionDto(
    Guid Id,
    string Name,
    DatabaseEngine Engine,
    string DisplayName,
    string DatabaseName,
    bool IsDefault,
    McpConnectionStatus Status,
    DateTimeOffset? LastDiscoveredAt)
{
    public static McpConnectionDto FromDefinition(McpConnectionDefinition definition)
    {
        return new McpConnectionDto(
            definition.Id,
            definition.Name,
            definition.Engine,
            definition.DisplayName,
            definition.DatabaseName,
            definition.IsDefault,
            definition.Status,
            definition.LastDiscoveredAt);
    }
}

using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.Mcp.Models;

/// <summary>
/// MCP 连接定义，用于跨层传递连接配置。
/// </summary>
public sealed record McpConnectionDefinition(
    Guid Id,
    string Name,
    DatabaseEngine Engine,
    string DisplayName,
    string ServerCommand,
    IReadOnlyList<string> ServerArguments,
    IReadOnlyDictionary<string, string> EnvironmentVariables,
    string DatabaseConnectionString,
    string DatabaseName,
    bool IsDefault,
    McpConnectionStatus Status,
    DateTimeOffset? LastDiscoveredAt);

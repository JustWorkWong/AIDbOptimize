using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// MCP 连接配置仓储抽象。
/// </summary>
public interface IMcpConnectionRepository
{
    Task<IReadOnlyList<McpConnectionRecord>> GetAllAsync(CancellationToken cancellationToken = default);

    Task<McpConnectionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task AddAsync(McpConnectionRecord record, CancellationToken cancellationToken = default);

    Task UpdateAsync(McpConnectionRecord record, CancellationToken cancellationToken = default);
}

/// <summary>
/// Application 层对 MCP 连接配置的轻量表达。
/// 这里先用 record 承接配置，后续再根据需求演进为更丰富的 DTO。
/// </summary>
public sealed record McpConnectionRecord(
    Guid Id,
    string Name,
    DatabaseEngine Engine,
    string DisplayName,
    string ServerCommand,
    string ServerArgumentsJson,
    string EnvironmentJson,
    string DatabaseConnectionString,
    string DatabaseName,
    bool IsDefault,
    McpConnectionStatus Status,
    DateTimeOffset? LastDiscoveredAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// MCP 工具配置仓储抽象。
/// </summary>
public interface IMcpToolRepository
{
    Task<IReadOnlyList<McpToolRecord>> ListByConnectionAsync(Guid connectionId, CancellationToken cancellationToken = default);

    Task<McpToolRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<McpToolRecord>> UpsertManyAsync(IReadOnlyCollection<McpToolRecord> tools, CancellationToken cancellationToken = default);

    Task UpdateApprovalModeAsync(Guid id, ToolApprovalMode approvalMode, CancellationToken cancellationToken = default);
}

/// <summary>
/// Application 层对 MCP 工具的轻量表达。
/// </summary>
public sealed record McpToolRecord(
    Guid Id,
    Guid ConnectionId,
    string ToolName,
    string DisplayName,
    string Description,
    string InputSchemaJson,
    ToolApprovalMode ApprovalMode,
    bool IsEnabled,
    bool IsWriteTool,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt);

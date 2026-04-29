using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// Workflow 会话持久化抽象。
/// </summary>
public interface IWorkflowSessionRepository
{
    Task<WorkflowSessionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkflowSessionRecord>> ListByConnectionAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(WorkflowSessionRecord record, CancellationToken cancellationToken = default);

    Task UpdateAsync(WorkflowSessionRecord record, CancellationToken cancellationToken = default);
}

/// <summary>
/// Workflow 会话轻量记录。
/// </summary>
public sealed record WorkflowSessionRecord(
    Guid Id,
    Guid ConnectionId,
    string WorkflowName,
    WorkflowSessionStatus Status,
    string RequestedBy,
    string InputPayloadJson,
    string ResultPayloadJson,
    string? CurrentNodeKey,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

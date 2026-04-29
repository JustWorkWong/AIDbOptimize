using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// Workflow Review 任务持久化抽象。
/// </summary>
public interface IWorkflowReviewTaskRepository
{
    Task<WorkflowReviewTaskRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkflowReviewTaskRecord>> ListBySessionAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(WorkflowReviewTaskRecord record, CancellationToken cancellationToken = default);

    Task UpdateAsync(WorkflowReviewTaskRecord record, CancellationToken cancellationToken = default);
}

/// <summary>
/// Workflow Review 任务轻量记录。
/// </summary>
public sealed record WorkflowReviewTaskRecord(
    Guid Id,
    Guid WorkflowSessionId,
    Guid? NodeExecutionId,
    string Title,
    string PayloadJson,
    WorkflowReviewTaskStatus Status,
    string RequestedBy,
    string? DecisionBy,
    string? DecisionNote,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

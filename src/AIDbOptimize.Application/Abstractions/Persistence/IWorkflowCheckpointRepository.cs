namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// Workflow checkpoint 持久化抽象。
/// </summary>
public interface IWorkflowCheckpointRepository
{
    Task<WorkflowCheckpointRecord?> GetLatestAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkflowCheckpointRecord>> ListAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        WorkflowCheckpointRecord record,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Workflow checkpoint 轻量记录。
/// </summary>
public sealed record WorkflowCheckpointRecord(
    Guid Id,
    Guid WorkflowSessionId,
    int Sequence,
    string Status,
    string? CurrentNodeKey,
    string SnapshotJson,
    DateTimeOffset CreatedAt);

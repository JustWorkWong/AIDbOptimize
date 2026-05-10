namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagRetrievalSnapshotRepository
{
    Task<IReadOnlyList<RagRetrievalSnapshotRecord>> ListByWorkflowSessionAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);
}

public sealed record RagRetrievalSnapshotRecord(
    Guid Id,
    Guid WorkflowSessionId,
    Guid NodeExecutionId,
    string SnapshotTypeJson,
    string RetrievedItemsJson,
    DateTimeOffset CreatedAt);

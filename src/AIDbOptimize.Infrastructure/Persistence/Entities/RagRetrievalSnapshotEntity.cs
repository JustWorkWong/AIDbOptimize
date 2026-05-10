namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// RAG retrieval snapshot entity.
/// </summary>
public sealed class RagRetrievalSnapshotEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public Guid NodeExecutionId { get; set; }

    public string SnapshotTypeJson { get; set; } = "{}";

    public string RetrievedItemsJson { get; set; } = "[]";

    public DateTimeOffset CreatedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public WorkflowNodeExecutionEntity NodeExecution { get; set; } = null!;
}

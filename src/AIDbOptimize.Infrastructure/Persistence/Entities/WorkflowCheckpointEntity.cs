namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Workflow checkpoint entity.
/// </summary>
public sealed class WorkflowCheckpointEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public int Sequence { get; set; }

    public string RunId { get; set; } = string.Empty;

    public string CheckpointRef { get; set; } = string.Empty;

    public string Status { get; set; } = string.Empty;

    public string? CurrentNodeKey { get; set; }

    public string SnapshotJson { get; set; } = "{}";

    public string PayloadCompressed { get; set; } = string.Empty;

    public string PayloadEncoding { get; set; } = "gzip+base64";

    public string PayloadSha256 { get; set; } = string.Empty;

    public long PayloadSizeBytes { get; set; }

    public string PendingRequestsJson { get; set; } = "[]";

    public string AgentStateRefsJson { get; set; } = "{}";

    public DateTimeOffset CreatedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;
}

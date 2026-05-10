using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Workflow session entity.
/// </summary>
public sealed class WorkflowSessionEntity
{
    public Guid Id { get; set; }

    public Guid ConnectionId { get; set; }

    public string WorkflowName { get; set; } = string.Empty;

    public string EngineType { get; set; } = "maf";

    public WorkflowSessionStatus Status { get; set; } = WorkflowSessionStatus.Draft;

    public string RequestedBy { get; set; } = "system";

    public string InputPayloadJson { get; set; } = "{}";

    public string StateJson { get; set; } = "{}";

    public string ResultType { get; set; } = "db-config-optimization-report";

    public string ResultPayloadJson { get; set; } = "{}";

    public string? CurrentNodeKey { get; set; }

    public string? EngineRunId { get; set; }

    public string? EngineCheckpointRef { get; set; }

    public string? EngineStateJson { get; set; }

    public Guid? ActiveReviewTaskId { get; set; }

    public Guid? AgentSessionId { get; set; }

    public long TotalTokens { get; set; }

    public decimal EstimatedCost { get; set; }

    public string? ErrorMessage { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public McpConnectionEntity Connection { get; set; } = null!;

    public WorkflowReviewTaskEntity? ActiveReviewTask { get; set; }

    public AgentSessionEntity? PrimaryAgentSession { get; set; }

    public ICollection<WorkflowReviewTaskEntity> ReviewTasks { get; set; } = [];

    public ICollection<WorkflowNodeExecutionEntity> NodeExecutions { get; set; } = [];

    public ICollection<WorkflowEventEntity> Events { get; set; } = [];

    public ICollection<WorkflowCheckpointEntity> Checkpoints { get; set; } = [];

    public ICollection<AgentSessionEntity> AgentSessions { get; set; } = [];

    public ICollection<AgentMessageEntity> AgentMessages { get; set; } = [];

    public ICollection<McpToolExecutionEntity> ToolExecutions { get; set; } = [];

    public ICollection<RagRetrievalSnapshotEntity> RagRetrievalSnapshots { get; set; } = [];

    public ICollection<RagCaseRecordEntity> RagCaseRecords { get; set; } = [];
}

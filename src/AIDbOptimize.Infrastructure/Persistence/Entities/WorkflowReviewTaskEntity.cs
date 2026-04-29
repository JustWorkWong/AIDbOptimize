using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Workflow review task entity.
/// </summary>
public sealed class WorkflowReviewTaskEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public Guid? CheckpointId { get; set; }

    public Guid? NodeExecutionId { get; set; }

    public string? RequestId { get; set; }

    public string? EngineRunId { get; set; }

    public string? EngineCheckpointRef { get; set; }

    public string Title { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = "{}";

    public string AdjustmentsJson { get; set; } = "{}";

    public WorkflowReviewTaskStatus Status { get; set; } = WorkflowReviewTaskStatus.Pending;

    public string RequestedBy { get; set; } = "system";

    public string? DecisionBy { get; set; }

    public string? DecisionNote { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public WorkflowCheckpointEntity? Checkpoint { get; set; }

    public WorkflowNodeExecutionEntity? NodeExecution { get; set; }

    public ICollection<WorkflowEventEntity> Events { get; set; } = [];
}

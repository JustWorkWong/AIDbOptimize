using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Workflow node execution entity.
/// </summary>
public sealed class WorkflowNodeExecutionEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public string NodeKey { get; set; } = string.Empty;

    public string NodeType { get; set; } = string.Empty;

    public int Attempt { get; set; } = 1;

    public WorkflowNodeExecutionStatus Status { get; set; } = WorkflowNodeExecutionStatus.Pending;

    public string InputPayloadJson { get; set; } = "{}";

    public string OutputPayloadJson { get; set; } = "{}";

    public string? ErrorMessage { get; set; }

    public Guid? AgentSessionId { get; set; }

    public string TokenUsageJson { get; set; } = "{}";

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset? StartedAt { get; set; }

    public DateTimeOffset? CompletedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public AgentSessionEntity? AgentSession { get; set; }

    public ICollection<WorkflowReviewTaskEntity> ReviewTasks { get; set; } = [];

    public ICollection<WorkflowEventEntity> Events { get; set; } = [];
}

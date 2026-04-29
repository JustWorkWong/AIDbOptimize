using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Workflow event entity.
/// </summary>
public sealed class WorkflowEventEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public long SequenceNo { get; set; }

    public Guid? NodeExecutionId { get; set; }

    public Guid? ReviewTaskId { get; set; }

    public Guid? McpToolExecutionId { get; set; }

    public WorkflowEventType EventType { get; set; }

    public string EventName { get; set; } = string.Empty;

    public string PayloadJson { get; set; } = "{}";

    public string? Message { get; set; }

    public string? TraceId { get; set; }

    public string? SpanId { get; set; }

    public DateTimeOffset OccurredAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public WorkflowNodeExecutionEntity? NodeExecution { get; set; }

    public WorkflowReviewTaskEntity? ReviewTask { get; set; }

    public McpToolExecutionEntity? McpToolExecution { get; set; }
}

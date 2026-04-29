namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Agent 消息实体。
/// </summary>
public sealed class AgentMessageEntity
{
    public Guid Id { get; set; }

    public Guid AgentSessionId { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public long SequenceNo { get; set; }

    public string Role { get; set; } = string.Empty;

    public string MessageKind { get; set; } = string.Empty;

    public string Content { get; set; } = string.Empty;

    public string RawPayloadJson { get; set; } = "{}";

    public string? TraceId { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public AgentSessionEntity AgentSession { get; set; } = null!;

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;
}

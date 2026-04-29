namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Agent session entity.
/// </summary>
public sealed class AgentSessionEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public string AgentRole { get; set; } = string.Empty;

    public string SerializedSessionJson { get; set; } = "{}";

    public string SessionStateJson { get; set; } = "{}";

    public Guid? ActiveSummaryId { get; set; }

    public string PromptVersion { get; set; } = string.Empty;

    public string ModelId { get; set; } = string.Empty;

    public int MessageGroupCount { get; set; }

    public DateTimeOffset? LastCompactedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public ICollection<WorkflowNodeExecutionEntity> NodeExecutions { get; set; } = [];

    public AgentSummaryEntity? ActiveSummary { get; set; }

    public ICollection<AgentSummaryEntity> Summaries { get; set; } = [];

    public ICollection<AgentMessageEntity> Messages { get; set; } = [];
}

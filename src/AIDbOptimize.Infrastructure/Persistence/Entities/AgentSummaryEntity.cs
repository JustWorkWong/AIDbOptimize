namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// Agent rolling summary 实体。
/// </summary>
public sealed class AgentSummaryEntity
{
    public Guid Id { get; set; }

    public Guid AgentSessionId { get; set; }

    public string SummaryType { get; set; } = string.Empty;

    public string SummaryJson { get; set; } = "{}";

    public long SourceStartSequence { get; set; }

    public long SourceEndSequence { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public AgentSessionEntity AgentSession { get; set; } = null!;
}

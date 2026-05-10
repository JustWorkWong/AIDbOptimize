namespace AIDbOptimize.Infrastructure.Persistence.Entities;

public sealed class RagCaseRecordEntity
{
    public Guid Id { get; set; }

    public Guid WorkflowSessionId { get; set; }

    public string Engine { get; set; } = string.Empty;

    public string ProblemType { get; set; } = string.Empty;

    public string Outcome { get; set; } = string.Empty;

    public string ReviewStatus { get; set; } = string.Empty;

    public string RecommendationType { get; set; } = string.Empty;

    public string Summary { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public WorkflowSessionEntity WorkflowSession { get; set; } = null!;

    public ICollection<RagCaseEvidenceLinkEntity> EvidenceLinks { get; set; } = [];
}

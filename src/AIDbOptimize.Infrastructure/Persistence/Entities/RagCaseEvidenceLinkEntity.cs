namespace AIDbOptimize.Infrastructure.Persistence.Entities;

public sealed class RagCaseEvidenceLinkEntity
{
    public Guid Id { get; set; }

    public Guid CaseRecordId { get; set; }

    public string EvidenceReference { get; set; } = string.Empty;

    public string RecommendationKey { get; set; } = string.Empty;

    public DateTimeOffset CreatedAt { get; set; }

    public RagCaseRecordEntity CaseRecord { get; set; } = null!;
}

namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record CollectionExecutionSummary(
    string PlanId,
    int PlannedEvidenceCount,
    int CollectedEvidenceCount,
    IReadOnlyCollection<string> MissingRequiredEvidenceRefs,
    IReadOnlyCollection<string> MissingOptionalEvidenceRefs,
    IReadOnlyCollection<string> Warnings,
    IReadOnlyCollection<CapabilityCollectionResult> CapabilityResults);

public sealed record CapabilityCollectionResult(
    string CapabilityId,
    string SkillReference,
    bool IsCollected,
    bool IsNormalized,
    string ErrorClassification,
    DateTimeOffset? CapturedAt,
    DateTimeOffset? ExpiresAt,
    string SourceQuality,
    IReadOnlyCollection<string> ProducedEvidenceReferences);

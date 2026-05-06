namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record EvidenceCapabilityDefinition(
    string CapabilityId,
    string SkillReference,
    string Engine,
    string CollectorKey,
    string NormalizerKey,
    string NormalizedUnit,
    string FailureClassification,
    string SourceScope,
    IReadOnlyCollection<string> RequiredEvidenceReferences,
    IReadOnlyCollection<string> RequiredMetadataNames);

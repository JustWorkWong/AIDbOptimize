namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record DiagnosisSkillDefinition(
    string SkillId,
    string Engine,
    string Version,
    string SchemaVersion,
    string BundleId,
    string BundleVersion,
    string WorkflowType,
    IReadOnlyCollection<string> OutputContract,
    IReadOnlyCollection<string> RecommendationRules,
    IReadOnlyCollection<string> ConfidenceRules,
    IReadOnlyCollection<string> ForbiddenPatterns,
    IReadOnlyCollection<string> CitationRules);

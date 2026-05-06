namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record SkillBundleDefinition(
    string BundleId,
    string BundleVersion,
    string WorkflowType,
    string Engine,
    string InvestigationSkillId,
    string InvestigationSkillVersion,
    string DiagnosisSkillId,
    string DiagnosisSkillVersion,
    string SchemaVersion,
    IReadOnlyCollection<string> Scope,
    IReadOnlyCollection<string> SkillMapping,
    IReadOnlyCollection<string> CompatibilityContract,
    IReadOnlyCollection<string> EvidenceModel,
    IReadOnlyCollection<string> RagReservedBoundary);

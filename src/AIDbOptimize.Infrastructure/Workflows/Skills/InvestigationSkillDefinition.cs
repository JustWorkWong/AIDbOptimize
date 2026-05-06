namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record InvestigationSkillDefinition(
    string SkillId,
    string Engine,
    string Version,
    string SchemaVersion,
    string BundleId,
    string BundleVersion,
    string WorkflowType,
    IReadOnlyCollection<string> RequiredEvidence,
    IReadOnlyCollection<string> OptionalEvidence,
    IReadOnlyCollection<string> BlockingRules,
    IReadOnlyCollection<string> InvestigationQuestions,
    IReadOnlyCollection<string> CollectionHints,
    IReadOnlyCollection<string> RetrievalHints);

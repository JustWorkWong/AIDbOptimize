namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed record InvestigationPlan(
    string PlanId,
    string BundleId,
    string BundleVersion,
    string SkillId,
    string SkillVersion,
    string Engine,
    IReadOnlyCollection<InvestigationPlanStep> Steps,
    IReadOnlyCollection<string> RequiredEvidenceRefs,
    IReadOnlyCollection<string> OptionalEvidenceRefs,
    IReadOnlyCollection<string> BaselineEvidenceRefs,
    string MissingContextPolicy,
    IReadOnlyCollection<string> RetrievalHints);

public sealed record InvestigationPlanStep(
    string SkillReference,
    string CapabilityId,
    string RequirementLevel,
    string CollectorKey,
    string NormalizerKey);

using AIDbOptimize.Infrastructure.Workflows.Skills;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class InvestigationPlanner(EvidenceCapabilityCatalog capabilityCatalog)
{
    public InvestigationPlan BuildPlan(
        SkillBundleDefinition bundle,
        InvestigationSkillDefinition skill)
    {
        var requiredCapabilities = capabilityCatalog.ResolveCapabilities(skill.Engine, skill.RequiredEvidence);
        var optionalCapabilities = capabilityCatalog.ResolveCapabilities(skill.Engine, skill.OptionalEvidence);
        var baselineCapabilities = capabilityCatalog.GetBaselineCapabilities(skill.Engine);

        var steps = baselineCapabilities
            .Select(item => new InvestigationPlanStep(
                item.SkillReference,
                item.CapabilityId,
                "baseline",
                item.CollectorKey,
                item.NormalizerKey))
            .Concat(requiredCapabilities.Select(item => new InvestigationPlanStep(
                item.SkillReference,
                item.CapabilityId,
                "required",
                item.CollectorKey,
                item.NormalizerKey)))
            .Concat(optionalCapabilities.Select(item => new InvestigationPlanStep(
                item.SkillReference,
                item.CapabilityId,
                "optional",
                item.CollectorKey,
                item.NormalizerKey)))
            .GroupBy(item => item.CapabilityId, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToArray();

        return new InvestigationPlan(
            Guid.NewGuid().ToString("N"),
            bundle.BundleId,
            bundle.BundleVersion,
            skill.SkillId,
            skill.Version,
            skill.Engine,
            steps,
            skill.RequiredEvidence.ToArray(),
            skill.OptionalEvidence.ToArray(),
            baselineCapabilities.Select(item => item.SkillReference).Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            "block-on-blocking-rules,degrade-on-missing-optional-or-nonblocking-required",
            skill.RetrievalHints.ToArray());
    }
}

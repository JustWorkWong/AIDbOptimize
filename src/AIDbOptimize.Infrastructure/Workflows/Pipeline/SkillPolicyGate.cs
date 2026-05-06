using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Workflows.Skills;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class SkillPolicyGate
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public SkillPolicyDecisionResult Evaluate(
        InvestigationPlan plan,
        InvestigationSkillDefinition skill,
        CollectionExecutionSummary summary,
        DbConfigEvidencePack evidence)
    {
        var missingItems = evidence.MissingContextItems.ToList();
        AddMissingEvidenceItems(missingItems, summary.MissingRequiredEvidenceRefs, "required");
        AddMissingEvidenceItems(missingItems, summary.MissingOptionalEvidenceRefs, "optional");

        var blockedReferences = ParseBlockedReferences(skill.BlockingRules);
        var isBlocked = summary.MissingRequiredEvidenceRefs.Any(reference =>
            blockedReferences.Contains(reference));
        var isDegraded = !isBlocked && (
            summary.MissingRequiredEvidenceRefs.Count > 0 ||
            summary.MissingOptionalEvidenceRefs.Count > 0 ||
            missingItems.Count > 0);

        var decision = isBlocked
            ? SkillPolicyDecision.Blocked
            : isDegraded
                ? SkillPolicyDecision.Degraded
                : SkillPolicyDecision.Pass;

        var metadata = evidence.CollectionMetadata.ToList();
        UpsertMetadata(metadata, "skill_id", skill.SkillId, "Resolved investigation skill id.");
        UpsertMetadata(metadata, "skill_version", skill.Version, "Resolved investigation skill version.");
        UpsertMetadata(metadata, "plan_id", plan.PlanId, "Investigation plan id.");
        UpsertMetadata(
            metadata,
            "collection_completeness",
            $"{summary.CollectedEvidenceCount}/{summary.PlannedEvidenceCount}",
            "Collected capability steps over planned capability steps.");
        UpsertMetadata(metadata, "gate_status", decision.ToString().ToLowerInvariant(), "Skill policy gate decision.");

        var warnings = evidence.Warnings.ToList();
        if (decision == SkillPolicyDecision.Blocked)
        {
            warnings.Add("Required evidence defined by the skill blocking rules is missing, diagnosis was stopped.");
        }
        else if (decision == SkillPolicyDecision.Degraded)
        {
            warnings.Add("Evidence is degraded, diagnosis may continue only with guarded confidence semantics.");
        }

        var updatedEvidence = new DbConfigEvidencePack(
            evidence.Engine,
            evidence.DatabaseName,
            evidence.Source,
            evidence.Recommendations,
            evidence.EvidenceItems,
            warnings,
            evidence.ConfigurationItems,
            evidence.RuntimeMetricItems,
            evidence.HostContextItems,
            evidence.ObservabilityItems,
            missingItems,
            metadata,
            evidence.ExternalKnowledgeItems);

        return new SkillPolicyDecisionResult(
            decision,
            updatedEvidence,
            BuildBlockedReportJson(updatedEvidence, decision));
    }

    private static string BuildBlockedReportJson(
        DbConfigEvidencePack evidence,
        SkillPolicyDecision decision)
    {
        if (decision != SkillPolicyDecision.Blocked)
        {
            return string.Empty;
        }

        return JsonSerializer.Serialize(new
        {
            title = $"db-config blocked - {evidence.DatabaseName}",
            summary = "Required evidence is missing. Workflow stopped before diagnosis to avoid deterministic-looking conclusions.",
            recommendations = Array.Empty<object>(),
            evidenceItems = evidence.EvidenceItems,
            missingContextItems = evidence.MissingContextItems,
            collectionMetadata = evidence.CollectionMetadata,
            warnings = evidence.Warnings
        }, SerializerOptions);
    }

    private static HashSet<string> ParseBlockedReferences(IReadOnlyCollection<string> rules)
    {
        var references = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (var rule in rules.Where(item => item.Contains("Block", StringComparison.OrdinalIgnoreCase)))
        {
            foreach (System.Text.RegularExpressions.Match match in System.Text.RegularExpressions.Regex.Matches(rule, @"`([^`]+)`"))
            {
                references.Add(match.Groups[1].Value);
            }
        }

        return references;
    }

    private static void AddMissingEvidenceItems(
        List<DbConfigMissingContextItem> missingItems,
        IReadOnlyCollection<string> missingReferences,
        string requirementLevel)
    {
        foreach (var reference in missingReferences)
        {
            if (missingItems.Any(item => string.Equals(item.Reference, reference, StringComparison.OrdinalIgnoreCase)))
            {
                continue;
            }

            missingItems.Add(new DbConfigMissingContextItem(
                reference,
                $"Skill-planned evidence `{reference}` was not collected.",
                requirementLevel == "required" ? "missing-required-evidence" : "missing-optional-evidence",
                "workflow",
                requirementLevel == "required" ? "high" : "warning"));
        }
    }

    private static void UpsertMetadata(
        List<DbConfigCollectionMetadataItem> metadata,
        string name,
        string value,
        string description)
    {
        var existing = metadata.FirstOrDefault(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            metadata.Remove(existing);
        }

        metadata.Add(new DbConfigCollectionMetadataItem(name, value, description));
    }
}

public sealed record SkillPolicyDecisionResult(
    SkillPolicyDecision Decision,
    DbConfigEvidencePack Evidence,
    string BlockedReportJson);

public enum SkillPolicyDecision
{
    Pass = 0,
    Degraded = 1,
    Blocked = 2
}

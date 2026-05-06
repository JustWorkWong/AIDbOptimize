using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Workflows.Skills;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class EvidenceCollectionSubworkflow(
    DbConfigSnapshotCollectorExecutor snapshotCollectorExecutor,
    DbConfigRuleAnalysisExecutor ruleAnalysisExecutor,
    EvidenceCapabilityCatalog capabilityCatalog)
{
    public async Task<EvidenceCollectionSubworkflowResult> ExecuteAsync(
        McpConnectionEntity connection,
        Guid workflowSessionId,
        string workflowNodeName,
        string requestedBy,
        string? traceId,
        InvestigationPlan plan,
        CancellationToken cancellationToken = default)
    {
        var snapshot = await snapshotCollectorExecutor.CollectAsync(
            connection,
            workflowSessionId,
            workflowNodeName,
            requestedBy,
            traceId,
            BuildCollectionRequest(plan),
            cancellationToken);

        var enrichedSnapshot = EnrichSnapshot(snapshot, connection.Engine, plan);
        var evidence = ruleAnalysisExecutor.Analyze(enrichedSnapshot);
        var summary = BuildSummary(enrichedSnapshot, evidence, plan);
        var enrichedEvidence = AddCollectionMetadata(evidence, plan, summary);
        return new EvidenceCollectionSubworkflowResult(enrichedSnapshot, enrichedEvidence, summary);
    }

    private CollectionExecutionSummary BuildSummary(
        DbConfigSnapshot snapshot,
        DbConfigEvidencePack evidence,
        InvestigationPlan plan)
    {
        var evidenceByReference = evidence.EvidenceItems
            .GroupBy(item => item.Reference, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Last(), StringComparer.OrdinalIgnoreCase);
        var metadataByName = snapshot.CollectionMetadata
            .GroupBy(item => item.Name, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(group => group.Key, group => group.Last(), StringComparer.OrdinalIgnoreCase);

        var results = new List<CapabilityCollectionResult>();
        var missingRequired = new List<string>();
        var missingOptional = new List<string>();

        foreach (var step in plan.Steps)
        {
            var capability = capabilityCatalog.GetByCapabilityId(step.CapabilityId);
            var producedRefs = capability.RequiredEvidenceReferences
                .Where(reference => evidenceByReference.ContainsKey(reference))
                .Concat(capability.RequiredMetadataNames.Where(reference => metadataByName.ContainsKey(reference)))
                .ToArray();
            var collected = capability.RequiredEvidenceReferences.All(reference => evidenceByReference.ContainsKey(reference))
                && capability.RequiredMetadataNames.All(reference => metadataByName.ContainsKey(reference));
            if (!collected)
            {
                if (string.Equals(step.RequirementLevel, "optional", StringComparison.OrdinalIgnoreCase))
                {
                    missingOptional.Add(step.SkillReference);
                }
                else
                {
                    missingRequired.Add(step.SkillReference);
                }
            }

            var producedItems = capability.RequiredEvidenceReferences
                .Where(reference => evidenceByReference.TryGetValue(reference, out _))
                .Select(reference => evidenceByReference[reference])
                .ToArray();
            var sourceQuality = producedItems.Length == 0
                ? producedRefs.Length == 0
                    ? "derived"
                    : "metadata"
                : producedItems.All(item => item.IsCached)
                    ? "cached"
                    : "live";

            results.Add(new CapabilityCollectionResult(
                capability.CapabilityId,
                step.SkillReference,
                collected,
                collected,
                collected ? "none" : capability.FailureClassification,
                producedItems.Select(item => item.CapturedAt).Where(item => item.HasValue).OrderBy(item => item).FirstOrDefault(),
                producedItems.Select(item => item.ExpiresAt).Where(item => item.HasValue).OrderBy(item => item).FirstOrDefault(),
                sourceQuality,
                producedRefs));
        }

        return new CollectionExecutionSummary(
            plan.PlanId,
            plan.Steps.Count,
            results.Count(item => item.IsCollected),
            missingRequired.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            missingOptional.Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
            evidence.Warnings.ToArray(),
            results);
    }

    private static DbConfigSnapshot EnrichSnapshot(
        DbConfigSnapshot snapshot,
        AIDbOptimize.Domain.Mcp.Enums.DatabaseEngine engine,
        InvestigationPlan plan)
    {
        var metadata = snapshot.CollectionMetadata.ToList();
        var metadataByName = metadata.ToDictionary(item => item.Name, item => item, StringComparer.OrdinalIgnoreCase);

        UpsertMetadata(
            metadata,
            metadataByName,
            "engine.version",
            snapshot.CollectedValues.TryGetValue("engine_version", out var version) ? version : "unknown",
            "Normalized engine version captured for skill planning.");
        UpsertMetadata(
            metadata,
            metadataByName,
            "deployment.flavor",
            metadataByName.TryGetValue("resource_scope", out var scope) ? scope.Value : "unknown",
            "Normalized deployment flavor derived from runtime resource scope.");
        UpsertMetadata(
            metadata,
            metadataByName,
            "parameter.source",
            snapshot.Source,
            "Primary source used to collect configuration parameters.");
        UpsertMetadata(
            metadata,
            metadataByName,
            "parameter.apply_scope",
            engine == AIDbOptimize.Domain.Mcp.Enums.DatabaseEngine.MySql ? "global" : "instance",
            "Normalized parameter scope for the current engine.");
        UpsertMetadata(
            metadata,
            metadataByName,
            "parameter.normalized_unit",
            BuildUnitMap(snapshot),
            "Normalized unit map for collected evidence references.");
        UpsertMetadata(
            metadata,
            metadataByName,
            "plan_id",
            plan.PlanId,
            "Investigation plan id associated with this collection run.");

        return new DbConfigSnapshot(
            snapshot.Engine,
            snapshot.DatabaseName,
            snapshot.Source,
            snapshot.CollectedValues,
            snapshot.Warnings,
            snapshot.ConfigurationItems,
            snapshot.RuntimeMetricItems,
            snapshot.HostContextItems,
            snapshot.ObservabilityItems,
            snapshot.MissingContextItems,
            metadata);
    }

    private static DbConfigEvidencePack AddCollectionMetadata(
        DbConfigEvidencePack evidence,
        InvestigationPlan plan,
        CollectionExecutionSummary summary)
    {
        var metadata = evidence.CollectionMetadata.ToList();
        UpsertMetadata(metadata, "bundle_id", plan.BundleId, "Resolved bundle id used by the workflow.");
        UpsertMetadata(metadata, "bundle_version", plan.BundleVersion, "Resolved bundle version used by the workflow.");
        UpsertMetadata(metadata, "investigation_skill_id", plan.SkillId, "Resolved investigation skill id.");
        UpsertMetadata(metadata, "investigation_skill_version", plan.SkillVersion, "Resolved investigation skill version.");
        UpsertMetadata(metadata, "plan_id", plan.PlanId, "Investigation plan id.");
        UpsertMetadata(metadata, "planned_evidence_count", summary.PlannedEvidenceCount.ToString(), "Number of planned capability steps.");
        UpsertMetadata(metadata, "collected_evidence_count", summary.CollectedEvidenceCount.ToString(), "Number of capability steps collected successfully.");
        UpsertMetadata(metadata, "planned_capability_ids_json", JsonSerializer.Serialize(plan.Steps.Select(item => item.CapabilityId).Distinct(StringComparer.OrdinalIgnoreCase)), "Capability ids requested by the investigation planner.");
        UpsertMetadata(metadata, "planned_skill_references_json", JsonSerializer.Serialize(plan.Steps.Select(item => item.SkillReference).Distinct(StringComparer.OrdinalIgnoreCase)), "Skill references requested by the investigation planner.");
        UpsertMetadata(metadata, "missing_required_evidence_refs", JsonSerializer.Serialize(summary.MissingRequiredEvidenceRefs), "Required skill references missing after collection.");
        UpsertMetadata(metadata, "missing_optional_evidence_refs", JsonSerializer.Serialize(summary.MissingOptionalEvidenceRefs), "Optional skill references missing after collection.");
        UpsertMetadata(metadata, "capability_results_json", JsonSerializer.Serialize(summary.CapabilityResults), "Per-capability collection outcomes emitted by the subworkflow.");
        UpsertMetadata(metadata, "missing_context_policy", plan.MissingContextPolicy, "Planner-defined policy that maps collection gaps to gate behavior.");
        UpsertMetadata(metadata, "retrieval_hints_json", JsonSerializer.Serialize(plan.RetrievalHints), "Reserved retrieval hints passed through the investigation planner.");

        return new DbConfigEvidencePack(
            evidence.Engine,
            evidence.DatabaseName,
            evidence.Source,
            evidence.Recommendations,
            evidence.EvidenceItems,
            evidence.Warnings,
            evidence.ConfigurationItems,
            evidence.RuntimeMetricItems,
            evidence.HostContextItems,
            evidence.ObservabilityItems,
            evidence.MissingContextItems,
            metadata,
            evidence.ExternalKnowledgeItems);
    }

    private static string BuildUnitMap(DbConfigSnapshot snapshot)
    {
        var units = snapshot.ConfigurationItems
            .Concat(snapshot.RuntimeMetricItems)
            .Concat(snapshot.HostContextItems)
            .Concat(snapshot.ObservabilityItems)
            .GroupBy(item => item.Reference, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => group.Last().Unit ?? "raw",
                StringComparer.OrdinalIgnoreCase);
        return JsonSerializer.Serialize(units);
    }

    private DbConfigSnapshotCollectionRequest BuildCollectionRequest(InvestigationPlan plan)
    {
        var requestedCollectorKeys = plan.Steps
            .Select(item => item.CollectorKey)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var requestedValueKeys = plan.Steps
            .Select(item => capabilityCatalog.GetByCapabilityId(item.CapabilityId))
            .Where(item => string.Equals(item.CollectorKey, "db-config-snapshot", StringComparison.OrdinalIgnoreCase))
            .SelectMany(item => item.RequiredEvidenceReferences)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .ToArray();
        var includeHostContext = requestedCollectorKeys.Contains("host-context", StringComparer.OrdinalIgnoreCase);

        return new DbConfigSnapshotCollectionRequest(
            requestedCollectorKeys,
            requestedValueKeys,
            includeHostContext);
    }

    private static void UpsertMetadata(
        List<DbConfigCollectionMetadataItem> metadata,
        Dictionary<string, DbConfigCollectionMetadataItem> metadataByName,
        string name,
        string value,
        string description)
    {
        if (metadataByName.TryGetValue(name, out var existing))
        {
            metadata.Remove(existing);
        }

        var replacement = new DbConfigCollectionMetadataItem(name, value, description);
        metadata.Add(replacement);
        metadataByName[name] = replacement;
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

public sealed record EvidenceCollectionSubworkflowResult(
    DbConfigSnapshot Snapshot,
    DbConfigEvidencePack Evidence,
    CollectionExecutionSummary Summary);

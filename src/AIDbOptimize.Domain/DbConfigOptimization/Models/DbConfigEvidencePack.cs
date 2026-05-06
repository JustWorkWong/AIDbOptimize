using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 配置优化结构化证据包。
/// </summary>
public sealed record DbConfigEvidencePack
{
    public DbConfigEvidencePack(
        DatabaseEngine engine,
        string databaseName,
        string source,
        IReadOnlyList<DbConfigRecommendation> recommendations,
        IReadOnlyList<DbConfigEvidenceItem> evidenceItems,
        IReadOnlyList<string> warnings,
        IReadOnlyList<DbConfigEvidenceItem>? configurationItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? runtimeMetricItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? hostContextItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? observabilityItems = null,
        IReadOnlyList<DbConfigMissingContextItem>? missingContextItems = null,
        IReadOnlyList<DbConfigCollectionMetadataItem>? collectionMetadata = null,
        IReadOnlyList<DbConfigEvidenceItem>? externalKnowledgeItems = null)
    {
        Engine = engine;
        DatabaseName = databaseName;
        Source = source;
        Recommendations = recommendations;
        EvidenceItems = evidenceItems;
        Warnings = warnings;
        ConfigurationItems = configurationItems ?? evidenceItems;
        RuntimeMetricItems = runtimeMetricItems ?? [];
        HostContextItems = hostContextItems ?? [];
        ObservabilityItems = observabilityItems ?? [];
        MissingContextItems = missingContextItems ?? [];
        CollectionMetadata = collectionMetadata ?? [];
        ExternalKnowledgeItems = externalKnowledgeItems ?? [];
    }

    public DatabaseEngine Engine { get; }

    public string DatabaseName { get; }

    public string Source { get; }

    public IReadOnlyList<DbConfigRecommendation> Recommendations { get; }

    public IReadOnlyList<DbConfigEvidenceItem> EvidenceItems { get; }

    public IReadOnlyList<string> Warnings { get; }

    public IReadOnlyList<DbConfigEvidenceItem> ConfigurationItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> RuntimeMetricItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> HostContextItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> ObservabilityItems { get; }

    public IReadOnlyList<DbConfigMissingContextItem> MissingContextItems { get; }

    public IReadOnlyList<DbConfigCollectionMetadataItem> CollectionMetadata { get; }

    public IReadOnlyList<DbConfigEvidenceItem> ExternalKnowledgeItems { get; }
}

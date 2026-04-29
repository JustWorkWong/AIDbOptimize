using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 数据库配置采集快照。
/// </summary>
public sealed record DbConfigSnapshot
{
    public DbConfigSnapshot(
        DatabaseEngine engine,
        string databaseName,
        string source,
        IReadOnlyDictionary<string, string> collectedValues,
        IReadOnlyList<string> warnings,
        IReadOnlyList<DbConfigEvidenceItem>? configurationItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? runtimeMetricItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? hostContextItems = null,
        IReadOnlyList<DbConfigEvidenceItem>? observabilityItems = null,
        IReadOnlyList<DbConfigMissingContextItem>? missingContextItems = null,
        IReadOnlyList<DbConfigCollectionMetadataItem>? collectionMetadata = null)
    {
        Engine = engine;
        DatabaseName = databaseName;
        Source = source;
        CollectedValues = collectedValues;
        Warnings = warnings;
        ConfigurationItems = configurationItems ?? BuildConfigurationItems(collectedValues, source);
        RuntimeMetricItems = runtimeMetricItems ?? [];
        HostContextItems = hostContextItems ?? [];
        ObservabilityItems = observabilityItems ?? [];
        MissingContextItems = missingContextItems ?? [];
        CollectionMetadata = collectionMetadata ?? [];
    }

    public DatabaseEngine Engine { get; }

    public string DatabaseName { get; }

    public string Source { get; }

    public IReadOnlyDictionary<string, string> CollectedValues { get; }

    public IReadOnlyList<string> Warnings { get; }

    public IReadOnlyList<DbConfigEvidenceItem> ConfigurationItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> RuntimeMetricItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> HostContextItems { get; }

    public IReadOnlyList<DbConfigEvidenceItem> ObservabilityItems { get; }

    public IReadOnlyList<DbConfigMissingContextItem> MissingContextItems { get; }

    public IReadOnlyList<DbConfigCollectionMetadataItem> CollectionMetadata { get; }

    private static IReadOnlyList<DbConfigEvidenceItem> BuildConfigurationItems(
        IReadOnlyDictionary<string, string> collectedValues,
        string source)
    {
        return collectedValues
            .Where(static pair => !string.Equals(pair.Key, "engine", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(pair.Key, "database_name", StringComparison.OrdinalIgnoreCase)
                && !string.Equals(pair.Key, "tool_name", StringComparison.OrdinalIgnoreCase))
            .Select(pair => new DbConfigEvidenceItem(
                source,
                pair.Key,
                $"采集到配置项 {pair.Key}={pair.Value}",
                Category: "configuration",
                RawValue: pair.Value,
                NormalizedValue: pair.Value,
                SourceScope: "db"))
            .ToArray();
    }
}

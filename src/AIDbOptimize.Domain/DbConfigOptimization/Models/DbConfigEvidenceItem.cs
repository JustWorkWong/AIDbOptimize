namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 证据项。
/// </summary>
public sealed record DbConfigEvidenceItem(
    string SourceType,
    string Reference,
    string Description,
    string Category = "configuration",
    string? RawValue = null,
    string? NormalizedValue = null,
    string? Unit = null,
    string SourceScope = "db",
    DateTimeOffset? CapturedAt = null,
    DateTimeOffset? ExpiresAt = null,
    bool IsCached = false,
    string? CollectionMethod = null);

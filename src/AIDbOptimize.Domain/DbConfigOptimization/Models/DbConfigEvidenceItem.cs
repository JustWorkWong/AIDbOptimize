namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 证据项。
/// </summary>
public sealed record DbConfigEvidenceItem(
    string SourceType,
    string Reference,
    string Description);

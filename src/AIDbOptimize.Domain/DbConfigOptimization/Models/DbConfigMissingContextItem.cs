namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 缺失上下文项。
/// </summary>
public sealed record DbConfigMissingContextItem(
    string Reference,
    string Description,
    string Reason,
    string SourceScope = "unknown",
    string Severity = "warning");

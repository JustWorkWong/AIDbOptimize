namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 配置优化建议。
/// </summary>
public sealed record DbConfigRecommendation(
    string Key,
    string Suggestion,
    string Severity);

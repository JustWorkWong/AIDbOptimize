using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 配置优化建议。
/// </summary>
public sealed record DbConfigRecommendation
{
    public DbConfigRecommendation(
        string key,
        string suggestion,
        string severity,
        string findingType = "configuration",
        string confidence = "medium",
        bool requiresMoreContext = false,
        string? impactSummary = null,
        IReadOnlyList<string>? evidenceReferences = null,
        string recommendationClass = "tuning",
        DbConfigRecommendationType recommendationType = DbConfigRecommendationType.ActionableRecommendation,
        string? appliesWhen = null,
        string? ruleId = null,
        string? ruleVersion = null)
    {
        Key = key;
        Suggestion = suggestion;
        Severity = severity;
        FindingType = findingType;
        Confidence = confidence;
        RequiresMoreContext = requiresMoreContext;
        ImpactSummary = impactSummary;
        EvidenceReferences = evidenceReferences ?? [];
        RecommendationClass = recommendationClass;
        RecommendationType = recommendationType;
        AppliesWhen = appliesWhen;
        RuleId = ruleId;
        RuleVersion = ruleVersion;
    }

    public string Key { get; }

    public string Suggestion { get; }

    public string Severity { get; }

    public string FindingType { get; }

    public string Confidence { get; }

    public bool RequiresMoreContext { get; }

    public string? ImpactSummary { get; }

    public IReadOnlyList<string> EvidenceReferences { get; }

    public string RecommendationClass { get; }

    public DbConfigRecommendationType RecommendationType { get; }

    public string? AppliesWhen { get; }

    public string? RuleId { get; }

    public string? RuleVersion { get; }
}

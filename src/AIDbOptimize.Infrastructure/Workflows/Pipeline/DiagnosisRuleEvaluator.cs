using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Workflows.Skills;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class DiagnosisRuleEvaluator
{
    private readonly DiagnosisRuleContractEvaluator _contractEvaluator = new();
    private readonly DegradedRecommendationSemanticsEvaluator _degradedEvaluator = new();

    public IReadOnlyList<DbConfigRecommendation> Evaluate(
        DbConfigEvidencePack evidence,
        DiagnosisSkillDefinition skill,
        IReadOnlyCollection<DbConfigRecommendation> recommendations)
    {
        _contractEvaluator.Validate(evidence, recommendations);

        return recommendations
            .Select(item => NormalizeRecommendation(evidence, skill, item))
            .ToArray();
    }

    private DbConfigRecommendation NormalizeRecommendation(
        DbConfigEvidencePack evidence,
        DiagnosisSkillDefinition skill,
        DbConfigRecommendation recommendation)
    {
        ValidateForbiddenPatterns(skill, recommendation);

        var semantics = _degradedEvaluator.Evaluate(evidence, recommendation);
        var recommendationType = semantics.Type == DegradedRecommendationType.Actionable
            ? DbConfigRecommendationType.ActionableRecommendation
            : DbConfigRecommendationType.RequestMoreContext;
        var requiresMoreContext = recommendation.RequiresMoreContext || recommendationType == DbConfigRecommendationType.RequestMoreContext;

        return new DbConfigRecommendation(
            recommendation.Key,
            recommendation.Suggestion,
            recommendation.Severity,
            recommendation.FindingType,
            recommendation.Confidence,
            requiresMoreContext,
            recommendation.ImpactSummary,
            recommendation.EvidenceReferences,
            recommendation.RecommendationClass,
            recommendationType,
            recommendation.AppliesWhen,
            recommendation.RuleId,
            recommendation.RuleVersion);
    }

    private static void ValidateForbiddenPatterns(
        DiagnosisSkillDefinition skill,
        DbConfigRecommendation recommendation)
    {
        var text = $"{recommendation.Suggestion} {recommendation.AppliesWhen} {recommendation.ImpactSummary}";

        foreach (var pattern in skill.ForbiddenPatterns)
        {
            if (pattern.Contains("generic", StringComparison.OrdinalIgnoreCase) &&
                (text.Contains("best practice", StringComparison.OrdinalIgnoreCase) ||
                 text.Contains("最佳实践", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    $"Recommendation `{recommendation.Key}` matched a forbidden generic-best-practice pattern.");
            }

            if (pattern.Contains("external knowledge", StringComparison.OrdinalIgnoreCase) &&
                (text.Contains("external knowledge", StringComparison.OrdinalIgnoreCase) ||
                 text.Contains("RAG", StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException(
                    $"Recommendation `{recommendation.Key}` matched a forbidden external-knowledge grounding pattern.");
            }
        }
    }
}

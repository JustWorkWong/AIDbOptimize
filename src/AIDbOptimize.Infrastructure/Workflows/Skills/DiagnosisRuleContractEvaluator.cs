using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Skills;

/// <summary>
/// Minimal rule evaluator used by the diagnosis-rule spike.
/// </summary>
public sealed class DiagnosisRuleContractEvaluator
{
    public void Validate(DbConfigEvidencePack evidence, IReadOnlyCollection<DbConfigRecommendation> recommendations)
    {
        if (evidence.HostContextItems.Count == 0)
        {
            foreach (var recommendation in recommendations)
            {
                var text = $"{recommendation.Suggestion} {recommendation.AppliesWhen} {recommendation.ImpactSummary}";
                if (ContainsSpecificTargetValue(text))
                {
                    throw new InvalidOperationException(
                        $"Recommendation `{recommendation.Key}` cannot include a specific target value when host context is missing.");
                }
            }
        }

        foreach (var recommendation in recommendations)
        {
            if (string.Equals(recommendation.Confidence, "low", StringComparison.OrdinalIgnoreCase) &&
                !recommendation.RequiresMoreContext)
            {
                throw new InvalidOperationException(
                    $"Low-confidence recommendation `{recommendation.Key}` must set requiresMoreContext=true.");
            }
        }
    }

    private static bool ContainsSpecificTargetValue(string text)
    {
        if (string.IsNullOrWhiteSpace(text))
        {
            return false;
        }

        return System.Text.RegularExpressions.Regex.IsMatch(
            text,
            @"(\d+(\.\d+)?\s*(KB|MB|GB|TB|%|cores?))|(调到|设置为|set to|tune to)",
            System.Text.RegularExpressions.RegexOptions.IgnoreCase | System.Text.RegularExpressions.RegexOptions.CultureInvariant);
    }
}

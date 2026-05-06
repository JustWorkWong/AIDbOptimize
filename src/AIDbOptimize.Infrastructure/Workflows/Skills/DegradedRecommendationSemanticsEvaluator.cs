using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Skills;

/// <summary>
/// Minimal evaluator used by the degraded-semantics spike.
/// </summary>
public sealed class DegradedRecommendationSemanticsEvaluator
{
    public DegradedRecommendationSemanticsResult Evaluate(
        DbConfigEvidencePack evidence,
        DbConfigRecommendation recommendation)
    {
        var positiveReferences = evidence.EvidenceItems
            .Select(item => item.Reference)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var missingReferences = evidence.MissingContextItems
            .Select(item => item.Reference)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (recommendation.EvidenceReferences.Count == 0)
        {
            throw new InvalidOperationException(
                $"Recommendation `{recommendation.Key}` must declare explicit evidence references.");
        }

        var references = recommendation.EvidenceReferences;

        var resolvedPositive = new List<string>();
        var resolvedMissing = new List<string>();

        foreach (var reference in references)
        {
            if (positiveReferences.Contains(reference))
            {
                resolvedPositive.Add(reference);
                continue;
            }

            if (missingReferences.Contains(reference))
            {
                resolvedMissing.Add(reference);
                continue;
            }

            throw new InvalidOperationException(
                $"Recommendation `{recommendation.Key}` references unresolved evidence `{reference}`.");
        }

        if (resolvedPositive.Count > 0)
        {
            return new DegradedRecommendationSemanticsResult(
                DegradedRecommendationType.Actionable,
                resolvedPositive,
                resolvedMissing);
        }

        return new DegradedRecommendationSemanticsResult(
            DegradedRecommendationType.RequestMoreContext,
            resolvedPositive,
            resolvedMissing);
    }
}

public enum DegradedRecommendationType
{
    Actionable = 0,
    RequestMoreContext = 1
}

public sealed record DegradedRecommendationSemanticsResult(
    DegradedRecommendationType Type,
    IReadOnlyCollection<string> PositiveReferences,
    IReadOnlyCollection<string> MissingContextReferences);

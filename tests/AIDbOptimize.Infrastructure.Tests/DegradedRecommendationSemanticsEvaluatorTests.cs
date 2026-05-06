using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class DegradedRecommendationSemanticsEvaluatorTests
{
    [Fact]
    public void Evaluate_ReturnsActionable_WhenPositiveEvidenceReferenceExists()
    {
        var evaluator = new DegradedRecommendationSemanticsEvaluator();
        var evidence = CreateEvidencePack(
            evidenceItems:
            [
                new DbConfigEvidenceItem("collector", "config:max_connections", "max_connections")
            ],
            missingContextItems: []);
        var recommendation = new DbConfigRecommendation(
            "max_connections",
            "review connection cap",
            "medium",
            evidenceReferences: ["config:max_connections"]);

        var result = evaluator.Evaluate(evidence, recommendation);

        Assert.Equal(DegradedRecommendationType.Actionable, result.Type);
        Assert.Contains("config:max_connections", result.PositiveReferences);
        Assert.Empty(result.MissingContextReferences);
    }

    [Fact]
    public void Evaluate_ReturnsRequestMoreContext_WhenOnlyMissingContextReferenceExists()
    {
        var evaluator = new DegradedRecommendationSemanticsEvaluator();
        var evidence = CreateEvidencePack(
            evidenceItems: [],
            missingContextItems:
            [
                new DbConfigMissingContextItem("missing:host.memory.total", "host memory missing", "unavailable")
            ]);
        var recommendation = new DbConfigRecommendation(
            "host.memory.total",
            "collect host memory first",
            "warning",
            evidenceReferences: ["missing:host.memory.total"]);

        var result = evaluator.Evaluate(evidence, recommendation);

        Assert.Equal(DegradedRecommendationType.RequestMoreContext, result.Type);
        Assert.Empty(result.PositiveReferences);
        Assert.Contains("missing:host.memory.total", result.MissingContextReferences);
    }

    [Fact]
    public void Evaluate_ReturnsActionable_WhenPositiveAndMissingContextReferencesAreMixed()
    {
        var evaluator = new DegradedRecommendationSemanticsEvaluator();
        var evidence = CreateEvidencePack(
            evidenceItems:
            [
                new DbConfigEvidenceItem("collector", "config:max_connections", "max_connections")
            ],
            missingContextItems:
            [
                new DbConfigMissingContextItem("missing:host.memory.total", "host memory missing", "unavailable")
            ]);
        var recommendation = new DbConfigRecommendation(
            "max_connections",
            "review connection cap conservatively",
            "medium",
            evidenceReferences: ["config:max_connections", "missing:host.memory.total"]);

        var result = evaluator.Evaluate(evidence, recommendation);

        Assert.Equal(DegradedRecommendationType.Actionable, result.Type);
        Assert.Contains("config:max_connections", result.PositiveReferences);
        Assert.Contains("missing:host.memory.total", result.MissingContextReferences);
    }

    [Fact]
    public void Evaluate_RejectsUnresolvedReference()
    {
        var evaluator = new DegradedRecommendationSemanticsEvaluator();
        var evidence = CreateEvidencePack(evidenceItems: [], missingContextItems: []);
        var recommendation = new DbConfigRecommendation(
            "max_connections",
            "review connection cap",
            "medium",
            evidenceReferences: ["unknown:reference"]);

        var error = Assert.Throws<InvalidOperationException>(() =>
            evaluator.Evaluate(evidence, recommendation));

        Assert.Contains("unresolved", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Evaluate_RejectsRecommendationWithoutExplicitEvidenceReferences()
    {
        var evaluator = new DegradedRecommendationSemanticsEvaluator();
        var evidence = CreateEvidencePack(
            evidenceItems:
            [
                new DbConfigEvidenceItem("collector", "config:max_connections", "max_connections")
            ],
            missingContextItems: []);
        var recommendation = new DbConfigRecommendation(
            "max_connections",
            "review connection cap",
            "medium");

        var error = Assert.Throws<InvalidOperationException>(() =>
            evaluator.Evaluate(evidence, recommendation));

        Assert.Contains("explicit evidence references", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    private static DbConfigEvidencePack CreateEvidencePack(
        IReadOnlyList<DbConfigEvidenceItem> evidenceItems,
        IReadOnlyList<DbConfigMissingContextItem> missingContextItems)
    {
        return new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            evidenceItems,
            [],
            missingContextItems: missingContextItems);
    }
}

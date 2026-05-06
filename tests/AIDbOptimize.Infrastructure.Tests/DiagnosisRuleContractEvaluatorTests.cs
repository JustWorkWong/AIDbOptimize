using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class DiagnosisRuleContractEvaluatorTests
{
    [Fact]
    public void Validate_RejectsSpecificTargetValue_WhenHostContextIsMissing()
    {
        var evaluator = new DiagnosisRuleContractEvaluator();
        var evidence = CreateEvidencePack(hostContextItems: []);
        IReadOnlyCollection<DbConfigRecommendation> recommendations =
        [
            new DbConfigRecommendation(
                "innodb_buffer_pool_size",
                "set to 4GB",
                "medium",
                confidence: "medium",
                requiresMoreContext: false,
                recommendationClass: "tuning")
        ];

        var error = Assert.Throws<InvalidOperationException>(
            () => evaluator.Validate(evidence, recommendations));

        Assert.Contains("specific target value", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Validate_AllowsSpecificTargetValue_WhenHostContextExists()
    {
        var evaluator = new DiagnosisRuleContractEvaluator();
        var evidence = CreateEvidencePack(
            hostContextItems:
            [
                new DbConfigEvidenceItem("host", "host:memory.total", "memory total", Category: "hostContext")
            ]);
        IReadOnlyCollection<DbConfigRecommendation> recommendations =
        [
            new DbConfigRecommendation(
                "innodb_buffer_pool_size",
                "set to 4GB",
                "medium",
                confidence: "medium",
                requiresMoreContext: false,
                recommendationClass: "tuning")
        ];

        evaluator.Validate(evidence, recommendations);
    }

    [Fact]
    public void Validate_RejectsLowConfidenceRecommendation_WithoutRequiresMoreContext()
    {
        var evaluator = new DiagnosisRuleContractEvaluator();
        var evidence = CreateEvidencePack(hostContextItems: []);
        IReadOnlyCollection<DbConfigRecommendation> recommendations =
        [
            new DbConfigRecommendation(
                "max_connections",
                "review connection cap",
                "warning",
                confidence: "low",
                requiresMoreContext: false,
                recommendationClass: "capacity-planning")
        ];

        var error = Assert.Throws<InvalidOperationException>(
            () => evaluator.Validate(evidence, recommendations));

        Assert.Contains("requiresMoreContext", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Validate_AllowsLowConfidenceRecommendation_WhenRequiresMoreContextIsTrue()
    {
        var evaluator = new DiagnosisRuleContractEvaluator();
        var evidence = CreateEvidencePack(hostContextItems: []);
        IReadOnlyCollection<DbConfigRecommendation> recommendations =
        [
            new DbConfigRecommendation(
                "max_connections",
                "review connection cap",
                "warning",
                confidence: "low",
                requiresMoreContext: true,
                recommendationClass: "capacity-planning")
        ];

        evaluator.Validate(evidence, recommendations);
    }

    private static DbConfigEvidencePack CreateEvidencePack(IReadOnlyList<DbConfigEvidenceItem> hostContextItems)
    {
        return new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [],
            [],
            hostContextItems: hostContextItems);
    }
}

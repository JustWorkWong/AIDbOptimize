using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class DbConfigQualityGateTests
{
    [Fact]
    public void RecommendationSchemaValidator_RejectsMissingSummary()
    {
        var validator = new RecommendationSchemaValidator();
        var json = """{"title":"x","recommendations":[]}""";

        var error = Assert.Throws<InvalidOperationException>(() => validator.Validate(json));

        Assert.Contains("summary", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GroundingExecutor_RejectsRecommendationWithoutEvidence()
    {
        var executor = new DbConfigGroundingExecutor(new RecommendationSchemaValidator());
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            [
                new DbConfigRecommendation("shared_buffers", "Re-evaluate shared_buffers.", "medium")
            ],
            [
                new DbConfigEvidenceItem("config", "work_mem", "Only work_mem was collected.")
            ],
            Array.Empty<string>());
        var reportJson = """
        {
          "title":"db-config report",
          "summary":"Adjust shared_buffers.",
          "recommendations":[
            {"key":"shared_buffers","suggestion":"Adjust shared_buffers.","severity":"medium"}
          ],
          "evidenceItems":[
            {"sourceType":"config","reference":"work_mem","description":"Only work_mem was collected."}
          ],
          "warnings":[]
        }
        """;

        var error = Assert.Throws<InvalidOperationException>(() => executor.Validate(evidence, reportJson));

        Assert.Contains("evidence", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GroundingExecutor_AllowsExplicitEvidenceReferences()
    {
        var executor = new DbConfigGroundingExecutor(new RecommendationSchemaValidator());
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [
                new DbConfigRecommendation(
                    "observability-gap",
                    "Enable better observability.",
                    "warning",
                    evidenceReferences: ["slow_query_log"])
            ],
            [
                new DbConfigEvidenceItem("config", "slow_query_log", "Slow query log state.", Category: "observability")
            ],
            Array.Empty<string>());
        var reportJson = """
        {
          "title":"db-config report",
          "summary":"Improve observability.",
          "recommendations":[
            {
              "key":"observability-gap",
              "suggestion":"Enable better observability.",
              "severity":"warning",
              "evidenceReferences":["slow_query_log"]
            }
          ],
          "evidenceItems":[
            {"sourceType":"config","reference":"slow_query_log","description":"Slow query log state."}
          ],
          "warnings":[]
        }
        """;

        executor.Validate(evidence, reportJson);
    }

    [Fact]
    public void ReviewAdjustmentValidator_AllowsKnownAdjustmentKey()
    {
        var validator = new ReviewAdjustmentValidator();
        using var document = JsonDocument.Parse("""{"riskLevelOverrides":{"max_connections":"warning"}}""");

        var result = validator.ValidateAndNormalize(document.RootElement, "manual adjust");

        Assert.True(result.RiskLevelOverrides.TryGetValue("max_connections", out var riskLevel));
        Assert.Equal("warning", riskLevel);
    }

    [Fact]
    public void ReviewAdjustmentValidator_RejectsUnknownAdjustmentKey()
    {
        var validator = new ReviewAdjustmentValidator();
        using var document = JsonDocument.Parse("""{"dropTable":"users"}""");

        var error = Assert.Throws<InvalidOperationException>(() =>
            validator.ValidateAndNormalize(document.RootElement, "bad adjust"));

        Assert.Contains("dropTable", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void HumanReviewGateExecutor_CompletesImmediately_WhenReviewNotRequired()
    {
        var gate = new DbConfigHumanReviewGateExecutor();

        var result = gate.Decide(requireHumanReview: false, reportTitle: "db-config report");

        Assert.Equal(HumanReviewDecision.CompleteDirectly, result.Decision);
        Assert.Null(result.Title);
    }

    [Fact]
    public void HumanReviewGateExecutor_CreatesReview_WhenReviewRequired()
    {
        var gate = new DbConfigHumanReviewGateExecutor();

        var result = gate.Decide(requireHumanReview: true, reportTitle: "db-config report");

        Assert.Equal(HumanReviewDecision.RequireReview, result.Decision);
        Assert.Contains("审核", result.Title, StringComparison.Ordinal);
    }
}

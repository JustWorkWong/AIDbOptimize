using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using Xunit;

namespace AIDbOptimize.ApiService.Tests;

public sealed class DbConfigPipelineTests
{
    [Fact]
    public void InputValidationExecutor_RejectsEmptyDatabaseName()
    {
        var executor = new DbConfigInputValidationExecutor();

        Assert.Throws<InvalidOperationException>(() =>
            executor.Validate(Guid.NewGuid(), string.Empty, DatabaseEngine.PostgreSql, true));
    }

    [Fact]
    public void RuleAnalysisExecutor_BuildsEvidencePackFromSnapshot()
    {
        var executor = new DbConfigRuleAnalysisExecutor();
        var snapshot = new DbConfigSnapshot(
            DatabaseEngine.MySql,
            "orders",
            "metadata-fallback",
            new Dictionary<string, string>
            {
                ["max_connections"] = "500",
                ["buffer_pool"] = "256MB"
            },
            new[]
            {
                "未发现可直接执行的只读采集工具，当前使用 metadata fallback。"
            });

        var evidence = executor.Analyze(snapshot);

        Assert.Equal("metadata-fallback", evidence.Source);
        Assert.NotEmpty(evidence.Recommendations);
        Assert.NotEmpty(evidence.EvidenceItems);
    }

    [Fact]
    public void DiagnosisReportBuilder_CreatesStructuredReport()
    {
        var builder = new DbConfigDiagnosisReportBuilder();
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            new[]
            {
                new DbConfigRecommendation(
                    "shared_buffers",
                    "建议重新评估 shared_buffers 配置。",
                    "medium")
            },
            new[]
            {
                new DbConfigEvidenceItem(
                    "config",
                    "shared_buffers",
                    "当前实例配置来源于采集快照。")
            },
            Array.Empty<string>());

        var report = builder.Build(evidence, "降低 IO 抖动");

        Assert.Contains("shared_buffers", report);
        Assert.Contains("降低 IO 抖动", report);
    }
}

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
                ["innodb_buffer_pool_size"] = "256MB",
                ["engine_version"] = "8.0.36",
                ["version_comment"] = "MySQL Community Server",
                ["database_size_bytes"] = "104857600",
                ["parameter_apply_scope"] = "global",
                ["created_tmp_disk_tables"] = "50",
                ["created_tmp_tables"] = "100",
                ["tmp_table_size"] = "16777216",
                ["max_heap_table_size"] = "16777216"
            },
            new[]
            {
                "未发现可直接执行的只读采集工具，当前使用 metadata fallback。"
            },
            runtimeMetricItems:
            [
                new DbConfigEvidenceItem("collector", "created_tmp_disk_tables", "created_tmp_disk_tables", Category: "runtimeMetric", RawValue: "50", NormalizedValue: "50"),
                new DbConfigEvidenceItem("collector", "created_tmp_tables", "created_tmp_tables", Category: "runtimeMetric", RawValue: "100", NormalizedValue: "100")
            ],
            missingContextItems:
            [
                new DbConfigMissingContextItem(
                    "memory_limit_bytes",
                    "实例可用内存上限缺失。",
                    "metadata fallback")
            ]);

        var evidence = executor.Analyze(snapshot);

        Assert.Equal("metadata-fallback", evidence.Source);
        Assert.NotEmpty(evidence.Recommendations);
        Assert.NotEmpty(evidence.EvidenceItems);
        Assert.NotEmpty(evidence.ConfigurationItems);
        Assert.NotEmpty(evidence.MissingContextItems);
        Assert.Contains(evidence.Recommendations, item => item.RuleId == "mysql.buffer-pool.reassessment");
        Assert.Contains(evidence.Recommendations, item => item.RuleId == "generic.observability-gap");
        Assert.Contains(evidence.Recommendations, item => item.RuleId == "mysql.tmp-table.spill");
    }

    [Fact]
    public void RuleAnalysisExecutor_EmitsPostgreSqlCheckpointAndTempIoRecommendations()
    {
        var executor = new DbConfigRuleAnalysisExecutor();
        var snapshot = new DbConfigSnapshot(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            new Dictionary<string, string>
            {
                ["shared_buffers"] = "128MB",
                ["work_mem"] = "4MB",
                ["checkpoint_timeout"] = "5min",
                ["checkpoint_completion_target"] = "0.5",
                ["random_page_cost"] = "4.0",
                ["seq_page_cost"] = "1.0",
                ["engine_version"] = "16.3",
                ["database_size_bytes"] = "104857600",
                ["parameter_apply_scope"] = "instance",
                ["checkpoints_timed"] = "10",
                ["checkpoints_req"] = "25",
                ["temp_files"] = "42",
                ["blks_hit"] = "1000",
                ["blks_read"] = "500"
            },
            Array.Empty<string>(),
            configurationItems:
            [
                new DbConfigEvidenceItem("collector", "shared_buffers", "shared_buffers", RawValue: "128MB", NormalizedValue: "128MB"),
                new DbConfigEvidenceItem("collector", "work_mem", "work_mem", RawValue: "4MB", NormalizedValue: "4MB"),
                new DbConfigEvidenceItem("collector", "checkpoint_timeout", "checkpoint_timeout", RawValue: "5min", NormalizedValue: "5min"),
                new DbConfigEvidenceItem("collector", "checkpoint_completion_target", "checkpoint_completion_target", RawValue: "0.5", NormalizedValue: "0.5"),
                new DbConfigEvidenceItem("collector", "random_page_cost", "random_page_cost", RawValue: "4.0", NormalizedValue: "4.0"),
                new DbConfigEvidenceItem("collector", "seq_page_cost", "seq_page_cost", RawValue: "1.0", NormalizedValue: "1.0")
            ],
            runtimeMetricItems:
            [
                new DbConfigEvidenceItem("collector", "checkpoints_timed", "checkpoints_timed", Category: "runtimeMetric", RawValue: "10", NormalizedValue: "10"),
                new DbConfigEvidenceItem("collector", "checkpoints_req", "checkpoints_req", Category: "runtimeMetric", RawValue: "25", NormalizedValue: "25"),
                new DbConfigEvidenceItem("collector", "temp_files", "temp_files", Category: "runtimeMetric", RawValue: "42", NormalizedValue: "42"),
                new DbConfigEvidenceItem("collector", "blks_hit", "blks_hit", Category: "runtimeMetric", RawValue: "1000", NormalizedValue: "1000"),
                new DbConfigEvidenceItem("collector", "blks_read", "blks_read", Category: "runtimeMetric", RawValue: "500", NormalizedValue: "500")
            ],
            hostContextItems:
            [
                new DbConfigEvidenceItem("host", "memory_limit_bytes", "memory_limit_bytes", Category: "hostContext", RawValue: "17179869184", NormalizedValue: "17179869184", SourceScope: "container")
            ],
            observabilityItems:
            [
                new DbConfigEvidenceItem("collector", "pg_stat_statements_enabled", "pg_stat_statements_enabled", Category: "observability", RawValue: "true", NormalizedValue: "true"),
                new DbConfigEvidenceItem("collector", "track_io_timing", "track_io_timing", Category: "observability", RawValue: "true", NormalizedValue: "true")
            ]);

        var evidence = executor.Analyze(snapshot);

        Assert.Contains(evidence.Recommendations, item => item.RuleId == "postgres.checkpoint.frequency");
        Assert.Contains(evidence.Recommendations, item => item.RuleId == "postgres.temp-io.work-mem");
        Assert.Contains(evidence.Recommendations, item => item.RuleId == "postgres.planner-cost.review");
        Assert.Contains(evidence.Recommendations, item => item.RuleVersion == "2026-04-30");
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

using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using Microsoft.Extensions.DependencyInjection;
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
            {
              "key":"shared_buffers",
              "suggestion":"Adjust shared_buffers.",
              "severity":"medium",
              "findingType":"cache",
              "confidence":"medium",
              "requiresMoreContext":false,
              "impactSummary":"cache pressure",
              "evidenceReferences":["shared_buffers"],
              "recommendationClass":"tuning",
              "recommendationType":"actionableRecommendation",
              "appliesWhen":"cache pressure is visible",
              "ruleId":"postgres.shared-buffers.reassessment",
              "ruleVersion":"2026-04-30"
            }
          ],
          "evidenceItems":[
            {
              "sourceType":"config",
              "reference":"work_mem",
              "description":"Only work_mem was collected.",
              "category":"configuration",
              "rawValue":"4MB",
              "normalizedValue":"4MB",
              "unit":"bytes-or-engine-unit",
              "sourceScope":"db",
              "capturedAt":null,
              "expiresAt":null,
              "isCached":false,
              "collectionMethod":"query"
            }
          ],
          "missingContextItems":[],
          "collectionMetadata":[],
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
              "findingType":"observability-gap",
              "confidence":"high",
              "requiresMoreContext":false,
              "impactSummary":"missing observability",
              "evidenceReferences":["slow_query_log"],
              "recommendationClass":"observability",
              "recommendationType":"actionableRecommendation",
              "appliesWhen":"slow query visibility is missing",
              "ruleId":"generic.observability-gap",
              "ruleVersion":"2026-04-30"
            }
          ],
          "evidenceItems":[
            {
              "sourceType":"config",
              "reference":"slow_query_log",
              "description":"Slow query log state.",
              "category":"observability",
              "rawValue":"OFF",
              "normalizedValue":"OFF",
              "unit":null,
              "sourceScope":"db",
              "capturedAt":null,
              "expiresAt":null,
              "isCached":false,
              "collectionMethod":"query"
            }
          ],
          "missingContextItems":[],
          "collectionMetadata":[],
          "warnings":[]
        }
        """;

        executor.Validate(evidence, reportJson);
    }

    [Fact]
    public void GroundingExecutor_DoesNotTreatExternalKnowledgeAsObservedEvidence()
    {
        var executor = new DbConfigGroundingExecutor(new RecommendationSchemaValidator());
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [
                new DbConfigRecommendation(
                    "max_connections",
                    "re-evaluate max_connections.",
                    "medium",
                    evidenceReferences: ["max_connections"])
            ],
            [
                new DbConfigEvidenceItem("config", "threads_connected", "Only runtime concurrency was collected.")
            ],
            Array.Empty<string>(),
            externalKnowledgeItems:
            [
                new DbConfigEvidenceItem(
                    "fact",
                    "[fact:mysql:connections] max_connections > Connections | https://example.com/mysql",
                    "MySQL reference",
                    "externalKnowledge",
                    "max_connections is documented here",
                    "[fact:mysql:connections] max_connections > Connections | https://example.com/mysql",
                    null,
                    "rag")
            ]);
        var reportJson = """
        {
          "title":"db-config report",
          "summary":"re-evaluate max_connections",
          "recommendations":[
            {
              "key":"max_connections",
              "suggestion":"re-evaluate max_connections",
              "severity":"medium",
              "findingType":"connections",
              "confidence":"medium",
              "requiresMoreContext":false,
              "impactSummary":"connection pressure",
              "evidenceReferences":["[fact:mysql:connections] max_connections > Connections | https://example.com/mysql"],
              "recommendationClass":"tuning",
              "recommendationType":"actionableRecommendation",
              "appliesWhen":"connection pressure is visible",
              "ruleId":"mysql.connections.capacity",
              "ruleVersion":"2026-04-30"
            }
          ],
          "evidenceItems":[
            {
              "sourceType":"config",
              "reference":"threads_connected",
              "description":"Only runtime concurrency was collected.",
              "category":"runtimeMetric",
              "rawValue":"12",
              "normalizedValue":"12",
              "unit":null,
              "sourceScope":"db",
              "capturedAt":null,
              "expiresAt":null,
              "isCached":false,
              "collectionMethod":"query"
            }
          ],
          "missingContextItems":[],
          "collectionMetadata":[],
          "warnings":[]
        }
        """;

        var error = Assert.Throws<InvalidOperationException>(() => executor.Validate(evidence, reportJson));

        Assert.Contains("evidence", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void GroundingExecutor_RejectsUnknownExternalKnowledgeCitations()
    {
        var executor = new DbConfigGroundingExecutor(new RecommendationSchemaValidator());
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            [
                new DbConfigRecommendation(
                    "shared_buffers",
                    "re-evaluate shared_buffers.",
                    "medium",
                    evidenceReferences: ["shared_buffers"])
            ],
            [
                new DbConfigEvidenceItem("config", "shared_buffers", "shared_buffers value.")
            ],
            Array.Empty<string>(),
            externalKnowledgeItems:
            [
                new DbConfigEvidenceItem(
                    "fact",
                    "[fact:postgresql:memory] shared_buffers > Memory > shared_buffers | https://example.com/pg",
                    "PostgreSQL reference",
                    "externalKnowledge",
                    "shared_buffers controls shared memory",
                    "[fact:postgresql:memory] shared_buffers > Memory > shared_buffers | https://example.com/pg",
                    null,
                    "rag")
            ]);
        var reportJson = """
        {
          "title":"db-config report",
          "summary":"re-evaluate shared_buffers",
          "recommendations":[
            {
              "key":"shared_buffers",
              "suggestion":"re-evaluate shared_buffers",
              "severity":"medium",
              "findingType":"memory",
              "confidence":"medium",
              "requiresMoreContext":false,
              "impactSummary":"memory tuning",
              "evidenceReferences":["shared_buffers"],
              "recommendationClass":"tuning",
              "recommendationType":"actionableRecommendation",
              "appliesWhen":"memory pressure is visible",
              "ruleId":"postgres.shared-buffers.reassessment",
              "ruleVersion":"2026-04-30",
              "externalKnowledgeCitations":["[fact:postgresql:memory] missing | https://example.com/missing"]
            }
          ],
          "evidenceItems":[
            {
              "sourceType":"config",
              "reference":"shared_buffers",
              "description":"shared_buffers value",
              "category":"configuration",
              "rawValue":"256MB",
              "normalizedValue":"256MB",
              "unit":null,
              "sourceScope":"db",
              "capturedAt":null,
              "expiresAt":null,
              "isCached":false,
              "collectionMethod":"query"
            }
          ],
          "missingContextItems":[],
          "collectionMetadata":[],
          "warnings":[]
        }
        """;

        var error = Assert.Throws<InvalidOperationException>(() => executor.Validate(evidence, reportJson));

        Assert.Contains("externalKnowledgeCitations", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void RecommendationSchemaValidator_AllowsRicherRecommendationFields()
    {
        var validator = new RecommendationSchemaValidator();
        var json = """
        {
          "title":"db-config report",
          "summary":"rich result",
          "recommendations":[
            {
              "key":"max_connections",
              "suggestion":"review connection cap",
              "severity":"medium",
              "findingType":"concurrency",
              "confidence":"medium",
              "requiresMoreContext":false,
              "impactSummary":"higher memory reserve",
              "evidenceReferences":["max_connections","threads_connected"],
              "recommendationClass":"capacity-planning",
              "recommendationType":"actionableRecommendation",
              "appliesWhen":"current concurrency remains low",
              "ruleId":"mysql.connections.capacity",
              "ruleVersion":"2026-04-30"
            }
          ],
          "evidenceItems":[
            {
              "sourceType":"collector",
              "reference":"max_connections",
              "description":"max_connections",
              "category":"configuration",
              "rawValue":"500",
              "normalizedValue":"500",
              "unit":null,
              "sourceScope":"db",
              "capturedAt":null,
              "expiresAt":null,
              "isCached":false,
              "collectionMethod":"query"
            }
          ],
          "missingContextItems":[],
          "collectionMetadata":[],
          "warnings":[]
        }
        """;

        validator.Validate(json);
    }

    [Fact]
    public async Task DiagnosisAgentExecutor_RejectsSpecificTargetValue_WhenHostContextIsMissing()
    {
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [
                new DbConfigRecommendation(
                    "innodb_buffer_pool_size",
                    "review",
                    "medium",
                    recommendationClass: "tuning")
            ],
            [
                new DbConfigEvidenceItem("collector", "innodb_buffer_pool_size", "buffer pool", RawValue: "256MB", NormalizedValue: "256MB")
            ],
            Array.Empty<string>(),
            configurationItems:
            [
                new DbConfigEvidenceItem("collector", "innodb_buffer_pool_size", "buffer pool", RawValue: "256MB", NormalizedValue: "256MB")
            ],
            missingContextItems:
            [
                new DbConfigMissingContextItem("memory_limit_bytes", "missing", "unsupported")
            ]);

        var resultType = typeof(DbConfigDiagnosisAgentExecutor);
        var method = resultType.GetMethod("EnforceNoSpecificTargetValuesWithoutHostContext", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
            ?? throw new InvalidOperationException("Guard method was not found.");
        var responseType = resultType.Assembly.GetType("AIDbOptimize.Infrastructure.Workflows.Pipeline.DbConfigDiagnosisAgentResponse")
            ?? throw new InvalidOperationException("Response type was not found.");
        var response = Activator.CreateInstance(
            responseType,
            "db-config report",
            "with specific target",
            new[]
            {
                new DbConfigRecommendation(
                    "innodb_buffer_pool_size",
                    "set to 4GB",
                    "medium",
                    findingType: "cache",
                    confidence: "medium",
                    requiresMoreContext: false,
                    impactSummary: "memory tuning",
                    evidenceReferences: ["innodb_buffer_pool_size"],
                    recommendationClass: "tuning",
                    recommendationType: DbConfigRecommendationType.ActionableRecommendation,
                    appliesWhen: "tune to 4GB immediately",
                    ruleId: "mysql.buffer-pool.reassessment",
                    ruleVersion: "2026-04-30")
            },
            new[]
            {
                new DbConfigEvidenceItem(
                    "collector",
                    "innodb_buffer_pool_size",
                    "buffer pool",
                    Category: "configuration",
                    RawValue: "256MB",
                    NormalizedValue: "256MB",
                    Unit: "bytes-or-engine-unit",
                    SourceScope: "db",
                    CapturedAt: null,
                    ExpiresAt: null,
                    IsCached: false,
                    CollectionMethod: "query")
            },
            new[]
            {
                new DbConfigMissingContextItem("memory_limit_bytes", "missing", "unsupported")
            },
            Array.Empty<DbConfigCollectionMetadataItem>(),
            Array.Empty<string>())
            ?? throw new InvalidOperationException("Response could not be constructed.");

        var error = await Assert.ThrowsAsync<System.Reflection.TargetInvocationException>(async () =>
        {
            method.Invoke(null, [evidence, response]);
            await Task.CompletedTask;
        });

        Assert.Contains("具体目标参数值", error.InnerException?.Message ?? error.Message, StringComparison.Ordinal);
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

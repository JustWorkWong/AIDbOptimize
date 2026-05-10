using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Threading;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace AIDbOptimize.ApiService.Tests;

public sealed class WorkflowApiTests : IClassFixture<WorkflowApiTests.WorkflowApiFactory>
{
    private readonly WorkflowApiFactory _factory;

    public WorkflowApiTests(WorkflowApiFactory factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetWorkflows_ReturnsSuccessStatusCode()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/workflows");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task GetRagAssetStatus_ReturnsCounts()
    {
        var connectionId = await _factory.SeedConnectionAsync("rag-status-main", DatabaseEngine.PostgreSql);
        await _factory.SeedRagFactAsync(
            engine: "postgresql",
            vendor: "postgresql",
            topic: "memory",
            sourcePath: "facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md",
            title: "shared_buffers",
            sectionPath: "Memory > shared_buffers",
            text: "shared_buffers controls PostgreSQL shared memory.",
            parameterNamesJson: """["shared_buffers"]""",
            keywordsJson: """["postgresql","memory","shared_buffers"]""");
        await _factory.SeedRagCaseAsync(
            engine: "postgresql",
            problemType: "memory",
            summary: "shared_buffers was previously tuned successfully.",
            evidenceReferences: ["shared_buffers"]);

        using var client = _factory.CreateClient();
        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "rag-asset-status"
        });
        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var response = await client.GetAsync("/api/rag/assets/status");

        response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(json.RootElement.GetProperty("factDocumentCount").GetInt32() >= 1);
        Assert.True(json.RootElement.GetProperty("caseRecordCount").GetInt32() >= 1);
        Assert.True(json.RootElement.GetProperty("chunkCount").GetInt32() >= 1);
        Assert.True(json.RootElement.GetProperty("retrievalSnapshotCount").GetInt32() >= 1);
    }

    [Fact]
    public async Task GetRagCaseAudit_ReturnsProjectedCases()
    {
        await _factory.SeedRagCaseAsync(
            engine: "mysql",
            problemType: "memory",
            summary: "innodb_buffer_pool_size was previously tuned successfully.",
            evidenceReferences: ["innodb_buffer_pool_size"]);
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/rag/cases/audit");

        response.EnsureSuccessStatusCode();
        using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
        Assert.True(json.RootElement.GetArrayLength() >= 1);
        Assert.True(json.RootElement[0].TryGetProperty("problemType", out _));
        Assert.True(json.RootElement[0].TryGetProperty("evidenceLinkCount", out _));
    }

    [Fact]
    public async Task ValidateRagCorpus_ReturnsCoverageAndIssues()
    {
        var corpusRootPath = CreateTempRagCorpus(
            "facts/mysql/mysql__mysql__memory__server-system-variables.md",
            """
            ---
            source_title: Sample
            source_url: https://example.com/mysql
            ---
            # Buffer Pool

            Use a larger buffer pool.
            """);

        try
        {
            using var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/rag/validate", new
            {
                corpusRootPath
            });

            response.EnsureSuccessStatusCode();
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.Equal(1, json.RootElement.GetProperty("validDocumentCount").GetInt32());
            Assert.Equal(0, json.RootElement.GetProperty("invalidDocumentCount").GetInt32());
            Assert.True(json.RootElement.GetProperty("coverage").GetArrayLength() >= 1);
        }
        finally
        {
            DeleteTempDirectory(corpusRootPath);
        }
    }

    [Fact]
    public async Task RebuildRagCorpus_ReingestsDocumentsAndProjectsCases()
    {
        var corpusRootPath = CreateTempRagCorpus(
            "facts/mysql/mysql__mysql__memory__server-system-variables.md",
            """
            ---
            source_title: Sample
            source_url: https://example.com/mysql
            ---
            # innodb_buffer_pool_size

            innodb_buffer_pool_size should align with available memory.
            """);

        try
        {
            await _factory.SeedSucceededWorkflowAsync(
                bundleId: "mysql-default",
                summary: "innodb_buffer_pool_size was previously tuned successfully.",
                findingType: "memory",
                recommendationType: "actionableRecommendation",
                evidenceReferences: ["innodb_buffer_pool_size"]);
            using var client = _factory.CreateClient();
            var response = await client.PostAsJsonAsync("/api/rag/rebuild", new
            {
                corpusRootPath
            });

            response.EnsureSuccessStatusCode();
            using var json = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            Assert.True(json.RootElement.GetProperty("documentCount").GetInt32() >= 1);
            Assert.True(json.RootElement.GetProperty("chunkCount").GetInt32() >= 1);
            Assert.True(json.RootElement.GetProperty("preparedFileCount").GetInt32() >= 2);
            Assert.True(json.RootElement.GetProperty("projectedCaseCount").GetInt32() >= 1);
            Assert.True(Directory.GetFiles(Path.Combine(corpusRootPath, "prepared", "facts", "mysql"), "*__metadata.json", SearchOption.TopDirectoryOnly).Length >= 1);
            Assert.True(Directory.GetFiles(Path.Combine(corpusRootPath, "cases", "mysql"), "*.md", SearchOption.TopDirectoryOnly).Length >= 1);
            Assert.True(Directory.GetFiles(Path.Combine(corpusRootPath, "prepared", "cases", "mysql"), "*__metadata.json", SearchOption.TopDirectoryOnly).Length >= 1);
        }
        finally
        {
            DeleteTempDirectory(corpusRootPath);
        }
    }

    [Fact]
    public async Task GetHistory_ReturnsSuccessStatusCode()
    {
        using var client = _factory.CreateClient();

        var response = await client.GetAsync("/api/history");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task StartWorkflow_CreatesHistoryEntry()
    {
        var connectionId = await _factory.SeedConnectionAsync("postgres-main", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "history-entry"
        });

        Assert.Equal(HttpStatusCode.Accepted, startResponse.StatusCode);
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        var historyResponse = await client.GetAsync("/api/history");
        historyResponse.EnsureSuccessStatusCode();
        using var historyJson = JsonDocument.Parse(await historyResponse.Content.ReadAsStringAsync());
        Assert.True(historyJson.RootElement.GetArrayLength() > 0);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);
    }

    [Fact]
    public async Task StartWorkflow_WithExplicitBundleVersion_CompletesSuccessfully()
    {
        var connectionId = await _factory.SeedConnectionAsync("postgres-bundle", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            bundleId = "postgresql-default",
            bundleVersion = "1.0.0",
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "explicit-bundle"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        var metadata = detailJson.RootElement
            .GetProperty("result")
            .GetProperty("parsedReport")
            .GetProperty("collectionMetadata")
            .EnumerateArray()
            .ToDictionary(
                item => item.GetProperty("name").GetString() ?? string.Empty,
                item => item.GetProperty("value").GetString() ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);
        Assert.Equal("postgresql-default", metadata["bundle_id"]);
        Assert.Equal("1.0.0", metadata["bundle_version"]);
        Assert.Equal("1.0.0", metadata["investigation_skill_version"]);
        Assert.Equal("1.0.0", metadata["diagnosis_skill_version"]);
        Assert.Equal("degraded", metadata["gate_status"]);
    }

    [Fact]
    public async Task StartWorkflow_EmitsRagAssemblerNodeAndMetadata()
    {
        var connectionId = await _factory.SeedConnectionAsync("postgres-rag", DatabaseEngine.PostgreSql);
        await _factory.SeedRagFactAsync(
            engine: "postgresql",
            vendor: "postgresql",
            topic: "memory",
            sourcePath: "facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md",
            title: "shared_buffers",
            sectionPath: "Memory > shared_buffers",
            text: "shared_buffers controls PostgreSQL shared memory and should align with available host memory.",
            parameterNamesJson: """["shared_buffers"]""",
            keywordsJson: """["postgresql","memory","shared_buffers"]""");
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "rag-node"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        var metadata = detailJson.RootElement
            .GetProperty("result")
            .GetProperty("parsedReport")
            .GetProperty("collectionMetadata")
            .EnumerateArray()
            .ToDictionary(
                item => item.GetProperty("name").GetString() ?? string.Empty,
                item => item.GetProperty("value").GetString() ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);
        Assert.Equal("facts-hit", metadata["rag_status"]);
        Assert.Equal("1", metadata["external_knowledge_count"]);
        Assert.Equal("degraded", metadata["gate_status"]);
        Assert.Equal(
            1,
            detailJson.RootElement
                .GetProperty("result")
                .GetProperty("parsedReport")
                .GetProperty("externalKnowledgeItems")
                .GetArrayLength());
        Assert.True(
            detailJson.RootElement
                .GetProperty("result")
                .GetProperty("parsedReport")
                .GetProperty("recommendations")[0]
                .GetProperty("externalKnowledgeCitations")
                .GetArrayLength() >= 1);

        var historyResponse = await client.GetAsync($"/api/history/{workflowId}");
        historyResponse.EnsureSuccessStatusCode();
        using var historyJson = JsonDocument.Parse(await historyResponse.Content.ReadAsStringAsync());
        Assert.Contains(
            historyJson.RootElement.GetProperty("nodeExecutions").EnumerateArray(),
            item => string.Equals(item.GetProperty("nodeName").GetString(), "WorkflowRagContextAssembler", StringComparison.Ordinal));

        await using var dbContext = _factory.CreateControlPlaneDbContext();
        var workflowSessionId = Guid.Parse(workflowId!);
        var snapshot = dbContext.RagRetrievalSnapshots.Single(item => item.WorkflowSessionId == workflowSessionId);
        Assert.NotEqual(Guid.Empty, snapshot.NodeExecutionId);
        Assert.Contains("facts-hit", snapshot.SnapshotTypeJson, StringComparison.Ordinal);
        var session = dbContext.WorkflowSessions.Single(item => item.Id == workflowSessionId);
        Assert.Contains(snapshot.Id.ToString(), session.StateJson, StringComparison.OrdinalIgnoreCase);
        var latestCheckpoint = dbContext.WorkflowCheckpoints
            .Where(item => item.WorkflowSessionId == workflowSessionId)
            .OrderByDescending(item => item.Sequence)
            .First();
        Assert.Contains(snapshot.Id.ToString(), latestCheckpoint.SnapshotJson, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("shared_buffers", detailJson.RootElement.GetProperty("result").GetProperty("payloadJson").GetString(), StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task StartWorkflow_InjectsCaseKnowledgeIntoResult()
    {
        var connectionId = await _factory.SeedConnectionAsync("mysql-rag-case", DatabaseEngine.MySql);
        await _factory.SeedRagCaseAsync(
            engine: "mysql",
            problemType: "memory",
            summary: "innodb_buffer_pool_size was previously tuned successfully.",
            evidenceReferences: ["innodb_buffer_pool_size"]);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "rag-case-node"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        var metadata = detailJson.RootElement
            .GetProperty("result")
            .GetProperty("parsedReport")
            .GetProperty("collectionMetadata")
            .EnumerateArray()
            .ToDictionary(
                item => item.GetProperty("name").GetString() ?? string.Empty,
                item => item.GetProperty("value").GetString() ?? string.Empty,
                StringComparer.OrdinalIgnoreCase);
        Assert.Equal("case-hit", metadata["rag_status"]);
        Assert.Equal("1", metadata["external_knowledge_count"]);
        Assert.Equal(
            "case",
            detailJson.RootElement
                .GetProperty("result")
                .GetProperty("parsedReport")
                .GetProperty("externalKnowledgeItems")[0]
                .GetProperty("sourceType")
                .GetString());
        Assert.Contains(
            "/api/history/",
            detailJson.RootElement
                .GetProperty("result")
                .GetProperty("parsedReport")
                .GetProperty("externalKnowledgeItems")[0]
                .GetProperty("normalizedValue")
                .GetString(),
            StringComparison.Ordinal);
    }

    [Fact]
    public async Task StartWorkflow_WhenBlockingEvidenceMissing_ProducesBlockedReport()
    {
        var connectionId = await _factory.SeedConnectionAsync("mysql-blocked", DatabaseEngine.MySql);
        _factory.OmitToolFields(DatabaseEngine.MySql, "engine_version", "version_comment");
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "blocked-skill-policy"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        var report = detailJson.RootElement.GetProperty("result").GetProperty("parsedReport");
        Assert.Equal(0, report.GetProperty("recommendations").GetArrayLength());
        Assert.Contains(
            report.GetProperty("missingContextItems").EnumerateArray(),
            item => string.Equals(item.GetProperty("reference").GetString(), "mysql.version_profile", StringComparison.OrdinalIgnoreCase));
        Assert.Contains("Workflow stopped before diagnosis", report.GetProperty("summary").GetString(), StringComparison.OrdinalIgnoreCase);
        Assert.Contains(
            report.GetProperty("collectionMetadata").EnumerateArray(),
            item =>
                string.Equals(item.GetProperty("name").GetString(), "gate_status", StringComparison.OrdinalIgnoreCase) &&
                string.Equals(item.GetProperty("value").GetString(), "blocked", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public async Task StartWorkflow_WhenDiagnosisThrows_PersistsNodeFailureAndOriginalError()
    {
        var connectionId = await _factory.SeedConnectionAsync("mysql-diagnosis-failure", DatabaseEngine.MySql);
        _factory.FailDiagnosis("diagnosis-agent-boom");
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                allowFallbackSnapshot = true,
                requireHumanReview = false,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "diagnosis-failure"
        });

        Assert.Equal(HttpStatusCode.Accepted, startResponse.StatusCode);
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Failed"], timeoutSeconds: 60);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        Assert.Equal("Failed", detailJson.RootElement.GetProperty("status").GetString());
        Assert.Equal("DbConfigDiagnosisAgentExecutor", detailJson.RootElement.GetProperty("currentNode").GetString());
        var error = detailJson.RootElement.GetProperty("error").GetString();
        Assert.Contains("diagnosis-agent-boom", error, StringComparison.Ordinal);
        Assert.DoesNotContain("without a completion output", error, StringComparison.OrdinalIgnoreCase);

        var historyResponse = await client.GetAsync($"/api/history/{workflowId}");
        historyResponse.EnsureSuccessStatusCode();
        using var historyJson = JsonDocument.Parse(await historyResponse.Content.ReadAsStringAsync());
        var diagnosisNode = historyJson.RootElement
            .GetProperty("nodeExecutions")
            .EnumerateArray()
            .Single(item => string.Equals(item.GetProperty("nodeName").GetString(), "DbConfigDiagnosisAgentExecutor", StringComparison.Ordinal));
        Assert.Equal("Failed", diagnosisNode.GetProperty("status").GetString());
        Assert.Contains("diagnosis-agent-boom", diagnosisNode.GetProperty("error").GetString(), StringComparison.Ordinal);
    }

    [Fact]
    public async Task SubmitReview_UpdatesWorkflowStatus()
    {
        var connectionId = await _factory.SeedConnectionAsync("mysql-main", DatabaseEngine.MySql);
        await _factory.SeedRagFactAsync(
            engine: "mysql",
            vendor: "mysql",
            topic: "memory",
            sourcePath: "facts/mysql/mysql__mysql__memory__server-system-variables.md",
            title: "innodb_buffer_pool_size",
            sectionPath: "Memory > innodb_buffer_pool_size",
            text: "innodb_buffer_pool_size should align with available host memory and workload shape.",
            parameterNamesJson: """["innodb_buffer_pool_size"]""",
            keywordsJson: """["mysql","memory","innodb_buffer_pool_size"]""");
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "review-flow"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

        var taskId = await WaitForReviewTaskAsync(client, workflowId!, 60);

        Assert.False(string.IsNullOrWhiteSpace(taskId));

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "approve",
            reviewer = "tester",
            comment = "approved"
        });

        submitResponse.EnsureSuccessStatusCode();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        await using var dbContext = _factory.CreateControlPlaneDbContext();
        var workflowSessionId = Guid.Parse(workflowId!);
        Assert.Equal(1, dbContext.RagRetrievalSnapshots.Count(item => item.WorkflowSessionId == workflowSessionId));
    }

    [Fact]
    public async Task GetWorkflowEvents_ReturnsEventStreamPayload()
    {
        var connectionId = await _factory.SeedConnectionAsync("events-main", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = false,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "events-test"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed", "Failed"], timeoutSeconds: 30);

        var response = await client.GetAsync($"/api/workflows/{workflowId}/events");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.Equal("text/event-stream", response.Content.Headers.ContentType?.MediaType);

        var body = await response.Content.ReadAsStringAsync();
        Assert.Contains("event: snapshot", body);
        Assert.Contains("\"workflowType\":\"DbConfigOptimization\"", body);
    }

    [Fact]
    public async Task ReviewSubmission_CreatesAdditionalCheckpoint()
    {
        var connectionId = await _factory.SeedConnectionAsync("checkpoint-main", DatabaseEngine.MySql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "checkpoint-test"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.True(Guid.TryParse(workflowId, out var sessionId));

        var taskId = await WaitForReviewTaskAsync(client, workflowId!, 60);

        await using (var dbContext = _factory.CreateControlPlaneDbContext())
        {
            var checkpointCount = dbContext.WorkflowCheckpoints.Count(x => x.WorkflowSessionId == sessionId);
            Assert.True(checkpointCount >= 1);
        }

        int checkpointCountBeforeSubmit;
        await using (var dbContextBefore = _factory.CreateControlPlaneDbContext())
        {
            checkpointCountBeforeSubmit = dbContextBefore.WorkflowCheckpoints.Count(x => x.WorkflowSessionId == sessionId);
        }

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "approve",
            reviewer = "checkpoint-tester",
            comment = "approved"
        });

        submitResponse.EnsureSuccessStatusCode();

        await using var dbContextAfter = _factory.CreateControlPlaneDbContext();
        var checkpointCountAfter = dbContextAfter.WorkflowCheckpoints.Count(x => x.WorkflowSessionId == sessionId);
        Assert.True(checkpointCountAfter > checkpointCountBeforeSubmit);
    }

    [Fact]
    public async Task StartWorkflow_PersistsAgentSessionMessagesAndSummary()
    {
        var connectionId = await _factory.SeedConnectionAsync("agent-main", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = false,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "agent-persistence"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.True(Guid.TryParse(workflowId, out var sessionId));

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        await using var dbContext = _factory.CreateControlPlaneDbContext();
        var session = dbContext.WorkflowSessions.Single(x => x.Id == sessionId);
        Assert.NotNull(session.AgentSessionId);

        var agentSession = dbContext.AgentSessions.Single(x => x.Id == session.AgentSessionId);
        Assert.Equal("DbConfigDiagnosisAgent", agentSession.AgentRole);
        Assert.NotNull(agentSession.ActiveSummaryId);

        var messageCount = dbContext.AgentMessages.Count(x => x.WorkflowSessionId == sessionId);
        Assert.Equal(2, messageCount);

        var summaryCount = dbContext.AgentSummaries.Count(x => x.AgentSessionId == agentSession.Id);
        Assert.True(summaryCount >= 1);
    }

    [Fact]
    public async Task StartWorkflow_WithRequireHumanReviewFalse_CompletesDirectly()
    {
        var connectionId = await _factory.SeedConnectionAsync("noreview-main", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = false,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "test-runner",
            notes = "no-review"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();

        Assert.False(string.IsNullOrWhiteSpace(workflowId));
        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var reviewsResponse = await client.GetAsync("/api/reviews");
        using var reviewsJson = JsonDocument.Parse(await reviewsResponse.Content.ReadAsStringAsync());
        var hasRelatedReview = reviewsJson.RootElement
            .EnumerateArray()
            .Any(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase));
        Assert.False(hasRelatedReview);
    }

    [Fact]
    public async Task SubmitReview_Adjust_ProducesAdjustedResultAndCompletes()
    {
        var connectionId = await _factory.SeedConnectionAsync("adjust-main", DatabaseEngine.MySql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "adjust"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();

        var taskId = await WaitForReviewTaskAsync(client, workflowId!, 60);

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "adjust",
            reviewer = "adjust-tester",
            comment = "lower max_connections risk",
            adjustments = new
            {
                riskLevelOverrides = new
                {
                    max_connections = "warning"
                }
            }
        });

        submitResponse.EnsureSuccessStatusCode();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var workflowResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        using var workflowJson = JsonDocument.Parse(await workflowResponse.Content.ReadAsStringAsync());
        Assert.Equal("Completed", workflowJson.RootElement.GetProperty("status").GetString());
        Assert.Contains("warning", workflowJson.RootElement.GetProperty("result").GetProperty("payloadJson").GetString());

        var reviewResponse = await client.GetAsync($"/api/reviews/{taskId}");
        using var reviewJson = JsonDocument.Parse(await reviewResponse.Content.ReadAsStringAsync());
        Assert.Equal("Adjusted", reviewJson.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task HistoryDetail_ContainsNodeAndToolExecutions()
    {
        var connectionId = await _factory.SeedConnectionAsync("history-main", DatabaseEngine.PostgreSql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = false,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "history-detail"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Completed"], timeoutSeconds: 60);

        var historyDetailResponse = await client.GetAsync($"/api/history/{workflowId}");
        historyDetailResponse.EnsureSuccessStatusCode();
        using var historyDetailJson = JsonDocument.Parse(await historyDetailResponse.Content.ReadAsStringAsync());
        Assert.True(historyDetailJson.RootElement.GetProperty("nodeExecutions").GetArrayLength() >= 1);
        Assert.True(historyDetailJson.RootElement.GetProperty("toolExecutions").GetArrayLength() >= 1);
        Assert.True(historyDetailJson.RootElement.GetProperty("ragSnapshots").GetArrayLength() >= 1);
        Assert.Contains(
            historyDetailJson.RootElement.GetProperty("nodeExecutions").EnumerateArray(),
            item => string.Equals(
                item.GetProperty("nodeName").GetString(),
                "InvestigationPlanner",
                StringComparison.Ordinal));
        Assert.Contains(
            historyDetailJson.RootElement.GetProperty("ragSnapshots").EnumerateArray(),
            item =>
            {
                var snapshotTypeJson = item.GetProperty("snapshotTypeJson").GetString()!;
                return snapshotTypeJson.Contains("facts-hit", StringComparison.Ordinal)
                    || snapshotTypeJson.Contains("facts-and-cases-hit", StringComparison.Ordinal)
                    || snapshotTypeJson.Contains("vector-and-cases-hit", StringComparison.Ordinal)
                    || snapshotTypeJson.Contains("empty-corpus", StringComparison.Ordinal)
                    || snapshotTypeJson.Contains("no-match", StringComparison.Ordinal);
            });
        Assert.True(historyDetailJson.RootElement.GetProperty("result").GetProperty("parsedReport").ValueKind == JsonValueKind.Object);
        Assert.True(historyDetailJson.RootElement
            .GetProperty("result")
            .GetProperty("parsedReport")
            .GetProperty("recommendations")[0]
            .TryGetProperty("ruleId", out _));
        Assert.True(historyDetailJson.RootElement
            .GetProperty("result")
            .GetProperty("parsedReport")
            .GetProperty("recommendations")[0]
            .TryGetProperty("ruleVersion", out _));
    }

    [Fact]
    public async Task Recovery_OnRunningSession_RehydratesPendingReview()
    {
        var connectionId = await _factory.SeedConnectionAsync("recovery-main", DatabaseEngine.MySql);
        await _factory.SeedRagFactAsync(
            engine: "mysql",
            vendor: "mysql",
            topic: "memory",
            sourcePath: "facts/mysql/mysql__mysql__memory__server-system-variables.md",
            title: "innodb_buffer_pool_size",
            sectionPath: "Memory > innodb_buffer_pool_size",
            text: "innodb_buffer_pool_size should align with available host memory and workload shape.",
            parameterNamesJson: """["innodb_buffer_pool_size"]""",
            keywordsJson: """["mysql","memory","innodb_buffer_pool_size"]""");
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "recovery"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.True(Guid.TryParse(workflowId, out var sessionId));

        await WaitForReviewTaskAsync(client, workflowId!, 60);
        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["WaitingForReview"], timeoutSeconds: 60);

        await using (var dbContext = _factory.CreateControlPlaneDbContext())
        {
            var session = await dbContext.WorkflowSessions.SingleAsync(x => x.Id == sessionId);
            session.Status = Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Running;
            session.ActiveReviewTaskId = null;
            await dbContext.SaveChangesAsync();
        }

        var runtime = _factory.Services.GetRequiredService<Infrastructure.Workflows.Runtime.IWorkflowRuntime>();
        var result = await runtime.ResumeAsync(
            sessionId,
            new Infrastructure.Workflows.Runtime.WorkflowResumeRequest("recovery"),
            CancellationToken.None);

        Assert.Equal("WaitingForReview", result.Status);

        await using var dbContextAfter = _factory.CreateControlPlaneDbContext();
        var sessionAfter = await dbContextAfter.WorkflowSessions.SingleAsync(x => x.Id == sessionId);
        Assert.Equal(Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.WaitingForReview, sessionAfter.Status);
        Assert.NotNull(sessionAfter.ActiveReviewTaskId);
        Assert.Equal(1, dbContextAfter.RagRetrievalSnapshots.Count(x => x.WorkflowSessionId == sessionId));
    }

    [Fact]
    public async Task CancelWorkflow_WhileRunning_RemainsCancelled()
    {
        var connectionId = await _factory.SeedConnectionAsync("cancel-running", DatabaseEngine.PostgreSql);
        _factory.SetToolExecutionDelay(TimeSpan.FromSeconds(2));
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = false,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "cancel-running"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.True(Guid.TryParse(workflowId, out var sessionId));

        var cancelResponse = await client.PostAsync($"/api/workflows/{workflowId}/cancel", content: null);
        cancelResponse.EnsureSuccessStatusCode();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Cancelled"], timeoutSeconds: 30);
        await Task.Delay(2500);

        var detailResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        detailResponse.EnsureSuccessStatusCode();
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        Assert.Equal("Cancelled", detailJson.RootElement.GetProperty("status").GetString());

        await using var dbContext = _factory.CreateControlPlaneDbContext();
        var session = await dbContext.WorkflowSessions.SingleAsync(x => x.Id == sessionId);
        Assert.Equal(Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Cancelled, session.Status);
    }

    [Fact]
    public async Task CancelWorkflow_WhileWaitingForReview_ClosesTask_AndRejectsLaterSubmission()
    {
        var connectionId = await _factory.SeedConnectionAsync("cancel-review", DatabaseEngine.MySql);
        using var client = _factory.CreateClient();

        var startResponse = await client.PostAsJsonAsync("/api/workflows/db-config-optimization", new
        {
            connectionId = connectionId.ToString(),
            options = new
            {
                requireHumanReview = true,
                allowFallbackSnapshot = true,
                enableEvidenceGrounding = true
            },
            requestedBy = "frontend",
            notes = "cancel-review"
        });

        startResponse.EnsureSuccessStatusCode();
        using var startJson = JsonDocument.Parse(await startResponse.Content.ReadAsStringAsync());
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();
        Assert.True(Guid.TryParse(workflowId, out var sessionId));

        var taskId = await WaitForReviewTaskAsync(client, workflowId!, 60);
        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["WaitingForReview"], timeoutSeconds: 60);

        var cancelResponse = await client.PostAsync($"/api/workflows/{workflowId}/cancel", content: null);
        cancelResponse.EnsureSuccessStatusCode();

        await WaitForWorkflowStatusAsync(client, workflowId!, expectedStatuses: ["Cancelled"], timeoutSeconds: 30);

        var reviewResponse = await client.GetAsync($"/api/reviews/{taskId}");
        reviewResponse.EnsureSuccessStatusCode();
        using var reviewJson = JsonDocument.Parse(await reviewResponse.Content.ReadAsStringAsync());
        Assert.Equal("Cancelled", reviewJson.RootElement.GetProperty("status").GetString());

        await using (var dbContext = _factory.CreateControlPlaneDbContext())
        {
            var session = await dbContext.WorkflowSessions.SingleAsync(x => x.Id == sessionId);
            Assert.Null(session.ActiveReviewTaskId);
        }

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "approve",
            reviewer = "late-reviewer",
            comment = "too late"
        });

        Assert.Equal(HttpStatusCode.Conflict, submitResponse.StatusCode);
    }

    [Fact]
    public async Task Recovery_OnRunningSession_WithoutCheckpointRef_RestartsQueuedStart()
    {
        var connectionId = await _factory.SeedConnectionAsync("recovery-no-checkpoint", DatabaseEngine.MySql);
        var runtime = _factory.Services.GetRequiredService<Infrastructure.Workflows.Runtime.IWorkflowRuntime>();
        var queued = await runtime.QueueStartDbConfigAsync(
            new Infrastructure.Workflows.Runtime.DbConfigWorkflowCommand(
                connectionId,
                "mysql-default",
                "1.0.0",
                "mysql-investigation",
                "1.0.0",
                "mysql-diagnosis",
                "1.0.0",
                "tester",
                "queued-recovery",
                true,
                true,
                true),
            CancellationToken.None);

        Assert.True(Guid.TryParse(queued.SessionId, out var sessionId));

        var result = await runtime.ResumeAsync(
            sessionId,
            new Infrastructure.Workflows.Runtime.WorkflowResumeRequest("recovery"),
            CancellationToken.None);

        Assert.Equal("WaitingForReview", result.Status);

        await using var dbContext = _factory.CreateControlPlaneDbContext();
        var session = await dbContext.WorkflowSessions.SingleAsync(x => x.Id == sessionId);
        Assert.Equal(Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.WaitingForReview, session.Status);
        Assert.NotNull(session.ActiveReviewTaskId);
        Assert.False(string.IsNullOrWhiteSpace(session.EngineCheckpointRef));
        using var stateJson = JsonDocument.Parse(session.StateJson);
        Assert.Equal("mysql-default", stateJson.RootElement.GetProperty("bundleId").GetString());
        Assert.Equal("1.0.0", stateJson.RootElement.GetProperty("bundleVersion").GetString());
        Assert.Equal("mysql-investigation", stateJson.RootElement.GetProperty("investigationSkillId").GetString());
        Assert.Equal("mysql-diagnosis", stateJson.RootElement.GetProperty("diagnosisSkillId").GetString());

        var latestCheckpoint = await dbContext.WorkflowCheckpoints
            .Where(x => x.WorkflowSessionId == sessionId)
            .OrderByDescending(x => x.Sequence)
            .FirstAsync();
        using var checkpointJson = JsonDocument.Parse(latestCheckpoint.SnapshotJson);
        Assert.Equal("mysql-default", checkpointJson.RootElement.GetProperty("skillSelection").GetProperty("bundleId").GetString());
        Assert.Equal("1.0.0", checkpointJson.RootElement.GetProperty("skillSelection").GetProperty("bundleVersion").GetString());
    }

    public sealed class WorkflowApiFactory : WebApplicationFactory<Program>
    {
        private readonly TestControlPlaneDbContextFactory _dbContextFactory = new();
        private readonly WorkflowTestBehavior _behavior = new();

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Testing");

            builder.ConfigureServices(services =>
            {
                services.RemoveAll(typeof(IDbContextFactory<ControlPlaneDbContext>));
                services.RemoveAll<IHostedService>();
                services.AddSingleton<IDbContextFactory<ControlPlaneDbContext>>(_dbContextFactory);
                services.AddSingleton(new DbConfigDiagnosisAgentOptions());
                services.RemoveAll<IDbConfigDiagnosisAgentExecutor>();
                services.AddSingleton<IDbConfigDiagnosisAgentExecutor>(new FakeDiagnosisAgentExecutor(_behavior));
                services.RemoveAll<IMcpToolExecutionService>();
                services.AddSingleton<IMcpToolExecutionService>(new FakeWorkflowToolExecutionService(_dbContextFactory, _behavior));
            });
        }

        public async Task<Guid> SeedConnectionAsync(string name, DatabaseEngine engine)
        {
            _behavior.Reset();
            await using var dbContext = _dbContextFactory.CreateDbContext();
            var connection = new McpConnectionEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                Engine = engine,
                DisplayName = name,
                ServerCommand = "npx",
                ServerArgumentsJson = "[]",
                EnvironmentJson = "{}",
                DatabaseConnectionString = "Host=localhost",
                DatabaseName = engine == DatabaseEngine.MySql ? "orders" : "appdb",
                IsDefault = false,
                Status = McpConnectionStatus.Ready,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };

            dbContext.McpConnections.Add(connection);
            dbContext.McpTools.Add(new McpToolEntity
            {
                Id = Guid.NewGuid(),
                ConnectionId = connection.Id,
                ToolName = "query",
                DisplayName = "query",
                Description = "read-only query",
                InputSchemaJson = "{}",
                ApprovalMode = ToolApprovalMode.NoApproval,
                IsEnabled = true,
                IsWriteTool = false,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();

            return connection.Id;
        }

        public ControlPlaneDbContext CreateControlPlaneDbContext()
        {
            return _dbContextFactory.CreateDbContext();
        }

        public async Task SeedRagFactAsync(
            string engine,
            string vendor,
            string topic,
            string sourcePath,
            string title,
            string sectionPath,
            string text,
            string parameterNamesJson,
            string keywordsJson)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            var document = new RagDocumentEntity
            {
                Id = Guid.NewGuid(),
                DocumentType = AIDbOptimize.Infrastructure.Rag.Corpus.RagDocumentType.Fact,
                Engine = engine,
                Vendor = vendor,
                Topic = topic,
                SourcePath = sourcePath,
                SourceUrl = $"https://example.com/{title}",
                SourceTitle = title,
                ContentHash = Guid.NewGuid().ToString("N"),
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagDocuments.Add(document);
            dbContext.RagDocumentChunks.Add(new RagDocumentChunkEntity
            {
                Id = Guid.NewGuid(),
                Document = document,
                ChunkKey = $"{title}-chunk-1",
                Title = title,
                SectionPath = sectionPath,
                Text = text,
                ProductVersion = "current",
                AppliesTo = $"{vendor}:{engine}",
                ParameterNamesJson = parameterNamesJson,
                KeywordsJson = keywordsJson,
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        public async Task SeedRagCaseAsync(
            string engine,
            string problemType,
            string summary,
            IReadOnlyList<string> evidenceReferences)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            var caseRecord = new RagCaseRecordEntity
            {
                Id = Guid.NewGuid(),
                WorkflowSessionId = Guid.NewGuid(),
                Engine = engine,
                ProblemType = problemType,
                Outcome = "Completed",
                ReviewStatus = "Approved",
                RecommendationType = "actionableRecommendation",
                Summary = summary,
                CreatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagCaseRecords.Add(caseRecord);
            foreach (var evidenceReference in evidenceReferences)
            {
                dbContext.RagCaseEvidenceLinks.Add(new RagCaseEvidenceLinkEntity
                {
                    Id = Guid.NewGuid(),
                    CaseRecord = caseRecord,
                    EvidenceReference = evidenceReference,
                    RecommendationKey = problemType,
                    CreatedAt = DateTimeOffset.UtcNow
                });
            }

            await dbContext.SaveChangesAsync();
        }

        public async Task SeedSucceededWorkflowAsync(
            string bundleId,
            string summary,
            string findingType,
            string recommendationType,
            IReadOnlyList<string> evidenceReferences)
        {
            await using var dbContext = _dbContextFactory.CreateDbContext();
            dbContext.WorkflowSessions.Add(new WorkflowSessionEntity
            {
                Id = Guid.NewGuid(),
                ConnectionId = Guid.NewGuid(),
                WorkflowName = "DbConfigOptimization",
                EngineType = "maf",
                Status = WorkflowSessionStatus.Succeeded,
                RequestedBy = "tester",
                InputPayloadJson = JsonSerializer.Serialize(new { bundleId }),
                StateJson = "{}",
                ResultType = "db-config-optimization-report",
                ResultPayloadJson = JsonSerializer.Serialize(new
                {
                    title = "db-config report",
                    summary,
                    recommendations = new[]
                    {
                        new
                        {
                            key = evidenceReferences[0],
                            findingType,
                            recommendationType,
                            evidenceReferences
                        }
                    }
                }),
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        public void SetToolExecutionDelay(TimeSpan delay)
        {
            _behavior.ToolExecutionDelay = delay;
        }

        public void OmitToolFields(DatabaseEngine engine, params string[] fieldNames)
        {
            _behavior.OmitFields(engine, fieldNames);
        }

        public void FailDiagnosis(string errorMessage)
        {
            _behavior.DiagnosisExceptionMessage = errorMessage;
        }
    }

    private static string CreateTempRagCorpus(string relativePath, string content)
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"rag-corpus-{Guid.NewGuid():N}");
        var fullPath = Path.Combine(rootPath, relativePath.Replace('/', Path.DirectorySeparatorChar));
        Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
        File.WriteAllText(fullPath, content);
        return rootPath;
    }

    private static void DeleteTempDirectory(string rootPath)
    {
        if (Directory.Exists(rootPath))
        {
            Directory.Delete(rootPath, recursive: true);
        }
    }

    private sealed class FakeDiagnosisAgentExecutor(WorkflowTestBehavior behavior) : IDbConfigDiagnosisAgentExecutor
    {
        public string PromptVersion => "test-prompt-v1";

        public string ModelId => "fake-model";

        public string BuildPrompt(
            string displayName,
            string databaseName,
            string engine,
            string optimizationGoal,
            DbConfigEvidencePack evidence,
            DiagnosisSkillDefinition? diagnosisSkill = null)
        {
            return $"fake-prompt:{displayName}:{databaseName}:{engine}:{optimizationGoal}";
        }

        public Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(
            DbConfigEvidencePack evidence,
            string? optimizationGoal,
            DiagnosisSkillDefinition? diagnosisSkill = null,
            CancellationToken cancellationToken = default)
        {
            if (!string.IsNullOrWhiteSpace(behavior.DiagnosisExceptionMessage))
            {
                throw new InvalidOperationException(behavior.DiagnosisExceptionMessage);
            }

            var payload = JsonSerializer.Serialize(new
            {
                title = $"db-config report - {evidence.DatabaseName}",
                summary = $"fake diagnosis for {evidence.DatabaseName} {optimizationGoal}",
                recommendations = evidence.Recommendations.Select(item => new
                {
                    key = item.Key,
                    suggestion = item.Suggestion,
                    severity = item.Severity,
                    findingType = item.FindingType,
                    confidence = item.Confidence,
                    requiresMoreContext = item.RequiresMoreContext,
                    impactSummary = item.ImpactSummary,
                    evidenceReferences = item.EvidenceReferences,
                    recommendationClass = item.RecommendationClass,
                    recommendationType = item.RecommendationType == DbConfigRecommendationType.ActionableRecommendation
                        ? "actionableRecommendation"
                        : "requestMoreContext",
                    appliesWhen = item.AppliesWhen,
                    ruleId = item.RuleId,
                    ruleVersion = item.RuleVersion,
                    externalKnowledgeCitations = evidence.ExternalKnowledgeItems
                        .Where(knowledge =>
                            knowledge.Reference.Contains(item.Key, StringComparison.OrdinalIgnoreCase) ||
                            (knowledge.RawValue?.Contains(item.Key, StringComparison.OrdinalIgnoreCase) ?? false) ||
                            (knowledge.NormalizedValue?.Contains(item.Key, StringComparison.OrdinalIgnoreCase) ?? false))
                        .Select(knowledge => knowledge.NormalizedValue ?? knowledge.Reference)
                        .Distinct(StringComparer.OrdinalIgnoreCase)
                }),
                evidenceItems = evidence.EvidenceItems.Select(item => new
                {
                    sourceType = item.SourceType,
                    reference = item.Reference,
                    description = item.Description,
                    category = item.Category,
                    rawValue = item.RawValue,
                    normalizedValue = item.NormalizedValue,
                    unit = item.Unit,
                    sourceScope = item.SourceScope,
                    capturedAt = item.CapturedAt,
                    expiresAt = item.ExpiresAt,
                    isCached = item.IsCached,
                    collectionMethod = item.CollectionMethod
                }),
                missingContextItems = evidence.MissingContextItems.Select(item => new
                {
                    reference = item.Reference,
                    description = item.Description,
                    reason = item.Reason,
                    sourceScope = item.SourceScope,
                    severity = item.Severity
                }),
                collectionMetadata = evidence.CollectionMetadata.Select(item => new
                {
                    name = item.Name,
                    value = item.Value,
                    description = item.Description
                }),
                externalKnowledgeItems = evidence.ExternalKnowledgeItems.Select(item => new
                {
                    sourceType = item.SourceType,
                    reference = item.Reference,
                    description = item.Description,
                    category = item.Category,
                    rawValue = item.RawValue,
                    normalizedValue = item.NormalizedValue,
                    unit = item.Unit,
                    sourceScope = item.SourceScope,
                    capturedAt = item.CapturedAt,
                    expiresAt = item.ExpiresAt,
                    isCached = item.IsCached,
                    collectionMethod = item.CollectionMethod
                }),
                warnings = evidence.Warnings
            });

            return Task.FromResult(new DbConfigDiagnosisExecutionResult(
                payload,
                """{"promptTokens":10,"completionTokens":20,"totalTokens":30}"""));
        }
    }

    private sealed class FakeWorkflowToolExecutionService(TestControlPlaneDbContextFactory dbContextFactory, WorkflowTestBehavior behavior) : IMcpToolExecutionService
    {
        public async Task<McpToolExecutionResult> ExecuteAsync(McpToolExecutionRequest request, CancellationToken cancellationToken = default)
        {
            if (behavior.ToolExecutionDelay > TimeSpan.Zero)
            {
                await Task.Delay(behavior.ToolExecutionDelay, cancellationToken);
            }

            await using var dbContext = dbContextFactory.CreateDbContext();
            var tool = await dbContext.McpTools.AsNoTracking().SingleAsync(x => x.Id == request.ToolId, cancellationToken);
            var connectionId = tool.ConnectionId;
            var connection = await dbContext.McpConnections.AsNoTracking().SingleAsync(x => x.Id == connectionId, cancellationToken);
            var responseJson = connection.Engine == DatabaseEngine.MySql
                ? BuildMySqlResponseJson(behavior.GetOmittedFields(DatabaseEngine.MySql))
                : BuildPostgreSqlResponseJson(behavior.GetOmittedFields(DatabaseEngine.PostgreSql));
            var entity = new McpToolExecutionEntity
            {
                Id = Guid.NewGuid(),
                ConnectionId = connectionId,
                ToolId = request.ToolId,
                WorkflowSessionId = request.WorkflowSessionId,
                WorkflowNodeName = request.WorkflowNodeName,
                ExecutionScope = request.ExecutionScope,
                TraceId = request.TraceId,
                RequestedBy = request.RequestedBy,
                RequestPayloadJson = request.Payload.GetRawText(),
                ResponsePayloadJson = responseJson,
                Status = "Succeeded",
                CreatedAt = DateTimeOffset.UtcNow
            };
            dbContext.McpToolExecutions.Add(entity);
            await dbContext.SaveChangesAsync(cancellationToken);

            return new McpToolExecutionResult(entity.Id, entity.Status, entity.ResponsePayloadJson, null);
        }

        private static string BuildMySqlResponseJson(IReadOnlyCollection<string> omittedFields)
        {
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["max_connections"] = 300,
                ["innodb_buffer_pool_size"] = 536870912,
                ["thread_cache_size"] = 64,
                ["tmp_table_size"] = 16777216,
                ["max_heap_table_size"] = 16777216,
                ["slow_query_log"] = 0,
                ["long_query_time"] = 10,
                ["performance_schema_enabled"] = 1,
                ["engine_version"] = "8.0.36",
                ["version_comment"] = "MySQL Community Server",
                ["database_size_bytes"] = 104857600,
                ["parameter_apply_scope"] = "global",
                ["threads_connected"] = 8,
                ["threads_running"] = 2,
                ["slow_queries"] = 1,
                ["connections"] = 120,
                ["aborted_connects"] = 0,
                ["innodb_buffer_pool_reads"] = 500,
                ["innodb_buffer_pool_read_requests"] = 20000,
                ["created_tmp_disk_tables"] = 12,
                ["created_tmp_tables"] = 40
            };
            RemoveFields(row, omittedFields);

            return JsonSerializer.Serialize(new
            {
                content = new[]
                {
                    new
                    {
                        type = "text",
                        text = JsonSerializer.Serialize(new[] { row })
                    }
                }
            });
        }

        private static string BuildPostgreSqlResponseJson(IReadOnlyCollection<string> omittedFields)
        {
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase)
            {
                ["shared_buffers"] = "256MB",
                ["work_mem"] = "4MB",
                ["maintenance_work_mem"] = "64MB",
                ["effective_cache_size"] = "1024MB",
                ["max_connections"] = "200",
                ["checkpoint_timeout"] = "5min",
                ["checkpoint_completion_target"] = "0.5",
                ["random_page_cost"] = "4.0",
                ["seq_page_cost"] = "1.0",
                ["track_io_timing"] = "false",
                ["shared_preload_libraries"] = "",
                ["engine_version"] = "16.3",
                ["database_size_bytes"] = "104857600",
                ["parameter_apply_scope"] = "instance",
                ["blks_hit"] = "1000",
                ["blks_read"] = "500",
                ["temp_files"] = "42",
                ["deadlocks"] = "0",
                ["checkpoints_timed"] = "10",
                ["checkpoints_req"] = "25",
                ["pg_stat_statements_enabled"] = "false"
            };
            RemoveFields(row, omittedFields);

            return JsonSerializer.Serialize(new
            {
                structuredContent = new[] { row }
            });
        }

        private static void RemoveFields(IDictionary<string, object?> values, IReadOnlyCollection<string> omittedFields)
        {
            foreach (var field in omittedFields)
            {
                values.Remove(field);
            }
        }
    }

    private sealed class WorkflowTestBehavior
    {
        public TimeSpan ToolExecutionDelay { get; set; } = TimeSpan.Zero;

        public string? DiagnosisExceptionMessage { get; set; }

        private readonly HashSet<string> _omittedMySqlFields = new(StringComparer.OrdinalIgnoreCase);
        private readonly HashSet<string> _omittedPostgreSqlFields = new(StringComparer.OrdinalIgnoreCase);

        public void Reset()
        {
            ToolExecutionDelay = TimeSpan.Zero;
            DiagnosisExceptionMessage = null;
            _omittedMySqlFields.Clear();
            _omittedPostgreSqlFields.Clear();
        }

        public void OmitFields(DatabaseEngine engine, IEnumerable<string> fieldNames)
        {
            var target = engine == DatabaseEngine.MySql
                ? _omittedMySqlFields
                : _omittedPostgreSqlFields;

            foreach (var fieldName in fieldNames)
            {
                target.Add(fieldName);
            }
        }

        public IReadOnlyCollection<string> GetOmittedFields(DatabaseEngine engine)
        {
            return engine == DatabaseEngine.MySql
                ? _omittedMySqlFields.ToArray()
                : _omittedPostgreSqlFields.ToArray();
        }
    }

    private sealed class TestControlPlaneDbContextFactory : IDbContextFactory<ControlPlaneDbContext>
    {
        private readonly DbContextOptions<ControlPlaneDbContext> _options =
            new DbContextOptionsBuilder<ControlPlaneDbContext>()
                .UseInMemoryDatabase($"aidbopt-tests-{Guid.NewGuid():N}")
                .Options;

        public ControlPlaneDbContext CreateDbContext()
        {
            return new ControlPlaneDbContext(_options);
        }

        public Task<ControlPlaneDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateDbContext());
        }
    }

    private static async Task<string> WaitForReviewTaskAsync(
        HttpClient client,
        string workflowId,
        int timeoutSeconds)
    {
        var deadline = DateTimeOffset.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTimeOffset.UtcNow < deadline)
        {
            var response = await client.GetAsync("/api/reviews");
            response.EnsureSuccessStatusCode();
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var match = payload.RootElement
                .EnumerateArray()
                .FirstOrDefault(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase));
            if (match.ValueKind == JsonValueKind.Object &&
                match.TryGetProperty("taskId", out var taskIdElement) &&
                !string.IsNullOrWhiteSpace(taskIdElement.GetString()))
            {
                return taskIdElement.GetString()!;
            }

            await Task.Delay(500);
        }

        throw new TimeoutException($"Workflow {workflowId} did not create a review task in time.");
    }

    private static async Task WaitForWorkflowStatusAsync(
        HttpClient client,
        string workflowId,
        IReadOnlyCollection<string> expectedStatuses,
        int timeoutSeconds)
    {
        var deadline = DateTimeOffset.UtcNow.AddSeconds(timeoutSeconds);
        while (DateTimeOffset.UtcNow < deadline)
        {
            var response = await client.GetAsync($"/api/workflows/{workflowId}");
            response.EnsureSuccessStatusCode();
            using var payload = JsonDocument.Parse(await response.Content.ReadAsStringAsync());
            var status = payload.RootElement.GetProperty("status").GetString();
            if (status is not null && expectedStatuses.Contains(status, StringComparer.OrdinalIgnoreCase))
            {
                return;
            }

            await Task.Delay(500);
        }

        throw new TimeoutException($"Workflow {workflowId} did not reach one of [{string.Join(", ", expectedStatuses)}].");
    }
}

using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
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
        using var detailJson = JsonDocument.Parse(await detailResponse.Content.ReadAsStringAsync());
        Assert.Equal("Completed", detailJson.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task SubmitReview_UpdatesWorkflowStatus()
    {
        var connectionId = await _factory.SeedConnectionAsync("mysql-main", DatabaseEngine.MySql);
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

        var reviewsResponse = await client.GetAsync("/api/reviews");
        reviewsResponse.EnsureSuccessStatusCode();
        using var reviewsJson = JsonDocument.Parse(await reviewsResponse.Content.ReadAsStringAsync());
        var taskId = reviewsJson.RootElement.EnumerateArray()
            .First(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase))
            .GetProperty("taskId").GetString();

        Assert.False(string.IsNullOrWhiteSpace(taskId));

        var submitResponse = await client.PostAsJsonAsync($"/api/reviews/{taskId}/submit", new
        {
            action = "approve",
            reviewer = "tester",
            comment = "approved"
        });

        submitResponse.EnsureSuccessStatusCode();

        var workflowResponse = await client.GetAsync($"/api/workflows/{workflowId}");
        workflowResponse.EnsureSuccessStatusCode();
        using var workflowJson = JsonDocument.Parse(await workflowResponse.Content.ReadAsStringAsync());
        Assert.Equal("Completed", workflowJson.RootElement.GetProperty("status").GetString());
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

        await using (var dbContext = _factory.CreateControlPlaneDbContext())
        {
            var checkpointCount = dbContext.WorkflowCheckpoints.Count(x => x.WorkflowSessionId == sessionId);
            Assert.True(checkpointCount >= 1);
        }

        var reviewsResponse = await client.GetAsync("/api/reviews");
        using var reviewsJson = JsonDocument.Parse(await reviewsResponse.Content.ReadAsStringAsync());
        var taskId = reviewsJson.RootElement.EnumerateArray()
            .First(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase))
            .GetProperty("taskId").GetString();

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
        var status = startJson.RootElement.GetProperty("status").GetString();
        var workflowId = startJson.RootElement.GetProperty("sessionId").GetString();

        Assert.Equal("Completed", status);
        Assert.False(string.IsNullOrWhiteSpace(workflowId));

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

        var reviewsResponse = await client.GetAsync("/api/reviews");
        using var reviewsJson = JsonDocument.Parse(await reviewsResponse.Content.ReadAsStringAsync());
        var taskId = reviewsJson.RootElement.EnumerateArray()
            .First(item => string.Equals(item.GetProperty("sessionId").GetString(), workflowId, StringComparison.OrdinalIgnoreCase))
            .GetProperty("taskId").GetString();

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

        var historyDetailResponse = await client.GetAsync($"/api/history/{workflowId}");
        historyDetailResponse.EnsureSuccessStatusCode();
        using var historyDetailJson = JsonDocument.Parse(await historyDetailResponse.Content.ReadAsStringAsync());
        Assert.True(historyDetailJson.RootElement.GetProperty("nodeExecutions").GetArrayLength() >= 1);
        Assert.True(historyDetailJson.RootElement.GetProperty("toolExecutions").GetArrayLength() >= 1);
        Assert.True(historyDetailJson.RootElement.GetProperty("result").GetProperty("parsedReport").ValueKind == JsonValueKind.Object);
    }

    [Fact]
    public async Task Recovery_OnRunningSession_RehydratesPendingReview()
    {
        var connectionId = await _factory.SeedConnectionAsync("recovery-main", DatabaseEngine.MySql);
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
    }

    public sealed class WorkflowApiFactory : WebApplicationFactory<Program>
    {
        private readonly TestControlPlaneDbContextFactory _dbContextFactory = new();

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
                services.AddSingleton<IDbConfigDiagnosisAgentExecutor>(new FakeDiagnosisAgentExecutor());
                services.RemoveAll<IMcpToolExecutionService>();
                services.AddSingleton<IMcpToolExecutionService>(new FakeWorkflowToolExecutionService(_dbContextFactory));
            });
        }

        public async Task<Guid> SeedConnectionAsync(string name, DatabaseEngine engine)
        {
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
    }

    private sealed class FakeDiagnosisAgentExecutor : IDbConfigDiagnosisAgentExecutor
    {
        public string PromptVersion => "test-prompt-v1";

        public string ModelId => "fake-model";

        public string BuildPrompt(string displayName, string databaseName, string engine, string optimizationGoal, DbConfigEvidencePack evidence)
        {
            return $"fake-prompt:{displayName}:{databaseName}:{engine}:{optimizationGoal}";
        }

        public Task<DbConfigDiagnosisExecutionResult> ExecuteAsync(DbConfigEvidencePack evidence, string? optimizationGoal, CancellationToken cancellationToken = default)
        {
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
                    appliesWhen = item.AppliesWhen,
                    ruleId = item.RuleId,
                    ruleVersion = item.RuleVersion
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
                warnings = evidence.Warnings
            });

            return Task.FromResult(new DbConfigDiagnosisExecutionResult(
                payload,
                """{"promptTokens":10,"completionTokens":20,"totalTokens":30}"""));
        }
    }

    private sealed class FakeWorkflowToolExecutionService(TestControlPlaneDbContextFactory dbContextFactory) : IMcpToolExecutionService
    {
        public async Task<McpToolExecutionResult> ExecuteAsync(McpToolExecutionRequest request, CancellationToken cancellationToken = default)
        {
            await using var dbContext = dbContextFactory.CreateDbContext();
            var tool = await dbContext.McpTools.AsNoTracking().SingleAsync(x => x.Id == request.ToolId, cancellationToken);
            var connectionId = tool.ConnectionId;
            var responseJson = """{"max_connections":"300","innodb_buffer_pool_size":"512MB","shared_buffers":"256MB","work_mem":"4MB"}""";
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

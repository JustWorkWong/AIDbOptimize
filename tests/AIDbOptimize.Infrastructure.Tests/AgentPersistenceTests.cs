using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Infrastructure.Agents;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class AgentPersistenceTests
{
    [Fact]
    public async Task SessionPersistenceService_CreatesSessionAndMessages()
    {
        var options = CreateOptions();
        await using var dbContext = new ControlPlaneDbContext(options);
        SeedWorkflowSession(dbContext);
        var factory = new TestDbContextFactory(options);
        var service = new AgentSessionPersistenceService(factory);

        var createdAt = DateTimeOffset.UtcNow;
        var created = await service.CreateSessionAsync(
            new AgentSessionCreateRequest(
                Guid.Parse(SessionIdText),
                "DbConfigDiagnosisAgent",
                "control-plane-v1",
                "deterministic-report-builder",
                """{"prompt":"p"}""",
                """{"workflow":"DbConfigOptimization"}""",
                createdAt));

        await service.AppendMessageAsync(
            new AgentMessageCreateRequest(
                created.AgentSessionId,
                Guid.Parse(SessionIdText),
                1,
                "system",
                "PromptInput",
                "prompt body",
                """{"prompt":"prompt body"}""",
                null,
                createdAt));

        await service.AppendMessageAsync(
            new AgentMessageCreateRequest(
                created.AgentSessionId,
                Guid.Parse(SessionIdText),
                2,
                "assistant",
                "FinalAnswer",
                "report body",
                """{"report":"report body"}""",
                null,
                createdAt));

        var session = await dbContext.AgentSessions.SingleAsync(x => x.Id == created.AgentSessionId);
        var messageCount = await dbContext.AgentMessages.CountAsync(x => x.AgentSessionId == created.AgentSessionId);

        Assert.Equal("DbConfigDiagnosisAgent", session.AgentRole);
        Assert.Equal(2, messageCount);
    }

    [Fact]
    public async Task SummaryService_CreatesSummaryAndAttachsIt()
    {
        var options = CreateOptions();
        await using var dbContext = new ControlPlaneDbContext(options);
        SeedWorkflowSession(dbContext);
        var factory = new TestDbContextFactory(options);
        var sessionService = new AgentSessionPersistenceService(factory);
        var summaryService = new AgentSummaryService(factory);
        var createdAt = DateTimeOffset.UtcNow;

        var created = await sessionService.CreateSessionAsync(
            new AgentSessionCreateRequest(
                Guid.Parse(SessionIdText),
                "DbConfigDiagnosisAgent",
                "control-plane-v1",
                "deterministic-report-builder",
                "{}",
                "{}",
                createdAt));

        var summary = await summaryService.CreateRollingSummaryAsync(
            new AgentSummaryCreateRequest(
                created.AgentSessionId,
                "rolling",
                AgentSummaryService.BuildRollingSummaryJson(
                    "DbConfigOptimization",
                    "appdb",
                    "report body",
                    "降低慢查询",
                    2),
                1,
                2,
                createdAt));

        await sessionService.AttachSummaryAsync(
            created.AgentSessionId,
            summary.SummaryId,
            2,
            createdAt,
            """{"activeSummaryId":"1"}""",
            """{"workflow":"DbConfigOptimization"}""",
            CancellationToken.None);

        var session = await dbContext.AgentSessions.SingleAsync(x => x.Id == created.AgentSessionId);
        var persistedSummary = await dbContext.AgentSummaries.SingleAsync(x => x.Id == summary.SummaryId);

        Assert.Equal(summary.SummaryId, session.ActiveSummaryId);
        Assert.Equal(2, session.MessageGroupCount);
        Assert.Equal("rolling", persistedSummary.SummaryType);
    }

    private static DbContextOptions<ControlPlaneDbContext> CreateOptions()
    {
        return new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"agent-tests-{Guid.NewGuid():N}")
            .Options;
    }

    private static void SeedWorkflowSession(ControlPlaneDbContext dbContext)
    {
        dbContext.WorkflowSessions.Add(new WorkflowSessionEntity
        {
            Id = Guid.Parse(SessionIdText),
            ConnectionId = Guid.Parse(ConnectionIdText),
            WorkflowName = "DbConfigOptimization",
            Status = Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded,
            RequestedBy = "tester",
            InputPayloadJson = "{}",
            ResultPayloadJson = "{}",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Connection = new McpConnectionEntity
            {
                Id = Guid.Parse(ConnectionIdText),
                Name = "test-connection",
                Engine = Domain.Mcp.Enums.DatabaseEngine.PostgreSql,
                DisplayName = "test-connection",
                ServerCommand = "npx",
                ServerArgumentsJson = "[]",
                EnvironmentJson = "{}",
                DatabaseConnectionString = "Host=localhost",
                DatabaseName = "appdb",
                IsDefault = false,
                Status = Domain.Mcp.Enums.McpConnectionStatus.Ready,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            }
        });

        dbContext.SaveChanges();
    }

    private sealed class TestDbContextFactory(DbContextOptions<ControlPlaneDbContext> options)
        : IDbContextFactory<ControlPlaneDbContext>
    {
        public DbContextOptions<ControlPlaneDbContext> Options { get; } = options;

        public ControlPlaneDbContext CreateDbContext()
        {
            return new ControlPlaneDbContext(Options);
        }

        public Task<ControlPlaneDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateDbContext());
        }
    }

    private const string SessionIdText = "aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa";
    private const string ConnectionIdText = "bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb";
}

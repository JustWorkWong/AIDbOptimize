using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Workflows.Runtime;
using Microsoft.Agents.AI.Workflows;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class MafWorkflowGraphTests
{
    [Fact]
    public async Task MinimalWorkflowGraph_CanRunToCompletion()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"maf-graph-tests-{Guid.NewGuid():N}")
            .Options;
        var factory = new TestDbContextFactory(options);
        await using var dbContext = new ControlPlaneDbContext(options);
        dbContext.WorkflowSessions.Add(new Infrastructure.Persistence.Entities.WorkflowSessionEntity
        {
            Id = Guid.Parse(SessionIdText),
            ConnectionId = Guid.Parse(ConnectionIdText),
            WorkflowName = "DbConfigOptimization",
            EngineType = "maf",
            RequestedBy = "tester",
            InputPayloadJson = "{}",
            StateJson = "{}",
            ResultType = "db-config-optimization-report",
            ResultPayloadJson = "{}",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Connection = new Infrastructure.Persistence.Entities.McpConnectionEntity
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
        await dbContext.SaveChangesAsync();

        var store = new MafJsonCheckpointStore(factory);
        var result = await MafWorkflowGraphFactory.ExecuteMinimalAsync(store, Guid.Parse(SessionIdText), CancellationToken.None);

        Assert.Equal(Microsoft.Agents.AI.Workflows.RunStatus.Idle, result.Status);
    }

    [Fact]
    public async Task ReviewProbeWorkflow_CanPauseForRequest_AndResumeWithResponse()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"maf-review-probe-{Guid.NewGuid():N}")
            .Options;
        var factory = new TestDbContextFactory(options);
        await using var dbContext = new ControlPlaneDbContext(options);
        dbContext.WorkflowSessions.Add(new Infrastructure.Persistence.Entities.WorkflowSessionEntity
        {
            Id = Guid.Parse(SessionIdText),
            ConnectionId = Guid.Parse(ConnectionIdText),
            WorkflowName = "DbConfigOptimization",
            EngineType = "maf",
            RequestedBy = "tester",
            InputPayloadJson = "{}",
            StateJson = "{}",
            ResultType = "db-config-optimization-report",
            ResultPayloadJson = "{}",
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow,
            Connection = new Infrastructure.Persistence.Entities.McpConnectionEntity
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
        await dbContext.SaveChangesAsync();

        var workflow = MafWorkflowGraphFactory.BuildReviewProbeWorkflow();
        var store = new MafJsonCheckpointStore(factory);
        var checkpointManager = MafWorkflowGraphFactory.CreateCheckpointManager(store);
        var run = await InProcessExecution.RunAsync(
            workflow,
            new MafWorkflowStartMessage(
                Guid.Parse(SessionIdText),
                new DbConfigWorkflowCommand(
                    Guid.Parse(ConnectionIdText),
                    "tester",
                    null,
                    true,
                    true,
                    true)),
            checkpointManager,
            SessionIdText,
            CancellationToken.None);

        var status = await run.GetStatusAsync(CancellationToken.None);
        Assert.Equal(RunStatus.PendingRequests, status);

        var requestEvent = run.OutgoingEvents.OfType<RequestInfoEvent>().Single();
        var response = requestEvent.Request.CreateResponse(new MafReviewResponse(
            "approve",
            "tester",
            "approved",
            "{}"));

        var hadOutput = await run.ResumeAsync(new[] { response }, CancellationToken.None);
        Assert.True(hadOutput);

        var resumedStatus = await run.GetStatusAsync(CancellationToken.None);
        Assert.Equal(RunStatus.Idle, resumedStatus);
        Assert.Contains(run.OutgoingEvents, workflowEvent =>
            workflowEvent is WorkflowOutputEvent output &&
            output.Is<MafCompletionMessage>());
    }

    private sealed class TestDbContextFactory(DbContextOptions<ControlPlaneDbContext> options)
        : IDbContextFactory<ControlPlaneDbContext>
    {
        public ControlPlaneDbContext CreateDbContext()
        {
            return new ControlPlaneDbContext(options);
        }

        public Task<ControlPlaneDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult(CreateDbContext());
        }
    }

    private const string SessionIdText = "cccccccc-cccc-cccc-cccc-cccccccccccc";
    private const string ConnectionIdText = "dddddddd-dddd-dddd-dddd-dddddddddddd";
}

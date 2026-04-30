using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class DbConfigHostContextCollectorTests
{
    [Fact]
    public async Task CollectAsync_UsesHostContextMcpToolset_WhenAvailable()
    {
        var databaseConnectionId = Guid.Parse("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaaaaaa");
        var hostConnectionId = Guid.Parse("bbbbbbbb-bbbb-bbbb-bbbb-bbbbbbbbbbbb");
        var resolveToolId = Guid.Parse("11111111-1111-1111-1111-111111111111");
        var limitsToolId = Guid.Parse("22222222-2222-2222-2222-222222222222");
        var statsToolId = Guid.Parse("33333333-3333-3333-3333-333333333333");
        var diskToolId = Guid.Parse("44444444-4444-4444-4444-444444444444");

        var connectionRepository = new FakeConnectionRepository(
            new McpConnectionRecord(
                databaseConnectionId,
                "mysql-main",
                DatabaseEngine.MySql,
                "MySQL Main",
                "npx",
                "[]",
                "{}",
                "Server=127.0.0.1;Port=3306;Database=orders;",
                "orders",
                true,
                McpConnectionStatus.Ready,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow),
            new McpConnectionRecord(
                hostConnectionId,
                "host-context-main",
                DatabaseEngine.PostgreSql,
                "Host Context",
                "npx",
                "[]",
                "{}",
                string.Empty,
                "hostctx",
                false,
                McpConnectionStatus.Ready,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow));
        var toolRepository = new FakeToolRepository(
            new McpToolRecord(resolveToolId, hostConnectionId, "resolve_runtime_target", "resolve_runtime_target", string.Empty, "{}", ToolApprovalMode.NoApproval, true, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow),
            new McpToolRecord(limitsToolId, hostConnectionId, "get_container_limits", "get_container_limits", string.Empty, "{}", ToolApprovalMode.NoApproval, true, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow),
            new McpToolRecord(statsToolId, hostConnectionId, "get_container_stats", "get_container_stats", string.Empty, "{}", ToolApprovalMode.NoApproval, true, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow),
            new McpToolRecord(diskToolId, hostConnectionId, "get_disk_usage", "get_disk_usage", string.Empty, "{}", ToolApprovalMode.NoApproval, true, false, DateTimeOffset.UtcNow, DateTimeOffset.UtcNow));
        var executionService = new FakeExecutionService(new Dictionary<Guid, string>
        {
            [resolveToolId] = """{"resourceScope":"container","targetType":"docker-container","targetId":"mysql-1","targetName":"mysql-1"}""",
            [limitsToolId] = """{"memoryLimitBytes":8589934592,"cpuLimitCores":4}""",
            [statsToolId] = """{"memoryAvailableBytes":8053063680,"cpuUsagePercent":17.5}""",
            [diskToolId] = """{"diskAvailableBytes":10737418240,"diskTotalBytes":21474836480}"""
        });
        var collector = new McpDbConfigHostContextCollector(connectionRepository, toolRepository, executionService);

        var result = await collector.CollectAsync(
            new McpConnectionEntity
            {
                Id = databaseConnectionId,
                Name = "mysql-main",
                Engine = DatabaseEngine.MySql,
                DisplayName = "MySQL Main",
                DatabaseName = "orders"
            },
            Guid.NewGuid(),
            "DbConfigSnapshotCollectorExecutor",
            "workflow",
            null,
            CancellationToken.None);

        Assert.Equal("container", result.ResourceScope);
        Assert.Contains(result.Items, item => item.Reference == "memory_limit_bytes");
        Assert.Contains(result.Items, item => item.Reference == "memory_available_bytes");
        Assert.Contains(result.Items, item => item.Reference == "cpu_limit_cores");
        Assert.Contains(result.Items, item => item.Reference == "disk_available_bytes");
        Assert.Empty(result.MissingContextItems);
        Assert.Contains(result.CollectionMetadata, item => item.Name == "host_context_connection_name" && item.Value == "host-context-main");
    }

    [Fact]
    public async Task CollectAsync_ReturnsStructuredUnavailable_WhenNoHostContextToolsetExists()
    {
        var databaseConnectionId = Guid.Parse("cccccccc-cccc-cccc-cccc-cccccccccccc");
        var connectionRepository = new FakeConnectionRepository(
            new McpConnectionRecord(
                databaseConnectionId,
                "pgsql-main",
                DatabaseEngine.PostgreSql,
                "PostgreSQL Main",
                "npx",
                "[]",
                "{}",
                "Host=127.0.0.1;Port=5432;Database=appdb;",
                "appdb",
                true,
                McpConnectionStatus.Ready,
                null,
                DateTimeOffset.UtcNow,
                DateTimeOffset.UtcNow));
        var collector = new McpDbConfigHostContextCollector(connectionRepository, new FakeToolRepository(), new FakeExecutionService(new Dictionary<Guid, string>()));

        var result = await collector.CollectAsync(
            new McpConnectionEntity
            {
                Id = databaseConnectionId,
                Name = "pgsql-main",
                Engine = DatabaseEngine.PostgreSql,
                DisplayName = "PostgreSQL Main",
                DatabaseName = "appdb"
            },
            Guid.NewGuid(),
            "DbConfigSnapshotCollectorExecutor",
            "workflow",
            null,
            CancellationToken.None);

        Assert.Equal("unknown", result.ResourceScope);
        Assert.Empty(result.Items);
        Assert.NotEmpty(result.MissingContextItems);
        Assert.Contains(result.CollectionMetadata, item => item.Name == "host_context_status");
    }

    private sealed class FakeConnectionRepository(params McpConnectionRecord[] records) : IMcpConnectionRepository
    {
        public Task<IReadOnlyList<McpConnectionRecord>> GetAllAsync(CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<McpConnectionRecord>>(records);

        public Task<McpConnectionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(records.FirstOrDefault(record => record.Id == id));

        public Task AddAsync(McpConnectionRecord record, CancellationToken cancellationToken = default) => Task.CompletedTask;

        public Task UpdateAsync(McpConnectionRecord record, CancellationToken cancellationToken = default) => Task.CompletedTask;
    }

    private sealed class FakeToolRepository(params McpToolRecord[] records) : IMcpToolRepository
    {
        public Task<IReadOnlyList<McpToolRecord>> ListByConnectionAsync(Guid connectionId, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<McpToolRecord>>(records.Where(record => record.ConnectionId == connectionId).ToArray());

        public Task<McpToolRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
            => Task.FromResult(records.FirstOrDefault(record => record.Id == id));

        public Task<IReadOnlyList<McpToolRecord>> UpsertManyAsync(IReadOnlyCollection<McpToolRecord> tools, CancellationToken cancellationToken = default)
            => Task.FromResult<IReadOnlyList<McpToolRecord>>(tools.ToArray());

        public Task UpdateApprovalModeAsync(Guid id, ToolApprovalMode approvalMode, CancellationToken cancellationToken = default)
            => Task.CompletedTask;
    }

    private sealed class FakeExecutionService(IReadOnlyDictionary<Guid, string> responses) : IMcpToolExecutionService
    {
        public Task<McpToolExecutionResult> ExecuteAsync(McpToolExecutionRequest request, CancellationToken cancellationToken = default)
        {
            var payload = responses.TryGetValue(request.ToolId, out var response) ? response : "{}";
            return Task.FromResult(new McpToolExecutionResult(request.ToolId, "Succeeded", payload, null));
        }
    }
}

using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.ApiService.DatabaseMigrations;

/// <summary>
/// 控制面默认数据种子服务。
/// 当前负责写入 PostgreSQL / MySQL 默认 MCP 连接，便于后续前端直接管理。
/// </summary>
internal sealed class ControlPlaneDefaultSeedHostedService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    IConfiguration configuration,
    ILogger<ControlPlaneDefaultSeedHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        await EnsureConnectionAsync(
            dbContext,
            name: "pgsql-lab-default",
            engine: DatabaseEngine.PostgreSql,
            displayName: "PostgreSQL 测试库",
            databaseName: "aidbopt_lab_pg",
            serverCommand: "npx",
            serverArgumentsJson: """["-y","@modelcontextprotocol/server-postgres"]""",
            environmentJson: BuildEnvironmentJson(ResolveConnectionString(configuration, "aidbopt-lab-pg", "PostgreSqlLab", string.Empty)),
            databaseConnectionString: ResolveConnectionString(configuration, "aidbopt-lab-pg", "PostgreSqlLab", string.Empty),
            isDefault: true,
            cancellationToken: cancellationToken);

        await EnsureConnectionAsync(
            dbContext,
            name: "mysql-lab-default",
            engine: DatabaseEngine.MySql,
            displayName: "MySQL 测试库",
            databaseName: "aidbopt_lab_mysql",
            serverCommand: "npx",
            serverArgumentsJson: """["-y","@modelcontextprotocol/server-mysql"]""",
            environmentJson: BuildEnvironmentJson(ResolveConnectionString(configuration, "aidbopt-lab-mysql", "MySqlLab", string.Empty)),
            databaseConnectionString: ResolveConnectionString(configuration, "aidbopt-lab-mysql", "MySqlLab", string.Empty),
            isDefault: true,
            cancellationToken: cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("控制面默认 MCP 连接已检查并补齐。");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 若默认连接不存在，则补写一条默认记录。
    /// </summary>
    private static async Task EnsureConnectionAsync(
        ControlPlaneDbContext dbContext,
        string name,
        DatabaseEngine engine,
        string displayName,
        string databaseName,
        string serverCommand,
        string serverArgumentsJson,
        string environmentJson,
        string databaseConnectionString,
        bool isDefault,
        CancellationToken cancellationToken)
    {
        var exists = await dbContext.McpConnections.AnyAsync(x => x.Name == name, cancellationToken);
        if (exists)
        {
            return;
        }

        dbContext.McpConnections.Add(new McpConnectionEntity
        {
            Id = Guid.NewGuid(),
            Name = name,
            Engine = engine,
            DisplayName = displayName,
            ServerCommand = serverCommand,
            ServerArgumentsJson = serverArgumentsJson,
            EnvironmentJson = environmentJson,
            DatabaseConnectionString = databaseConnectionString,
            DatabaseName = databaseName,
            IsDefault = isDefault,
            Status = McpConnectionStatus.Draft,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        });
    }

    private static string BuildEnvironmentJson(string connectionString)
    {
        return $$"""{"DATABASE_URL":"{{connectionString.Replace("\\", "\\\\").Replace("\"", "\\\"")}}"}""";
    }

    private static string ResolveConnectionString(
        IConfiguration configuration,
        string primaryName,
        string secondaryName,
        string defaultValue)
    {
        return configuration.GetConnectionString(primaryName)
            ?? configuration.GetConnectionString(secondaryName)
            ?? defaultValue;
    }
}

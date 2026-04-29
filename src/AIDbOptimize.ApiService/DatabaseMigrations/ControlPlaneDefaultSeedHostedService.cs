using System.Text.Json;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;

namespace AIDbOptimize.ApiService.DatabaseMigrations;

/// <summary>
/// Seeds default MCP connections into the control plane database.
/// </summary>
internal sealed class ControlPlaneDefaultSeedHostedService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    IConfiguration configuration,
    IConnectionSecretProtector secretProtector,
    ILogger<ControlPlaneDefaultSeedHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var postgreSqlConnectionString = ResolveConnectionString(configuration, "aidbopt-lab-pg", "PostgreSqlLab", string.Empty);
        var mySqlConnectionString = ResolveConnectionString(configuration, "aidbopt-lab-mysql", "MySqlLab", string.Empty);

        await UpsertConnectionAsync(
            dbContext,
            name: "pgsql-lab-default",
            engine: DatabaseEngine.PostgreSql,
            displayName: "PostgreSQL 测试库",
            databaseName: "aidbopt_lab_pg",
            serverCommand: "npx",
            serverArgumentsJson: BuildPostgreSqlArgumentsJson(postgreSqlConnectionString),
            environmentJson: "{}",
            databaseConnectionString: postgreSqlConnectionString,
            isDefault: true,
            cancellationToken: cancellationToken);

        await UpsertConnectionAsync(
            dbContext,
            name: "mysql-lab-default",
            engine: DatabaseEngine.MySql,
            displayName: "MySQL 测试库",
            databaseName: "aidbopt_lab_mysql",
            serverCommand: "npx",
            serverArgumentsJson: """["-y","mysql-mcp-server"]""",
            environmentJson: BuildMySqlEnvironmentJson(mySqlConnectionString),
            databaseConnectionString: mySqlConnectionString,
            isDefault: true,
            cancellationToken: cancellationToken);

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Default MCP connections are ready.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task UpsertConnectionAsync(
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
        var entity = await dbContext.McpConnections.FirstOrDefaultAsync(x => x.Name == name, cancellationToken);
        if (entity is null)
        {
            entity = new McpConnectionEntity
            {
                Id = Guid.NewGuid(),
                Name = name,
                CreatedAt = DateTimeOffset.UtcNow
            };

            dbContext.McpConnections.Add(entity);
        }

        entity.Engine = engine;
        entity.DisplayName = displayName;
        entity.ServerCommand = serverCommand;
        entity.ServerArgumentsJson = secretProtector.ProtectIfNeeded(serverArgumentsJson);
        entity.EnvironmentJson = secretProtector.ProtectIfNeeded(environmentJson);
        entity.DatabaseConnectionString = secretProtector.ProtectIfNeeded(databaseConnectionString);
        entity.DatabaseName = databaseName;
        entity.IsDefault = isDefault;
        entity.Status = McpConnectionStatus.Draft;
        entity.LastDiscoveredAt = null;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
    }

    private static string BuildPostgreSqlArgumentsJson(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var password = Uri.EscapeDataString(builder.Password ?? string.Empty);
        var userName = Uri.EscapeDataString(builder.Username ?? string.Empty);
        var database = Uri.EscapeDataString(builder.Database ?? string.Empty);
        var host = builder.Host;
        var port = builder.Port;
        var url = $"postgresql://{userName}:{password}@{host}:{port}/{database}";

        return JsonSerializer.Serialize(new[]
        {
            "-y",
            "@modelcontextprotocol/server-postgres",
            url
        });
    }

    private static string BuildMySqlEnvironmentJson(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);

        return JsonSerializer.Serialize(new Dictionary<string, string>
        {
            ["MYSQL_HOST"] = builder.Server,
            ["MYSQL_PORT"] = builder.Port.ToString(),
            ["MYSQL_USER"] = builder.UserID ?? string.Empty,
            ["MYSQL_PASSWORD"] = builder.Password ?? string.Empty,
            ["MYSQL_DATABASE"] = builder.Database ?? string.Empty
        });
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

using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;

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
        logger.LogInformation("控制面默认 MCP 连接已检查并补齐。");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 插入或更新一条默认连接配置。
    /// </summary>
    private static async Task UpsertConnectionAsync(
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
        entity.ServerArgumentsJson = serverArgumentsJson;
        entity.EnvironmentJson = environmentJson;
        entity.DatabaseConnectionString = databaseConnectionString;
        entity.DatabaseName = databaseName;
        entity.IsDefault = isDefault;
        entity.Status = McpConnectionStatus.Draft;
        entity.LastDiscoveredAt = null;
        entity.UpdatedAt = DateTimeOffset.UtcNow;
    }

    /// <summary>
    /// 将 Npgsql 连接串转换为官方 postgres MCP server 所需的 URL 参数。
    /// </summary>
    private static string BuildPostgreSqlArgumentsJson(string connectionString)
    {
        var builder = new NpgsqlConnectionStringBuilder(connectionString);
        var password = Uri.EscapeDataString(builder.Password ?? string.Empty);
        var userName = Uri.EscapeDataString(builder.Username ?? string.Empty);
        var database = Uri.EscapeDataString(builder.Database ?? string.Empty);
        var host = builder.Host;
        var port = builder.Port;
        var url = $"postgresql://{userName}:{password}@{host}:{port}/{database}";

        return $$"""["-y","@modelcontextprotocol/server-postgres","{{url}}"]""";
    }

    /// <summary>
    /// 将 MySQL 连接串转换为社区 mysql MCP server 所需的环境变量。
    /// 这里使用支持固定端口的 mysql-mcp-server，便于兼容当前 13306 端口。
    /// </summary>
    private static string BuildMySqlEnvironmentJson(string connectionString)
    {
        var builder = new MySqlConnectionStringBuilder(connectionString);

        return $$"""
        {
          "MYSQL_HOST": "{{builder.Server}}",
          "MYSQL_PORT": "{{builder.Port}}",
          "MYSQL_USER": "{{builder.UserID}}",
          "MYSQL_PASSWORD": "{{builder.Password}}",
          "MYSQL_DATABASE": "{{builder.Database}}"
        }
        """;
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

using System.Text.Json;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;
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

        var hostContextServerScriptPath = ResolveHostContextServerScriptPath();
        if (!string.IsNullOrWhiteSpace(hostContextServerScriptPath))
        {
            var hostContextConnection = await UpsertConnectionAsync(
                dbContext,
                name: "host-context-default",
                engine: DatabaseEngine.PostgreSql,
                displayName: "Host Context MCP",
                databaseName: "host_context",
                serverCommand: "node",
                serverArgumentsJson: JsonSerializer.Serialize(new[] { hostContextServerScriptPath }),
                environmentJson: "{}",
                databaseConnectionString: string.Empty,
                isDefault: false,
                cancellationToken: cancellationToken);

            hostContextConnection.Status = McpConnectionStatus.Ready;
            hostContextConnection.LastDiscoveredAt = DateTimeOffset.UtcNow;
            await UpsertHostContextToolsAsync(dbContext, hostContextConnection.Id, cancellationToken);
        }
        else
        {
            logger.LogWarning("HostContext MCP server script was not found. Default host-context connection seed was skipped.");
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        logger.LogInformation("Default MCP connections are ready.");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    private async Task<McpConnectionEntity> UpsertConnectionAsync(
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

        return entity;
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

    private async Task UpsertHostContextToolsAsync(
        ControlPlaneDbContext dbContext,
        Guid connectionId,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        foreach (var definition in HostContextToolDefinitions(connectionId))
        {
            var entity = await dbContext.McpTools
                .FirstOrDefaultAsync(
                    x => x.ConnectionId == connectionId && x.ToolName == definition.ToolName,
                    cancellationToken);

            if (entity is null)
            {
                dbContext.McpTools.Add(new McpToolEntity
                {
                    Id = Guid.NewGuid(),
                    ConnectionId = connectionId,
                    ToolName = definition.ToolName,
                    DisplayName = definition.DisplayName,
                    Description = definition.Description ?? string.Empty,
                    InputSchemaJson = definition.InputSchemaJson,
                    ApprovalMode = definition.ApprovalMode,
                    IsEnabled = definition.IsEnabled,
                    IsWriteTool = definition.IsWriteTool,
                    CreatedAt = now,
                    UpdatedAt = now
                });
                continue;
            }

            entity.DisplayName = definition.DisplayName;
            entity.Description = definition.Description ?? string.Empty;
            entity.InputSchemaJson = definition.InputSchemaJson;
            entity.ApprovalMode = definition.ApprovalMode;
            entity.IsEnabled = definition.IsEnabled;
            entity.IsWriteTool = definition.IsWriteTool;
            entity.UpdatedAt = now;
        }
    }

    private static IReadOnlyList<McpToolDefinition> HostContextToolDefinitions(Guid connectionId)
    {
        return
        [
            Tool("resolve_runtime_target", "Resolve Runtime Target", "Resolve a db connection into container, host, or managed-service scope.",
                """
                {
                  "type":"object",
                  "properties":{
                    "connectionId":{"type":"string"},
                    "engine":{"type":"string"},
                    "displayName":{"type":"string"},
                    "databaseName":{"type":"string"},
                    "host":{"type":"string"},
                    "port":{"type":["integer","string"]},
                    "metadata":{"type":"object"}
                  }
                }
                """),
            Tool("get_container_limits", "Get Container Limits", "Read container resource limits and mount metadata.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}"""),
            Tool("get_container_stats", "Get Container Stats", "Read current container runtime stats.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}"""),
            Tool("get_disk_usage", "Get Disk Usage", "Read disk usage for the target container or host fallback.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"},"mode":{"type":"string"}}}"""),
            Tool("get_host_memory", "Get Host Memory", "Read host memory totals and availability.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}"""),
            Tool("get_host_cpu", "Get Host CPU", "Read host CPU core count and optional current usage.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}"""),
            Tool("get_process_limits", "Get Process Limits", "Read process-level runtime context.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}"""),
            Tool("get_managed_service_profile", "Get Managed Service Profile", "Read managed-service profile metadata when available.",
                """{"type":"object","properties":{"targetId":{"type":"string"},"targetName":{"type":"string"}}}""")
        ];

        McpToolDefinition Tool(string toolName, string displayName, string description, string inputSchemaJson)
        {
            return new McpToolDefinition(
                Guid.Empty,
                connectionId,
                toolName,
                displayName,
                description,
                inputSchemaJson,
                ToolApprovalMode.NoApproval,
                true,
                false);
        }
    }

    private string? ResolveHostContextServerScriptPath()
    {
        var candidateDirectories = new List<string>();
        var directory = new DirectoryInfo(AppContext.BaseDirectory);
        while (directory is not null)
        {
            candidateDirectories.Add(directory.FullName);
            directory = directory.Parent;
        }

        foreach (var baseDirectory in candidateDirectories)
        {
            var candidate = Path.GetFullPath(Path.Combine(baseDirectory, "AIDbOptimize.HostContextMcp", "server.mjs"));
            if (File.Exists(candidate))
            {
                return candidate;
            }

            candidate = Path.GetFullPath(Path.Combine(baseDirectory, "src", "AIDbOptimize.HostContextMcp", "server.mjs"));
            if (File.Exists(candidate))
            {
                return candidate;
            }
        }

        return null;
    }
}

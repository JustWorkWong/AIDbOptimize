using AIDbOptimize.Domain.Mcp.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP client factory.
/// Starts a stdio MCP server process from persisted connection settings.
/// </summary>
public sealed class McpClientFactory(ILoggerFactory loggerFactory)
{
    public async Task<McpProcessSession> CreateAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default)
    {
        var logger = loggerFactory.CreateLogger<McpClientFactory>();

        logger.LogInformation("[MCP:{ConnectionName}] Starting MCP session", connection.Name);
        logger.LogInformation(
            "[MCP:{ConnectionName}] Command: {Command} {Arguments}",
            connection.Name,
            connection.ServerCommand,
            string.Join(" ", connection.ServerArguments));

        var transport = new StdioClientTransport(
            new StdioClientTransportOptions
            {
                Name = connection.Name,
                Command = connection.ServerCommand,
                Arguments = connection.ServerArguments.ToList(),
                EnvironmentVariables = MergeEnvironment(connection.EnvironmentVariables),
                ShutdownTimeout = TimeSpan.FromSeconds(5),
                StandardErrorLines = line =>
                {
                    if (!string.IsNullOrWhiteSpace(line))
                    {
                        logger.LogInformation("[MCP:{ConnectionName}] stderr: {Line}", connection.Name, line);
                    }
                }
            },
            loggerFactory);

        logger.LogInformation("[MCP:{ConnectionName}] Initializing MCP client...", connection.Name);

        var client = await McpClient.CreateAsync(
            transport,
            new McpClientOptions
            {
                InitializationTimeout = TimeSpan.FromSeconds(60)
            },
            loggerFactory,
            cancellationToken);

        logger.LogInformation("[MCP:{ConnectionName}] MCP client initialized", connection.Name);

        return new McpProcessSession(client);
    }

    private static Dictionary<string, string?> MergeEnvironment(IReadOnlyDictionary<string, string> overrides)
    {
        var environment = new Dictionary<string, string?>(StringComparer.OrdinalIgnoreCase);

        foreach (var key in Environment.GetEnvironmentVariables().Keys)
        {
            var keyText = key?.ToString();
            if (string.IsNullOrWhiteSpace(keyText))
            {
                continue;
            }

            environment[keyText] = Environment.GetEnvironmentVariable(keyText);
        }

        foreach (var pair in overrides)
        {
            environment[pair.Key] = pair.Value;
        }

        return environment;
    }
}

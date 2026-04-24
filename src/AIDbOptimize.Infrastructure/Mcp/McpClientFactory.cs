using System.Text.Json;
using AIDbOptimize.Domain.Mcp.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 客户端工厂。
/// 负责根据持久化的连接配置启动 stdio MCP server，并建立客户端会话。
/// </summary>
public sealed class McpClientFactory(ILoggerFactory loggerFactory)
{
    /// <summary>
    /// 创建一个短生命周期 MCP 会话。
    /// 当前每次发现工具或执行工具时都创建新会话，优先保证实现简单和稳定。
    /// </summary>
    public async Task<McpProcessSession> CreateAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default)
    {
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
                        loggerFactory.CreateLogger<McpClientFactory>().LogInformation("[MCP:{ConnectionName}] {Line}", connection.Name, line);
                    }
                }
            },
            loggerFactory);

        var client = await McpClient.CreateAsync(
            transport,
            new McpClientOptions
            {
                InitializationTimeout = TimeSpan.FromSeconds(60)
            },
            loggerFactory,
            cancellationToken);

        return new McpProcessSession(client);
    }

    /// <summary>
    /// 将数据库中保存的环境变量与当前进程环境合并。
    /// </summary>
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

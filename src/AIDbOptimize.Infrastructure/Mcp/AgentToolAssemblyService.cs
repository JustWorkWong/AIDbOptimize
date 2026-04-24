using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;
using Microsoft.Extensions.AI;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// 基于真实 MCP 会话装配 Agent 工具。
/// </summary>
public sealed class AgentToolAssemblyService(
    IMcpToolRepository toolRepository,
    IMcpConnectionRepository connectionRepository,
    McpClientFactory clientFactory) : IAgentToolAssemblyService
{
    /// <summary>
    /// 创建真实 MCP 会话，并把已启用工具装配为 Agent 可用的 `AIFunction` 集合。
    /// </summary>
    public async Task<AgentToolAssemblySession> AssembleAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default)
    {
        var configuredTools = await toolRepository.ListByConnectionAsync(connectionId, cancellationToken);
        var enabledTools = configuredTools.Where(tool => tool.IsEnabled).ToArray();

        var connection = await connectionRepository.GetByIdAsync(connectionId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 连接：{connectionId}");

        var session = await clientFactory.CreateAsync(
            new Domain.Mcp.Models.McpConnectionDefinition(
                connection.Id,
                connection.Name,
                connection.Engine,
                connection.DisplayName,
                connection.ServerCommand,
                DeserializeList(connection.ServerArgumentsJson),
                DeserializeDictionary(connection.EnvironmentJson),
                connection.DatabaseConnectionString,
                connection.DatabaseName,
                connection.IsDefault,
                connection.Status,
                connection.LastDiscoveredAt),
            cancellationToken);

        var runtimeTools = await session.Client.ListToolsAsync(cancellationToken: cancellationToken);
        var configuredMap = enabledTools.ToDictionary(tool => tool.ToolName, StringComparer.OrdinalIgnoreCase);

        var tools = runtimeTools
            .Where(tool => configuredMap.ContainsKey(tool.Name))
            .Select(tool =>
            {
                var configured = configuredMap[tool.Name];
                AIFunction function = tool;

                if (configured.ApprovalMode == ToolApprovalMode.ApprovalRequired)
                {
                    function = new ApprovalRequiredAIFunction(function);
                }

                return function;
            })
            .ToArray();

        return new AgentToolAssemblySession(tools, session);
    }

    private static IReadOnlyList<string> DeserializeList(string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? []
            : System.Text.Json.JsonSerializer.Deserialize<List<string>>(json) ?? [];
    }

    private static IReadOnlyDictionary<string, string> DeserializeDictionary(string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? new Dictionary<string, string>()
            : System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }
}

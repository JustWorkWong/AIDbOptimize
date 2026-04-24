using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Mcp.Dtos;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;

namespace AIDbOptimize.Application.Mcp.Services;

/// <summary>
/// MCP 管理应用服务。
/// 当前负责连接查询、工具查询和工具发现编排。
/// </summary>
public sealed class McpDiscoveryAppService(
    IMcpConnectionRepository connectionRepository,
    IMcpToolRepository toolRepository,
    IMcpDiscoveryService discoveryService)
{
    /// <summary>
    /// 查询全部 MCP 连接。
    /// </summary>
    public async Task<IReadOnlyCollection<McpConnectionDto>> GetConnectionsAsync(CancellationToken cancellationToken = default)
    {
        var connections = await connectionRepository.GetAllAsync(cancellationToken);
        return connections
            .Select(record => McpConnectionDto.FromDefinition(ToDefinition(record)))
            .ToArray();
    }

    /// <summary>
    /// 查询连接下的工具列表。
    /// </summary>
    public async Task<IReadOnlyCollection<McpToolDto>> GetToolsAsync(Guid connectionId, CancellationToken cancellationToken = default)
    {
        var tools = await toolRepository.ListByConnectionAsync(connectionId, cancellationToken);
        return tools
            .Select(record => McpToolDto.FromDefinition(ToDefinition(record)))
            .ToArray();
    }

    /// <summary>
    /// 连接 MCP server 并发现工具。
    /// </summary>
    public async Task<IReadOnlyCollection<McpToolDto>> DiscoverToolsAsync(Guid connectionId, CancellationToken cancellationToken = default)
    {
        var connection = await connectionRepository.GetByIdAsync(connectionId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 连接：{connectionId}");

        var discovered = await discoveryService.DiscoverToolsAsync(ToDefinition(connection), cancellationToken);
        var now = DateTimeOffset.UtcNow;

        var records = discovered
            .Select(tool => new McpToolRecord(
                tool.Id == Guid.Empty ? Guid.NewGuid() : tool.Id,
                tool.ConnectionId == Guid.Empty ? connection.Id : tool.ConnectionId,
                tool.ToolName,
                string.IsNullOrWhiteSpace(tool.DisplayName) ? tool.ToolName : tool.DisplayName,
                tool.Description ?? string.Empty,
                tool.InputSchemaJson,
                tool.ApprovalMode,
                tool.IsEnabled,
                tool.IsWriteTool,
                now,
                now))
            .ToArray();

        await toolRepository.UpsertManyAsync(records, cancellationToken);
        await connectionRepository.UpdateAsync(
            connection with
            {
                LastDiscoveredAt = now,
                Status = McpConnectionStatus.Ready
            },
            cancellationToken);

        return records.Select(record => McpToolDto.FromDefinition(ToDefinition(record))).ToArray();
    }

    /// <summary>
    /// 将连接记录转换为跨层定义对象。
    /// </summary>
    private static McpConnectionDefinition ToDefinition(McpConnectionRecord record)
    {
        return new McpConnectionDefinition(
            record.Id,
            record.Name,
            record.Engine,
            record.DisplayName,
            record.ServerCommand,
            DeserializeList(record.ServerArgumentsJson),
            DeserializeDictionary(record.EnvironmentJson),
            record.DatabaseConnectionString,
            record.DatabaseName,
            record.IsDefault,
            record.Status,
            record.LastDiscoveredAt);
    }

    /// <summary>
    /// 将工具记录转换为跨层定义对象。
    /// </summary>
    private static McpToolDefinition ToDefinition(McpToolRecord record)
    {
        return new McpToolDefinition(
            record.Id,
            record.ConnectionId,
            record.ToolName,
            record.DisplayName,
            record.Description,
            record.InputSchemaJson,
            record.ApprovalMode,
            record.IsEnabled,
            record.IsWriteTool);
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

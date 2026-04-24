using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;
using ModelContextProtocol.Client;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 工具发现服务。
/// 该服务会根据连接配置启动真实 MCP server，并调用 `tools/list` 获取工具定义。
/// </summary>
public sealed class McpDiscoveryService(McpClientFactory clientFactory) : IMcpDiscoveryService
{
    /// <summary>
    /// 连接真实 MCP server，读取并转换全部工具定义。
    /// </summary>
    public async Task<IReadOnlyCollection<McpToolDefinition>> DiscoverToolsAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default)
    {
        await using var session = await clientFactory.CreateAsync(connection, cancellationToken);
        var tools = await session.Client.ListToolsAsync(cancellationToken: cancellationToken);

        return tools
            .Select(tool => new McpToolDefinition(
                Id: Guid.Empty,
                ConnectionId: connection.Id,
                ToolName: tool.Name,
                DisplayName: string.IsNullOrWhiteSpace(tool.Title) ? tool.Name : tool.Title,
                Description: tool.Description,
                InputSchemaJson: JsonSerializer.Serialize(tool.ProtocolTool.InputSchema),
                ApprovalMode: ToolApprovalMode.NoApproval,
                IsEnabled: true,
                IsWriteTool: InferWriteTool(tool)))
            .ToArray();
    }

    /// <summary>
    /// 根据工具元数据推断该工具是否为写入型工具。
    /// 优先使用协议注解，其次才回退到名称关键字推断。
    /// </summary>
    private static bool InferWriteTool(McpClientTool tool)
    {
        if (tool.ProtocolTool.Annotations?.ReadOnlyHint is true)
        {
            return false;
        }

        if (tool.ProtocolTool.Annotations?.DestructiveHint is true)
        {
            return true;
        }

        var name = tool.Name.ToLowerInvariant();
        return name.Contains("insert")
            || name.Contains("update")
            || name.Contains("delete")
            || name.Contains("create")
            || name.Contains("write");
    }
}

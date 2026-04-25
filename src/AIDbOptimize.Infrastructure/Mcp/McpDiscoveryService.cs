using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;
using Microsoft.Extensions.Logging;
using ModelContextProtocol.Client;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP tool discovery service.
/// Connects to the real MCP server and calls tools/list.
/// </summary>
public sealed class McpDiscoveryService(
    McpClientFactory clientFactory,
    ILogger<McpDiscoveryService> logger) : IMcpDiscoveryService
{
    public async Task<IReadOnlyCollection<McpToolDefinition>> DiscoverToolsAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default)
    {
        logger.LogInformation("Starting tool discovery for MCP connection [{ConnectionName}]", connection.Name);

        try
        {
            await using var session = await clientFactory.CreateAsync(connection, cancellationToken);

            logger.LogInformation("Calling ListToolsAsync...");
            var tools = await session.Client.ListToolsAsync(cancellationToken: cancellationToken);

            logger.LogInformation("Discovered {ToolCount} tool(s)", tools.Count);

            var definitions = tools
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

            foreach (var tool in definitions)
            {
                logger.LogInformation("  - {ToolName}: {Description}", tool.ToolName, tool.Description);
            }

            return definitions;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Tool discovery failed: {Message}", ex.Message);
            throw;
        }
    }

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

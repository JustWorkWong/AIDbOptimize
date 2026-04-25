using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;

namespace AIDbOptimize.Application.Mcp.Dtos;

/// <summary>
/// MCP 工具查询结果。
/// </summary>
public sealed record McpToolDto(
    Guid Id,
    Guid ConnectionId,
    string ToolName,
    string DisplayName,
    string? Description,
    string InputSchemaJson,
    ToolApprovalMode ApprovalMode,
    bool IsEnabled,
    bool IsWriteTool)
{
    public static McpToolDto FromDefinition(McpToolDefinition definition)
    {
        return new McpToolDto(
            definition.Id,
            definition.ConnectionId,
            definition.ToolName,
            definition.DisplayName,
            definition.Description,
            definition.InputSchemaJson,
            definition.ApprovalMode,
            definition.IsEnabled,
            definition.IsWriteTool);
    }
}

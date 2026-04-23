using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.Mcp.Models;

/// <summary>
/// MCP 工具定义，用于应用层和基础设施层之间传递。
/// </summary>
public sealed record McpToolDefinition(
    Guid Id,
    Guid ConnectionId,
    string ToolName,
    string DisplayName,
    string? Description,
    string InputSchemaJson,
    ToolApprovalMode ApprovalMode,
    bool IsEnabled,
    bool IsWriteTool);

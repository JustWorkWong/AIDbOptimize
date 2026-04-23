namespace AIDbOptimize.Application.Mcp.Dtos;

/// <summary>
/// MCP 工具执行请求。
/// </summary>
public sealed record McpToolExecutionRequest(
    Guid ConnectionId,
    Guid ToolId,
    string PayloadJson,
    string RequestedBy);

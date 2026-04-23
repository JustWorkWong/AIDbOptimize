namespace AIDbOptimize.Application.Mcp.Dtos;

/// <summary>
/// MCP 工具执行结果。
/// </summary>
public sealed record McpToolExecutionResultDto(
    bool Succeeded,
    string ResponsePayloadJson,
    string? ErrorMessage);

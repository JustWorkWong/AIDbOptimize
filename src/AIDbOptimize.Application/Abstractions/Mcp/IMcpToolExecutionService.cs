using System.Text.Json;

namespace AIDbOptimize.Application.Abstractions.Mcp;

/// <summary>
/// MCP tool execution abstraction.
/// </summary>
public interface IMcpToolExecutionService
{
    Task<McpToolExecutionResult> ExecuteAsync(
        McpToolExecutionRequest request,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// MCP tool execution request.
/// </summary>
public sealed record McpToolExecutionRequest(
    Guid ToolId,
    JsonElement Payload,
    Guid? WorkflowSessionId,
    string? WorkflowNodeName,
    string RequestedBy,
    string ExecutionScope,
    string? TraceId);

/// <summary>
/// Tool execution result.
/// </summary>
public sealed record McpToolExecutionResult(
    Guid ExecutionId,
    string Status,
    string ResponsePayloadJson,
    string? ErrorMessage);

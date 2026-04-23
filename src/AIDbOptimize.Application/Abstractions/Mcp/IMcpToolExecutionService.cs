using System.Text.Json;

namespace AIDbOptimize.Application.Abstractions.Mcp;

/// <summary>
/// MCP 工具执行抽象。
/// </summary>
public interface IMcpToolExecutionService
{
    Task<McpToolExecutionResult> ExecuteAsync(Guid toolId, JsonElement payload, CancellationToken cancellationToken = default);
}

/// <summary>
/// 工具执行结果。
/// </summary>
public sealed record McpToolExecutionResult(
    Guid ExecutionId,
    string Status,
    string ResponsePayloadJson,
    string? ErrorMessage);

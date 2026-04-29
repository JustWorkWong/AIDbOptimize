using System.Diagnostics;
using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// Executes MCP tools and persists an audit record.
/// </summary>
public sealed class McpToolExecutionService(
    IMcpToolRepository toolRepository,
    IMcpConnectionRepository connectionRepository,
    McpClientFactory clientFactory,
    IDbContextFactory<Persistence.ControlPlaneDbContext> dbContextFactory) : IMcpToolExecutionService
{
    public async Task<McpToolExecutionResult> ExecuteAsync(
        McpToolExecutionRequest request,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.McpActivitySource.StartActivity("mcp.tool.execute");
        activity?.SetTag("mcp.tool_id", request.ToolId);
        activity?.SetTag("workflow.session_id", request.WorkflowSessionId);
        activity?.SetTag("workflow.node_name", request.WorkflowNodeName);
        activity?.SetTag("mcp.execution_scope", request.ExecutionScope);
        var startedAt = Stopwatch.StartNew();
        AIDbOptimizeTelemetry.McpToolExecutionStarted.Add(1);

        var tool = await toolRepository.GetByIdAsync(request.ToolId, cancellationToken)
            ?? throw new InvalidOperationException($"MCP tool not found: {request.ToolId}");

        var connection = await connectionRepository.GetByIdAsync(tool.ConnectionId, cancellationToken)
            ?? throw new InvalidOperationException($"MCP connection not found: {tool.ConnectionId}");

        var executionId = Guid.NewGuid();

        try
        {
            await using var session = await clientFactory.CreateAsync(ToConnectionDefinition(connection), cancellationToken);
            var arguments = ToArguments(request.Payload);
            var result = await session.Client.CallToolAsync(tool.ToolName, arguments, cancellationToken: cancellationToken);
            var responseJson = ToolOutputSanitizer.SanitizeJson(JsonSerializer.Serialize(result));
            var errorMessage = ExtractToolError(result);
            var status = string.IsNullOrWhiteSpace(errorMessage) ? "Succeeded" : "Failed";

            await SaveExecutionAsync(executionId, connection.Id, tool.Id, request, responseJson, status, errorMessage, cancellationToken);
            startedAt.Stop();
            AIDbOptimizeTelemetry.McpToolExecutionDurationMs.Record(startedAt.Elapsed.TotalMilliseconds);
            if (!string.Equals(status, "Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                AIDbOptimizeTelemetry.McpToolExecutionFailed.Add(1);
            }

            return new McpToolExecutionResult(executionId, status, responseJson, errorMessage);
        }
        catch (Exception ex)
        {
            var sanitizedMessage = SensitiveDataMasker.MaskFreeText(ex.Message);
            await SaveExecutionAsync(executionId, connection.Id, tool.Id, request, "{}", "Failed", sanitizedMessage, cancellationToken);
            startedAt.Stop();
            AIDbOptimizeTelemetry.McpToolExecutionDurationMs.Record(startedAt.Elapsed.TotalMilliseconds);
            AIDbOptimizeTelemetry.McpToolExecutionFailed.Add(1);
            return new McpToolExecutionResult(executionId, "Failed", "{}", sanitizedMessage);
        }
    }

    private async Task SaveExecutionAsync(
        Guid executionId,
        Guid connectionId,
        Guid toolId,
        McpToolExecutionRequest request,
        string responseJson,
        string status,
        string? errorMessage,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.McpToolExecutions.Add(new Persistence.Entities.McpToolExecutionEntity
        {
            Id = executionId,
            ConnectionId = connectionId,
            ToolId = toolId,
            WorkflowSessionId = request.WorkflowSessionId,
            WorkflowNodeName = request.WorkflowNodeName,
            ExecutionScope = request.ExecutionScope,
            TraceId = request.TraceId,
            RequestedBy = string.IsNullOrWhiteSpace(request.RequestedBy) ? "frontend" : request.RequestedBy,
            RequestPayloadJson = ToolOutputSanitizer.SanitizeJson(request.Payload.GetRawText()),
            ResponsePayloadJson = responseJson,
            Status = status,
            ErrorMessage = SensitiveDataMasker.MaskFreeText(errorMessage),
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Domain.Mcp.Models.McpConnectionDefinition ToConnectionDefinition(McpConnectionRecord record)
    {
        return new Domain.Mcp.Models.McpConnectionDefinition(
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

    private static IReadOnlyDictionary<string, object?> ToArguments(JsonElement payload)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("Tool payload must be a JSON object.");
        }

        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in payload.EnumerateObject())
        {
            result[property.Name] = ConvertJsonElement(property.Value);
        }

        return result;
    }

    private static string? ExtractToolError(ModelContextProtocol.Protocol.CallToolResult result)
    {
        if (result.IsError is not true)
        {
            return null;
        }

        return JsonSerializer.Serialize(result.Content);
    }

    private static IReadOnlyList<string> DeserializeList(string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? []
            : JsonSerializer.Deserialize<List<string>>(json) ?? [];
    }

    private static IReadOnlyDictionary<string, string> DeserializeDictionary(string json)
    {
        return string.IsNullOrWhiteSpace(json)
            ? new Dictionary<string, string>()
            : JsonSerializer.Deserialize<Dictionary<string, string>>(json) ?? new Dictionary<string, string>();
    }

    private static object? ConvertJsonElement(JsonElement element)
    {
        return element.ValueKind switch
        {
            JsonValueKind.Object => element.EnumerateObject()
                .ToDictionary(property => property.Name, property => ConvertJsonElement(property.Value)),
            JsonValueKind.Array => element.EnumerateArray().Select(ConvertJsonElement).ToArray(),
            JsonValueKind.String => element.GetString(),
            JsonValueKind.Number when element.TryGetInt64(out var longValue) => longValue,
            JsonValueKind.Number when element.TryGetDecimal(out var decimalValue) => decimalValue,
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.Null => null,
            _ => element.GetRawText()
        };
    }
}

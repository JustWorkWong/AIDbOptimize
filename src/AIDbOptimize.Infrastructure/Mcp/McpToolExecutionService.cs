using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 工具执行服务。
/// 该服务会根据工具所属连接建立真实 MCP 会话，并调用 `tools/call` 执行工具。
/// </summary>
public sealed class McpToolExecutionService(
    IMcpToolRepository toolRepository,
    IMcpConnectionRepository connectionRepository,
    McpClientFactory clientFactory,
    IDbContextFactory<Persistence.ControlPlaneDbContext> dbContextFactory) : IMcpToolExecutionService
{
    /// <summary>
    /// 执行指定工具，并把执行结果写入控制面数据库。
    /// </summary>
    public async Task<McpToolExecutionResult> ExecuteAsync(Guid toolId, JsonElement payload, CancellationToken cancellationToken = default)
    {
        var tool = await toolRepository.GetByIdAsync(toolId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 工具：{toolId}");

        var connection = await connectionRepository.GetByIdAsync(tool.ConnectionId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 连接：{tool.ConnectionId}");

        var executionId = Guid.NewGuid();

        try
        {
            await using var session = await clientFactory.CreateAsync(ToConnectionDefinition(connection), cancellationToken);
            var arguments = ToArguments(payload);
            var result = await session.Client.CallToolAsync(tool.ToolName, arguments, cancellationToken: cancellationToken);
            var responseJson = JsonSerializer.Serialize(result);
            var errorMessage = ExtractToolError(result);
            var status = string.IsNullOrWhiteSpace(errorMessage) ? "Succeeded" : "Failed";

            await SaveExecutionAsync(executionId, connection.Id, tool.Id, payload, responseJson, status, errorMessage, cancellationToken);
            return new McpToolExecutionResult(executionId, status, responseJson, errorMessage);
        }
        catch (Exception ex)
        {
            await SaveExecutionAsync(executionId, connection.Id, tool.Id, payload, "{}", "Failed", ex.Message, cancellationToken);
            return new McpToolExecutionResult(executionId, "Failed", "{}", ex.Message);
        }
    }

    /// <summary>
    /// 将执行结果落库，方便后续前端查询历史。
    /// </summary>
    private async Task SaveExecutionAsync(
        Guid executionId,
        Guid connectionId,
        Guid toolId,
        JsonElement payload,
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
            RequestedBy = "frontend",
            RequestPayloadJson = payload.GetRawText(),
            ResponsePayloadJson = responseJson,
            Status = status,
            ErrorMessage = errorMessage,
            CreatedAt = DateTimeOffset.UtcNow
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 将轻量连接记录转换为跨层连接定义。
    /// </summary>
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

    /// <summary>
    /// 将原始 JSON 参数转换为 MCP 客户端可接受的参数字典。
    /// </summary>
    private static IReadOnlyDictionary<string, object?> ToArguments(JsonElement payload)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("工具参数必须是 JSON 对象。");
        }

        var result = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
        foreach (var property in payload.EnumerateObject())
        {
            result[property.Name] = ConvertJsonElement(property.Value);
        }

        return result;
    }

    /// <summary>
    /// 从 MCP 调用结果中提取错误文本。
    /// 如果没有显式错误，则返回 null。
    /// </summary>
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

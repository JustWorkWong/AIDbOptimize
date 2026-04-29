using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Security;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Collects a db config snapshot through read-only MCP tools when available.
/// </summary>
public sealed class DbConfigSnapshotCollectorExecutor(
    IMcpToolRepository toolRepository,
    IMcpToolExecutionService toolExecutionService)
{
    public async Task<DbConfigSnapshot> CollectAsync(
        McpConnectionEntity connection,
        Guid? workflowSessionId = null,
        string workflowNodeName = "DbConfigSnapshotCollectorExecutor",
        string requestedBy = "workflow",
        string? traceId = null,
        CancellationToken cancellationToken = default)
    {
        var tools = await toolRepository.ListByConnectionAsync(connection.Id, cancellationToken);
        var selectedTool = SelectTool(tools);

        if (selectedTool is null)
        {
            return CreateFallbackSnapshot(connection, "No allowed read-only tool was discovered, using metadata fallback.");
        }

        try
        {
            using var document = JsonDocument.Parse(BuildPayloadJson(connection.Engine, selectedTool.ToolName));
            var result = await toolExecutionService.ExecuteAsync(
                new McpToolExecutionRequest(
                    selectedTool.Id,
                    document.RootElement.Clone(),
                    workflowSessionId,
                    workflowNodeName,
                    requestedBy,
                    workflowSessionId.HasValue ? "workflow" : "manual",
                    traceId),
                cancellationToken);

            if (!string.Equals(result.Status, "Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                return CreateFallbackSnapshot(
                    connection,
                    $"Read-only tool {selectedTool.ToolName} failed, using metadata fallback.");
            }

            var values = ExtractValues(connection, selectedTool.ToolName, result.ResponsePayloadJson);
            return new DbConfigSnapshot(
                connection.Engine,
                connection.DatabaseName,
                $"mcp-tool:{selectedTool.ToolName}",
                values,
                Array.Empty<string>());
        }
        catch (Exception ex)
        {
            return CreateFallbackSnapshot(
                connection,
                $"Read-only tool collection failed, using metadata fallback. Reason: {SensitiveDataMasker.MaskFreeText(ex.Message)}");
        }
    }

    public DbConfigSnapshot Collect(McpConnectionEntity connection)
    {
        return CreateFallbackSnapshot(connection, "No allowed read-only tool was discovered, using metadata fallback.");
    }

    private static DbConfigSnapshot CreateFallbackSnapshot(McpConnectionEntity connection, string warning)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["engine"] = connection.Engine.ToString(),
            ["database_name"] = connection.DatabaseName,
            ["display_name"] = connection.DisplayName
        };

        if (connection.Engine == DatabaseEngine.MySql)
        {
            values["max_connections"] = "500";
            values["innodb_buffer_pool_size"] = "256MB";
        }
        else
        {
            values["shared_buffers"] = "128MB";
            values["work_mem"] = "4MB";
        }

        return new DbConfigSnapshot(
            connection.Engine,
            connection.DatabaseName,
            "metadata-fallback",
            values,
            [warning]);
    }

    private static McpToolRecord? SelectTool(IReadOnlyList<McpToolRecord> tools)
    {
        var candidates = tools
            .Where(tool => tool.IsEnabled && ToolAllowlistPolicy.IsAllowed(tool.ToolName, tool.IsWriteTool))
            .ToArray();

        string[] preferredOrder = ["get_config", "query", "execute_query", "describe_table", "show_indexes", "explain"];
        foreach (var toolName in preferredOrder)
        {
            var match = candidates.FirstOrDefault(tool => string.Equals(tool.ToolName, toolName, StringComparison.OrdinalIgnoreCase));
            if (match is not null)
            {
                return match;
            }
        }

        return null;
    }

    private static string BuildPayloadJson(DatabaseEngine engine, string toolName)
    {
        if (string.Equals(toolName, "query", StringComparison.OrdinalIgnoreCase))
        {
            return engine == DatabaseEngine.MySql
                ? """{"sql":"SELECT @@max_connections AS max_connections, @@innodb_buffer_pool_size AS innodb_buffer_pool_size"}"""
                : """{"sql":"SELECT current_setting('shared_buffers') AS shared_buffers, current_setting('work_mem') AS work_mem"}""";
        }

        if (string.Equals(toolName, "execute_query", StringComparison.OrdinalIgnoreCase))
        {
            return engine == DatabaseEngine.MySql
                ? """{"query":"SELECT @@max_connections AS max_connections, @@innodb_buffer_pool_size AS innodb_buffer_pool_size"}"""
                : """{"query":"SELECT current_setting('shared_buffers') AS shared_buffers, current_setting('work_mem') AS work_mem"}""";
        }

        return "{}";
    }

    private static IReadOnlyDictionary<string, string> ExtractValues(
        McpConnectionEntity connection,
        string toolName,
        string responsePayloadJson)
    {
        var values = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            ["engine"] = connection.Engine.ToString(),
            ["database_name"] = connection.DatabaseName,
            ["tool_name"] = toolName
        };

        try
        {
            using var document = JsonDocument.Parse(responsePayloadJson);
            if (TryExtractStructuredValues(document.RootElement, values))
            {
                return values;
            }

            if (document.RootElement.ValueKind == JsonValueKind.Object)
            {
                foreach (var property in document.RootElement.EnumerateObject())
                {
                    values[property.Name] = property.Value.ValueKind switch
                    {
                        JsonValueKind.String => property.Value.GetString() ?? string.Empty,
                        JsonValueKind.Number => property.Value.GetRawText(),
                        JsonValueKind.True => "true",
                        JsonValueKind.False => "false",
                        _ => property.Value.GetRawText()
                    };
                }
            }
            else
            {
                values["tool_response_json"] = responsePayloadJson;
            }
        }
        catch
        {
            values["tool_response_json"] = responsePayloadJson;
        }

        return values;
    }

    private static bool TryExtractStructuredValues(
        JsonElement root,
        IDictionary<string, string> values)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            return false;
        }

        if (!root.TryGetProperty("content", out var content) || content.ValueKind != JsonValueKind.Array)
        {
            return false;
        }

        foreach (var item in content.EnumerateArray())
        {
            if (item.ValueKind != JsonValueKind.Object)
            {
                continue;
            }

            if (!item.TryGetProperty("text", out var textElement) || textElement.ValueKind != JsonValueKind.String)
            {
                continue;
            }

            var text = textElement.GetString();
            if (string.IsNullOrWhiteSpace(text))
            {
                continue;
            }

            try
            {
                using var nested = JsonDocument.Parse(text);
                if (nested.RootElement.ValueKind == JsonValueKind.Array && nested.RootElement.GetArrayLength() > 0)
                {
                    var first = nested.RootElement[0];
                    if (first.ValueKind == JsonValueKind.Object)
                    {
                        foreach (var property in first.EnumerateObject())
                        {
                            values[property.Name] = property.Value.ValueKind switch
                            {
                                JsonValueKind.String => property.Value.GetString() ?? string.Empty,
                                JsonValueKind.Number => property.Value.GetRawText(),
                                JsonValueKind.True => "true",
                                JsonValueKind.False => "false",
                                _ => property.Value.GetRawText()
                            };
                        }

                        return true;
                    }
                }
            }
            catch
            {
                // Ignore non-JSON text blocks and fall back to the generic extraction path.
            }
        }

        return false;
    }
}

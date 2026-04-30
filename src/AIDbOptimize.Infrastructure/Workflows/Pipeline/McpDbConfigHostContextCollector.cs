using System.Data.Common;
using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Persistence.Entities;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Collects host-context data through a dedicated read-only MCP toolset when available.
/// </summary>
public sealed class McpDbConfigHostContextCollector(
    IMcpConnectionRepository connectionRepository,
    IMcpToolRepository toolRepository,
    IMcpToolExecutionService toolExecutionService) : IDbConfigHostContextCollector
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);
    private static readonly string[] RequiredHostKeys =
    [
        "memory_limit_bytes",
        "memory_available_bytes",
        "cpu_limit_cores",
        "disk_available_bytes"
    ];

    public async Task<DbConfigHostContextCollectionResult> CollectAsync(
        McpConnectionEntity connection,
        Guid? workflowSessionId,
        string workflowNodeName,
        string requestedBy,
        string? traceId,
        CancellationToken cancellationToken = default)
    {
        var connectionRecord = await connectionRepository.GetByIdAsync(connection.Id, cancellationToken);
        var hostToolset = await DiscoverHostToolsetAsync(cancellationToken);
        if (hostToolset is null)
        {
            return BuildUnavailable(
                "unknown",
                "unsupported",
                "No host-context MCP connection exposing read-only host tools was discovered.");
        }

        var resolveTargetResult = await ExecuteToolAsync(
            hostToolset.ResolveRuntimeTarget,
            new
            {
                connectionId = connection.Id,
                engine = connection.Engine.ToString(),
                displayName = connection.DisplayName,
                databaseName = connection.DatabaseName,
                host = TryReadConnectionStringValue(connectionRecord?.DatabaseConnectionString, "Host", "Server", "Data Source"),
                port = TryReadConnectionStringValue(connectionRecord?.DatabaseConnectionString, "Port"),
                metadata = new
                {
                    connectionName = connection.Name
                }
            },
            workflowSessionId,
            workflowNodeName,
            requestedBy,
            traceId,
            cancellationToken);

        if (resolveTargetResult is null || !string.Equals(resolveTargetResult.Status, "Succeeded", StringComparison.OrdinalIgnoreCase))
        {
            return BuildUnavailable(
                "unknown",
                "unresolved-target",
                $"resolve_runtime_target failed: {resolveTargetResult?.ErrorMessage ?? "unknown error"}");
        }

        var resolvedTarget = ParseToolPayload(resolveTargetResult.ResponsePayloadJson);
        var resourceScope = GetString(resolvedTarget, "resourceScope")
            ?? GetString(resolvedTarget, "resource_scope")
            ?? "unknown";
        var targetType = GetString(resolvedTarget, "targetType")
            ?? GetString(resolvedTarget, "target_type")
            ?? "unknown";
        var targetId = GetString(resolvedTarget, "targetId")
            ?? GetString(resolvedTarget, "target_id");
        var targetName = GetString(resolvedTarget, "targetName")
            ?? GetString(resolvedTarget, "target_name");

        var items = new List<DbConfigEvidenceItem>();
        var missing = new List<DbConfigMissingContextItem>();
        var warnings = new List<string>();
        var metadata = new List<DbConfigCollectionMetadataItem>
        {
            new("resource_scope", resourceScope, "Resolved runtime resource scope."),
            new("target_type", targetType, "Resolved runtime target type."),
            new("host_context_connection_id", hostToolset.Connection.Id.ToString(), "MCP connection used for host-context collection."),
            new("host_context_connection_name", hostToolset.Connection.Name, "MCP connection used for host-context collection.")
        };

        if (!string.IsNullOrWhiteSpace(targetId))
        {
            metadata.Add(new DbConfigCollectionMetadataItem("target_id", targetId, "Resolved runtime target id."));
        }

        if (!string.IsNullOrWhiteSpace(targetName))
        {
            metadata.Add(new DbConfigCollectionMetadataItem("target_name", targetName, "Resolved runtime target name."));
        }

        var toolInvocations = SelectToolInvocations(hostToolset, resourceScope, targetId, targetName);
        foreach (var invocation in toolInvocations)
        {
            var toolResult = await ExecuteToolAsync(
                invocation.Tool,
                invocation.Payload,
                workflowSessionId,
                workflowNodeName,
                requestedBy,
                traceId,
                cancellationToken);

            if (toolResult is null || !string.Equals(toolResult.Status, "Succeeded", StringComparison.OrdinalIgnoreCase))
            {
                warnings.Add($"{invocation.Tool.ToolName} failed: {toolResult?.ErrorMessage ?? "unknown error"}");
                continue;
            }

            var payload = ParseToolPayload(toolResult.ResponsePayloadJson);
            AddItems(items, payload, resourceScope, invocation.Tool.ToolName);
        }

        var classifiedMissingReason = ClassifyMissingReason(warnings);
        foreach (var key in RequiredHostKeys)
        {
            if (!items.Any(item => string.Equals(item.Reference, key, StringComparison.OrdinalIgnoreCase)))
            {
                missing.Add(new DbConfigMissingContextItem(
                    key,
                    $"Host-context field `{key}` was not collected.",
                    classifiedMissingReason,
                    resourceScope));
            }
        }

        if (items.Count == 0)
        {
            return BuildUnavailable(
                resourceScope,
                "unsupported",
                "Host-context MCP connection was discovered, but no usable data was returned.");
        }

        return new DbConfigHostContextCollectionResult(resourceScope, items, missing, metadata, warnings);
    }

    private async Task<HostContextToolset?> DiscoverHostToolsetAsync(CancellationToken cancellationToken)
    {
        var connections = await connectionRepository.GetAllAsync(cancellationToken);
        HostContextToolset? best = null;

        foreach (var connection in connections)
        {
            var tools = await toolRepository.ListByConnectionAsync(connection.Id, cancellationToken);
            var enabledTools = tools
                .Where(tool => tool.IsEnabled && HostToolNames.Contains(tool.ToolName))
                .ToArray();

            if (enabledTools.Length == 0)
            {
                continue;
            }

            var resolve = enabledTools.FirstOrDefault(tool => string.Equals(tool.ToolName, "resolve_runtime_target", StringComparison.OrdinalIgnoreCase));
            if (resolve is null)
            {
                continue;
            }

            var score = enabledTools.Length;
            if (connection.Name.Contains("host", StringComparison.OrdinalIgnoreCase) ||
                connection.DisplayName.Contains("host", StringComparison.OrdinalIgnoreCase))
            {
                score += 10;
            }

            if (best is null || score > best.Score)
            {
                best = new HostContextToolset(connection, enabledTools, resolve, score);
            }
        }

        return best;
    }

    private async Task<McpToolExecutionResult?> ExecuteToolAsync(
        McpToolRecord tool,
        object payload,
        Guid? workflowSessionId,
        string workflowNodeName,
        string requestedBy,
        string? traceId,
        CancellationToken cancellationToken)
    {
        using var document = JsonDocument.Parse(JsonSerializer.Serialize(payload, SerializerOptions));
        return await toolExecutionService.ExecuteAsync(
            new McpToolExecutionRequest(
                tool.Id,
                document.RootElement.Clone(),
                workflowSessionId,
                workflowNodeName,
                requestedBy,
                workflowSessionId.HasValue ? "workflow" : "manual",
                traceId),
            cancellationToken);
    }

    private static IReadOnlyList<HostToolInvocation> SelectToolInvocations(
        HostContextToolset toolset,
        string resourceScope,
        string? targetId,
        string? targetName)
    {
        var invocations = new List<HostToolInvocation>();

        if (string.Equals(resourceScope, "container", StringComparison.OrdinalIgnoreCase))
        {
            TryAdd(invocations, toolset, "get_container_limits", new { targetId, targetName });
            TryAdd(invocations, toolset, "get_container_stats", new { targetId, targetName });
            TryAdd(invocations, toolset, "get_disk_usage", new { targetId, targetName, mode = "data-directory" });
        }
        else if (string.Equals(resourceScope, "managed-service", StringComparison.OrdinalIgnoreCase))
        {
            TryAdd(invocations, toolset, "get_managed_service_profile", new { targetId, targetName });
        }
        else
        {
            TryAdd(invocations, toolset, "get_host_memory", new { targetId, targetName });
            TryAdd(invocations, toolset, "get_host_cpu", new { targetId, targetName });
            TryAdd(invocations, toolset, "get_disk_usage", new { targetId, targetName, mode = "data-directory" });
        }

        TryAdd(invocations, toolset, "get_process_limits", new { targetId, targetName });
        return invocations;
    }

    private static void TryAdd(List<HostToolInvocation> invocations, HostContextToolset toolset, string toolName, object payload)
    {
        var tool = toolset.Tools.FirstOrDefault(candidate => string.Equals(candidate.ToolName, toolName, StringComparison.OrdinalIgnoreCase));
        if (tool is not null)
        {
            invocations.Add(new HostToolInvocation(tool, payload));
        }
    }

    private static void AddItems(
        List<DbConfigEvidenceItem> items,
        JsonElement payload,
        string resourceScope,
        string collectionMethod)
    {
        if (payload.ValueKind != JsonValueKind.Object)
        {
            return;
        }

        var capturedAt = DateTimeOffset.UtcNow;
        foreach (var property in payload.EnumerateObject())
        {
            if (property.Value.ValueKind is JsonValueKind.Object or JsonValueKind.Array)
            {
                continue;
            }

            var normalized = property.Value.ValueKind switch
            {
                JsonValueKind.String => property.Value.GetString(),
                JsonValueKind.Number => property.Value.GetRawText(),
                JsonValueKind.True => "true",
                JsonValueKind.False => "false",
                _ => property.Value.GetRawText()
            };

            if (string.IsNullOrWhiteSpace(normalized))
            {
                continue;
            }

            items.Add(new DbConfigEvidenceItem(
                collectionMethod,
                NormalizeReference(property.Name),
                $"Collected host-context field {property.Name}={normalized}",
                Category: "hostContext",
                RawValue: normalized,
                NormalizedValue: normalized,
                Unit: InferUnit(property.Name),
                SourceScope: resourceScope,
                CapturedAt: capturedAt,
                IsCached: false,
                CollectionMethod: collectionMethod));
        }
    }

    private static DbConfigHostContextCollectionResult BuildUnavailable(
        string resourceScope,
        string reason,
        string description)
    {
        return new DbConfigHostContextCollectionResult(
            resourceScope,
            [],
            RequiredHostKeys.Select(key => new DbConfigMissingContextItem(
                key,
                $"Host-context field `{key}` was not collected.",
                reason,
                resourceScope)).ToArray(),
            [
                new DbConfigCollectionMetadataItem("resource_scope", resourceScope, "Resolved runtime resource scope."),
                new DbConfigCollectionMetadataItem("host_context_status", "unavailable", description)
            ],
            []);
    }

    private static string? TryReadConnectionStringValue(string? connectionString, params string[] keys)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
        {
            return null;
        }

        try
        {
            var builder = new DbConnectionStringBuilder
            {
                ConnectionString = connectionString
            };

            foreach (var key in keys)
            {
                if (builder.TryGetValue(key, out var value))
                {
                    return Convert.ToString(value);
                }
            }
        }
        catch
        {
            return null;
        }

        return null;
    }

    private static JsonElement ParseToolPayload(string responsePayloadJson)
    {
        try
        {
            using var document = JsonDocument.Parse(responsePayloadJson);
            if (TryUnwrapStructuredContent(document.RootElement, out var structured))
            {
                return structured;
            }

            return document.RootElement.Clone();
        }
        catch
        {
            using var fallback = JsonDocument.Parse("{}");
            return fallback.RootElement.Clone();
        }
    }

    private static bool TryUnwrapStructuredContent(JsonElement root, out JsonElement result)
    {
        if (root.ValueKind == JsonValueKind.Object &&
            root.TryGetProperty("structuredContent", out var structuredContent))
        {
            result = ExtractFirstObject(structuredContent);
            return true;
        }

        if (root.ValueKind == JsonValueKind.Object &&
            root.TryGetProperty("content", out var content) &&
            content.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in content.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object ||
                    !item.TryGetProperty("text", out var text) ||
                    text.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                try
                {
                    using var nested = JsonDocument.Parse(text.GetString() ?? "{}");
                    result = ExtractFirstObject(nested.RootElement);
                    return true;
                }
                catch
                {
                    // ignore invalid nested json
                }
            }
        }

        result = root.Clone();
        return false;
    }

    private static JsonElement ExtractFirstObject(JsonElement root)
    {
        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
        {
            return root[0].Clone();
        }

        return root.Clone();
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (element.ValueKind != JsonValueKind.Object || !element.TryGetProperty(propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString(),
            JsonValueKind.Number => property.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => null
        };
    }

    private static string NormalizeReference(string name)
    {
        return name switch
        {
            "memoryLimitBytes" => "memory_limit_bytes",
            "memoryTotalBytes" => "memory_total_bytes",
            "memoryAvailableBytes" => "memory_available_bytes",
            "memoryUsageBytes" => "memory_usage_bytes",
            "cpuLimitCores" => "cpu_limit_cores",
            "cpuTotalCores" => "cpu_total_cores",
            "cpuUsagePercent" => "cpu_usage_percent",
            "diskTotalBytes" => "disk_total_bytes",
            "diskAvailableBytes" => "disk_available_bytes",
            _ => name
        };
    }

    private static string? InferUnit(string name)
    {
        if (name.Contains("Bytes", StringComparison.OrdinalIgnoreCase) || name.Contains("_bytes", StringComparison.OrdinalIgnoreCase))
        {
            return "bytes";
        }

        if (name.Contains("Percent", StringComparison.OrdinalIgnoreCase) || name.Contains("_percent", StringComparison.OrdinalIgnoreCase))
        {
            return "percent";
        }

        if (name.Contains("Cores", StringComparison.OrdinalIgnoreCase) || name.Contains("_cores", StringComparison.OrdinalIgnoreCase))
        {
            return "cores";
        }

        return null;
    }

    private static string ClassifyMissingReason(IReadOnlyCollection<string> warnings)
    {
        if (warnings.Any(warning => warning.Contains("timeout", StringComparison.OrdinalIgnoreCase)))
        {
            return "timeout";
        }

        if (warnings.Any(warning =>
                warning.Contains("permission", StringComparison.OrdinalIgnoreCase) ||
                warning.Contains("forbidden", StringComparison.OrdinalIgnoreCase) ||
                warning.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
                warning.Contains("denied", StringComparison.OrdinalIgnoreCase)))
        {
            return "permission";
        }

        return "unsupported";
    }

    private static readonly HashSet<string> HostToolNames = new(StringComparer.OrdinalIgnoreCase)
    {
        "resolve_runtime_target",
        "get_container_limits",
        "get_container_stats",
        "get_disk_usage",
        "get_host_memory",
        "get_host_cpu",
        "get_process_limits",
        "get_managed_service_profile"
    };

    private sealed record HostContextToolset(
        McpConnectionRecord Connection,
        IReadOnlyList<McpToolRecord> Tools,
        McpToolRecord ResolveRuntimeTarget,
        int Score);

    private sealed record HostToolInvocation(
        McpToolRecord Tool,
        object Payload);
}

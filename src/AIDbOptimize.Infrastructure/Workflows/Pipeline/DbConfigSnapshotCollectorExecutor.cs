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
    IMcpToolExecutionService toolExecutionService,
    IEnumerable<IDbConfigHostContextCollector> hostContextCollectors)
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
        var hostContextCollector = hostContextCollectors.FirstOrDefault();

        if (selectedTool is null)
        {
            return CreateFallbackSnapshot(
                connection,
                "No allowed read-only tool was discovered, using metadata fallback.",
                BuildUnavailableHostContext("No host-context collector is configured."));
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
                    $"Read-only tool {selectedTool.ToolName} failed, using metadata fallback.",
                    BuildUnavailableHostContext("Database snapshot collection failed before host-context collection."));
            }

            var values = ExtractValues(connection, selectedTool.ToolName, result.ResponsePayloadJson);
            var extractionMetadata = ExtractCollectionMetadata(selectedTool.ToolName, result.ResponsePayloadJson);
            var hostContext = hostContextCollector is null
                ? BuildUnavailableHostContext("No host-context collector is configured.")
                : await hostContextCollector.CollectAsync(
                    connection,
                    workflowSessionId,
                    workflowNodeName,
                    requestedBy,
                    traceId,
                    cancellationToken);
            return BuildSnapshot(
                connection,
                $"mcp-tool:{selectedTool.ToolName}",
                values,
                Array.Empty<string>(),
                hostContext,
                selectedTool.ToolName,
                extractionMetadata);
        }
        catch (Exception ex)
        {
            return CreateFallbackSnapshot(
                connection,
                $"Read-only tool collection failed, using metadata fallback. Reason: {SensitiveDataMasker.MaskFreeText(ex.Message)}",
                BuildUnavailableHostContext("Database snapshot collection threw before host-context collection."));
        }
    }

    public DbConfigSnapshot Collect(McpConnectionEntity connection)
    {
        return CreateFallbackSnapshot(
            connection,
            "No allowed read-only tool was discovered, using metadata fallback.",
            BuildUnavailableHostContext("No host-context collector is configured."));
    }

    private static DbConfigSnapshot CreateFallbackSnapshot(
        McpConnectionEntity connection,
        string warning,
        DbConfigHostContextCollectionResult hostContext)
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

        return BuildSnapshot(connection, "metadata-fallback", values, [warning], hostContext, "metadata-fallback");
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
                ? """{"sql":"SELECT @@max_connections AS max_connections, @@innodb_buffer_pool_size AS innodb_buffer_pool_size, @@thread_cache_size AS thread_cache_size, @@tmp_table_size AS tmp_table_size, @@max_heap_table_size AS max_heap_table_size, @@slow_query_log AS slow_query_log, @@long_query_time AS long_query_time, @@performance_schema AS performance_schema_enabled, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Threads_connected') AS threads_connected, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Threads_running') AS threads_running, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Slow_queries') AS slow_queries, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Connections') AS connections, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Aborted_connects') AS aborted_connects, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Innodb_buffer_pool_reads') AS innodb_buffer_pool_reads, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Innodb_buffer_pool_read_requests') AS innodb_buffer_pool_read_requests, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Created_tmp_disk_tables') AS created_tmp_disk_tables, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Created_tmp_tables') AS created_tmp_tables"}"""
                : """{"sql":"SELECT current_setting('shared_buffers') AS shared_buffers, current_setting('work_mem') AS work_mem, current_setting('maintenance_work_mem') AS maintenance_work_mem, current_setting('effective_cache_size') AS effective_cache_size, current_setting('max_connections') AS max_connections, current_setting('checkpoint_timeout') AS checkpoint_timeout, current_setting('checkpoint_completion_target') AS checkpoint_completion_target, current_setting('random_page_cost') AS random_page_cost, current_setting('seq_page_cost') AS seq_page_cost, current_setting('track_io_timing', true) AS track_io_timing, current_setting('shared_preload_libraries', true) AS shared_preload_libraries, stats.blks_hit AS blks_hit, stats.blks_read AS blks_read, stats.temp_files AS temp_files, stats.deadlocks AS deadlocks, bgw.checkpoints_timed AS checkpoints_timed, bgw.checkpoints_req AS checkpoints_req, EXISTS (SELECT 1 FROM pg_extension WHERE extname='pg_stat_statements') AS pg_stat_statements_enabled FROM pg_stat_database AS stats CROSS JOIN pg_stat_bgwriter AS bgw WHERE stats.datname = current_database() LIMIT 1"}""";
        }

        if (string.Equals(toolName, "execute_query", StringComparison.OrdinalIgnoreCase))
        {
            return engine == DatabaseEngine.MySql
                ? """{"query":"SELECT @@max_connections AS max_connections, @@innodb_buffer_pool_size AS innodb_buffer_pool_size, @@thread_cache_size AS thread_cache_size, @@tmp_table_size AS tmp_table_size, @@max_heap_table_size AS max_heap_table_size, @@slow_query_log AS slow_query_log, @@long_query_time AS long_query_time, @@performance_schema AS performance_schema_enabled, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Threads_connected') AS threads_connected, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Threads_running') AS threads_running, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Slow_queries') AS slow_queries, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Connections') AS connections, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Aborted_connects') AS aborted_connects, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Innodb_buffer_pool_reads') AS innodb_buffer_pool_reads, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Innodb_buffer_pool_read_requests') AS innodb_buffer_pool_read_requests, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Created_tmp_disk_tables') AS created_tmp_disk_tables, (SELECT VARIABLE_VALUE FROM performance_schema.global_status WHERE VARIABLE_NAME='Created_tmp_tables') AS created_tmp_tables"}"""
                : """{"query":"SELECT current_setting('shared_buffers') AS shared_buffers, current_setting('work_mem') AS work_mem, current_setting('maintenance_work_mem') AS maintenance_work_mem, current_setting('effective_cache_size') AS effective_cache_size, current_setting('max_connections') AS max_connections, current_setting('checkpoint_timeout') AS checkpoint_timeout, current_setting('checkpoint_completion_target') AS checkpoint_completion_target, current_setting('random_page_cost') AS random_page_cost, current_setting('seq_page_cost') AS seq_page_cost, current_setting('track_io_timing', true) AS track_io_timing, current_setting('shared_preload_libraries', true) AS shared_preload_libraries, stats.blks_hit AS blks_hit, stats.blks_read AS blks_read, stats.temp_files AS temp_files, stats.deadlocks AS deadlocks, bgw.checkpoints_timed AS checkpoints_timed, bgw.checkpoints_req AS checkpoints_req, EXISTS (SELECT 1 FROM pg_extension WHERE extname='pg_stat_statements') AS pg_stat_statements_enabled FROM pg_stat_database AS stats CROSS JOIN pg_stat_bgwriter AS bgw WHERE stats.datname = current_database() LIMIT 1"}""";
        }

        return "{}";
    }

    private static DbConfigSnapshot BuildSnapshot(
        McpConnectionEntity connection,
        string source,
        IReadOnlyDictionary<string, string> values,
        IReadOnlyList<string> warnings,
        DbConfigHostContextCollectionResult hostContext,
        string collectionMethod,
        IReadOnlyList<DbConfigCollectionMetadataItem>? extractionMetadata = null)
    {
        var capturedAt = DateTimeOffset.UtcNow;
        var configurationItems = new List<DbConfigEvidenceItem>();
        var runtimeItems = new List<DbConfigEvidenceItem>();
        var observabilityItems = new List<DbConfigEvidenceItem>();

        foreach (var pair in values)
        {
            if (IsMetadataKey(pair.Key))
            {
                continue;
            }

            if (TryClassifyKey(connection.Engine, pair.Key, out var category))
            {
                var item = new DbConfigEvidenceItem(
                    source,
                    pair.Key,
                    BuildDescription(category, pair.Key, pair.Value),
                    category,
                    pair.Value,
                    pair.Value,
                    InferUnit(pair.Key),
                    "db",
                    capturedAt,
                    null,
                    false,
                    collectionMethod);

                switch (category)
                {
                    case "runtimeMetric":
                        runtimeItems.Add(item);
                        break;
                    case "observability":
                        observabilityItems.Add(item);
                        break;
                    default:
                        configurationItems.Add(item);
                        break;
                }
            }
        }

        var metadata = new List<DbConfigCollectionMetadataItem>
        {
            new("source", source, "Snapshot source label."),
            new("collection_method", collectionMethod, "Primary read-only collection method.")
        };
        if (extractionMetadata is not null)
        {
            metadata.AddRange(extractionMetadata);
        }
        metadata.AddRange(hostContext.CollectionMetadata);

        return new DbConfigSnapshot(
            connection.Engine,
            connection.DatabaseName,
            source,
            values,
            warnings.Concat(hostContext.Warnings).ToArray(),
            configurationItems,
            runtimeItems,
            hostContext.Items,
            observabilityItems,
            hostContext.MissingContextItems,
            metadata);
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
                if (TryExtractNestedObjectValues(document.RootElement, values))
                {
                    return values;
                }

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

    private static IReadOnlyList<DbConfigCollectionMetadataItem> ExtractCollectionMetadata(
        string toolName,
        string responsePayloadJson)
    {
        try
        {
            using var document = JsonDocument.Parse(responsePayloadJson);
            if (TryExtractTableSummary(document.RootElement, out var summary))
            {
                return
                [
                    new DbConfigCollectionMetadataItem("topn_source_tool", toolName, "Tool that produced the summarized tabular result."),
                    new DbConfigCollectionMetadataItem("topn_row_count", summary.RowCount.ToString(), "Total row count observed in the source result."),
                    new DbConfigCollectionMetadataItem("topn_summary_json", summary.SummaryJson, "TopN summary for tabular tool output.")
                ];
            }
        }
        catch
        {
            // ignore malformed payloads and continue without metadata
        }

        return [];
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
            if (root.TryGetProperty("structuredContent", out var structuredContent))
            {
                return TryExtractFirstObject(structuredContent, values);
            }

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
                if (TryExtractFirstObject(nested.RootElement, values))
                {
                    return true;
                }
            }
            catch
            {
                // Ignore non-JSON text blocks and fall back to the generic extraction path.
            }
        }

        return false;
    }

    private static bool TryExtractTableSummary(JsonElement root, out DbConfigTableSummary summary)
    {
        if (root.ValueKind == JsonValueKind.Object && root.TryGetProperty("structuredContent", out var structuredContent))
        {
            return TryBuildTableSummary(structuredContent, out summary);
        }

        if (root.ValueKind == JsonValueKind.Object &&
            root.TryGetProperty("content", out var content) &&
            content.ValueKind == JsonValueKind.Array)
        {
            foreach (var item in content.EnumerateArray())
            {
                if (item.ValueKind != JsonValueKind.Object ||
                    !item.TryGetProperty("text", out var textElement) ||
                    textElement.ValueKind != JsonValueKind.String)
                {
                    continue;
                }

                try
                {
                    using var nested = JsonDocument.Parse(textElement.GetString() ?? "[]");
                    if (TryBuildTableSummary(nested.RootElement, out summary))
                    {
                        return true;
                    }
                }
                catch
                {
                    // ignore invalid nested json
                }
            }
        }

        summary = default;
        return false;
    }

    private static bool TryBuildTableSummary(JsonElement root, out DbConfigTableSummary summary)
    {
        if (root.ValueKind != JsonValueKind.Array || root.GetArrayLength() <= 1)
        {
            summary = default;
            return false;
        }

        var top = root.EnumerateArray()
            .Take(3)
            .Select(static item => item.Clone())
            .ToArray();
        summary = new DbConfigTableSummary(
            root.GetArrayLength(),
            JsonSerializer.Serialize(top));
        return true;
    }

    private static bool TryExtractNestedObjectValues(JsonElement root, IDictionary<string, string> values)
    {
        if (!root.TryGetProperty("structuredContent", out var structuredContent))
        {
            return false;
        }

        return TryExtractFirstObject(structuredContent, values);
    }

    private static bool TryExtractFirstObject(JsonElement root, IDictionary<string, string> values)
    {
        if (root.ValueKind == JsonValueKind.Array && root.GetArrayLength() > 0)
        {
            var first = root[0];
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

        if (root.ValueKind == JsonValueKind.Object)
        {
            foreach (var property in root.EnumerateObject())
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

        return false;
    }

    private static DbConfigHostContextCollectionResult BuildUnavailableHostContext(string reason)
    {
        return new DbConfigHostContextCollectionResult(
            "unknown",
            [],
            [
                new DbConfigMissingContextItem("memory_limit_bytes", "实例可用内存上限缺失。", reason),
                new DbConfigMissingContextItem("memory_available_bytes", "当前可用内存缺失。", reason),
                new DbConfigMissingContextItem("cpu_limit_cores", "实例 CPU 限额缺失。", reason),
                new DbConfigMissingContextItem("disk_available_bytes", "数据目录可用磁盘容量缺失。", reason)
            ],
            [
                new DbConfigCollectionMetadataItem("resource_scope", "unknown", "Host context is not currently available."),
                new DbConfigCollectionMetadataItem("host_context_status", "unavailable", reason)
            ],
            []);
    }

    private static bool TryClassifyKey(DatabaseEngine engine, string key, out string category)
    {
        if (ObservabilityKeys.Contains(key))
        {
            category = "observability";
            return true;
        }

        if (engine == DatabaseEngine.MySql ? MySqlRuntimeKeys.Contains(key) : PostgreSqlRuntimeKeys.Contains(key))
        {
            category = "runtimeMetric";
            return true;
        }

        if (engine == DatabaseEngine.MySql ? MySqlConfigurationKeys.Contains(key) : PostgreSqlConfigurationKeys.Contains(key))
        {
            category = "configuration";
            return true;
        }

        category = string.Empty;
        return false;
    }

    private static bool IsMetadataKey(string key)
    {
        return string.Equals(key, "engine", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "database_name", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "tool_name", StringComparison.OrdinalIgnoreCase)
            || string.Equals(key, "display_name", StringComparison.OrdinalIgnoreCase);
    }

    private static string BuildDescription(string category, string key, string value)
    {
        return category switch
        {
            "runtimeMetric" => $"采集到运行指标 {key}={value}",
            "observability" => $"采集到观测能力字段 {key}={value}",
            _ => $"采集到配置项 {key}={value}"
        };
    }

    private static string? InferUnit(string key)
    {
        if (key.Contains("size", StringComparison.OrdinalIgnoreCase) ||
            key.Contains("buffers", StringComparison.OrdinalIgnoreCase) ||
            key.Contains("memory", StringComparison.OrdinalIgnoreCase))
        {
            return "bytes-or-engine-unit";
        }

        if (key.Contains("time", StringComparison.OrdinalIgnoreCase) || key.Contains("timeout", StringComparison.OrdinalIgnoreCase))
        {
            return "seconds-or-engine-unit";
        }

        return null;
    }

    private static readonly HashSet<string> MySqlConfigurationKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "max_connections",
        "innodb_buffer_pool_size",
        "thread_cache_size",
        "tmp_table_size",
        "max_heap_table_size",
        "slow_query_log",
        "long_query_time"
    };

    private static readonly HashSet<string> MySqlRuntimeKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "threads_connected",
        "threads_running",
        "slow_queries",
        "connections",
        "aborted_connects",
        "innodb_buffer_pool_reads",
        "innodb_buffer_pool_read_requests",
        "created_tmp_disk_tables",
        "created_tmp_tables"
    };

    private static readonly HashSet<string> PostgreSqlConfigurationKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "shared_buffers",
        "work_mem",
        "maintenance_work_mem",
        "effective_cache_size",
        "max_connections",
        "checkpoint_timeout",
        "checkpoint_completion_target",
        "random_page_cost",
        "seq_page_cost"
    };

    private static readonly HashSet<string> PostgreSqlRuntimeKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "blks_hit",
        "blks_read",
        "temp_files",
        "deadlocks",
        "checkpoints_timed",
        "checkpoints_req"
    };

    private static readonly HashSet<string> ObservabilityKeys = new(StringComparer.OrdinalIgnoreCase)
    {
        "performance_schema_enabled",
        "pg_stat_statements_enabled",
        "track_io_timing",
        "shared_preload_libraries"
    };

    private readonly record struct DbConfigTableSummary(
        int RowCount,
        string SummaryJson);
}

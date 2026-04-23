using System.Data.Common;
using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;
using Microsoft.EntityFrameworkCore;
using MySqlConnector;
using Npgsql;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 工具执行服务。
/// 当前阶段先支持一组最小的只读数据库工具，便于前端执行链路跑通。
/// </summary>
public sealed class McpToolExecutionService(
    IMcpToolRepository toolRepository,
    IMcpConnectionRepository connectionRepository,
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
            var responseJson = await ExecuteInternalAsync(connection, tool, payload, cancellationToken);
            await SaveExecutionAsync(executionId, connection.Id, tool.Id, payload, responseJson, "Succeeded", null, cancellationToken);
            return new McpToolExecutionResult(executionId, "Succeeded", responseJson, null);
        }
        catch (Exception ex)
        {
            await SaveExecutionAsync(executionId, connection.Id, tool.Id, payload, "{}", "Failed", ex.Message, cancellationToken);
            return new McpToolExecutionResult(executionId, "Failed", "{}", ex.Message);
        }
    }

    /// <summary>
    /// 根据工具名称分发到对应的只读数据库操作。
    /// </summary>
    private async Task<string> ExecuteInternalAsync(
        McpConnectionRecord connection,
        McpToolRecord tool,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        return tool.ToolName switch
        {
            "query" => await ExecuteQueryAsync(connection, payload, cancellationToken),
            "describe_table" => await ExecuteDescribeTableAsync(connection, payload, cancellationToken),
            "show_indexes" => await ExecuteShowIndexesAsync(connection, payload, cancellationToken),
            "get_config" => await ExecuteGetConfigAsync(connection, cancellationToken),
            _ => throw new NotSupportedException($"当前版本暂不支持工具：{tool.ToolName}")
        };
    }

    /// <summary>
    /// 执行只读查询工具。
    /// </summary>
    private async Task<string> ExecuteQueryAsync(
        McpConnectionRecord connection,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var sql = GetRequiredString(payload, "sql");
        EnsureReadOnlySql(sql);
        return await ExecuteReaderAsync(connection, sql, null, cancellationToken);
    }

    /// <summary>
    /// 执行表结构读取工具。
    /// </summary>
    private async Task<string> ExecuteDescribeTableAsync(
        McpConnectionRecord connection,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var tableName = GetRequiredString(payload, "tableName");

        return connection.Engine switch
        {
            DatabaseEngine.PostgreSql => await ExecuteReaderAsync(
                connection,
                """
                SELECT column_name, data_type, is_nullable, column_default
                FROM information_schema.columns
                WHERE table_schema = current_schema()
                  AND table_name = @tableName
                ORDER BY ordinal_position;
                """,
                command => command.Parameters.Add(new NpgsqlParameter("@tableName", tableName)),
                cancellationToken),
            DatabaseEngine.MySql => await ExecuteReaderAsync(
                connection,
                """
                SELECT COLUMN_NAME, COLUMN_TYPE, IS_NULLABLE, COLUMN_DEFAULT
                FROM INFORMATION_SCHEMA.COLUMNS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = @tableName
                ORDER BY ORDINAL_POSITION;
                """,
                command => command.Parameters.Add(new MySqlParameter("@tableName", tableName)),
                cancellationToken),
            _ => throw new NotSupportedException($"不支持的数据库引擎：{connection.Engine}")
        };
    }

    /// <summary>
    /// 执行索引读取工具。
    /// </summary>
    private async Task<string> ExecuteShowIndexesAsync(
        McpConnectionRecord connection,
        JsonElement payload,
        CancellationToken cancellationToken)
    {
        var tableName = GetRequiredString(payload, "tableName");

        return connection.Engine switch
        {
            DatabaseEngine.PostgreSql => await ExecuteReaderAsync(
                connection,
                """
                SELECT indexname, indexdef
                FROM pg_indexes
                WHERE schemaname = current_schema()
                  AND tablename = @tableName
                ORDER BY indexname;
                """,
                command => command.Parameters.Add(new NpgsqlParameter("@tableName", tableName)),
                cancellationToken),
            DatabaseEngine.MySql => await ExecuteReaderAsync(
                connection,
                """
                SELECT INDEX_NAME, COLUMN_NAME, NON_UNIQUE, INDEX_TYPE, SEQ_IN_INDEX
                FROM INFORMATION_SCHEMA.STATISTICS
                WHERE TABLE_SCHEMA = DATABASE()
                  AND TABLE_NAME = @tableName
                ORDER BY INDEX_NAME, SEQ_IN_INDEX;
                """,
                command => command.Parameters.Add(new MySqlParameter("@tableName", tableName)),
                cancellationToken),
            _ => throw new NotSupportedException($"不支持的数据库引擎：{connection.Engine}")
        };
    }

    /// <summary>
    /// 执行数据库配置读取工具。
    /// </summary>
    private async Task<string> ExecuteGetConfigAsync(
        McpConnectionRecord connection,
        CancellationToken cancellationToken)
    {
        return connection.Engine switch
        {
            DatabaseEngine.PostgreSql => await ExecuteReaderAsync(
                connection,
                """
                SELECT name, setting, unit, short_desc
                FROM pg_settings
                ORDER BY name
                LIMIT 100;
                """,
                null,
                cancellationToken),
            DatabaseEngine.MySql => await ExecuteReaderAsync(
                connection,
                "SHOW VARIABLES;",
                null,
                cancellationToken),
            _ => throw new NotSupportedException($"不支持的数据库引擎：{connection.Engine}")
        };
    }

    /// <summary>
    /// 执行查询并以 JSON 数组形式返回结果。
    /// </summary>
    private async Task<string> ExecuteReaderAsync(
        McpConnectionRecord connection,
        string sql,
        Action<DbCommand>? configureCommand,
        CancellationToken cancellationToken)
    {
        await using var dbConnection = CreateConnection(connection);
        await dbConnection.OpenAsync(cancellationToken);
        await using var command = dbConnection.CreateCommand();
        command.CommandText = sql;
        command.CommandTimeout = 30;
        configureCommand?.Invoke(command);

        await using var reader = await command.ExecuteReaderAsync(cancellationToken);
        var rows = new List<Dictionary<string, object?>>();

        while (await reader.ReadAsync(cancellationToken))
        {
            var row = new Dictionary<string, object?>(StringComparer.OrdinalIgnoreCase);
            for (var index = 0; index < reader.FieldCount; index++)
            {
                row[reader.GetName(index)] = await reader.IsDBNullAsync(index, cancellationToken)
                    ? null
                    : reader.GetValue(index);
            }

            rows.Add(row);
        }

        return JsonSerializer.Serialize(rows);
    }

    /// <summary>
    /// 创建数据库连接对象。
    /// </summary>
    private static DbConnection CreateConnection(McpConnectionRecord connection)
    {
        return connection.Engine switch
        {
            DatabaseEngine.PostgreSql => new NpgsqlConnection(connection.DatabaseConnectionString),
            DatabaseEngine.MySql => new MySqlConnection(connection.DatabaseConnectionString),
            _ => throw new NotSupportedException($"不支持的数据库引擎：{connection.Engine}")
        };
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
    /// 读取必填字符串参数。
    /// </summary>
    private static string GetRequiredString(JsonElement payload, string name)
    {
        if (!payload.TryGetProperty(name, out var property) || property.ValueKind != JsonValueKind.String)
        {
            throw new InvalidOperationException($"缺少必填参数：{name}");
        }

        var value = property.GetString();
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"参数 {name} 不能为空。");
        }

        return value;
    }

    /// <summary>
    /// 只允许执行只读 SQL，避免当前阶段误伤数据库内容。
    /// </summary>
    private static void EnsureReadOnlySql(string sql)
    {
        var trimmed = sql.TrimStart();
        if (trimmed.StartsWith("SELECT", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("WITH", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("SHOW", StringComparison.OrdinalIgnoreCase)
            || trimmed.StartsWith("EXPLAIN", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        throw new InvalidOperationException("当前版本只允许执行只读 SQL。");
    }
}

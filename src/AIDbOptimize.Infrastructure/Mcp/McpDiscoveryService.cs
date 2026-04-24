using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;

namespace AIDbOptimize.Infrastructure.Mcp;

/// <summary>
/// MCP 工具发现服务。
/// 当前版本仍是最小可运行骨架，返回一组固定数据库工具定义。
/// 后续应替换为真实 MCP server 的 `tools/list` 调用。
/// </summary>
public sealed class McpDiscoveryService : IMcpDiscoveryService
{
    /// <summary>
    /// 返回当前连接下可用的工具列表。
    /// 当前实现不启动真实 MCP 进程，只提供演示级工具集合。
    /// </summary>
    public Task<IReadOnlyCollection<McpToolDefinition>> DiscoverToolsAsync(
        McpConnectionDefinition connection,
        CancellationToken cancellationToken = default)
    {
        IReadOnlyCollection<McpToolDefinition> tools =
        [
            CreateTool(connection, "query", "query", "执行查询语句", isWriteTool: false),
            CreateTool(connection, "describe_table", "describe_table", "读取表结构", isWriteTool: false),
            CreateTool(connection, "show_indexes", "show_indexes", "读取索引信息", isWriteTool: false),
            CreateTool(connection, "get_config", "get_config", "读取数据库配置", isWriteTool: false)
        ];

        return Task.FromResult(tools);
    }

    private static McpToolDefinition CreateTool(
        McpConnectionDefinition connection,
        string toolName,
        string displayName,
        string description,
        bool isWriteTool)
    {
        return new McpToolDefinition(
            Guid.NewGuid(),
            connection.Id,
            toolName,
            displayName,
            description,
            """{"type":"object","properties":{}}""",
            ToolApprovalMode.NoApproval,
            IsEnabled: true,
            IsWriteTool: isWriteTool);
    }
}

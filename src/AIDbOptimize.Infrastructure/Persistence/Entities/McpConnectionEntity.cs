using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// MCP 连接配置实体。
/// </summary>
public sealed class McpConnectionEntity
{
    /// <summary>
    /// 连接主键。
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 连接内部名称。
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库引擎类型。
    /// </summary>
    public DatabaseEngine Engine { get; set; }

    /// <summary>
    /// 前端展示名称。
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// MCP Server 启动命令。
    /// </summary>
    public string ServerCommand { get; set; } = string.Empty;

    /// <summary>
    /// MCP Server 启动参数 JSON。
    /// </summary>
    public string ServerArgumentsJson { get; set; } = "[]";

    /// <summary>
    /// MCP Server 环境变量 JSON。
    /// </summary>
    public string EnvironmentJson { get; set; } = "{}";

    /// <summary>
    /// 目标数据库连接串。
    /// </summary>
    public string DatabaseConnectionString { get; set; } = string.Empty;

    /// <summary>
    /// 目标数据库名称。
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 是否为默认连接。
    /// </summary>
    public bool IsDefault { get; set; }

    /// <summary>
    /// 当前连接状态。
    /// </summary>
    public McpConnectionStatus Status { get; set; } = McpConnectionStatus.Draft;

    /// <summary>
    /// 最后一次完成工具发现的时间。
    /// </summary>
    public DateTimeOffset? LastDiscoveredAt { get; set; }

    /// <summary>
    /// 创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新时间。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// 连接下的工具集合。
    /// </summary>
    public ICollection<McpToolEntity> Tools { get; set; } = [];
}

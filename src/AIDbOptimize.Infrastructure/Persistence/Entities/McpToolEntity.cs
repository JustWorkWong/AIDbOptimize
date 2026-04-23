using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// MCP 工具配置实体。
/// </summary>
public sealed class McpToolEntity
{
    /// <summary>
    /// 工具主键。
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 所属连接主键。
    /// </summary>
    public Guid ConnectionId { get; set; }

    /// <summary>
    /// MCP 工具原始名称。
    /// </summary>
    public string ToolName { get; set; } = string.Empty;

    /// <summary>
    /// 前端展示名称。
    /// </summary>
    public string DisplayName { get; set; } = string.Empty;

    /// <summary>
    /// 工具描述。
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// 输入参数 Schema 的 JSON 快照。
    /// </summary>
    public string InputSchemaJson { get; set; } = "{}";

    /// <summary>
    /// 工具审批模式。
    /// </summary>
    public ToolApprovalMode ApprovalMode { get; set; } = ToolApprovalMode.NoApproval;

    /// <summary>
    /// 工具是否启用。
    /// </summary>
    public bool IsEnabled { get; set; } = true;

    /// <summary>
    /// 是否为写入型工具。
    /// </summary>
    public bool IsWriteTool { get; set; }

    /// <summary>
    /// 创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 更新时间。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// 所属连接导航属性。
    /// </summary>
    public McpConnectionEntity Connection { get; set; } = null!;

    /// <summary>
    /// 工具执行记录集合。
    /// </summary>
    public ICollection<McpToolExecutionEntity> Executions { get; set; } = [];
}

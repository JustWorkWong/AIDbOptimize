namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// MCP 工具执行记录实体。
/// </summary>
public sealed class McpToolExecutionEntity
{
    /// <summary>
    /// 执行记录主键。
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 执行时使用的连接主键。
    /// </summary>
    public Guid ConnectionId { get; set; }

    /// <summary>
    /// 被执行工具的主键。
    /// </summary>
    public Guid ToolId { get; set; }

    /// <summary>
    /// 发起执行的操作者标识。
    /// </summary>
    public string RequestedBy { get; set; } = "system";

    /// <summary>
    /// 请求参数 JSON。
    /// </summary>
    public string RequestPayloadJson { get; set; } = "{}";

    /// <summary>
    /// 响应结果 JSON。
    /// </summary>
    public string ResponsePayloadJson { get; set; } = "{}";

    /// <summary>
    /// 执行状态。
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 错误信息。
    /// </summary>
    public string? ErrorMessage { get; set; }

    /// <summary>
    /// 执行时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 所属连接导航属性。
    /// </summary>
    public McpConnectionEntity Connection { get; set; } = null!;

    /// <summary>
    /// 所属工具导航属性。
    /// </summary>
    public McpToolEntity Tool { get; set; } = null!;
}

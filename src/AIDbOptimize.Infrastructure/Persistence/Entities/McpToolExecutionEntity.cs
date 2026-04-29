namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// MCP tool execution entity.
/// </summary>
public sealed class McpToolExecutionEntity
{
    public Guid Id { get; set; }

    public Guid ConnectionId { get; set; }

    public Guid ToolId { get; set; }

    public Guid? WorkflowSessionId { get; set; }

    public string? WorkflowNodeName { get; set; }

    public string ExecutionScope { get; set; } = "manual";

    public string? TraceId { get; set; }

    public string RequestedBy { get; set; } = "system";

    public string RequestPayloadJson { get; set; } = "{}";

    public string ResponsePayloadJson { get; set; } = "{}";

    public string Status { get; set; } = string.Empty;

    public string? ErrorMessage { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public McpConnectionEntity Connection { get; set; } = null!;

    public McpToolEntity Tool { get; set; } = null!;

    public WorkflowSessionEntity? WorkflowSession { get; set; }

    public ICollection<WorkflowEventEntity> WorkflowEvents { get; set; } = [];
}

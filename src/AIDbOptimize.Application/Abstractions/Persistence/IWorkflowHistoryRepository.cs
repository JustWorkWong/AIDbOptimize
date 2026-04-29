using AIDbOptimize.Domain.DbConfigOptimization.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// Workflow 历史记录持久化抽象。
/// </summary>
public interface IWorkflowHistoryRepository
{
    Task<WorkflowNodeExecutionRecord?> GetNodeExecutionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkflowNodeExecutionRecord>> ListNodeExecutionsAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);

    Task AddNodeExecutionAsync(
        WorkflowNodeExecutionRecord record,
        CancellationToken cancellationToken = default);

    Task UpdateNodeExecutionAsync(
        WorkflowNodeExecutionRecord record,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<WorkflowEventRecord>> ListEventsAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default);

    Task AddEventAsync(
        WorkflowEventRecord record,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Workflow 节点执行轻量记录。
/// </summary>
public sealed record WorkflowNodeExecutionRecord(
    Guid Id,
    Guid WorkflowSessionId,
    string NodeKey,
    string NodeType,
    int Attempt,
    WorkflowNodeExecutionStatus Status,
    string InputPayloadJson,
    string OutputPayloadJson,
    string? ErrorMessage,
    DateTimeOffset CreatedAt,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt);

/// <summary>
/// Workflow 事件轻量记录。
/// </summary>
public sealed record WorkflowEventRecord(
    Guid Id,
    Guid WorkflowSessionId,
    Guid? NodeExecutionId,
    Guid? ReviewTaskId,
    Guid? McpToolExecutionId,
    WorkflowEventType EventType,
    string EventName,
    string PayloadJson,
    string? Message,
    DateTimeOffset OccurredAt);

namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Workflow history list item.
/// </summary>
public sealed record WorkflowHistoryEntryDto(
    string SessionId,
    string WorkflowType,
    string EngineType,
    string Status,
    string? CurrentNode,
    WorkflowConnectionDto Connection,
    DateTimeOffset StartedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

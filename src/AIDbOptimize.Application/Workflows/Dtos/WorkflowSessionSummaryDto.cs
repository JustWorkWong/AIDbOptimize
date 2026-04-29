namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Workflow session list item.
/// </summary>
public sealed record WorkflowSessionSummaryDto(
    string SessionId,
    string WorkflowType,
    string EngineType,
    string Status,
    string? CurrentNode,
    int ProgressPercent,
    WorkflowConnectionDto Connection,
    string? ActiveReviewTaskId,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

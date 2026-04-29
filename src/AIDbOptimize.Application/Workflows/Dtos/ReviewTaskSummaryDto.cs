namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Review task list item.
/// </summary>
public sealed record ReviewTaskSummaryDto(
    string TaskId,
    string SessionId,
    string Title,
    string Status,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ReviewedAt);

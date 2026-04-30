namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Review task detail.
/// </summary>
public sealed record ReviewTaskDetailDto(
    string TaskId,
    string SessionId,
    string Title,
    string Status,
    string PayloadJson,
    WorkflowStructuredResultDto? ParsedReport,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ReviewedAt,
    string? Reviewer,
    string? Decision,
    string? Comment,
    string? AdjustmentsJson);

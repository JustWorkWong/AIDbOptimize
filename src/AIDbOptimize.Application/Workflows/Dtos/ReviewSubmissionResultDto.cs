namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Review submission result.
/// </summary>
public sealed record ReviewSubmissionResultDto(
    string TaskId,
    string ReviewStatus,
    string WorkflowStatus,
    DateTimeOffset SubmittedAt);

namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// workflow 历史步骤。
/// </summary>
public sealed record WorkflowHistoryStepDto(
    string Name,
    string Status,
    DateTimeOffset OccurredAt,
    string? Message);

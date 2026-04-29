namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Db config workflow options.
/// </summary>
public sealed class DbConfigWorkflowOptionsDto
{
    public bool AllowFallbackSnapshot { get; init; } = true;

    public bool RequireHumanReview { get; init; } = true;

    public bool EnableEvidenceGrounding { get; init; } = true;
}

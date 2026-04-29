namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Start db-config-optimization workflow request.
/// </summary>
public sealed class StartDbConfigOptimizationWorkflowRequest
{
    public string ConnectionId { get; init; } = string.Empty;

    public DbConfigWorkflowOptionsDto Options { get; init; } = new();

    public string RequestedBy { get; init; } = "frontend";

    public string? Notes { get; init; }
}

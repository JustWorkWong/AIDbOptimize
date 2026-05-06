namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

/// <summary>
/// Start command for db-config workflow.
/// </summary>
public sealed record DbConfigWorkflowCommand(
    Guid ConnectionId,
    string BundleId,
    string BundleVersion,
    string InvestigationSkillId,
    string InvestigationSkillVersion,
    string DiagnosisSkillId,
    string DiagnosisSkillVersion,
    string RequestedBy,
    string? Notes,
    bool AllowFallbackSnapshot,
    bool RequireHumanReview,
    bool EnableEvidenceGrounding);

/// <summary>
/// Resume request for workflow review or recovery.
/// </summary>
public sealed record WorkflowResumeRequest(
    string Trigger,
    string? ReviewTaskId = null,
    string? Action = null,
    string? Reviewer = null,
    string? Comment = null);

internal enum WorkflowRunMode
{
    Start = 0,
    Resume = 1,
    Recovery = 2
}

internal sealed record WorkflowRunDescriptor(
    WorkflowRunMode Mode,
    Guid? SessionId,
    DbConfigWorkflowCommand? Command,
    WorkflowResumeRequest? ResumeRequest);

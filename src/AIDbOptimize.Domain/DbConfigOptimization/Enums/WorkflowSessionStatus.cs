namespace AIDbOptimize.Domain.DbConfigOptimization.Enums;

/// <summary>
/// Workflow 会话状态。
/// </summary>
public enum WorkflowSessionStatus
{
    Draft = 0,
    Running = 1,
    WaitingForReview = 2,
    Succeeded = 3,
    Failed = 4,
    Cancelled = 5,
    Recovering = 6
}

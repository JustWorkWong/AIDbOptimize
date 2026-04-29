namespace AIDbOptimize.Domain.DbConfigOptimization.Enums;

/// <summary>
/// Review 任务状态。
/// </summary>
public enum WorkflowReviewTaskStatus
{
    Pending = 0,
    Approved = 1,
    Rejected = 2,
    Adjusted = 3,
    Cancelled = 4
}

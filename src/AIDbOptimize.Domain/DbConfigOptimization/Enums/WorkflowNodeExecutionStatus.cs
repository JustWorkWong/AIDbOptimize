namespace AIDbOptimize.Domain.DbConfigOptimization.Enums;

/// <summary>
/// Workflow 节点执行状态。
/// </summary>
public enum WorkflowNodeExecutionStatus
{
    Pending = 0,
    Running = 1,
    WaitingForReview = 2,
    Succeeded = 3,
    Failed = 4,
    Skipped = 5
}

namespace AIDbOptimize.Domain.DbConfigOptimization.Enums;

/// <summary>
/// Workflow 事件类型。
/// </summary>
public enum WorkflowEventType
{
    WorkflowStarted = 0,
    ExecutorStarted = 1,
    ExecutorCompleted = 2,
    CheckpointSaved = 3,
    ReviewRequested = 4,
    ReviewResolved = 5,
    ToolExecutionLinked = 6,
    WorkflowCompleted = 7,
    WorkflowFailed = 8,
    WorkflowCancelled = 9,
    WorkflowRecovered = 10
}

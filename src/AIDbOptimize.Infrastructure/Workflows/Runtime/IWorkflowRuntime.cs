using AIDbOptimize.Application.Workflows.Dtos;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

/// <summary>
/// Unified workflow runtime.
/// </summary>
public interface IWorkflowRuntime
{
    Task<WorkflowSessionDetailDto> QueueStartDbConfigAsync(
        DbConfigWorkflowCommand command,
        CancellationToken cancellationToken = default);

    Task<WorkflowSessionDetailDto> StartDbConfigAsync(
        DbConfigWorkflowCommand command,
        CancellationToken cancellationToken = default);

    Task ContinueStartAsync(
        Guid sessionId,
        DbConfigWorkflowCommand command,
        CancellationToken cancellationToken = default);

    Task<WorkflowSessionDetailDto> ResumeAsync(
        Guid sessionId,
        WorkflowResumeRequest request,
        CancellationToken cancellationToken = default);

    Task<WorkflowCancellationResultDto> CancelAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default);
}

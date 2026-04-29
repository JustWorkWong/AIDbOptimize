using AIDbOptimize.Application.Workflows.Dtos;

namespace AIDbOptimize.Application.Workflows.Services;

/// <summary>
/// 数据库配置优化 workflow 的 API 入口契约。
/// </summary>
public interface IDbConfigOptimizationWorkflowService
{
    Task<WorkflowSessionDetailDto> StartAsync(
        StartDbConfigOptimizationWorkflowRequest request,
        CancellationToken cancellationToken = default);

    Task<WorkflowSessionDetailDto?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default);

    Task<IReadOnlyCollection<WorkflowSessionSummaryDto>> ListSessionsAsync(
        CancellationToken cancellationToken = default);

    Task<WorkflowCancellationResultDto> CancelAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
}

using AIDbOptimize.Application.Workflows.Dtos;

namespace AIDbOptimize.Application.Workflows.Services;

/// <summary>
/// workflow 历史查询契约。
/// </summary>
public interface IWorkflowHistoryService
{
    Task<IReadOnlyCollection<WorkflowHistoryEntryDto>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<WorkflowHistoryDetailDto?> GetAsync(
        string sessionId,
        CancellationToken cancellationToken = default);
}

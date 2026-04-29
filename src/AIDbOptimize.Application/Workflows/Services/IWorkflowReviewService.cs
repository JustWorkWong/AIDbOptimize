using AIDbOptimize.Application.Workflows.Dtos;

namespace AIDbOptimize.Application.Workflows.Services;

/// <summary>
/// workflow review 契约。
/// </summary>
public interface IWorkflowReviewService
{
    Task<IReadOnlyCollection<ReviewTaskSummaryDto>> ListAsync(
        CancellationToken cancellationToken = default);

    Task<ReviewTaskDetailDto?> GetAsync(
        string taskId,
        CancellationToken cancellationToken = default);

    Task<ReviewSubmissionResultDto> SubmitAsync(
        string taskId,
        SubmitReviewRequest request,
        CancellationToken cancellationToken = default);
}

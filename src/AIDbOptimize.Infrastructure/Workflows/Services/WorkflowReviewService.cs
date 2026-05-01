using System.Diagnostics;
using System.Text.Json;
using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Application.Workflows.Services;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Runtime;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Workflows.Services;

/// <summary>
/// Workflow review service.
/// </summary>
public sealed class WorkflowReviewService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    ReviewAdjustmentValidator reviewAdjustmentValidator,
    IWorkflowRuntime workflowRuntime)
    : IWorkflowReviewService
{
    public async Task<IReadOnlyCollection<ReviewTaskSummaryDto>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowReviewTasks
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new ReviewTaskSummaryDto(
                x.Id.ToString(),
                x.WorkflowSessionId.ToString(),
                x.Title,
                x.Status.ToString(),
                x.CreatedAt,
                x.CompletedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<ReviewTaskDetailDto?> GetAsync(
        string taskId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(taskId, out var reviewTaskId))
        {
            return null;
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowReviewTasks
            .AsNoTracking()
            .Where(x => x.Id == reviewTaskId)
            .Select(x => new ReviewTaskDetailDto(
                x.Id.ToString(),
                x.WorkflowSessionId.ToString(),
                x.Title,
                x.Status.ToString(),
                x.PayloadJson,
                WorkflowResultParser.TryParse(x.PayloadJson),
                x.CreatedAt,
                x.CompletedAt,
                x.DecisionBy,
                x.Status == WorkflowReviewTaskStatus.Pending ? null : x.Status.ToString(),
                x.DecisionNote,
                string.IsNullOrWhiteSpace(x.AdjustmentsJson) || x.AdjustmentsJson == "{}" ? null : x.AdjustmentsJson))
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<ReviewSubmissionResultDto> SubmitAsync(
        string taskId,
        SubmitReviewRequest request,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.ReviewActivitySource.StartActivity("review.submit");
        activity?.SetTag("review.task_id", taskId);
        activity?.SetTag("review.action", request.Action);
        var startedAt = Stopwatch.StartNew();
        AIDbOptimizeTelemetry.ReviewSubmitted.Add(1);

        if (!Guid.TryParse(taskId, out var reviewTaskId))
        {
            throw new InvalidOperationException("审核任务 ID 必须是合法的 Guid。");
        }

        var normalizedAction = (request.Action ?? string.Empty).Trim().ToLowerInvariant();
        if (normalizedAction is not ("approve" or "reject" or "adjust"))
        {
            throw new InvalidOperationException("仅支持 approve、reject、adjust 三种审核动作。");
        }

        WorkflowReviewAdjustment? normalizedAdjustment = null;
        if (normalizedAction == "adjust")
        {
            normalizedAdjustment = reviewAdjustmentValidator.ValidateAndNormalize(
                request.Adjustments ?? JsonDocument.Parse("{}").RootElement,
                request.Comment);
            AIDbOptimizeTelemetry.ReviewAdjusted.Add(1);
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var reviewTask = await dbContext.WorkflowReviewTasks
            .SingleOrDefaultAsync(x => x.Id == reviewTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到审核任务：{taskId}");

        var sessionStatus = await dbContext.WorkflowSessions
            .Where(x => x.Id == reviewTask.WorkflowSessionId)
            .Select(x => x.Status)
            .SingleAsync(cancellationToken);

        if (sessionStatus is WorkflowSessionStatus.Cancelled or WorkflowSessionStatus.Failed or WorkflowSessionStatus.Succeeded)
        {
            throw new InvalidOperationException($"褰撳墠宸ヤ綔娴佺姸鎬佷负 {sessionStatus}锛屼笉鍏佽缁х画鎻愪氦瀹℃牳銆?");
        }

        if (reviewTask.Status != WorkflowReviewTaskStatus.Pending)
        {
            throw new InvalidOperationException("该审核任务已经处理完成，不能重复提交。");
        }

        var now = DateTimeOffset.UtcNow;
        reviewTask.DecisionBy = string.IsNullOrWhiteSpace(request.Reviewer) ? "reviewer" : request.Reviewer;
        reviewTask.DecisionNote = request.Comment;
        reviewTask.UpdatedAt = now;
        reviewTask.CompletedAt = now;
        reviewTask.Status = normalizedAction == "approve"
            ? WorkflowReviewTaskStatus.Approved
            : normalizedAction == "adjust"
                ? WorkflowReviewTaskStatus.Adjusted
                : WorkflowReviewTaskStatus.Rejected;
        reviewTask.AdjustmentsJson = normalizedAdjustment is null
            ? "{}"
            : JsonSerializer.Serialize(new
            {
                riskLevelOverrides = normalizedAdjustment.RiskLevelOverrides
            });

        await dbContext.SaveChangesAsync(cancellationToken);

        var resumed = await workflowRuntime.ResumeAsync(
            reviewTask.WorkflowSessionId,
            new WorkflowResumeRequest(
                "review-submit",
                reviewTask.Id.ToString(),
                normalizedAction,
                reviewTask.DecisionBy,
                reviewTask.DecisionNote),
            cancellationToken);

        startedAt.Stop();
        AIDbOptimizeTelemetry.ReviewResumeDurationMs.Record(startedAt.Elapsed.TotalMilliseconds);

        return new ReviewSubmissionResultDto(
            reviewTask.Id.ToString(),
            reviewTask.Status.ToString(),
            resumed.Status,
            now);
    }
}

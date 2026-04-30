using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Application.Workflows.Services;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Workflows.Runtime;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.Infrastructure.Workflows.Services;

/// <summary>
/// Db config workflow application service.
/// </summary>
public sealed class DbConfigOptimizationWorkflowService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    IWorkflowRuntime workflowRuntime,
    IServiceScopeFactory serviceScopeFactory,
    ILogger<DbConfigOptimizationWorkflowService> logger)
    : IDbConfigOptimizationWorkflowService
{
    public async Task<WorkflowSessionDetailDto> StartAsync(
        StartDbConfigOptimizationWorkflowRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(request.ConnectionId, out var connectionId))
        {
            throw new InvalidOperationException("连接 ID 必须是合法的 Guid。");
        }

        var command = new DbConfigWorkflowCommand(
            connectionId,
            string.IsNullOrWhiteSpace(request.RequestedBy) ? "frontend" : request.RequestedBy,
            request.Notes,
            request.Options.AllowFallbackSnapshot,
            request.Options.RequireHumanReview,
            request.Options.EnableEvidenceGrounding);

        var queued = await workflowRuntime.QueueStartDbConfigAsync(command, cancellationToken);

        _ = Task.Run(async () =>
        {
            try
            {
                using var scope = serviceScopeFactory.CreateScope();
                var runtime = scope.ServiceProvider.GetRequiredService<IWorkflowRuntime>();
                await runtime.ContinueStartAsync(Guid.Parse(queued.SessionId), command, CancellationToken.None);
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "异步启动工作流失败，SessionId={SessionId}", queued.SessionId);
            }
        });

        return queued;
    }

    public async Task<WorkflowSessionDetailDto?> GetSessionAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            return null;
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(x => x.Connection)
            .SingleOrDefaultAsync(x => x.Id == workflowSessionId, cancellationToken);

        if (session is null)
        {
            return null;
        }

        WorkflowReviewReferenceDto? review = null;
        if (session.ActiveReviewTaskId.HasValue)
        {
            var reviewTask = await dbContext.WorkflowReviewTasks
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == session.ActiveReviewTaskId.Value, cancellationToken);
            if (reviewTask is not null)
            {
                review = new WorkflowReviewReferenceDto(reviewTask.Id.ToString(), reviewTask.Status.ToString());
            }
        }

        WorkflowSummaryReferenceDto? summary = null;
        if (session.AgentSessionId.HasValue)
        {
            var agentSession = await dbContext.AgentSessions
                .AsNoTracking()
                .SingleOrDefaultAsync(x => x.Id == session.AgentSessionId.Value, cancellationToken);
            if (agentSession is not null)
            {
                summary = new WorkflowSummaryReferenceDto(agentSession.Id.ToString(), agentSession.UpdatedAt);
            }
        }

        return new WorkflowSessionDetailDto(
            session.Id.ToString(),
            session.WorkflowName,
            session.EngineType,
            PublicStatus(session.Status),
            session.CurrentNodeKey,
            WorkflowProgressCalculator.GetProgressPercent(session.CurrentNodeKey, PublicStatus(session.Status)),
            new WorkflowConnectionDto(
                session.ConnectionId.ToString(),
                session.Connection.DisplayName,
                session.Connection.Engine.ToString(),
                session.Connection.DatabaseName),
            review,
            string.IsNullOrWhiteSpace(session.ResultPayloadJson) || session.ResultPayloadJson == "{}"
                ? null
                : new WorkflowResultDto(
                    session.ResultType,
                    session.ResultPayloadJson,
                    WorkflowResultParser.TryParse(session.ResultPayloadJson)),
            summary,
            session.ErrorMessage,
            $"/api/workflows/{session.Id}/events",
            session.CreatedAt,
            session.UpdatedAt,
            session.CompletedAt);
    }

    public async Task<IReadOnlyCollection<WorkflowSessionSummaryDto>> ListSessionsAsync(
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(x => x.Connection)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new WorkflowSessionSummaryDto(
                x.Id.ToString(),
                x.WorkflowName,
                x.EngineType,
                x.Status == Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded ? "Completed" : x.Status.ToString(),
                x.CurrentNodeKey,
                WorkflowProgressCalculator.GetProgressPercent(
                    x.CurrentNodeKey,
                    x.Status == Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded ? "Completed" : x.Status.ToString()),
                new WorkflowConnectionDto(
                    x.ConnectionId.ToString(),
                    x.Connection.DisplayName,
                    x.Connection.Engine.ToString(),
                    x.Connection.DatabaseName),
                x.ActiveReviewTaskId.HasValue ? x.ActiveReviewTaskId.Value.ToString() : null,
                x.CreatedAt,
                x.UpdatedAt,
                x.CompletedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkflowCancellationResultDto> CancelAsync(
        string sessionId,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(sessionId, out var workflowSessionId))
        {
            throw new InvalidOperationException("工作流会话 ID 必须是合法的 Guid。");
        }

        return await workflowRuntime.CancelAsync(workflowSessionId, cancellationToken);
    }

    private static string PublicStatus(Domain.DbConfigOptimization.Enums.WorkflowSessionStatus status)
    {
        return status == Domain.DbConfigOptimization.Enums.WorkflowSessionStatus.Succeeded
            ? "Completed"
            : status.ToString();
    }
}

using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Application.Workflows.Services;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Workflows.Runtime;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Workflows.Services;

/// <summary>
/// Db config workflow application service.
/// </summary>
public sealed class DbConfigOptimizationWorkflowService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    IWorkflowRuntime workflowRuntime)
    : IDbConfigOptimizationWorkflowService
{
    public async Task<WorkflowSessionDetailDto> StartAsync(
        StartDbConfigOptimizationWorkflowRequest request,
        CancellationToken cancellationToken = default)
    {
        if (!Guid.TryParse(request.ConnectionId, out var connectionId))
        {
            throw new InvalidOperationException("ConnectionId must be a valid Guid.");
        }

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var connection = await dbContext.McpConnections
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == connectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Connection not found: {request.ConnectionId}");

        return await workflowRuntime.StartDbConfigAsync(
            new DbConfigWorkflowCommand(
                connectionId,
                string.IsNullOrWhiteSpace(request.RequestedBy) ? "frontend" : request.RequestedBy,
                request.Notes,
                request.Options.AllowFallbackSnapshot,
                request.Options.RequireHumanReview,
                request.Options.EnableEvidenceGrounding),
            cancellationToken);
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
                : new WorkflowResultDto(session.ResultType, session.ResultPayloadJson),
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
            throw new InvalidOperationException("sessionId must be a valid Guid.");
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

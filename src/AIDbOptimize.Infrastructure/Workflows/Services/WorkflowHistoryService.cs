using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Application.Workflows.Services;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Workflows.Services;

/// <summary>
/// Workflow history service.
/// </summary>
public sealed class WorkflowHistoryService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IWorkflowHistoryService
{
    public async Task<IReadOnlyCollection<WorkflowHistoryEntryDto>> ListAsync(
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(x => x.Connection)
            .OrderByDescending(x => x.CreatedAt)
            .Select(x => new WorkflowHistoryEntryDto(
                x.Id.ToString(),
                x.WorkflowName,
                x.EngineType,
                x.Status == WorkflowSessionStatus.Succeeded ? "Completed" : x.Status.ToString(),
                x.CurrentNodeKey,
                new WorkflowConnectionDto(
                    x.ConnectionId.ToString(),
                    x.Connection.DisplayName,
                    x.Connection.Engine.ToString(),
                    x.Connection.DatabaseName),
                x.CreatedAt,
                x.UpdatedAt,
                x.CompletedAt))
            .ToListAsync(cancellationToken);
    }

    public async Task<WorkflowHistoryDetailDto?> GetAsync(
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

        var nodeExecutions = await dbContext.WorkflowNodeExecutions
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new WorkflowHistoryNodeExecutionDto(
                x.Id.ToString(),
                x.NodeKey,
                x.NodeType,
                x.Status.ToString(),
                x.InputPayloadJson,
                x.OutputPayloadJson,
                x.ErrorMessage,
                x.AgentSessionId.HasValue ? x.AgentSessionId.Value.ToString() : null,
                x.TokenUsageJson,
                x.StartedAt ?? x.CreatedAt,
                x.CompletedAt))
            .ToListAsync(cancellationToken);

        var toolExecutions = await dbContext.McpToolExecutions
            .AsNoTracking()
            .Include(x => x.Tool)
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new WorkflowHistoryToolExecutionDto(
                x.Id.ToString(),
                x.ToolId.ToString(),
                x.Tool.ToolName,
                x.Status,
                x.RequestPayloadJson,
                x.ResponsePayloadJson,
                x.ErrorMessage,
                x.WorkflowNodeName,
                x.ExecutionScope,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

        var reviews = await dbContext.WorkflowReviewTasks
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new WorkflowHistoryReviewDto(
                x.Id.ToString(),
                x.Status.ToString(),
                x.PayloadJson,
                x.DecisionBy,
                x.DecisionNote,
                string.IsNullOrWhiteSpace(x.AdjustmentsJson) || x.AdjustmentsJson == "{}" ? null : x.AdjustmentsJson,
                x.CreatedAt,
                x.CompletedAt))
            .ToListAsync(cancellationToken);

        var ragSnapshots = await dbContext.RagRetrievalSnapshots
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.CreatedAt)
            .Select(x => new WorkflowRagSnapshotDto(
                x.Id.ToString(),
                x.NodeExecutionId.ToString(),
                x.SnapshotTypeJson,
                x.RetrievedItemsJson,
                x.CreatedAt))
            .ToListAsync(cancellationToken);

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

        return new WorkflowHistoryDetailDto(
            session.Id.ToString(),
            session.WorkflowName,
            session.EngineType,
            session.Status == WorkflowSessionStatus.Succeeded ? "Completed" : session.Status.ToString(),
            session.CurrentNodeKey,
            new WorkflowConnectionDto(
                session.ConnectionId.ToString(),
                session.Connection.DisplayName,
                session.Connection.Engine.ToString(),
                session.Connection.DatabaseName),
            string.IsNullOrWhiteSpace(session.ResultPayloadJson) || session.ResultPayloadJson == "{}"
                ? null
                : new WorkflowResultDto(
                    session.ResultType,
                    session.ResultPayloadJson,
                    WorkflowResultParser.TryParse(session.ResultPayloadJson)),
            summary,
            WorkflowResultParser.TryParseSkillSelection(session.InputPayloadJson),
            session.ErrorMessage,
            nodeExecutions,
            toolExecutions,
            ragSnapshots,
            reviews,
            session.CreatedAt,
            session.UpdatedAt,
            session.CompletedAt);
    }
}

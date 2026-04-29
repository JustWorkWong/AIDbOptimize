using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Agents;

/// <summary>
/// Agent 会话持久化服务实现。
/// </summary>
public sealed class AgentSessionPersistenceService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IAgentSessionPersistenceService
{
    public async Task<AgentSessionPersistenceResult> CreateSessionAsync(
        AgentSessionCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.AgentActivitySource.StartActivity("agent.session.create");
        activity?.SetTag("agent.role", request.AgentRole);
        activity?.SetTag("workflow.session_id", request.WorkflowSessionId);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = new AgentSessionEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = request.WorkflowSessionId,
            AgentRole = request.AgentRole,
            SerializedSessionJson = request.SerializedSessionJson,
            SessionStateJson = request.SessionStateJson,
            PromptVersion = request.PromptVersion,
            ModelId = request.ModelId,
            CreatedAt = request.CreatedAt,
            UpdatedAt = request.CreatedAt
        };

        dbContext.AgentSessions.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        AIDbOptimizeTelemetry.AgentSessionCreated.Add(1);

        return new AgentSessionPersistenceResult(entity.Id);
    }

    public async Task AppendMessageAsync(
        AgentMessageCreateRequest request,
        CancellationToken cancellationToken = default)
    {
        using var activity = AIDbOptimizeTelemetry.AgentActivitySource.StartActivity("agent.message.append");
        activity?.SetTag("agent.session_id", request.AgentSessionId);
        activity?.SetTag("agent.message_kind", request.MessageKind);

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.AgentMessages.Add(new AgentMessageEntity
        {
            Id = Guid.NewGuid(),
            AgentSessionId = request.AgentSessionId,
            WorkflowSessionId = request.WorkflowSessionId,
            SequenceNo = request.SequenceNo,
            Role = request.Role,
            MessageKind = request.MessageKind,
            Content = request.Content,
            RawPayloadJson = request.RawPayloadJson,
            TraceId = request.TraceId,
            CreatedAt = request.CreatedAt
        });

        await dbContext.SaveChangesAsync(cancellationToken);
        AIDbOptimizeTelemetry.AgentMessagesPersisted.Add(1);
    }

    public async Task AttachSummaryAsync(
        Guid agentSessionId,
        Guid summaryId,
        int messageGroupCount,
        DateTimeOffset summarizedAt,
        string serializedSessionJson,
        string sessionStateJson,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.AgentSessions
            .SingleOrDefaultAsync(x => x.Id == agentSessionId, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 agent session：{agentSessionId}");

        entity.ActiveSummaryId = summaryId;
        entity.MessageGroupCount = messageGroupCount;
        entity.LastCompactedAt = summarizedAt;
        entity.SerializedSessionJson = serializedSessionJson;
        entity.SessionStateJson = sessionStateJson;
        entity.UpdatedAt = summarizedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }
}

using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// Workflow 历史记录仓储实现。
/// </summary>
public sealed class WorkflowHistoryRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IWorkflowHistoryRepository
{
    public async Task<WorkflowNodeExecutionRecord?> GetNodeExecutionByIdAsync(
        Guid id,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowNodeExecutions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ToNodeRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkflowNodeExecutionRecord>> ListNodeExecutionsAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowNodeExecutions
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.CreatedAt)
            .Select(ToNodeRecordExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task AddNodeExecutionAsync(
        WorkflowNodeExecutionRecord record,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.WorkflowNodeExecutions.Add(ToNodeEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateNodeExecutionAsync(
        WorkflowNodeExecutionRecord record,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.WorkflowNodeExecutions.FirstOrDefaultAsync(x => x.Id == record.Id, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 Workflow 节点执行：{record.Id}");

        entity.Attempt = record.Attempt;
        entity.Status = record.Status;
        entity.InputPayloadJson = record.InputPayloadJson;
        entity.OutputPayloadJson = record.OutputPayloadJson;
        entity.ErrorMessage = record.ErrorMessage;
        entity.StartedAt = record.StartedAt;
        entity.CompletedAt = record.CompletedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkflowEventRecord>> ListEventsAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowEvents
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.OccurredAt)
            .Select(ToEventRecordExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task AddEventAsync(
        WorkflowEventRecord record,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.WorkflowEvents.Add(ToEventEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<WorkflowNodeExecutionEntity, WorkflowNodeExecutionRecord>> ToNodeRecordExpression()
    {
        return entity => new WorkflowNodeExecutionRecord(
            entity.Id,
            entity.WorkflowSessionId,
            entity.NodeKey,
            entity.NodeType,
            entity.Attempt,
            entity.Status,
            entity.InputPayloadJson,
            entity.OutputPayloadJson,
            entity.ErrorMessage,
            entity.CreatedAt,
            entity.StartedAt,
            entity.CompletedAt);
    }

    private static Expression<Func<WorkflowEventEntity, WorkflowEventRecord>> ToEventRecordExpression()
    {
        return entity => new WorkflowEventRecord(
            entity.Id,
            entity.WorkflowSessionId,
            entity.NodeExecutionId,
            entity.ReviewTaskId,
            entity.McpToolExecutionId,
            entity.EventType,
            entity.EventName,
            entity.PayloadJson,
            entity.Message,
            entity.OccurredAt);
    }

    private static WorkflowNodeExecutionEntity ToNodeEntity(WorkflowNodeExecutionRecord record)
    {
        return new WorkflowNodeExecutionEntity
        {
            Id = record.Id,
            WorkflowSessionId = record.WorkflowSessionId,
            NodeKey = record.NodeKey,
            NodeType = record.NodeType,
            Attempt = record.Attempt,
            Status = record.Status,
            InputPayloadJson = record.InputPayloadJson,
            OutputPayloadJson = record.OutputPayloadJson,
            ErrorMessage = record.ErrorMessage,
            CreatedAt = record.CreatedAt,
            StartedAt = record.StartedAt,
            CompletedAt = record.CompletedAt
        };
    }

    private static WorkflowEventEntity ToEventEntity(WorkflowEventRecord record)
    {
        return new WorkflowEventEntity
        {
            Id = record.Id,
            WorkflowSessionId = record.WorkflowSessionId,
            NodeExecutionId = record.NodeExecutionId,
            ReviewTaskId = record.ReviewTaskId,
            McpToolExecutionId = record.McpToolExecutionId,
            EventType = record.EventType,
            EventName = record.EventName,
            PayloadJson = record.PayloadJson,
            Message = record.Message,
            OccurredAt = record.OccurredAt
        };
    }
}

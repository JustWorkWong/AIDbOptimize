using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// Workflow 会话仓储实现。
/// </summary>
public sealed class WorkflowSessionRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IWorkflowSessionRepository
{
    public async Task<WorkflowSessionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowSessions
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ToRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkflowSessionRecord>> ListByConnectionAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowSessions
            .AsNoTracking()
            .Where(x => x.ConnectionId == connectionId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(ToRecordExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(WorkflowSessionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.WorkflowSessions.Add(ToEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorkflowSessionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.WorkflowSessions.FirstOrDefaultAsync(x => x.Id == record.Id, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 Workflow 会话：{record.Id}");

        entity.Status = record.Status;
        entity.RequestedBy = record.RequestedBy;
        entity.InputPayloadJson = record.InputPayloadJson;
        entity.ResultPayloadJson = record.ResultPayloadJson;
        entity.CurrentNodeKey = record.CurrentNodeKey;
        entity.UpdatedAt = record.UpdatedAt;
        entity.CompletedAt = record.CompletedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<WorkflowSessionEntity, WorkflowSessionRecord>> ToRecordExpression()
    {
        return entity => new WorkflowSessionRecord(
            entity.Id,
            entity.ConnectionId,
            entity.WorkflowName,
            entity.Status,
            entity.RequestedBy,
            entity.InputPayloadJson,
            entity.ResultPayloadJson,
            entity.CurrentNodeKey,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.CompletedAt);
    }

    private static WorkflowSessionEntity ToEntity(WorkflowSessionRecord record)
    {
        return new WorkflowSessionEntity
        {
            Id = record.Id,
            ConnectionId = record.ConnectionId,
            WorkflowName = record.WorkflowName,
            Status = record.Status,
            RequestedBy = record.RequestedBy,
            InputPayloadJson = record.InputPayloadJson,
            ResultPayloadJson = record.ResultPayloadJson,
            CurrentNodeKey = record.CurrentNodeKey,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt,
            CompletedAt = record.CompletedAt
        };
    }
}

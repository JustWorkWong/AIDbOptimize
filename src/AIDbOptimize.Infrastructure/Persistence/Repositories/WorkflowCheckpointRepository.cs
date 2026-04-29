using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// Workflow checkpoint 仓储实现。
/// </summary>
public sealed class WorkflowCheckpointRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IWorkflowCheckpointRepository
{
    public async Task<WorkflowCheckpointRecord?> GetLatestAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowCheckpoints
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderByDescending(x => x.Sequence)
            .ThenByDescending(x => x.CreatedAt)
            .Select(ToRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkflowCheckpointRecord>> ListAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowCheckpoints
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderBy(x => x.Sequence)
            .ThenBy(x => x.CreatedAt)
            .Select(ToRecordExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(
        WorkflowCheckpointRecord record,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.WorkflowCheckpoints.Add(new WorkflowCheckpointEntity
        {
            Id = record.Id,
            WorkflowSessionId = record.WorkflowSessionId,
            Sequence = record.Sequence,
            Status = record.Status,
            CurrentNodeKey = record.CurrentNodeKey,
            SnapshotJson = record.SnapshotJson,
            CreatedAt = record.CreatedAt
        });

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<WorkflowCheckpointEntity, WorkflowCheckpointRecord>> ToRecordExpression()
    {
        return entity => new WorkflowCheckpointRecord(
            entity.Id,
            entity.WorkflowSessionId,
            entity.Sequence,
            entity.Status,
            entity.CurrentNodeKey,
            entity.SnapshotJson,
            entity.CreatedAt);
    }
}

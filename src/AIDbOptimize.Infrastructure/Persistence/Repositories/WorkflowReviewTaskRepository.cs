using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// Workflow Review 任务仓储实现。
/// </summary>
public sealed class WorkflowReviewTaskRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
    : IWorkflowReviewTaskRepository
{
    public async Task<WorkflowReviewTaskRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowReviewTasks
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ToRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyList<WorkflowReviewTaskRecord>> ListBySessionAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.WorkflowReviewTasks
            .AsNoTracking()
            .Where(x => x.WorkflowSessionId == workflowSessionId)
            .OrderByDescending(x => x.CreatedAt)
            .Select(ToRecordExpression())
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(WorkflowReviewTaskRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.WorkflowReviewTasks.Add(ToEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(WorkflowReviewTaskRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.WorkflowReviewTasks.FirstOrDefaultAsync(x => x.Id == record.Id, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 Workflow Review 任务：{record.Id}");

        entity.Title = record.Title;
        entity.PayloadJson = record.PayloadJson;
        entity.Status = record.Status;
        entity.RequestedBy = record.RequestedBy;
        entity.DecisionBy = record.DecisionBy;
        entity.DecisionNote = record.DecisionNote;
        entity.UpdatedAt = record.UpdatedAt;
        entity.CompletedAt = record.CompletedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<WorkflowReviewTaskEntity, WorkflowReviewTaskRecord>> ToRecordExpression()
    {
        return entity => new WorkflowReviewTaskRecord(
            entity.Id,
            entity.WorkflowSessionId,
            entity.NodeExecutionId,
            entity.Title,
            entity.PayloadJson,
            entity.Status,
            entity.RequestedBy,
            entity.DecisionBy,
            entity.DecisionNote,
            entity.CreatedAt,
            entity.UpdatedAt,
            entity.CompletedAt);
    }

    private static WorkflowReviewTaskEntity ToEntity(WorkflowReviewTaskRecord record)
    {
        return new WorkflowReviewTaskEntity
        {
            Id = record.Id,
            WorkflowSessionId = record.WorkflowSessionId,
            NodeExecutionId = record.NodeExecutionId,
            Title = record.Title,
            PayloadJson = record.PayloadJson,
            Status = record.Status,
            RequestedBy = record.RequestedBy,
            DecisionBy = record.DecisionBy,
            DecisionNote = record.DecisionNote,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt,
            CompletedAt = record.CompletedAt
        };
    }
}

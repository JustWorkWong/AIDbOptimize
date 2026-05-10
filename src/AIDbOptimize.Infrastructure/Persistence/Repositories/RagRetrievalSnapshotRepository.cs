using AIDbOptimize.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

public sealed class RagRetrievalSnapshotRepository(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IRagRetrievalSnapshotRepository
{
    public async Task<IReadOnlyList<RagRetrievalSnapshotRecord>> ListByWorkflowSessionAsync(
        Guid workflowSessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.RagRetrievalSnapshots
            .AsNoTracking()
            .Where(item => item.WorkflowSessionId == workflowSessionId)
            .OrderBy(item => item.CreatedAt)
            .Select(item => new RagRetrievalSnapshotRecord(
                item.Id,
                item.WorkflowSessionId,
                item.NodeExecutionId,
                item.SnapshotTypeJson,
                item.RetrievedItemsJson,
                item.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}

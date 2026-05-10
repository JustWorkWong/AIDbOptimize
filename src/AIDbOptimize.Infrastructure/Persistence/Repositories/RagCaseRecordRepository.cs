using AIDbOptimize.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

public sealed class RagCaseRecordRepository(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IRagCaseRecordRepository
{
    public async Task<IReadOnlyList<RagCaseRecordRecord>> ListByEvidenceReferencesAsync(
        string engine,
        IReadOnlyCollection<string> evidenceReferences,
        int topK,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.RagCaseRecords
            .AsNoTracking()
            .Include(item => item.EvidenceLinks)
            .Where(item => item.Engine == engine)
            .Where(item => item.EvidenceLinks.Any(link => evidenceReferences.Contains(link.EvidenceReference)))
            .OrderByDescending(item => item.CreatedAt)
            .Take(topK)
            .Select(item => new RagCaseRecordRecord(
                item.Id,
                item.WorkflowSessionId,
                item.Engine,
                item.ProblemType,
                item.Outcome,
                item.ReviewStatus,
                item.RecommendationType,
                item.Summary,
                item.EvidenceLinks.Select(link => link.EvidenceReference).ToArray(),
                item.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}

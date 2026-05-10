using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Rag;

public sealed class RagCaseAuditQuery(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IRagCaseAuditQuery
{
    public async Task<IReadOnlyList<RagCaseAuditItemDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.RagCaseRecords
            .AsNoTracking()
            .Include(item => item.EvidenceLinks)
            .OrderByDescending(item => item.CreatedAt)
            .Select(item => new RagCaseAuditItemDto(
                item.Id.ToString(),
                item.WorkflowSessionId.ToString(),
                item.Engine,
                item.ProblemType,
                item.Outcome,
                item.ReviewStatus,
                item.RecommendationType,
                item.Summary,
                item.EvidenceLinks.Count,
                item.CreatedAt))
            .ToListAsync(cancellationToken);
    }
}

using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Rag;

public sealed class RagAssetStatusQuery(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IRagAssetStatusQuery
{
    public async Task<RagAssetStatusDto> GetAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var factDocumentCount = await dbContext.RagDocuments
            .AsNoTracking()
            .CountAsync(item => item.DocumentType == RagDocumentType.Fact && item.IsActive, cancellationToken);
        var caseRecordCount = await dbContext.RagCaseRecords
            .AsNoTracking()
            .CountAsync(cancellationToken);
        var chunkCount = await dbContext.RagDocumentChunks
            .AsNoTracking()
            .CountAsync(cancellationToken);
        var retrievalSnapshotCount = await dbContext.RagRetrievalSnapshots
            .AsNoTracking()
            .CountAsync(cancellationToken);
        var latestDocumentIngestedAt = await dbContext.RagDocuments
            .AsNoTracking()
            .MaxAsync(item => (DateTimeOffset?)item.UpdatedAt, cancellationToken);
        var latestCaseProjectedAt = await dbContext.RagCaseRecords
            .AsNoTracking()
            .MaxAsync(item => (DateTimeOffset?)item.CreatedAt, cancellationToken);
        var latestSnapshotCreatedAt = await dbContext.RagRetrievalSnapshots
            .AsNoTracking()
            .MaxAsync(item => (DateTimeOffset?)item.CreatedAt, cancellationToken);
        var now = DateTimeOffset.UtcNow;

        return new RagAssetStatusDto(
            factDocumentCount,
            caseRecordCount,
            chunkCount,
            retrievalSnapshotCount,
            latestDocumentIngestedAt,
            latestCaseProjectedAt,
            latestSnapshotCreatedAt,
            latestDocumentIngestedAt.HasValue ? (now - latestDocumentIngestedAt.Value).TotalHours : null,
            latestCaseProjectedAt.HasValue ? (now - latestCaseProjectedAt.Value).TotalHours : null,
            latestSnapshotCreatedAt.HasValue ? (now - latestSnapshotCreatedAt.Value).TotalHours : null);
    }
}

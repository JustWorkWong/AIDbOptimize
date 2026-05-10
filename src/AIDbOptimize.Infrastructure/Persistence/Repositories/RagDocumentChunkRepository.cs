using AIDbOptimize.Application.Abstractions.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

public sealed class RagDocumentChunkRepository(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IRagDocumentChunkRepository
{
    public async Task<IReadOnlyList<RagDocumentChunkRecord>> ListAsync(
        RagDocumentChunkFilter filter,
        int topK,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var query = dbContext.RagDocumentChunks
            .AsNoTracking()
            .Include(item => item.Document)
            .Where(item => item.Document.IsActive)
            .AsQueryable();

        if (!string.IsNullOrWhiteSpace(filter.Engine))
        {
            query = query.Where(item => item.Document.Engine == filter.Engine);
        }

        if (!string.IsNullOrWhiteSpace(filter.Vendor))
        {
            query = query.Where(item => item.Document.Vendor == filter.Vendor);
        }

        if (!string.IsNullOrWhiteSpace(filter.Topic))
        {
            query = query.Where(item => item.Document.Topic == filter.Topic);
        }

        if (filter.RequireEmbedding)
        {
            query = query.Where(item => item.Embedding != null);
        }

        var result = await query
            .OrderBy(item => item.Document.Topic)
            .ThenBy(item => item.Title)
            .Select(item => new RagDocumentChunkRecord(
                item.Id,
                item.DocumentId,
                item.Document.Engine,
                item.Document.Vendor,
                item.Document.Topic,
                item.Title,
                item.SectionPath,
                item.Text,
                item.ProductVersion,
                item.AppliesTo,
                item.ParameterNamesJson,
                item.KeywordsJson,
                item.Embedding != null,
                item.Document.SourceUrl))
            .ToListAsync(cancellationToken);

        if (filter.ParameterNames is { Count: > 0 })
        {
            result = result
                .Where(item => filter.ParameterNames.Any(parameterName => item.ParameterNamesJson.Contains(parameterName, StringComparison.OrdinalIgnoreCase)))
                .ToList();
        }

        return result.Take(topK).ToList();
    }
}

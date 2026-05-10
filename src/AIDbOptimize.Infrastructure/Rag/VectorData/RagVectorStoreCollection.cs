using System.Linq;
using System.Text.Json;
using System.Text.RegularExpressions;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Pgvector.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Rag.VectorData;

public sealed partial class RagVectorStoreCollection(
    IServiceProvider serviceProvider) : VectorStoreCollection<Guid, RagKnowledgeVectorRecord>
{
    private readonly IDbContextFactory<ControlPlaneDbContext> _dbContextFactory = serviceProvider.GetRequiredService<IDbContextFactory<ControlPlaneDbContext>>();
    private readonly IRagEmbeddingService _embeddingService = serviceProvider.GetRequiredService<IRagEmbeddingService>();

    public override string Name => RagVectorStore.KnowledgeCollectionName;

    public override Task<bool> CollectionExistsAsync(CancellationToken cancellationToken = default) => Task.FromResult(true);

    public override Task EnsureCollectionExistsAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;

    public override Task EnsureCollectionDeletedAsync(CancellationToken cancellationToken = default) => throw new NotSupportedException("RagVectorStoreCollection is read-only.");

    public override async Task<RagKnowledgeVectorRecord?> GetAsync(Guid key, RecordRetrievalOptions? options = null, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var chunk = await dbContext.RagDocumentChunks
            .AsNoTracking()
            .Include(item => item.Document)
            .Where(item => item.Document.IsActive)
            .Where(item => item.Document.DocumentType == RagDocumentType.Fact)
            .FirstOrDefaultAsync(item => item.Id == key, cancellationToken);

        return chunk is null ? null : MapRecord(chunk);
    }

    public override async IAsyncEnumerable<RagKnowledgeVectorRecord> GetAsync(
        System.Linq.Expressions.Expression<Func<RagKnowledgeVectorRecord, bool>> filter,
        int top,
        FilteredRecordRetrievalOptions<RagKnowledgeVectorRecord>? options = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var compiledFilter = filter.Compile();
        var records = await QueryFactRecordsAsync(null, Math.Max(top * 4, 20), cancellationToken);
        foreach (var record in records.Select(item => item.Record).Where(compiledFilter).Take(top))
        {
            yield return record;
        }
    }

    public override Task DeleteAsync(Guid key, CancellationToken cancellationToken = default) => throw new NotSupportedException("RagVectorStoreCollection is read-only.");

    public override Task UpsertAsync(RagKnowledgeVectorRecord record, CancellationToken cancellationToken = default) => throw new NotSupportedException("RagVectorStoreCollection is read-only.");

    public override Task UpsertAsync(IEnumerable<RagKnowledgeVectorRecord> records, CancellationToken cancellationToken = default) => throw new NotSupportedException("RagVectorStoreCollection is read-only.");

    public override async IAsyncEnumerable<VectorSearchResult<RagKnowledgeVectorRecord>> SearchAsync<TInput>(
        TInput searchValue,
        int top,
        VectorSearchOptions<RagKnowledgeVectorRecord>? options = null,
        [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        var compiledFilter = options?.Filter?.Compile();
        var scoredRecords = await QueryFactRecordsAsync(searchValue?.ToString(), Math.Max(top * 4, 20), cancellationToken);
        foreach (var record in scoredRecords
                     .Where(item => compiledFilter?.Invoke(item.Record) ?? true)
                     .Take(top))
        {
            yield return new VectorSearchResult<RagKnowledgeVectorRecord>(record.Record, record.Score);
        }
    }

    public override object? GetService(Type serviceType, object? serviceKey = null)
    {
        return serviceKey is null ? serviceProvider.GetService(serviceType) : null;
    }

    private async Task<IReadOnlyList<ScoredRecord>> QueryFactRecordsAsync(
        string? searchText,
        int candidateCount,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await _dbContextFactory.CreateDbContextAsync(cancellationToken);
        var activeFacts = dbContext.RagDocumentChunks
            .AsNoTracking()
            .Include(item => item.Document)
            .Where(item => item.Document.IsActive)
            .Where(item => item.Document.DocumentType == RagDocumentType.Fact);

        if (string.IsNullOrWhiteSpace(searchText))
        {
            return await activeFacts
                .OrderBy(item => item.Document.Engine)
                .ThenBy(item => item.Document.Topic)
                .ThenBy(item => item.Title)
                .Take(candidateCount)
                .Select(item => new ScoredRecord(MapRecord(item), 0d))
                .ToArrayAsync(cancellationToken);
        }

        var queryVector = await _embeddingService.GenerateAsync(searchText, cancellationToken);
        if (CanUseVector(dbContext, queryVector))
        {
            return await activeFacts
                .Where(item => item.Embedding != null)
                .OrderBy(item => item.Embedding!.CosineDistance(queryVector!))
                .Take(candidateCount)
                .Select(item => new ScoredRecord(MapRecord(item), 1d))
                .ToArrayAsync(cancellationToken);
        }

        var signalTerms = BuildSignalTerms(searchText);
        var scoredItems = await activeFacts.ToListAsync(cancellationToken);
        return scoredItems
            .Select(item => new ScoredRecord(MapRecord(item), ScoreRecord(item, signalTerms)))
            .Where(item => item.Score > 0)
            .OrderByDescending(item => item.Score)
            .ThenBy(item => item.Record.Engine, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Record.Topic, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Record.SourceTitle, StringComparer.OrdinalIgnoreCase)
            .Take(candidateCount)
            .ToArray();
    }

    private bool CanUseVector(ControlPlaneDbContext dbContext, Pgvector.Vector? queryVector)
    {
        return queryVector is not null
            && !string.Equals(dbContext.Database.ProviderName, "Microsoft.EntityFrameworkCore.InMemory", StringComparison.OrdinalIgnoreCase)
            && dbContext.RagDocumentChunks.Any(item => item.Embedding != null);
    }

    private static double ScoreRecord(
        Persistence.Entities.RagDocumentChunkEntity chunk,
        HashSet<string> signalTerms)
    {
        var score = 0d;
        foreach (var parameterName in DeserializeList(chunk.ParameterNamesJson))
        {
            if (signalTerms.Contains(parameterName))
            {
                score += 10;
            }
        }

        foreach (var keyword in DeserializeList(chunk.KeywordsJson))
        {
            if (signalTerms.Contains(keyword))
            {
                score += 4;
            }
        }

        foreach (var term in signalTerms)
        {
            if (chunk.Title.Contains(term, StringComparison.OrdinalIgnoreCase))
            {
                score += 2;
            }

            if (chunk.Text.Contains(term, StringComparison.OrdinalIgnoreCase))
            {
                score += 1;
            }
        }

        return score;
    }

    private static HashSet<string> BuildSignalTerms(string searchText)
    {
        var terms = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (Match match in SnakeCaseTokenRegex().Matches(searchText))
        {
            terms.Add(match.Value);
        }

        foreach (Match match in WordRegex().Matches(searchText))
        {
            terms.Add(match.Value.ToLowerInvariant());
        }

        return terms;
    }

    private static RagKnowledgeVectorRecord MapRecord(Persistence.Entities.RagDocumentChunkEntity chunk)
    {
        return new RagKnowledgeVectorRecord
        {
            Key = chunk.Id,
            SourceType = "fact",
            Engine = chunk.Document.Engine,
            Vendor = chunk.Document.Vendor,
            Topic = chunk.Document.Topic,
            SectionPath = chunk.SectionPath,
            SourceTitle = chunk.Title,
            SourceUrl = chunk.Document.SourceUrl,
            ParameterNamesJson = chunk.ParameterNamesJson,
            Text = chunk.Text,
            Citation = $"[fact] {chunk.Title} ¡ª {chunk.SectionPath} ({chunk.ProductVersion}) {chunk.Document.SourceUrl}",
            Vector = ReadOnlyMemory<float>.Empty
        };
    }

    private static IReadOnlyList<string> DeserializeList(string json)
    {
        try
        {
            return JsonSerializer.Deserialize<string[]>(json) ?? [];
        }
        catch
        {
            return [];
        }
    }

    [GeneratedRegex(@"\b[a-z][a-z0-9_]+\b", RegexOptions.IgnoreCase)]
    private static partial Regex SnakeCaseTokenRegex();

    [GeneratedRegex(@"\b[a-z]{4,}\b", RegexOptions.IgnoreCase)]
    private static partial Regex WordRegex();

    private sealed record ScoredRecord(RagKnowledgeVectorRecord Record, double Score);
}

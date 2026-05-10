using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using AIDbOptimize.Infrastructure.Rag.VectorData;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.VectorData;
using Pgvector;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public interface IRagKnowledgeQueryService
{
    Task<RagKnowledgeQueryResult> QueryAsync(
        InvestigationPlan plan,
        DbConfigEvidencePack evidence,
        CancellationToken cancellationToken = default);
}

public sealed record RagKnowledgeQueryResult(
    IReadOnlyList<RetrievedKnowledgeItem> KnowledgeItems,
    IReadOnlyList<DbConfigCollectionMetadataItem> MetadataItems,
    IReadOnlyList<string> Warnings);

public sealed partial class RagKnowledgeQueryService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    VectorStore vectorStore,
    IRagEmbeddingService embeddingService,
    RagQueryOptions queryOptions) : IRagKnowledgeQueryService
{
    public async Task<RagKnowledgeQueryResult> QueryAsync(
        InvestigationPlan plan,
        DbConfigEvidencePack evidence,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var engine = NormalizeEngine(evidence.Engine);
        var knowledgeCollection = vectorStore.GetCollection<Guid, RagKnowledgeVectorRecord>(RagVectorStore.KnowledgeCollectionName);
        var hasFactCorpus = await dbContext.RagDocumentChunks
            .AsNoTracking()
            .Include(item => item.Document)
            .AnyAsync(
                item => item.Document.IsActive
                    && item.Document.Engine == engine
                    && item.Document.DocumentType == RagDocumentType.Fact,
                cancellationToken);
        var caseMatches = await QueryCaseKnowledgeAsync(dbContext, engine, evidence, queryOptions.CaseTopK, cancellationToken);

        if (!hasFactCorpus)
        {
            return new RagKnowledgeQueryResult(
                caseMatches,
                BuildMetadata(caseMatches.Count == 0 ? "empty-corpus" : "case-hit", plan, 0, caseMatches.Count),
                []);
        }

        var queryVector = await embeddingService.GenerateAsync(BuildQueryText(plan, evidence), cancellationToken);
        var canUseVector = queryVector is not null
            && !string.Equals(dbContext.Database.ProviderName, "Microsoft.EntityFrameworkCore.InMemory", StringComparison.OrdinalIgnoreCase)
            && await dbContext.RagDocumentChunks.AnyAsync(
                item => item.Document.IsActive
                    && item.Document.Engine == engine
                    && item.Document.DocumentType == RagDocumentType.Fact
                    && item.Embedding != null,
                cancellationToken);
        var factMatches = await SearchFactKnowledgeAsync(
            knowledgeCollection,
            engine,
            BuildQueryText(plan, evidence),
            queryOptions.FactTopK,
            cancellationToken);
        var scoredItems = factMatches
            .Concat(caseMatches)
            .ToArray();

        return new RagKnowledgeQueryResult(
            scoredItems,
            BuildMetadata(
                scoredItems.Length == caseMatches.Count
                        ? (caseMatches.Count == 0 ? "no-match" : "case-hit")
                        : caseMatches.Count == 0
                            ? (canUseVector ? "vector-hit" : "facts-hit")
                            : (canUseVector ? "vector-and-cases-hit" : "facts-and-cases-hit"),
                plan,
                factMatches.Count,
                caseMatches.Count),
            []);
    }

    private static IReadOnlyList<DbConfigCollectionMetadataItem> BuildMetadata(
        string status,
        InvestigationPlan plan,
        int factResultCount,
        int caseResultCount)
    {
        return
        [
            new DbConfigCollectionMetadataItem(
                "rag_status",
                status,
                "Current RAG retrieval outcome for the workflow."),
            new DbConfigCollectionMetadataItem(
                "rag_hint_count",
                plan.RetrievalHints.Count.ToString(),
                "Number of retrieval hints emitted by the investigation planner."),
            new DbConfigCollectionMetadataItem(
                "rag_result_count",
                (factResultCount + caseResultCount).ToString(),
                "Number of retrieved knowledge items returned before diagnosis."),
            new DbConfigCollectionMetadataItem(
                "rag_fact_result_count",
                factResultCount.ToString(),
                "Number of fact knowledge items returned before diagnosis."),
            new DbConfigCollectionMetadataItem(
                "rag_case_result_count",
                caseResultCount.ToString(),
                "Number of case knowledge items returned before diagnosis.")
        ];
    }

    private static string NormalizeEngine(AIDbOptimize.Domain.Mcp.Enums.DatabaseEngine engine)
    {
        return engine == AIDbOptimize.Domain.Mcp.Enums.DatabaseEngine.MySql
            ? "mysql"
            : "postgresql";
    }

    private static string BuildQueryText(InvestigationPlan plan, DbConfigEvidencePack evidence)
    {
        var references = evidence.EvidenceItems.Select(item => item.Reference);
        return string.Join("\n", references.Concat(plan.RetrievalHints));
    }

    private static RetrievedKnowledgeItem BuildKnowledgeItem(RagKnowledgeVectorRecord record, double score)
    {
        var parameterNames = DeserializeList(record.ParameterNamesJson);
        var snippet = record.Text.Length <= 240 ? record.Text : $"{record.Text[..240]}...";
        return new RetrievedKnowledgeItem(
            record.Key.ToString(),
            "fact",
            record.Citation,
            record.SourceTitle,
            record.SourceTitle,
            snippet,
            record.SourceUrl,
            record.SectionPath,
            record.Topic,
            parameterNames,
            score,
            record.Citation);
    }

    private static async Task<IReadOnlyList<RetrievedKnowledgeItem>> SearchFactKnowledgeAsync(
        VectorStoreCollection<Guid, RagKnowledgeVectorRecord> knowledgeCollection,
        string engine,
        string queryText,
        int topK,
        CancellationToken cancellationToken)
    {
        var results = new List<RetrievedKnowledgeItem>(topK);
        var searchOptions = new VectorSearchOptions<RagKnowledgeVectorRecord>
        {
            Filter = record => record.SourceType == "fact" && record.Engine == engine
        };
        await foreach (var result in knowledgeCollection.SearchAsync(queryText, topK, searchOptions, cancellationToken))
        {
            results.Add(BuildKnowledgeItem(result.Record, result.Score ?? 0d));
        }

        return results;
    }

    private static async Task<IReadOnlyList<RetrievedKnowledgeItem>> QueryCaseKnowledgeAsync(
        ControlPlaneDbContext dbContext,
        string engine,
        DbConfigEvidencePack evidence,
        int caseTopK,
        CancellationToken cancellationToken)
    {
        var references = evidence.EvidenceItems
            .Select(item => item.Reference)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        var cases = await dbContext.RagCaseRecords
            .AsNoTracking()
            .Include(item => item.EvidenceLinks)
            .Where(item => item.Engine == engine)
            .ToListAsync(cancellationToken);

        return cases
            .Select(item => new
            {
                Entity = item,
                MatchCount = item.EvidenceLinks.Count(link => references.Contains(link.EvidenceReference))
            })
            .Where(item => item.MatchCount > 0)
            .OrderByDescending(item => item.MatchCount)
            .ThenByDescending(item => item.Entity.CreatedAt)
            .Take(caseTopK)
            .Select(item => BuildCaseKnowledgeItem(item.Entity, item.MatchCount))
            .ToArray();
    }

    private static RetrievedKnowledgeItem BuildCaseKnowledgeItem(RagCaseRecordEntity caseRecord, int matchCount)
    {
        var historyUrl = $"/api/history/{caseRecord.WorkflowSessionId}";
        var reference = $"[case] {caseRecord.ProblemType} — workflow-session:{caseRecord.WorkflowSessionId} ({caseRecord.CreatedAt:yyyy-MM-dd}) {historyUrl}";
        var citation = $"[case] {caseRecord.ProblemType} — workflow-session:{caseRecord.WorkflowSessionId} ({caseRecord.CreatedAt:yyyy-MM-dd}) {historyUrl}";
        return new RetrievedKnowledgeItem(
            caseRecord.Id.ToString(),
            "case",
            reference,
            caseRecord.ProblemType,
            caseRecord.Summary,
            caseRecord.Summary,
            historyUrl,
            $"workflow-session:{caseRecord.WorkflowSessionId}",
            caseRecord.ProblemType,
            [],
            matchCount,
            citation);
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
}

public sealed class WorkflowRagContextAssembler(WorkflowDocumentContextProvider documentContextProvider, RagQueryOptions queryOptions)
{
    public async Task<DbConfigEvidencePack> EnrichAsync(
        InvestigationPlan plan,
        DbConfigEvidencePack evidence,
        CancellationToken cancellationToken = default)
    {
        var queryResult = await documentContextProvider.GetContextAsync(plan, evidence, cancellationToken);
        var metadata = evidence.CollectionMetadata.ToList();
        foreach (var item in queryResult.MetadataItems)
        {
            UpsertMetadata(metadata, item.Name, item.Value, item.Description ?? string.Empty);
        }

        var externalKnowledgeItems = evidence.ExternalKnowledgeItems
            .Concat(queryResult.KnowledgeItems.Select(MapKnowledgeItem))
            .GroupBy(item => item.Reference, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.Last())
            .Take(queryOptions.MaxInjectTotal)
            .ToArray();
        UpsertMetadata(
            metadata,
            "external_knowledge_count",
            externalKnowledgeItems.Length.ToString(),
            "Number of supplemental knowledge items injected before diagnosis.");

        return new DbConfigEvidencePack(
            evidence.Engine,
            evidence.DatabaseName,
            evidence.Source,
            evidence.Recommendations,
            evidence.EvidenceItems,
            evidence.Warnings.Concat(queryResult.Warnings).Distinct(StringComparer.Ordinal).ToArray(),
            evidence.ConfigurationItems,
            evidence.RuntimeMetricItems,
            evidence.HostContextItems,
            evidence.ObservabilityItems,
            evidence.MissingContextItems,
            metadata,
            externalKnowledgeItems);
    }

    private static DbConfigEvidenceItem MapKnowledgeItem(RetrievedKnowledgeItem item)
    {
        return new DbConfigEvidenceItem(
            item.SourceType,
            item.Reference,
            item.Summary,
            "externalKnowledge",
            item.Snippet,
            item.Citation,
            null,
            "rag",
            null,
            null,
            false,
            "rag-query");
    }

    private static void UpsertMetadata(
        List<DbConfigCollectionMetadataItem> metadata,
        string name,
        string value,
        string description)
    {
        var existing = metadata.FirstOrDefault(item => string.Equals(item.Name, name, StringComparison.OrdinalIgnoreCase));
        if (existing is not null)
        {
            metadata.Remove(existing);
        }

        metadata.Add(new DbConfigCollectionMetadataItem(name, value, description));
    }
}

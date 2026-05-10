using AIDbOptimize.DataInit.Options;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Ingestion;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace AIDbOptimize.DataInit.Services;

public sealed class RagSeedCorpusInitializer(
    SeedPreloadCommand seedPreloadCommand,
    RagKnowledgeIngestionService ragKnowledgeIngestionService,
    IOptions<RagSeedOptions> options,
    ILogger<RagSeedCorpusInitializer> logger,
    IHostEnvironment hostEnvironment)
{
    public async Task RunAsync(CancellationToken cancellationToken = default)
    {
        if (!options.Value.Enabled)
        {
            logger.LogInformation("RAG seed preload disabled, skip corpus download.");
            return;
        }

        var corpusRootPath = ResolveCorpusRootPath(options.Value.CorpusRootPath, hostEnvironment.ContentRootPath);
        var preloadResult = await seedPreloadCommand.ExecuteAsync(corpusRootPath, cancellationToken);
        var ingestionResult = await ragKnowledgeIngestionService.IngestAsync(corpusRootPath, cancellationToken);
        logger.LogInformation(
            "RAG seed preload completed. Downloaded {DownloadedCount} files into {CorpusRootPath}; ingested {DocumentCount} documents and {ChunkCount} chunks.",
            preloadResult.DownloadedCount,
            ingestionResult.DocumentCount,
            ingestionResult.ChunkCount,
            corpusRootPath);
    }

    private static string ResolveCorpusRootPath(string? configuredPath, string contentRootPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.GetFullPath(configuredPath);
        }

        return Path.GetFullPath(Path.Combine(contentRootPath, "..", "..", "docs", "rag"));
    }
}

using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Rag.Ingestion;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using AIDbOptimize.Infrastructure.Tests.Testing;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagKnowledgeIngestionServiceTests
{
    [Fact]
    public async Task IngestAsync_PersistsDocumentsAndChunks_FromCorpusDirectory()
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"rag-ingest-{Guid.NewGuid():N}");
        Directory.CreateDirectory(Path.Combine(rootPath, "facts", "mysql"));
        var sourcePath = Path.Combine(rootPath, "facts", "mysql", "mysql__mysql__memory__server-system-variables.md");
        await File.WriteAllTextAsync(sourcePath, """
        ---
        source_title: Sample
        source_url: https://example.com/mysql
        ---
        # Buffer Pool

        Use a larger buffer pool for read-heavy workloads.

        ## Constraints

        Do not exceed host memory.
        """);

        try
        {
            var databaseName = $"rag-ingest-{Guid.NewGuid():N}";
            var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
                .UseInMemoryDatabase(databaseName)
                .Options;
            var factory = new TestDbContextFactory(options);
            var service = new RagKnowledgeIngestionService(
                factory,
                new CorpusPreprocessor(),
                new CorpusChunker(),
                new CorpusChunkMetadataExtractor(),
                new NullRagEmbeddingService(),
                new RagPreparedArtifactWriter());

            var result = await service.IngestAsync(rootPath);
            await using var assertionContext = new ControlPlaneDbContext(options);

            Assert.Equal(1, result.DocumentCount);
            Assert.True(result.ChunkCount >= 1);
            Assert.True(result.PreparedFileCount >= 2);
            Assert.Single(assertionContext.RagDocuments);
            Assert.NotEmpty(assertionContext.RagDocumentChunks);
            Assert.Contains("memory", assertionContext.RagDocumentChunks.First().KeywordsJson, StringComparison.OrdinalIgnoreCase);
            var preparedDir = Path.Combine(rootPath, "prepared", "facts", "mysql");
            Assert.True(Directory.Exists(preparedDir));
            Assert.NotEmpty(Directory.GetFiles(preparedDir, "*__metadata.json", SearchOption.TopDirectoryOnly));
            var chunkFile = Directory.GetFiles(preparedDir, "*__chunk-0001.md", SearchOption.TopDirectoryOnly).Single();
            var chunkContent = await File.ReadAllTextAsync(chunkFile);
            Assert.Contains("source_url: https://example.com/mysql", chunkContent, StringComparison.Ordinal);
            Assert.Contains("section_path:", chunkContent, StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(rootPath))
            {
                Directory.Delete(rootPath, recursive: true);
            }
        }
    }

    private sealed class TestDbContextFactory(DbContextOptions<ControlPlaneDbContext> options) : IDbContextFactory<ControlPlaneDbContext>
    {
        public ControlPlaneDbContext CreateDbContext()
        {
            return new ControlPlaneDbContext(options);
        }

        public ValueTask<ControlPlaneDbContext> CreateDbContextAsync(CancellationToken cancellationToken = default)
        {
            return ValueTask.FromResult(new ControlPlaneDbContext(options));
        }
    }
}

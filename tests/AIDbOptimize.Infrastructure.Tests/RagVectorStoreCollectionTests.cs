using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using AIDbOptimize.Infrastructure.Rag.VectorData;
using AIDbOptimize.Infrastructure.Tests.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.VectorData;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagVectorStoreCollectionTests
{
    [Fact]
    public async Task SearchAsync_ReturnsFilteredFactMatches()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-vector-search-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            dbContext.RagDocuments.AddRange(
                CreateDocument("mysql", "mysql", "memory", "facts/mysql/mysql__mysql__memory__server-system-variables.md", "https://example.com/mysql", "MySQL Variables"),
                CreateDocument("postgresql", "postgresql", "memory", "facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md", "https://example.com/postgresql", "PostgreSQL Resource"));
            await dbContext.SaveChangesAsync();

            var mysqlDocument = await dbContext.RagDocuments.SingleAsync(item => item.Engine == "mysql");
            var postgresqlDocument = await dbContext.RagDocuments.SingleAsync(item => item.Engine == "postgresql");
            dbContext.RagDocumentChunks.AddRange(
                CreateChunk(mysqlDocument, "innodb_buffer_pool_size", "Memory > innodb_buffer_pool_size", "innodb_buffer_pool_size controls InnoDB cache sizing.", """["innodb_buffer_pool_size"]""", """["mysql","memory","innodb_buffer_pool_size"]"""),
                CreateChunk(postgresqlDocument, "shared_buffers", "Memory > shared_buffers", "shared_buffers controls shared memory.", """["shared_buffers"]""", """["postgresql","memory","shared_buffers"]"""));
            await dbContext.SaveChangesAsync();
        }

        var collection = CreateCollection(options);
        var searchOptions = new VectorSearchOptions<RagKnowledgeVectorRecord>
        {
            Filter = record => record.Engine == "mysql"
        };

        var results = new List<VectorSearchResult<RagKnowledgeVectorRecord>>();
        await foreach (var item in collection.SearchAsync("innodb_buffer_pool_size memory", 5, searchOptions))
        {
            results.Add(item);
        }

        var result = Assert.Single(results);
        Assert.Equal("mysql", result.Record.Engine);
        Assert.Equal("fact", result.Record.SourceType);
        Assert.Contains("innodb_buffer_pool_size", result.Record.Citation, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task GetAsync_ReturnsFactByKey()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-vector-get-{Guid.NewGuid():N}")
            .Options;
        Guid chunkId;
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            var document = CreateDocument("mysql", "mysql", "memory", "facts/mysql/mysql__mysql__memory__server-system-variables.md", "https://example.com/mysql", "MySQL Variables");
            dbContext.RagDocuments.Add(document);
            var chunk = CreateChunk(document, "innodb_buffer_pool_size", "Memory > innodb_buffer_pool_size", "innodb_buffer_pool_size controls InnoDB cache sizing.", """["innodb_buffer_pool_size"]""", """["mysql","memory","innodb_buffer_pool_size"]""");
            chunkId = chunk.Id;
            dbContext.RagDocumentChunks.Add(chunk);
            await dbContext.SaveChangesAsync();
        }

        var collection = CreateCollection(options);
        var result = await collection.GetAsync(chunkId);

        Assert.NotNull(result);
        Assert.Equal(chunkId, result!.Key);
        Assert.Equal("mysql", result.Engine);
        Assert.Contains("https://example.com/mysql", result.Citation, StringComparison.OrdinalIgnoreCase);
    }

    private static RagVectorStoreCollection CreateCollection(DbContextOptions<ControlPlaneDbContext> options)
    {
        var dbContextFactory = new TestDbContextFactory(options);
        var embeddingService = new NullRagEmbeddingService();
        var services = new ServiceCollection()
            .AddSingleton<IDbContextFactory<ControlPlaneDbContext>>(dbContextFactory)
            .AddSingleton<IRagEmbeddingService>(embeddingService)
            .BuildServiceProvider();

        return new RagVectorStoreCollection(services);
    }

    private static RagDocumentEntity CreateDocument(
        string engine,
        string vendor,
        string topic,
        string sourcePath,
        string sourceUrl,
        string sourceTitle)
    {
        return new RagDocumentEntity
        {
            Id = Guid.NewGuid(),
            DocumentType = RagDocumentType.Fact,
            Engine = engine,
            Vendor = vendor,
            Topic = topic,
            SourcePath = sourcePath,
            SourceUrl = sourceUrl,
            SourceTitle = sourceTitle,
            ContentHash = Guid.NewGuid().ToString("N"),
            CapturedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
    }

    private static RagDocumentChunkEntity CreateChunk(
        RagDocumentEntity document,
        string title,
        string sectionPath,
        string text,
        string parameterNamesJson,
        string keywordsJson)
    {
        return new RagDocumentChunkEntity
        {
            Id = Guid.NewGuid(),
            Document = document,
            ChunkKey = $"{title}-chunk-1",
            Title = title,
            SectionPath = sectionPath,
            Text = text,
            ProductVersion = "current",
            AppliesTo = document.Engine,
            ParameterNamesJson = parameterNamesJson,
            KeywordsJson = keywordsJson,
            CreatedAt = DateTimeOffset.UtcNow
        };
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

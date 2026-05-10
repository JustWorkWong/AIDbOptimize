using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using AIDbOptimize.Infrastructure.Rag.VectorData;
using AIDbOptimize.Infrastructure.Tests.Testing;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Microsoft.Agents.AI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.AI;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.VectorData;
using Pgvector;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagPgvectorIntegrationTests : IClassFixture<PgvectorDockerFixture>
{
    private readonly PgvectorDockerFixture _fixture;

    public RagPgvectorIntegrationTests(PgvectorDockerFixture fixture)
    {
        _fixture = fixture;
    }

    [Fact]
    public async Task RagVectorStoreCollection_SearchAsync_UsesPgvectorOrdering()
    {
        var databaseConnectionString = await _fixture.CreateDatabaseAsync();
        var services = BuildServices(databaseConnectionString, new DeterministicRagEmbeddingService());
        await using var dbContext = await services.GetRequiredService<IDbContextFactory<ControlPlaneDbContext>>().CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();

        var expectedChunkId = await SeedFactChunksAsync(dbContext);
        var collection = new RagVectorStoreCollection(services);

        var results = new List<Microsoft.Extensions.VectorData.VectorSearchResult<RagKnowledgeVectorRecord>>();
        await foreach (var result in collection.SearchAsync("shared_buffers vector query", 2))
        {
            results.Add(result);
        }

        var top = Assert.Single(results.Take(1));
        Assert.Equal(expectedChunkId, top.Record.Key);
        Assert.Equal("fact", top.Record.SourceType);
    }

    [Fact]
    public async Task RagRetrievedKnowledgeContextProvider_InjectsPostgreSqlKnowledgeIntoMafAgentPipeline()
    {
        var databaseConnectionString = await _fixture.CreateDatabaseAsync();
        var services = BuildServices(databaseConnectionString, new DeterministicRagEmbeddingService());
        await using var dbContext = await services.GetRequiredService<IDbContextFactory<ControlPlaneDbContext>>().CreateDbContextAsync();
        await dbContext.Database.MigrateAsync();
        await SeedFactChunksAsync(dbContext);

        var queryService = ActivatorUtilities.CreateInstance<RagKnowledgeQueryService>(services);
        var assembler = new WorkflowRagContextAssembler(new WorkflowDocumentContextProvider(queryService), new RagQueryOptions());
        var plan = new InvestigationPlan(
            "plan-1",
            "postgresql-default",
            "1.0.0",
            "postgresql-investigation",
            "1.0.0",
            "postgresql",
            [],
            [],
            [],
            [],
            "policy",
            ["shared_buffers"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            [],
            [
                new DbConfigEvidenceItem("configuration", "shared_buffers", "Current shared_buffers value.")
            ],
            []);
        var enriched = await assembler.EnrichAsync(plan, evidence);
        Assert.NotEmpty(enriched.ExternalKnowledgeItems);

        var recordingClient = new RecordingChatClient();
        var agent = new ChatClientAgent(
            recordingClient,
            new ChatClientAgentOptions
            {
                Name = "diagnosis-test",
                Description = "diagnosis-test",
                AIContextProviders = [new RagRetrievedKnowledgeContextProvider(enriched)]
            },
            NullLoggerFactory.Instance,
            services);

        _ = await agent.RunAsync("return json", session: null, cancellationToken: CancellationToken.None);

        Assert.NotNull(recordingClient.LastMessages);
        Assert.Contains(recordingClient.LastMessages!, item => item.Role == ChatRole.System);
        Assert.Contains(recordingClient.LastMessages!, item => item.Text?.Contains("shared_buffers", StringComparison.OrdinalIgnoreCase) == true);
    }

    private static ServiceProvider BuildServices(string connectionString, IRagEmbeddingService embeddingService)
    {
        return new ServiceCollection()
            .AddSingleton<IDbContextFactory<ControlPlaneDbContext>>(
                new TestDbContextFactory(
                    new DbContextOptionsBuilder<ControlPlaneDbContext>()
                        .UseNpgsql(connectionString, options => options.UseVector())
                        .Options))
            .AddSingleton(embeddingService)
            .AddSingleton<VectorStore, RagVectorStore>()
            .AddSingleton(new RagQueryOptions())
            .BuildServiceProvider();
    }

    private static async Task<Guid> SeedFactChunksAsync(ControlPlaneDbContext dbContext)
    {
        var document = new RagDocumentEntity
        {
            Id = Guid.NewGuid(),
            DocumentType = RagDocumentType.Fact,
            Engine = "postgresql",
            Vendor = "postgresql",
            Topic = "memory",
            SourcePath = "facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md",
            SourceUrl = "https://www.postgresql.org/docs/current/runtime-config-resource.html",
            SourceTitle = "Runtime Config Resource",
            ContentHash = Guid.NewGuid().ToString("N"),
            CapturedAt = DateTimeOffset.UtcNow,
            IsActive = true,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        var expectedChunkId = Guid.NewGuid();
        dbContext.RagDocuments.Add(document);
        dbContext.RagDocumentChunks.AddRange(
            new RagDocumentChunkEntity
            {
                Id = expectedChunkId,
                Document = document,
                ChunkKey = "chunk-1",
                Title = "background-cache-note",
                SectionPath = "Memory > background-cache-note",
                Text = "Background cache heuristics for memory pressure.",
                ProductVersion = "current",
                AppliesTo = "postgresql",
                ParameterNamesJson = """["shared_buffers"]""",
                KeywordsJson = """["cache","memory"]""",
                Embedding = new Vector(new[] { 1f, 0f, 0f }),
                CreatedAt = DateTimeOffset.UtcNow
            },
            new RagDocumentChunkEntity
            {
                Id = Guid.NewGuid(),
                Document = document,
                ChunkKey = "chunk-2",
                Title = "shared_buffers",
                SectionPath = "Memory > shared_buffers",
                Text = "shared_buffers shared_buffers shared_buffers textual hint.",
                ProductVersion = "current",
                AppliesTo = "postgresql",
                ParameterNamesJson = """["shared_buffers"]""",
                KeywordsJson = """["shared_buffers","memory"]""",
                Embedding = new Vector(new[] { 0f, 1f, 0f }),
                CreatedAt = DateTimeOffset.UtcNow
            });
        await dbContext.SaveChangesAsync();
        return expectedChunkId;
    }

    private sealed class DeterministicRagEmbeddingService : IRagEmbeddingService
    {
        public bool IsConfigured => true;

        public Task<Vector?> GenerateAsync(string text, CancellationToken cancellationToken = default)
        {
            return Task.FromResult<Vector?>(new Vector(new[] { 1f, 0f, 0f }));
        }
    }

    private sealed class RecordingChatClient : IChatClient
    {
        public IReadOnlyList<ChatMessage>? LastMessages { get; private set; }

        public Task<ChatResponse> GetResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            CancellationToken cancellationToken = default)
        {
            LastMessages = messages.ToArray();
            return Task.FromResult(new ChatResponse([new ChatMessage(ChatRole.Assistant, """{"title":"ok","summary":"ok","recommendations":[],"evidenceItems":[],"missingContextItems":[],"collectionMetadata":[],"warnings":[]}""")]));
        }

        public async IAsyncEnumerable<ChatResponseUpdate> GetStreamingResponseAsync(
            IEnumerable<ChatMessage> messages,
            ChatOptions? options = null,
            [System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
        {
            LastMessages = messages.ToArray();
            await Task.CompletedTask;
            yield break;
        }

        public object? GetService(Type serviceType, object? serviceKey = null)
        {
            return null;
        }

        public void Dispose()
        {
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

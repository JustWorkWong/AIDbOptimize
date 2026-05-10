using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using AIDbOptimize.Infrastructure.Rag.VectorData;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using AIDbOptimize.Infrastructure.Tests.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class WorkflowRagContextAssemblerTests
{
    [Fact]
    public async Task EnrichAsync_WhenNoBackendConfigured_AddsAuditableRagMetadata()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-empty-{Guid.NewGuid():N}")
            .Options;
        var assembler = new WorkflowRagContextAssembler(
            new WorkflowDocumentContextProvider(
                new RagKnowledgeQueryService(
                    new TestDbContextFactory(options),
                    CreateVectorStore(options),
                    new NullRagEmbeddingService(),
                    new RagQueryOptions())),
            new RagQueryOptions());
        var plan = new InvestigationPlan(
            "plan-1",
            "postgresql-default",
            "1.0.0",
            "postgresql-investigation",
            "1.0.0",
            "postgresql",
            [],
            ["postgresql.runtime_config"],
            [],
            ["postgresql-baseline"],
            "block-on-blocking-rules,degrade-on-missing-optional-or-nonblocking-required",
            ["memory", "connections"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            [],
            [],
            [],
            collectionMetadata:
            [
                new DbConfigCollectionMetadataItem("bundle_id", "postgresql-default", "bundle")
            ]);

        var enriched = await assembler.EnrichAsync(plan, evidence);

        Assert.Empty(enriched.ExternalKnowledgeItems);
        Assert.Contains(enriched.CollectionMetadata, item =>
            string.Equals(item.Name, "rag_status", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(item.Value, "empty-corpus", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(enriched.CollectionMetadata, item =>
            string.Equals(item.Name, "rag_hint_count", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(item.Value, "2", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(enriched.CollectionMetadata, item =>
            string.Equals(item.Name, "external_knowledge_count", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(item.Value, "0", StringComparison.OrdinalIgnoreCase));
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

    private static RagVectorStore CreateVectorStore(DbContextOptions<ControlPlaneDbContext> options)
    {
        var dbContextFactory = new TestDbContextFactory(options);
        var embeddingService = new NullRagEmbeddingService();
        var services = new ServiceCollection()
            .AddSingleton<IDbContextFactory<ControlPlaneDbContext>>(dbContextFactory)
            .AddSingleton<IRagEmbeddingService>(embeddingService)
            .BuildServiceProvider();

        return new RagVectorStore(services);
    }
}

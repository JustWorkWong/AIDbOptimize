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
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagKnowledgeQueryServiceTests
{
    [Fact]
    public async Task QueryAsync_ReturnsFactsMatchingEvidenceReferences()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-query-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
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
                ContentHash = "hash",
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagDocuments.Add(document);
            dbContext.RagDocumentChunks.Add(new RagDocumentChunkEntity
            {
                Id = Guid.NewGuid(),
                Document = document,
                ChunkKey = "chunk-1",
                Title = "shared_buffers",
                SectionPath = "Memory > shared_buffers",
                Text = "shared_buffers controls the amount of memory the database server uses for shared memory buffers.",
                ProductVersion = "current",
                AppliesTo = "postgresql",
                ParameterNamesJson = """["shared_buffers"]""",
                KeywordsJson = """["postgresql","memory","shared_buffers"]""",
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        var service = CreateService(options);
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
            ["PostgreSQL memory sizing guidance"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.PostgreSql,
            "appdb",
            "collector",
            [],
            [
                new DbConfigEvidenceItem("configuration", "shared_buffers", "Current shared_buffers value.")
            ],
            []);

        var result = await service.QueryAsync(plan, evidence);

        Assert.Single(result.KnowledgeItems);
        Assert.Contains(result.MetadataItems, item => item.Name == "rag_status" && item.Value == "facts-hit");
        Assert.Contains("shared_buffers", result.KnowledgeItems[0].Snippet, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task QueryAsync_ReturnsCaseKnowledgeMatchingEvidenceReferences()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-query-case-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            var caseRecord = new RagCaseRecordEntity
            {
                Id = Guid.NewGuid(),
                WorkflowSessionId = Guid.NewGuid(),
                Engine = "mysql",
                ProblemType = "memory",
                Outcome = "Completed",
                ReviewStatus = "Approved",
                RecommendationType = "actionableRecommendation",
                Summary = "innodb_buffer_pool_size was previously tuned successfully.",
                CreatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagCaseRecords.Add(caseRecord);
            dbContext.RagCaseEvidenceLinks.Add(new RagCaseEvidenceLinkEntity
            {
                Id = Guid.NewGuid(),
                CaseRecord = caseRecord,
                EvidenceReference = "innodb_buffer_pool_size",
                RecommendationKey = "innodb_buffer_pool_size",
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        var service = CreateService(options);
        var plan = new InvestigationPlan(
            "plan-1",
            "mysql-default",
            "1.0.0",
            "mysql-investigation",
            "1.0.0",
            "mysql",
            [],
            [],
            [],
            [],
            "policy",
            ["MySQL buffer pool tuning history"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [
                new DbConfigEvidenceItem("configuration", "innodb_buffer_pool_size", "Current buffer pool value.")
            ],
            []);

        var result = await service.QueryAsync(plan, evidence);

        Assert.Single(result.KnowledgeItems);
        Assert.Equal("case", result.KnowledgeItems[0].SourceType);
        Assert.Contains(result.MetadataItems, item => item.Name == "rag_status" && item.Value == "case-hit");
    }

    [Fact]
    public async Task QueryAsync_ReturnsFactsAndCases_WhenBothChannelsMatch()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-query-both-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            var document = new RagDocumentEntity
            {
                Id = Guid.NewGuid(),
                DocumentType = RagDocumentType.Fact,
                Engine = "mysql",
                Vendor = "mysql",
                Topic = "memory",
                SourcePath = "facts/mysql/mysql__mysql__memory__server-system-variables.md",
                SourceUrl = "https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html",
                SourceTitle = "Server System Variables",
                ContentHash = "hash",
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagDocuments.Add(document);
            dbContext.RagDocumentChunks.Add(new RagDocumentChunkEntity
            {
                Id = Guid.NewGuid(),
                Document = document,
                ChunkKey = "chunk-1",
                Title = "innodb_buffer_pool_size",
                SectionPath = "Memory > innodb_buffer_pool_size",
                Text = "innodb_buffer_pool_size controls InnoDB cache sizing.",
                ProductVersion = "8.4",
                AppliesTo = "mysql:mysql",
                ParameterNamesJson = """["innodb_buffer_pool_size"]""",
                KeywordsJson = """["mysql","memory","innodb_buffer_pool_size"]""",
                CreatedAt = DateTimeOffset.UtcNow
            });

            var caseRecord = new RagCaseRecordEntity
            {
                Id = Guid.NewGuid(),
                WorkflowSessionId = Guid.NewGuid(),
                Engine = "mysql",
                ProblemType = "memory",
                Outcome = "Completed",
                ReviewStatus = "Approved",
                RecommendationType = "actionableRecommendation",
                Summary = "innodb_buffer_pool_size was previously tuned successfully.",
                CreatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagCaseRecords.Add(caseRecord);
            dbContext.RagCaseEvidenceLinks.Add(new RagCaseEvidenceLinkEntity
            {
                Id = Guid.NewGuid(),
                CaseRecord = caseRecord,
                EvidenceReference = "innodb_buffer_pool_size",
                RecommendationKey = "innodb_buffer_pool_size",
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        var service = CreateService(options);
        var plan = new InvestigationPlan(
            "plan-1",
            "mysql-default",
            "1.0.0",
            "mysql-investigation",
            "1.0.0",
            "mysql",
            [],
            [],
            [],
            [],
            "policy",
            ["MySQL memory tuning guidance"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [
                new DbConfigEvidenceItem("configuration", "innodb_buffer_pool_size", "Current buffer pool value.")
            ],
            []);

        var result = await service.QueryAsync(plan, evidence);

        Assert.Equal(2, result.KnowledgeItems.Count);
        Assert.Contains(result.KnowledgeItems, item => item.SourceType == "fact");
        Assert.Contains(result.KnowledgeItems, item => item.SourceType == "case");
        Assert.Contains(result.MetadataItems, item => item.Name == "rag_status" && item.Value == "facts-and-cases-hit");
    }

    [Fact]
    public async Task QueryAsync_FiltersOutFactsFromDifferentEngine()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-query-filter-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
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
                ContentHash = "hash",
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagDocuments.Add(document);
            dbContext.RagDocumentChunks.Add(new RagDocumentChunkEntity
            {
                Id = Guid.NewGuid(),
                Document = document,
                ChunkKey = "chunk-1",
                Title = "shared_buffers",
                SectionPath = "Memory > shared_buffers",
                Text = "shared_buffers controls shared memory.",
                ProductVersion = "current",
                AppliesTo = "postgresql",
                ParameterNamesJson = """["shared_buffers"]""",
                KeywordsJson = """["postgresql","memory","shared_buffers"]""",
                CreatedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        var service = CreateService(options);
        var plan = new InvestigationPlan(
            "plan-1",
            "mysql-default",
            "1.0.0",
            "mysql-investigation",
            "1.0.0",
            "mysql",
            [],
            [],
            [],
            [],
            "policy",
            ["MySQL memory tuning guidance"]);
        var evidence = new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [
                new DbConfigEvidenceItem("configuration", "shared_buffers", "Different engine evidence reference.")
            ],
            []);

        var result = await service.QueryAsync(plan, evidence);

        Assert.Empty(result.KnowledgeItems);
        Assert.Contains(result.MetadataItems, item => item.Name == "rag_status" && item.Value == "empty-corpus");
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

    private static RagKnowledgeQueryService CreateService(DbContextOptions<ControlPlaneDbContext> options)
    {
        var dbContextFactory = new TestDbContextFactory(options);
        var embeddingService = new NullRagEmbeddingService();
        var services = new ServiceCollection()
            .AddSingleton<IDbContextFactory<ControlPlaneDbContext>>(dbContextFactory)
            .AddSingleton<IRagEmbeddingService>(embeddingService)
            .BuildServiceProvider();

        return new RagKnowledgeQueryService(
            dbContextFactory,
            new RagVectorStore(services),
            embeddingService,
            new RagQueryOptions());
    }
}

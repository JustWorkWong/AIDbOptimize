using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Persistence.Repositories;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagDocumentChunkRepositoryTests
{
    [Fact]
    public async Task ListAsync_FiltersByEngineVendorTopicAndParameterNames()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-repo-{Guid.NewGuid():N}")
            .Options;
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            var matchedDocument = new RagDocumentEntity
            {
                Id = Guid.NewGuid(),
                DocumentType = RagDocumentType.Fact,
                Engine = "mysql",
                Vendor = "mysql",
                Topic = "memory",
                SourcePath = "facts/mysql/mysql__mysql__memory__server-system-variables.md",
                SourceUrl = "https://example.com/mysql",
                SourceTitle = "MySQL Variables",
                ContentHash = "hash-1",
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            var otherDocument = new RagDocumentEntity
            {
                Id = Guid.NewGuid(),
                DocumentType = RagDocumentType.Fact,
                Engine = "postgresql",
                Vendor = "postgresql",
                Topic = "memory",
                SourcePath = "facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md",
                SourceUrl = "https://example.com/postgresql",
                SourceTitle = "PostgreSQL Resource",
                ContentHash = "hash-2",
                CapturedAt = DateTimeOffset.UtcNow,
                IsActive = true,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.RagDocuments.AddRange(matchedDocument, otherDocument);
            dbContext.RagDocumentChunks.AddRange(
                new RagDocumentChunkEntity
                {
                    Id = Guid.NewGuid(),
                    Document = matchedDocument,
                    ChunkKey = "chunk-1",
                    Title = "innodb_buffer_pool_size",
                    SectionPath = "Memory > innodb_buffer_pool_size",
                    Text = "innodb_buffer_pool_size controls InnoDB cache sizing.",
                    ProductVersion = "8.4",
                    AppliesTo = "mysql:mysql",
                    ParameterNamesJson = """["innodb_buffer_pool_size"]""",
                    KeywordsJson = """["mysql","memory","innodb_buffer_pool_size"]""",
                    CreatedAt = DateTimeOffset.UtcNow
                },
                new RagDocumentChunkEntity
                {
                    Id = Guid.NewGuid(),
                    Document = otherDocument,
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

        var repository = new RagDocumentChunkRepository(new TestDbContextFactory(options));
        var result = await repository.ListAsync(
            new RagDocumentChunkFilter(
                Engine: "mysql",
                Vendor: "mysql",
                Topic: "memory",
                ParameterNames: ["innodb_buffer_pool_size"]),
            topK: 5);

        var record = Assert.Single(result);
        Assert.Equal("mysql", record.Engine);
        Assert.Equal("mysql", record.Vendor);
        Assert.Equal("memory", record.Topic);
        Assert.Contains("innodb_buffer_pool_size", record.ParameterNamesJson, StringComparison.OrdinalIgnoreCase);
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

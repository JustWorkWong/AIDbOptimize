using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Workflow;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class HistoricalOptimizationCaseProjectorTests
{
    [Fact]
    public async Task ProjectAsync_ProjectsCompletedWorkflowIntoCaseRecord()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseInMemoryDatabase($"rag-case-{Guid.NewGuid():N}")
            .Options;
        var corpusRootPath = Path.Combine(Path.GetTempPath(), $"rag-case-corpus-{Guid.NewGuid():N}");
        var sessionId = Guid.NewGuid();
        await using (var dbContext = new ControlPlaneDbContext(options))
        {
            dbContext.WorkflowSessions.Add(new WorkflowSessionEntity
            {
                Id = sessionId,
                ConnectionId = Guid.NewGuid(),
                WorkflowName = "DbConfigOptimization",
                EngineType = "maf",
                Status = WorkflowSessionStatus.Succeeded,
                RequestedBy = "tester",
                InputPayloadJson = """{"bundleId":"postgresql-default"}""",
                StateJson = "{}",
                ResultType = "db-config-optimization-report",
                ResultPayloadJson = """
                {
                  "title":"db-config report",
                  "summary":"shared_buffers should be reviewed",
                  "recommendations":[
                    {
                      "key":"shared_buffers",
                      "findingType":"memory",
                      "recommendationType":"actionableRecommendation",
                      "evidenceReferences":["shared_buffers","blks_hit"]
                    }
                  ]
                }
                """,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow,
                CompletedAt = DateTimeOffset.UtcNow
            });
            await dbContext.SaveChangesAsync();
        }

        try
        {
            var projector = new HistoricalOptimizationCaseProjector(new TestDbContextFactory(options));
            var result = await projector.ProjectAsync(corpusRootPath);

            await using var assertionContext = new ControlPlaneDbContext(options);
            var caseRecord = await assertionContext.RagCaseRecords.Include(item => item.EvidenceLinks).SingleAsync();
            Assert.Equal(1, result.ProjectedCount);
            Assert.Equal(1, result.ExportedDocumentCount);
            Assert.Equal("postgresql", caseRecord.Engine);
            Assert.Equal("memory", caseRecord.ProblemType);
            Assert.Equal("actionableRecommendation", caseRecord.RecommendationType);
            Assert.Equal(2, caseRecord.EvidenceLinks.Count);
            var caseFiles = Directory.GetFiles(Path.Combine(corpusRootPath, "cases", "postgresql"), "*.md", SearchOption.TopDirectoryOnly);
            Assert.Single(caseFiles);
            var caseContent = await File.ReadAllTextAsync(caseFiles[0]);
            Assert.Contains($"source_url: /api/history/{sessionId}", caseContent, StringComparison.Ordinal);
        }
        finally
        {
            if (Directory.Exists(corpusRootPath))
            {
                Directory.Delete(corpusRootPath, recursive: true);
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

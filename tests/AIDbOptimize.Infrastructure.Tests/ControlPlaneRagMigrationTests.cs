using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Pgvector.EntityFrameworkCore;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class ControlPlaneRagMigrationTests
{
    [Fact]
    public void ControlPlaneModel_ContainsRagTablesAndEmbeddingColumn()
    {
        var options = new DbContextOptionsBuilder<ControlPlaneDbContext>()
            .UseNpgsql(
                "Host=localhost;Port=15432;Username=postgres;Password=Postgres123!;Database=aidbopt_control_test",
                npgsql => npgsql.UseVector())
            .Options;

        using var dbContext = new ControlPlaneDbContext(options);
        var script = dbContext.Database.GenerateCreateScript();

        Assert.Contains("rag_documents", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rag_document_chunks", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rag_case_records", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("rag_retrieval_snapshots", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("vector", script, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Embedding", script, StringComparison.OrdinalIgnoreCase);
    }
}

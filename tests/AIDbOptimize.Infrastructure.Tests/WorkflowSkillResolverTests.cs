using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class WorkflowSkillResolverTests
{
    [Fact]
    public void Resolve_UsesDefaultBundle_ForMySql()
    {
        var parser = new MarkdownSkillParser();
        var catalog = new WorkflowSkillCatalog(
            Path.Combine("E:", "Db", "docs", "workflow", "skills"),
            parser);
        var resolver = new WorkflowSkillResolver(catalog);

        var resolution = resolver.Resolve(DatabaseEngine.MySql, "db-config-optimization");

        Assert.Equal("mysql-default", resolution.Bundle.BundleId);
        Assert.Equal("mysql-investigation", resolution.Investigation.SkillId);
        Assert.Equal("mysql-diagnosis", resolution.Diagnosis.SkillId);
    }

    [Fact]
    public void Resolve_RejectsBundleVersionMismatch()
    {
        var parser = new MarkdownSkillParser();
        var catalog = new WorkflowSkillCatalog(
            Path.Combine("E:", "Db", "docs", "workflow", "skills"),
            parser);
        var resolver = new WorkflowSkillResolver(catalog);

        var error = Assert.Throws<InvalidOperationException>(() =>
            resolver.Resolve(
                DatabaseEngine.MySql,
                "db-config-optimization",
                requestedBundleId: "mysql-default",
                requestedBundleVersion: "9.9.9"));

        Assert.Contains("version mismatch", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void Resolve_LoadsRepositoryAssets_WithCrossFileConsistency()
    {
        var parser = new MarkdownSkillParser();
        var catalog = new WorkflowSkillCatalog(
            Path.Combine("E:", "Db", "docs", "workflow", "skills"),
            parser);
        var resolver = new WorkflowSkillResolver(catalog);

        var resolution = resolver.Resolve(
            DatabaseEngine.PostgreSql,
            "db-config-optimization",
            requestedBundleId: "postgresql-default",
            requestedBundleVersion: "1.0.0");

        Assert.Equal("postgresql-default", resolution.Bundle.BundleId);
        Assert.Equal("postgresql", resolution.Investigation.Engine);
        Assert.Equal("db-config-optimization", resolution.Diagnosis.WorkflowType);
        Assert.Equal("1.0.0", resolution.Investigation.SchemaVersion);
    }
}

using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class InvestigationPlannerTests
{
    [Fact]
    public void BuildPlan_ProducesCapabilityDrivenSteps_WithBaselineAndRetrievalHints()
    {
        var parser = new MarkdownSkillParser();
        var catalog = new WorkflowSkillCatalog(
            Path.Combine("E:", "Db", "docs", "workflow", "skills"),
            parser);
        var resolver = new WorkflowSkillResolver(catalog);
        var planner = new InvestigationPlanner(new EvidenceCapabilityCatalog());

        var resolution = resolver.Resolve(DatabaseEngine.MySql, "db-config-optimization");
        var plan = planner.BuildPlan(resolution.Bundle, resolution.Investigation);

        Assert.Equal("mysql-default", plan.BundleId);
        Assert.Contains(plan.Steps, item => string.Equals(item.SkillReference, "engine.version", StringComparison.OrdinalIgnoreCase));
        Assert.Contains(plan.Steps, item => string.Equals(item.CapabilityId, "mysql.config.global-variables", StringComparison.OrdinalIgnoreCase));
        Assert.All(plan.Steps, item => Assert.True(
            new[] { "db-config-snapshot", "host-context" }.Contains(item.CollectorKey, StringComparer.OrdinalIgnoreCase)));
        Assert.Equal(
            plan.Steps.Select(item => item.CapabilityId).Distinct(StringComparer.OrdinalIgnoreCase).Count(),
            plan.Steps.Count);
        Assert.NotEmpty(plan.RetrievalHints);
    }
}

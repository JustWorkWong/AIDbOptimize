using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class SkillPolicyGateTests
{
    [Fact]
    public void Evaluate_ReturnsDegraded_WhenOnlyOptionalEvidenceIsMissing()
    {
        var gate = new SkillPolicyGate();
        var plan = CreatePlan();
        var skill = CreateSkill();
        var summary = new CollectionExecutionSummary(
            plan.PlanId,
            4,
            3,
            [],
            ["mysql.host_resource_snapshot"],
            [],
            []);
        var evidence = CreateEvidencePack();

        var result = gate.Evaluate(plan, skill, summary, evidence);

        Assert.Equal(SkillPolicyDecision.Degraded, result.Decision);
        Assert.Contains(result.Evidence.CollectionMetadata, item =>
            string.Equals(item.Name, "gate_status", StringComparison.OrdinalIgnoreCase) &&
            string.Equals(item.Value, "degraded", StringComparison.OrdinalIgnoreCase));
    }

    [Fact]
    public void Evaluate_ReturnsBlocked_WhenBlockingEvidenceIsMissing()
    {
        var gate = new SkillPolicyGate();
        var plan = CreatePlan();
        var skill = CreateSkill();
        var summary = new CollectionExecutionSummary(
            plan.PlanId,
            4,
            2,
            ["mysql.version_profile"],
            [],
            [],
            []);
        var evidence = CreateEvidencePack();

        var result = gate.Evaluate(plan, skill, summary, evidence);

        Assert.Equal(SkillPolicyDecision.Blocked, result.Decision);
        Assert.False(string.IsNullOrWhiteSpace(result.BlockedReportJson));
        Assert.Contains(result.Evidence.MissingContextItems, item =>
            string.Equals(item.Reference, "mysql.version_profile", StringComparison.OrdinalIgnoreCase));
    }

    private static InvestigationPlan CreatePlan()
    {
        return new InvestigationPlan(
            "plan-1",
            "mysql-default",
            "1.0.0",
            "mysql-investigation",
            "1.0.0",
            "mysql",
            [],
            ["mysql.global_variables", "mysql.version_profile"],
            ["mysql.host_resource_snapshot"],
            ["engine.version"],
            "block-on-blocking-rules,degrade-on-missing-optional-or-nonblocking-required",
            []);
    }

    private static InvestigationSkillDefinition CreateSkill()
    {
        return new InvestigationSkillDefinition(
            "mysql-investigation",
            "mysql",
            "1.0.0",
            "1.0.0",
            "mysql-default",
            "1.0.0",
            "db-config-optimization",
            ["mysql.global_variables", "mysql.version_profile"],
            ["mysql.host_resource_snapshot"],
            [
                "Block when `mysql.global_variables` is missing because no configuration recommendation can be grounded.",
                "Block when `mysql.version_profile` is missing because rule compatibility cannot be established."
            ],
            [],
            [],
            []);
    }

    private static DbConfigEvidencePack CreateEvidencePack()
    {
        return new DbConfigEvidencePack(
            DatabaseEngine.MySql,
            "orders",
            "collector",
            [],
            [],
            [],
            collectionMetadata:
            [
                new DbConfigCollectionMetadataItem("bundle_id", "mysql-default", "bundle"),
                new DbConfigCollectionMetadataItem("bundle_version", "1.0.0", "bundle version")
            ]);
    }
}

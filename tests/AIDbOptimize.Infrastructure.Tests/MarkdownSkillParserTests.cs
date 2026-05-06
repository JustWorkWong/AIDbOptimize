using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class MarkdownSkillParserTests
{
    [Fact]
    public void ParseInvestigationSkill_ReturnsStructuredDefinition()
    {
        var parser = new MarkdownSkillParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0.0
        schema_version: 1.0.0
        bundle_id: mysql-default
        bundle_version: 1.0.0
        workflow_type: db-config-optimization
        ---
        ## Scope
        - workflow: db-config-optimization
        ## Required Evidence
        - mysql.global_variables
        ## Optional Evidence
        - mysql.host_resource_snapshot
        Free-form prose should be ignored.
        ## Blocking Rules
        - block if missing core variables
        ## Investigation Questions
        - which settings constrain throughput
        ## Collection Hints
        - prefer effective values
        ## Retrieval Hints
        - reserved only
        """;

        var definition = parser.ParseInvestigationSkill(markdown);

        Assert.Equal("mysql-investigation", definition.SkillId);
        Assert.Equal("1.0.0", definition.SchemaVersion);
        Assert.Contains("mysql.global_variables", definition.RequiredEvidence);
        Assert.DoesNotContain(definition.RequiredEvidence, item => item.Contains("effective MySQL", StringComparison.OrdinalIgnoreCase));
        Assert.Contains("reserved only", definition.RetrievalHints);
        Assert.DoesNotContain("Free-form prose should be ignored.", definition.OptionalEvidence);
    }

    [Fact]
    public void ParseInvestigationSkill_NormalizesEvidenceReference_WhenLineContainsDescription()
    {
        var parser = new MarkdownSkillParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0.0
        schema_version: 1.0.0
        bundle_id: mysql-default
        bundle_version: 1.0.0
        workflow_type: db-config-optimization
        ---
        ## Scope
        - workflow: db-config-optimization
        ## Required Evidence
        - `mysql.version_profile`: version and edition context
        ## Optional Evidence
        - `mysql.host_resource_snapshot`: optional host context
        ## Blocking Rules
        - block
        ## Investigation Questions
        - question
        ## Collection Hints
        - hint
        ## Retrieval Hints
        - reserved
        """;

        var definition = parser.ParseInvestigationSkill(markdown);

        Assert.Equal(["mysql.version_profile"], definition.RequiredEvidence);
        Assert.Equal(["mysql.host_resource_snapshot"], definition.OptionalEvidence);
    }

    [Fact]
    public void ParseBundle_ReturnsStructuredDefinition()
    {
        var parser = new MarkdownSkillParser();
        var markdown = """
        ---
        bundle_id: mysql-default
        bundle_version: 1.0.0
        workflow_type: db-config-optimization
        engine: mysql
        investigation_skill_id: mysql-investigation
        investigation_skill_version: 1.0.0
        diagnosis_skill_id: mysql-diagnosis
        diagnosis_skill_version: 1.0.0
        schema_version: 1.0.0
        ---
        ## Scope
        - mysql default bundle
        ## Skill Mapping
        - investigation: mysql-investigation@1.0.0
        ## Compatibility Contract
        - same bundle version
        ## Evidence Model
        - investigation owns required evidence
        ## RAG Reserved Boundary
        - retrieval cannot satisfy required evidence
        """;

        var bundle = parser.ParseBundle(markdown);

        Assert.Equal("mysql-default", bundle.BundleId);
        Assert.Equal("mysql-investigation", bundle.InvestigationSkillId);
    }

    [Fact]
    public void ParseInvestigationSkill_FromRepositoryAsset_Succeeds()
    {
        var parser = new MarkdownSkillParser();
        var markdown = File.ReadAllText(Path.Combine(
            "E:",
            "Db",
            "docs",
            "workflow",
            "skills",
            "mysql-investigation",
            "SKILL.md"));

        var definition = parser.ParseInvestigationSkill(markdown);

        Assert.Equal("mysql-default", definition.BundleId);
        Assert.Equal("db-config-optimization", definition.WorkflowType);
        Assert.Contains(
            definition.RequiredEvidence,
            item => item.Contains("mysql.global_variables", StringComparison.Ordinal));
    }
}

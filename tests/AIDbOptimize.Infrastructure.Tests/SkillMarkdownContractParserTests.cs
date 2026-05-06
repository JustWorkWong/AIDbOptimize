using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class SkillMarkdownContractParserTests
{
    private static readonly string[] RequiredMetadataKeys =
    [
        "skill_id",
        "skill_type",
        "engine",
        "skill_version",
        "schema_version",
        "bundle_id",
        "bundle_version"
    ];

    private static readonly string[] RequiredSectionNames =
    [
        "Scope",
        "Required Evidence",
        "Optional Evidence"
    ];

    [Fact]
    public void ParseAndValidate_ReturnsMetadataAndSections_ForValidMarkdown()
    {
        var parser = new SkillMarkdownContractParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0
        schema_version: 1
        bundle_id: mysql-default
        bundle_version: 1.0
        ---
        # MySQL Investigation Skill

        ## Scope
        - workflow: db-config-optimization

        ## Required Evidence
        - config.max_connections

        ## Optional Evidence
        Extra explanatory prose is allowed here.
        """;

        var document = parser.ParseAndValidate(markdown, RequiredMetadataKeys, RequiredSectionNames);

        Assert.Equal("mysql-investigation", document.Metadata["skill_id"]);
        Assert.Contains("db-config-optimization", document.Sections["Scope"], StringComparison.Ordinal);
        Assert.Contains("config.max_connections", document.Sections["Required Evidence"], StringComparison.Ordinal);
        Assert.Contains("Extra explanatory prose", document.Sections["Optional Evidence"], StringComparison.Ordinal);
    }

    [Fact]
    public void ParseAndValidate_RejectsMissingRequiredMetadata()
    {
        var parser = new SkillMarkdownContractParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0
        schema_version: 1
        bundle_id: mysql-default
        ---
        ## Scope
        value
        ## Required Evidence
        value
        ## Optional Evidence
        value
        """;

        var error = Assert.Throws<InvalidOperationException>(() =>
            parser.ParseAndValidate(markdown, RequiredMetadataKeys, RequiredSectionNames));

        Assert.Contains("bundle_version", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ParseAndValidate_RejectsMissingRequiredSection()
    {
        var parser = new SkillMarkdownContractParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0
        schema_version: 1
        bundle_id: mysql-default
        bundle_version: 1.0
        ---
        ## Scope
        value

        ## Required Evidence
        value
        """;

        var error = Assert.Throws<InvalidOperationException>(() =>
            parser.ParseAndValidate(markdown, RequiredMetadataKeys, RequiredSectionNames));

        Assert.Contains("Optional Evidence", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void ParseAndValidate_DoesNotDependOnSectionOrder()
    {
        var parser = new SkillMarkdownContractParser();
        var markdown = """
        ---
        skill_id: postgresql-diagnosis
        skill_type: diagnosis
        engine: postgresql
        skill_version: 1.0
        schema_version: 1
        bundle_id: postgresql-default
        bundle_version: 1.0
        ---
        ## Optional Evidence
        value

        ## Scope
        value

        ## Required Evidence
        value
        """;

        var document = parser.ParseAndValidate(markdown, RequiredMetadataKeys, RequiredSectionNames);

        Assert.Equal(3, document.Sections.Count);
        Assert.Equal("value", document.Sections["Scope"]);
    }

    [Fact]
    public void ParseAndValidate_RejectsUnclosedFrontMatter()
    {
        var parser = new SkillMarkdownContractParser();
        var markdown = """
        ---
        skill_id: mysql-investigation
        skill_type: investigation
        engine: mysql
        skill_version: 1.0
        schema_version: 1
        bundle_id: mysql-default
        bundle_version: 1.0
        """;

        var error = Assert.Throws<InvalidOperationException>(() =>
            parser.ParseAndValidate(markdown, RequiredMetadataKeys, RequiredSectionNames));

        Assert.Contains("front matter is not closed", error.Message, StringComparison.OrdinalIgnoreCase);
    }
}

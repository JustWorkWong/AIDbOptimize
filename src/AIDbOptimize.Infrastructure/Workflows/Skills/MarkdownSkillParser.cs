namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed class MarkdownSkillParser
{
    private static readonly string[] RequiredBundleMetadata =
    [
        "bundle_id",
        "bundle_version",
        "workflow_type",
        "engine",
        "investigation_skill_id",
        "investigation_skill_version",
        "diagnosis_skill_id",
        "diagnosis_skill_version",
        "schema_version"
    ];

    private static readonly string[] RequiredSkillMetadata =
    [
        "skill_id",
        "skill_type",
        "engine",
        "skill_version",
        "schema_version",
        "bundle_id",
        "bundle_version",
        "workflow_type"
    ];

    private readonly SkillMarkdownContractParser _contractParser = new();

    public SkillBundleDefinition ParseBundle(string markdown)
    {
        var document = _contractParser.ParseAndValidate(
            markdown,
            RequiredBundleMetadata,
            ["Scope", "Skill Mapping", "Compatibility Contract", "Evidence Model", "RAG Reserved Boundary"]);

        return new SkillBundleDefinition(
            document.Metadata["bundle_id"],
            document.Metadata["bundle_version"],
            document.Metadata["workflow_type"],
            document.Metadata["engine"],
            document.Metadata["investigation_skill_id"],
            document.Metadata["investigation_skill_version"],
            document.Metadata["diagnosis_skill_id"],
            document.Metadata["diagnosis_skill_version"],
            document.Metadata["schema_version"],
            ParseList(document.Sections["Scope"]),
            ParseList(document.Sections["Skill Mapping"]),
            ParseList(document.Sections["Compatibility Contract"]),
            ParseList(document.Sections["Evidence Model"]),
            ParseList(document.Sections["RAG Reserved Boundary"]));
    }

    public InvestigationSkillDefinition ParseInvestigationSkill(string markdown)
    {
        var document = _contractParser.ParseAndValidate(
            markdown,
            RequiredSkillMetadata,
            ["Scope", "Required Evidence", "Optional Evidence", "Blocking Rules", "Investigation Questions", "Collection Hints", "Retrieval Hints"]);

        ValidateSkillType(document.Metadata["skill_type"], WorkflowSkillType.Investigation);

        return new InvestigationSkillDefinition(
            document.Metadata["skill_id"],
            document.Metadata["engine"],
            document.Metadata["skill_version"],
            document.Metadata["schema_version"],
            document.Metadata["bundle_id"],
            document.Metadata["bundle_version"],
            document.Metadata["workflow_type"],
            ParseEvidenceReferences(document.Sections["Required Evidence"]),
            ParseEvidenceReferences(document.Sections["Optional Evidence"]),
            ParseList(document.Sections["Blocking Rules"]),
            ParseList(document.Sections["Investigation Questions"]),
            ParseList(document.Sections["Collection Hints"]),
            ParseList(document.Sections["Retrieval Hints"]));
    }

    public DiagnosisSkillDefinition ParseDiagnosisSkill(string markdown)
    {
        var document = _contractParser.ParseAndValidate(
            markdown,
            RequiredSkillMetadata,
            ["Scope", "Output Contract", "Recommendation Rules", "Confidence Rules", "Forbidden Patterns", "Citation Rules"]);

        ValidateSkillType(document.Metadata["skill_type"], WorkflowSkillType.Diagnosis);

        return new DiagnosisSkillDefinition(
            document.Metadata["skill_id"],
            document.Metadata["engine"],
            document.Metadata["skill_version"],
            document.Metadata["schema_version"],
            document.Metadata["bundle_id"],
            document.Metadata["bundle_version"],
            document.Metadata["workflow_type"],
            ParseList(document.Sections["Output Contract"]),
            ParseList(document.Sections["Recommendation Rules"]),
            ParseList(document.Sections["Confidence Rules"]),
            ParseList(document.Sections["Forbidden Patterns"]),
            ParseList(document.Sections["Citation Rules"]));
    }

    private static void ValidateSkillType(string rawSkillType, WorkflowSkillType expectedType)
    {
        var expected = expectedType.ToString().ToLowerInvariant();
        if (!string.Equals(rawSkillType, expected, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Expected skill_type `{expected}` but found `{rawSkillType}`.");
        }
    }

    private static IReadOnlyCollection<string> ParseList(string sectionContent)
    {
        return sectionContent
            .Split('\n', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
            .Where(IsStructuredLine)
            .Select(TrimStructuredPrefix)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
    }

    private static IReadOnlyCollection<string> ParseEvidenceReferences(string sectionContent)
    {
        return ParseList(sectionContent)
            .Select(NormalizeEvidenceReference)
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToArray();
    }

    private static bool IsStructuredLine(string line)
    {
        return line.StartsWith("- ", StringComparison.Ordinal) ||
               System.Text.RegularExpressions.Regex.IsMatch(line, @"^\d+\.\s+");
    }

    private static string TrimStructuredPrefix(string line)
    {
        if (line.StartsWith("- ", StringComparison.Ordinal))
        {
            return line[2..].Trim();
        }

        var match = System.Text.RegularExpressions.Regex.Match(line, @"^\d+\.\s+(.*)$");
        return match.Success ? match.Groups[1].Value.Trim() : line.Trim();
    }

    private static string NormalizeEvidenceReference(string line)
    {
        var fencedMatch = System.Text.RegularExpressions.Regex.Match(line, @"`([^`]+)`");
        if (fencedMatch.Success)
        {
            return fencedMatch.Groups[1].Value.Trim();
        }

        var colonIndex = line.IndexOf(':');
        return colonIndex > 0
            ? line[..colonIndex].Trim()
            : line.Trim();
    }
}

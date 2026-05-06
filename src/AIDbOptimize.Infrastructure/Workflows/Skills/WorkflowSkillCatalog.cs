namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed class WorkflowSkillCatalog
{
    private readonly string _rootPath;
    private readonly MarkdownSkillParser _parser;

    public WorkflowSkillCatalog(string rootPath, MarkdownSkillParser parser)
    {
        _rootPath = rootPath;
        _parser = parser;
    }

    public SkillBundleDefinition LoadBundle(string bundleId, string? bundleVersion = null)
    {
        var path = ResolveAssetPath("bundles", bundleId, "bundle.md", bundleVersion);
        return _parser.ParseBundle(ReadRequiredFile(path));
    }

    public InvestigationSkillDefinition LoadInvestigationSkill(string skillId, string? skillVersion = null)
    {
        var path = ResolveAssetPath(null, skillId, "SKILL.md", skillVersion);
        return _parser.ParseInvestigationSkill(ReadRequiredFile(path));
    }

    public DiagnosisSkillDefinition LoadDiagnosisSkill(string skillId, string? skillVersion = null)
    {
        var path = ResolveAssetPath(null, skillId, "SKILL.md", skillVersion);
        return _parser.ParseDiagnosisSkill(ReadRequiredFile(path));
    }

    private string ResolveAssetPath(
        string? namespaceFolder,
        string assetId,
        string fileName,
        string? version)
    {
        var basePath = string.IsNullOrWhiteSpace(namespaceFolder)
            ? Path.Combine(_rootPath, assetId)
            : Path.Combine(_rootPath, namespaceFolder, assetId);

        if (!string.IsNullOrWhiteSpace(version))
        {
            var versionedPath = Path.Combine(basePath, version, fileName);
            if (File.Exists(versionedPath))
            {
                return versionedPath;
            }
        }

        return Path.Combine(basePath, fileName);
    }

    private static string ReadRequiredFile(string path)
    {
        if (!File.Exists(path))
        {
            throw new InvalidOperationException($"Skill asset file was not found: {path}");
        }

        return File.ReadAllText(path);
    }
}

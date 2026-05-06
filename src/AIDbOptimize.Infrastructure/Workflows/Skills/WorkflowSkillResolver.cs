using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed class WorkflowSkillResolver
{
    private readonly WorkflowSkillCatalog _catalog;

    public WorkflowSkillResolver(WorkflowSkillCatalog catalog)
    {
        _catalog = catalog;
    }

    public WorkflowSkillResolution Resolve(
        DatabaseEngine engine,
        string workflowType,
        string? requestedBundleId = null,
        string? requestedBundleVersion = null)
    {
        var bundleId = string.IsNullOrWhiteSpace(requestedBundleId)
            ? GetDefaultBundleId(engine)
            : requestedBundleId;
        var bundle = _catalog.LoadBundle(bundleId, requestedBundleVersion);

        if (!string.Equals(bundle.WorkflowType, workflowType, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Bundle `{bundle.BundleId}` does not support workflow type `{workflowType}`.");
        }

        if (!string.Equals(bundle.Engine, ToEngineToken(engine), StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Bundle `{bundle.BundleId}` does not support engine `{engine}`.");
        }

        if (!string.IsNullOrWhiteSpace(requestedBundleVersion) &&
            !string.Equals(bundle.BundleVersion, requestedBundleVersion, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Bundle `{bundle.BundleId}` version mismatch. Requested `{requestedBundleVersion}`, actual `{bundle.BundleVersion}`.");
        }

        var investigation = _catalog.LoadInvestigationSkill(
            bundle.InvestigationSkillId,
            bundle.InvestigationSkillVersion);
        var diagnosis = _catalog.LoadDiagnosisSkill(
            bundle.DiagnosisSkillId,
            bundle.DiagnosisSkillVersion);

        ValidateBundleConsistency(bundle, investigation, diagnosis);
        return new WorkflowSkillResolution(bundle, investigation, diagnosis);
    }

    private static void ValidateBundleConsistency(
        SkillBundleDefinition bundle,
        InvestigationSkillDefinition investigation,
        DiagnosisSkillDefinition diagnosis)
    {
        if (!string.Equals(bundle.BundleId, investigation.BundleId, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.BundleVersion, investigation.BundleVersion, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Investigation skill `{investigation.SkillId}` does not match bundle `{bundle.BundleId}@{bundle.BundleVersion}`.");
        }

        if (!string.Equals(bundle.BundleId, diagnosis.BundleId, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.BundleVersion, diagnosis.BundleVersion, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Diagnosis skill `{diagnosis.SkillId}` does not match bundle `{bundle.BundleId}@{bundle.BundleVersion}`.");
        }

        if (!string.Equals(bundle.InvestigationSkillId, investigation.SkillId, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.InvestigationSkillVersion, investigation.Version, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Investigation skill mismatch for bundle `{bundle.BundleId}`.");
        }

        if (!string.Equals(bundle.Engine, investigation.Engine, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.WorkflowType, investigation.WorkflowType, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.SchemaVersion, investigation.SchemaVersion, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Investigation skill `{investigation.SkillId}` does not match bundle engine/workflow/schema.");
        }

        if (!string.Equals(bundle.DiagnosisSkillId, diagnosis.SkillId, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.DiagnosisSkillVersion, diagnosis.Version, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Diagnosis skill mismatch for bundle `{bundle.BundleId}`.");
        }

        if (!string.Equals(bundle.Engine, diagnosis.Engine, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.WorkflowType, diagnosis.WorkflowType, StringComparison.OrdinalIgnoreCase) ||
            !string.Equals(bundle.SchemaVersion, diagnosis.SchemaVersion, StringComparison.OrdinalIgnoreCase))
        {
            throw new InvalidOperationException(
                $"Diagnosis skill `{diagnosis.SkillId}` does not match bundle engine/workflow/schema.");
        }
    }

    private static string GetDefaultBundleId(DatabaseEngine engine)
    {
        return engine switch
        {
            DatabaseEngine.MySql => "mysql-default",
            DatabaseEngine.PostgreSql => "postgresql-default",
            _ => throw new InvalidOperationException($"Unsupported engine `{engine}`.")
        };
    }

    private static string ToEngineToken(DatabaseEngine engine)
    {
        return engine switch
        {
            DatabaseEngine.MySql => "mysql",
            DatabaseEngine.PostgreSql => "postgresql",
            _ => throw new InvalidOperationException($"Unsupported engine `{engine}`.")
        };
    }
}

public sealed record WorkflowSkillResolution(
    SkillBundleDefinition Bundle,
    InvestigationSkillDefinition Investigation,
    DiagnosisSkillDefinition Diagnosis);

using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class EvidenceCapabilityCatalogTests
{
    [Fact]
    public void ResolveCapabilities_IncludesRuleRequiredObservabilityAndVersionFields()
    {
        var catalog = new EvidenceCapabilityCatalog();

        var definitions = catalog.ResolveCapabilities(
            "postgresql",
            ["postgresql.settings", "postgresql.version_profile"]);

        Assert.Contains(definitions, item =>
            string.Equals(item.CapabilityId, "postgresql.config.settings", StringComparison.OrdinalIgnoreCase) &&
            item.RequiredEvidenceReferences.Contains("track_io_timing", StringComparer.OrdinalIgnoreCase) &&
            item.RequiredEvidenceReferences.Contains("shared_preload_libraries", StringComparer.OrdinalIgnoreCase) &&
            item.RequiredEvidenceReferences.Contains("pg_stat_statements_enabled", StringComparer.OrdinalIgnoreCase));
        Assert.Contains(definitions, item =>
            string.Equals(item.CapabilityId, "postgresql.metadata.version-profile", StringComparison.OrdinalIgnoreCase) &&
            item.RequiredEvidenceReferences.Contains("engine_version", StringComparer.OrdinalIgnoreCase));
    }

    [Fact]
    public void GetBaselineCapabilities_UsesNormalizedMetadataNames()
    {
        var catalog = new EvidenceCapabilityCatalog();

        var baseline = catalog.GetBaselineCapabilities("mysql");

        Assert.Contains(baseline, item =>
            string.Equals(item.SkillReference, "deployment.flavor", StringComparison.OrdinalIgnoreCase) &&
            item.RequiredMetadataNames.Contains("deployment.flavor", StringComparer.OrdinalIgnoreCase));
        Assert.Contains(baseline, item =>
            string.Equals(item.SkillReference, "parameter.apply_scope", StringComparison.OrdinalIgnoreCase) &&
            item.RequiredMetadataNames.Contains("parameter.apply_scope", StringComparer.OrdinalIgnoreCase));
        Assert.Contains(baseline, item =>
            string.Equals(item.SkillReference, "parameter.normalized_unit", StringComparison.OrdinalIgnoreCase) &&
            item.RequiredMetadataNames.Contains("parameter.normalized_unit", StringComparer.OrdinalIgnoreCase));
    }
}

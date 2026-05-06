using AIDbOptimize.Infrastructure.Workflows.Skills;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class EvidenceReferenceNormalizerTests
{
    [Fact]
    public void Normalize_ResolvesSkillReferenceToGroundingReference()
    {
        var normalizer = new EvidenceReferenceNormalizer();
        var result = normalizer.Normalize(
            ["config.max_connections", "host.memory.total"],
            [
                new EvidenceCapabilityReference(
                    "config.max_connections",
                    "mysql.config.max_connections",
                    "config:max_connections"),
                new EvidenceCapabilityReference(
                    "host.memory.total",
                    "host.memory.total",
                    "host:memory.total")
            ]);

        var resolved = result.ResolveGroundingReferences(["config.max_connections", "host.memory.total"]);

        Assert.Equal(
            ["config:max_connections", "host:memory.total"],
            resolved);
    }

    [Fact]
    public void Normalize_RejectsMissingRequiredSkillReference()
    {
        var normalizer = new EvidenceReferenceNormalizer();

        var error = Assert.Throws<InvalidOperationException>(() =>
            normalizer.Normalize(
                ["config.max_connections", "host.memory.total"],
                [
                    new EvidenceCapabilityReference(
                        "config.max_connections",
                        "mysql.config.max_connections",
                        "config:max_connections")
                ]));

        Assert.Contains("host.memory.total", error.Message, StringComparison.Ordinal);
    }

    [Fact]
    public void Normalize_RejectsAmbiguousSkillReference()
    {
        var normalizer = new EvidenceReferenceNormalizer();

        var error = Assert.Throws<InvalidOperationException>(() =>
            normalizer.Normalize(
                ["config.max_connections"],
                [
                    new EvidenceCapabilityReference(
                        "config.max_connections",
                        "mysql.config.max_connections",
                        "config:max_connections"),
                    new EvidenceCapabilityReference(
                        "config.max_connections",
                        "mysql.compat.max_connections",
                        "config:max_connections")
                ]));

        Assert.Contains("ambiguous", error.Message, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public void ResolveGroundingReferences_AllowsMissingContextReference_WhenNormalized()
    {
        var normalizer = new EvidenceReferenceNormalizer();
        var result = normalizer.Normalize(
            ["host.memory.total"],
            [
                new EvidenceCapabilityReference(
                    "host.memory.total",
                    "host.memory.total",
                    "missing:host.memory.total",
                    IsMissingContext: true)
            ]);

        var resolved = result.ResolveGroundingReferences(["host.memory.total"]);

        Assert.Equal(["missing:host.memory.total"], resolved);
        Assert.True(result.ReferencesBySkillReference["host.memory.total"].IsMissingContext);
    }
}

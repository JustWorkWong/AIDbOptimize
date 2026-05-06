namespace AIDbOptimize.Infrastructure.Workflows.Skills;

/// <summary>
/// Minimal reference normalizer used by the evidence-ref spike.
/// </summary>
public sealed class EvidenceReferenceNormalizer
{
    public EvidenceReferenceNormalizationResult Normalize(
        IReadOnlyCollection<string> requiredSkillReferences,
        IReadOnlyCollection<EvidenceCapabilityReference> capabilityReferences)
    {
        var mapping = new Dictionary<string, EvidenceCapabilityReference>(StringComparer.OrdinalIgnoreCase);

        foreach (var capabilityReference in capabilityReferences)
        {
            if (mapping.TryGetValue(capabilityReference.SkillReference, out var existing))
            {
                throw new InvalidOperationException(
                    $"Skill reference `{capabilityReference.SkillReference}` is ambiguous between capability `{existing.CapabilityId}` and `{capabilityReference.CapabilityId}`.");
            }

            mapping[capabilityReference.SkillReference] = capabilityReference;
        }

        foreach (var requiredSkillReference in requiredSkillReferences)
        {
            if (!mapping.ContainsKey(requiredSkillReference))
            {
                throw new InvalidOperationException($"Missing capability mapping for required skill reference `{requiredSkillReference}`.");
            }
        }

        return new EvidenceReferenceNormalizationResult(mapping);
    }
}

public sealed class EvidenceReferenceNormalizationResult(
    IReadOnlyDictionary<string, EvidenceCapabilityReference> referencesBySkillReference)
{
    public IReadOnlyDictionary<string, EvidenceCapabilityReference> ReferencesBySkillReference { get; } =
        referencesBySkillReference;

    public IReadOnlyCollection<string> ResolveGroundingReferences(
        IReadOnlyCollection<string> skillReferences)
    {
        var resolved = new List<string>();

        foreach (var skillReference in skillReferences)
        {
            if (!ReferencesBySkillReference.TryGetValue(skillReference, out var reference))
            {
                throw new InvalidOperationException($"Skill reference `{skillReference}` was not normalized.");
            }

            resolved.Add(reference.EvidenceReference);
        }

        return resolved;
    }
}

public sealed record EvidenceCapabilityReference(
    string SkillReference,
    string CapabilityId,
    string EvidenceReference,
    bool IsMissingContext = false);

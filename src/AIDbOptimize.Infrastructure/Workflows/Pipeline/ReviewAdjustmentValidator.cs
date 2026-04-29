using System.Text.Json;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Review adjustment validator.
/// </summary>
public sealed class ReviewAdjustmentValidator
{
    private static readonly HashSet<string> AllowedRiskLevels = new(StringComparer.OrdinalIgnoreCase)
    {
        "low",
        "medium",
        "warning",
        "high",
        "critical"
    };

    public WorkflowReviewAdjustment ValidateAndNormalize(JsonElement adjustments, string? comment)
    {
        var riskOverrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        if (adjustments.ValueKind is JsonValueKind.Null or JsonValueKind.Undefined)
        {
            return new WorkflowReviewAdjustment(riskOverrides);
        }

        if (adjustments.ValueKind != JsonValueKind.Object)
        {
            throw new InvalidOperationException("adjustments must be a JSON object.");
        }

        foreach (var property in adjustments.EnumerateObject())
        {
            if (!string.Equals(property.Name, "riskLevelOverrides", StringComparison.Ordinal))
            {
                throw new InvalidOperationException($"Unsupported adjustment field: {property.Name}");
            }

            if (property.Value.ValueKind != JsonValueKind.Object)
            {
                throw new InvalidOperationException("riskLevelOverrides must be a JSON object.");
            }

            foreach (var overrideProperty in property.Value.EnumerateObject())
            {
                if (overrideProperty.Value.ValueKind != JsonValueKind.String)
                {
                    throw new InvalidOperationException($"Risk override for {overrideProperty.Name} must be a string.");
                }

                var riskLevel = overrideProperty.Value.GetString();
                if (string.IsNullOrWhiteSpace(riskLevel) || !AllowedRiskLevels.Contains(riskLevel))
                {
                    throw new InvalidOperationException($"Unsupported risk level override: {riskLevel}");
                }

                riskOverrides[overrideProperty.Name] = riskLevel;
            }
        }

        if (riskOverrides.Count > 0 && string.IsNullOrWhiteSpace(comment))
        {
            throw new InvalidOperationException("A comment is required when adjustments are supplied.");
        }

        return new WorkflowReviewAdjustment(riskOverrides);
    }
}

public sealed record WorkflowReviewAdjustment(
    IReadOnlyDictionary<string, string> RiskLevelOverrides);

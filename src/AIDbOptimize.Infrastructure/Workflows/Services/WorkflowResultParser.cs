using System.Text.Json;
using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Infrastructure.Workflows.Runtime;

namespace AIDbOptimize.Infrastructure.Workflows.Services;

internal static class WorkflowResultParser
{
    public static WorkflowSkillSelectionDto? TryParseSkillSelection(string? inputPayloadJson)
    {
        if (string.IsNullOrWhiteSpace(inputPayloadJson))
        {
            return null;
        }

        try
        {
            var command = JsonSerializer.Deserialize<DbConfigWorkflowCommand>(
                inputPayloadJson,
                new JsonSerializerOptions(JsonSerializerDefaults.Web));

            return command is null
                ? null
                : new WorkflowSkillSelectionDto(
                    command.BundleId,
                    command.BundleVersion,
                    command.InvestigationSkillId,
                    command.InvestigationSkillVersion,
                    command.DiagnosisSkillId,
                    command.DiagnosisSkillVersion);
        }
        catch
        {
            return null;
        }
    }

    public static WorkflowStructuredResultDto? TryParse(string? payloadJson)
    {
        if (string.IsNullOrWhiteSpace(payloadJson) || string.Equals(payloadJson, "{}", StringComparison.Ordinal))
        {
            return null;
        }

        try
        {
            using var document = JsonDocument.Parse(payloadJson);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            return new WorkflowStructuredResultDto(
                GetString(root, "title") ?? string.Empty,
                GetString(root, "summary") ?? string.Empty,
                ParseRecommendations(root),
                ParseEvidenceItems(root),
                ParseEvidenceItems(root, "externalKnowledgeItems"),
                ParseMissingContextItems(root),
                ParseCollectionMetadata(root),
                ParseWarnings(root));
        }
        catch
        {
            return null;
        }
    }

    private static IReadOnlyList<WorkflowRecommendationDto> ParseRecommendations(JsonElement root)
    {
        if (!root.TryGetProperty("recommendations", out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return array.EnumerateArray()
            .Where(static item => item.ValueKind == JsonValueKind.Object)
            .Select(item => new WorkflowRecommendationDto(
                GetString(item, "key") ?? string.Empty,
                GetString(item, "suggestion") ?? string.Empty,
                GetString(item, "severity") ?? string.Empty,
                GetString(item, "findingType") ?? "configuration",
                GetString(item, "confidence") ?? "medium",
                GetBoolean(item, "requiresMoreContext"),
                GetString(item, "impactSummary"),
                ParseStringArray(item, "evidenceReferences"),
                GetString(item, "recommendationClass") ?? "tuning",
                GetString(item, "recommendationType") ?? "actionableRecommendation",
                GetString(item, "appliesWhen"),
                GetString(item, "ruleId"),
                GetString(item, "ruleVersion"),
                ParseStringArray(item, "externalKnowledgeCitations")))
            .ToArray();
    }

    private static IReadOnlyList<WorkflowEvidenceItemDto> ParseEvidenceItems(
        JsonElement root,
        string propertyName = "evidenceItems")
    {
        if (!root.TryGetProperty(propertyName, out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return array.EnumerateArray()
            .Where(static item => item.ValueKind == JsonValueKind.Object)
            .Select(item => new WorkflowEvidenceItemDto(
                GetString(item, "sourceType") ?? string.Empty,
                GetString(item, "reference") ?? string.Empty,
                GetString(item, "description") ?? string.Empty,
                GetString(item, "category") ?? "configuration",
                GetString(item, "rawValue"),
                GetString(item, "normalizedValue"),
                GetString(item, "unit"),
                GetString(item, "sourceScope") ?? "db",
                GetDateTimeOffset(item, "capturedAt"),
                GetDateTimeOffset(item, "expiresAt"),
                GetBoolean(item, "isCached"),
                GetString(item, "collectionMethod")))
            .ToArray();
    }

    private static IReadOnlyList<WorkflowMissingContextItemDto> ParseMissingContextItems(JsonElement root)
    {
        if (!root.TryGetProperty("missingContextItems", out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return array.EnumerateArray()
            .Where(static item => item.ValueKind == JsonValueKind.Object)
            .Select(item => new WorkflowMissingContextItemDto(
                GetString(item, "reference") ?? string.Empty,
                GetString(item, "description") ?? string.Empty,
                GetString(item, "reason") ?? string.Empty,
                GetString(item, "sourceScope") ?? "unknown",
                GetString(item, "severity") ?? "warning"))
            .ToArray();
    }

    private static IReadOnlyList<WorkflowCollectionMetadataDto> ParseCollectionMetadata(JsonElement root)
    {
        if (!root.TryGetProperty("collectionMetadata", out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return array.EnumerateArray()
            .Where(static item => item.ValueKind == JsonValueKind.Object)
            .Select(item => new WorkflowCollectionMetadataDto(
                GetString(item, "name") ?? string.Empty,
                GetString(item, "value") ?? string.Empty,
                GetString(item, "description")))
            .ToArray();
    }

    private static IReadOnlyList<string> ParseWarnings(JsonElement root)
    {
        return ParseStringArray(root, "warnings");
    }

    private static IReadOnlyList<string> ParseStringArray(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var array) || array.ValueKind != JsonValueKind.Array)
        {
            return [];
        }

        return array.EnumerateArray()
            .Where(static item => item.ValueKind == JsonValueKind.String)
            .Select(static item => item.GetString() ?? string.Empty)
            .ToArray();
    }

    private static string? GetString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            return null;
        }

        return property.ValueKind switch
        {
            JsonValueKind.String => property.GetString(),
            JsonValueKind.Number => property.GetRawText(),
            JsonValueKind.True => "true",
            JsonValueKind.False => "false",
            _ => null
        };
    }

    private static DateTimeOffset? GetDateTimeOffset(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) || property.ValueKind != JsonValueKind.String)
        {
            return null;
        }

        return DateTimeOffset.TryParse(property.GetString(), out var value) ? value : null;
    }

    private static bool GetBoolean(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            return false;
        }

        return property.ValueKind switch
        {
            JsonValueKind.True => true,
            JsonValueKind.False => false,
            JsonValueKind.String when bool.TryParse(property.GetString(), out var value) => value,
            _ => false
        };
    }
}

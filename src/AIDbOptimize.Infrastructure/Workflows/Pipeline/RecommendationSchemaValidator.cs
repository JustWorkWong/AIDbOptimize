using System.Text.Json;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 推荐结果 schema 校验器。
/// </summary>
public sealed class RecommendationSchemaValidator
{
    public void Validate(string reportJson)
    {
        using var document = JsonDocument.Parse(reportJson);
        var root = document.RootElement;

        RequireNonEmptyString(root, "title");
        RequireNonEmptyString(root, "summary");

        RequireArray(root, "recommendations");
        foreach (var item in root.GetProperty("recommendations").EnumerateArray())
        {
            RequireNonEmptyString(item, "key");
            RequireNonEmptyString(item, "suggestion");
            RequireNonEmptyString(item, "severity");
            RequireNonEmptyString(item, "findingType");
            RequireNonEmptyString(item, "confidence");
            RequireBoolean(item, "requiresMoreContext");
            RequireArray(item, "evidenceReferences");
            RequireNonEmptyString(item, "recommendationClass");
            RequireNonEmptyString(item, "recommendationType");
            RequireNullableString(item, "impactSummary");
            RequireNullableString(item, "appliesWhen");
            RequireNullableString(item, "ruleId");
            RequireNullableString(item, "ruleVersion");
        }

        RequireArray(root, "evidenceItems");
        foreach (var item in root.GetProperty("evidenceItems").EnumerateArray())
        {
            RequireNonEmptyString(item, "sourceType");
            RequireNonEmptyString(item, "reference");
            RequireNonEmptyString(item, "description");
            RequireNonEmptyString(item, "category");
            RequireNullableString(item, "rawValue");
            RequireNullableString(item, "normalizedValue");
            RequireNullableString(item, "unit");
            RequireNonEmptyString(item, "sourceScope");
            RequireNullableString(item, "capturedAt");
            RequireNullableString(item, "expiresAt");
            RequireBoolean(item, "isCached");
            RequireNullableString(item, "collectionMethod");
        }

        RequireArray(root, "missingContextItems");
        foreach (var item in root.GetProperty("missingContextItems").EnumerateArray())
        {
            RequireNonEmptyString(item, "reference");
            RequireNonEmptyString(item, "description");
            RequireNonEmptyString(item, "reason");
            RequireNonEmptyString(item, "sourceScope");
            RequireNonEmptyString(item, "severity");
        }

        RequireArray(root, "collectionMetadata");
        foreach (var item in root.GetProperty("collectionMetadata").EnumerateArray())
        {
            RequireNonEmptyString(item, "name");
            RequireNonEmptyString(item, "value");
            RequireNullableString(item, "description");
        }

        RequireArray(root, "warnings");
    }

    private static void RequireNonEmptyString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.String ||
            string.IsNullOrWhiteSpace(property.GetString()))
        {
            throw new InvalidOperationException($"reportJson 缺少合法字段：{propertyName}。");
        }
    }

    private static void RequireNullableString(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property))
        {
            throw new InvalidOperationException($"reportJson 缺少字段：{propertyName}。");
        }

        if (property.ValueKind is not (JsonValueKind.String or JsonValueKind.Null))
        {
            throw new InvalidOperationException($"reportJson 字段类型不合法：{propertyName}。");
        }
    }

    private static void RequireBoolean(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind is not (JsonValueKind.True or JsonValueKind.False))
        {
            throw new InvalidOperationException($"reportJson 缺少合法布尔字段：{propertyName}。");
        }
    }

    private static void RequireArray(JsonElement element, string propertyName)
    {
        if (!element.TryGetProperty(propertyName, out var property) ||
            property.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException($"reportJson 缺少合法数组字段：{propertyName}。");
        }
    }
}

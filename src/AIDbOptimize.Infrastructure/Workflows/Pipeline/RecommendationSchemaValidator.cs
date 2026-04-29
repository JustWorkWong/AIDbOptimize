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

        if (!root.TryGetProperty("recommendations", out var recommendations) ||
            recommendations.ValueKind != JsonValueKind.Array)
        {
            throw new InvalidOperationException("reportJson 缺少 recommendations 数组。");
        }

        foreach (var item in recommendations.EnumerateArray())
        {
            RequireNonEmptyString(item, "key");
            RequireNonEmptyString(item, "suggestion");
            RequireNonEmptyString(item, "severity");
        }
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
}

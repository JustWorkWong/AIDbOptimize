using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 基于 evidence pack 对报告进行 grounding 校验。
/// </summary>
public sealed class DbConfigGroundingExecutor(
    RecommendationSchemaValidator schemaValidator)
{
    public void Validate(DbConfigEvidencePack evidence, string reportJson)
    {
        schemaValidator.Validate(reportJson);

        using var document = JsonDocument.Parse(reportJson);
        var root = document.RootElement;
        var evidenceReferences = evidence.EvidenceItems
            .Select(item => item.Reference)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        if (!root.TryGetProperty("recommendations", out var recommendations))
        {
            throw new InvalidOperationException("reportJson 缺少 recommendations。");
        }

        foreach (var item in recommendations.EnumerateArray())
        {
            var key = item.GetProperty("key").GetString();
            if (string.IsNullOrWhiteSpace(key))
            {
                throw new InvalidOperationException("recommendation.key 不能为空。");
            }

            if (!evidenceReferences.Contains(key))
            {
                throw new InvalidOperationException($"recommendation `{key}` 缺少对应 evidence。");
            }
        }
    }
}

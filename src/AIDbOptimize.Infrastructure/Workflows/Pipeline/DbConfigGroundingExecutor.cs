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
            .Concat(evidence.MissingContextItems.Select(item => item.Reference))
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
        var externalKnowledgeCitations = evidence.ExternalKnowledgeItems
            .Select(item => item.NormalizedValue ?? item.Reference)
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

            if (item.TryGetProperty("evidenceReferences", out var explicitReferences) &&
                explicitReferences.ValueKind == JsonValueKind.Array)
            {
                var resolved = explicitReferences.EnumerateArray()
                    .Where(static reference => reference.ValueKind == JsonValueKind.String)
                    .Select(static reference => reference.GetString())
                    .Where(static reference => !string.IsNullOrWhiteSpace(reference))
                    .Cast<string>()
                    .Any(evidenceReferences.Contains);

                if (!resolved)
                {
                    throw new InvalidOperationException($"recommendation `{key}` 缺少可解析的 evidenceReferences。");
                }

                continue;
            }

            if (!evidenceReferences.Contains(key))
            {
                throw new InvalidOperationException($"recommendation `{key}` 缺少对应 evidence。");
            }
        }

        ValidateExternalKnowledgeCitations(recommendations, externalKnowledgeCitations);
    }

    private static void ValidateExternalKnowledgeCitations(
        JsonElement recommendations,
        IReadOnlySet<string> externalKnowledgeCitations)
    {
        foreach (var item in recommendations.EnumerateArray())
        {
            if (!item.TryGetProperty("externalKnowledgeCitations", out var citations) ||
                citations.ValueKind != JsonValueKind.Array)
            {
                continue;
            }

            var key = item.TryGetProperty("key", out var keyProp) ? keyProp.GetString() : "unknown";
            var invalid = citations.EnumerateArray()
                .Where(static citation => citation.ValueKind == JsonValueKind.String)
                .Select(static citation => citation.GetString())
                .Where(static citation => !string.IsNullOrWhiteSpace(citation))
                .Cast<string>()
                .Any(citation => !externalKnowledgeCitations.Contains(citation));

            if (invalid)
            {
                throw new InvalidOperationException($"recommendation `{key}` 包含未解析的 externalKnowledgeCitations。");
            }
        }
    }
}

using System.Text.Json;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Rag.Workflow;

public sealed class HistoricalOptimizationCaseProjector(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
{
    public async Task<CaseProjectionResult> ProjectAsync(
        string? corpusRootPath = null,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var sessions = await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(item => item.ReviewTasks)
            .Where(item => item.Status == WorkflowSessionStatus.Succeeded)
            .Where(item => !string.IsNullOrWhiteSpace(item.ResultPayloadJson) && item.ResultPayloadJson != "{}")
            .ToListAsync(cancellationToken);

        var projected = 0;
        var exported = 0;
        foreach (var session in sessions)
        {
            if (session.ReviewTasks.Any(item => item.Status == WorkflowReviewTaskStatus.Rejected))
            {
                continue;
            }

            var parsed = TryParseResult(session.ResultPayloadJson);
            if (parsed is null || parsed.Recommendations.Count == 0)
            {
                continue;
            }

            var existing = await dbContext.RagCaseRecords
                .Include(item => item.EvidenceLinks)
                .SingleOrDefaultAsync(item => item.WorkflowSessionId == session.Id, cancellationToken);
            if (existing is null)
            {
                existing = new RagCaseRecordEntity
                {
                    Id = Guid.NewGuid(),
                    WorkflowSessionId = session.Id
                };
                dbContext.RagCaseRecords.Add(existing);
            }
            else if (existing.EvidenceLinks.Count > 0)
            {
                dbContext.RagCaseEvidenceLinks.RemoveRange(existing.EvidenceLinks);
            }

            var firstRecommendation = parsed.Recommendations[0];
            var engine = ParseEngine(session.InputPayloadJson);
            if (string.Equals(engine, "unknown", StringComparison.OrdinalIgnoreCase))
            {
                continue;
            }
            existing.Engine = engine;
            existing.ProblemType = firstRecommendation.FindingType ?? "unknown";
            existing.Outcome = "Completed";
            existing.ReviewStatus = ResolveReviewStatus(session.ReviewTasks);
            existing.RecommendationType = firstRecommendation.RecommendationType ?? "actionableRecommendation";
            existing.Summary = parsed.Summary ?? string.Empty;
            existing.CreatedAt = session.CompletedAt ?? session.UpdatedAt;

            foreach (var recommendation in parsed.Recommendations)
            {
                foreach (var evidenceReference in recommendation.EvidenceReferences)
                {
                    dbContext.RagCaseEvidenceLinks.Add(new RagCaseEvidenceLinkEntity
                    {
                        Id = Guid.NewGuid(),
                        CaseRecord = existing,
                        EvidenceReference = evidenceReference,
                        RecommendationKey = recommendation.Key ?? string.Empty,
                        CreatedAt = DateTimeOffset.UtcNow
                    });
                }
            }

            if (!string.IsNullOrWhiteSpace(corpusRootPath))
            {
                await WriteCaseCorpusDocumentAsync(corpusRootPath, existing, parsed, cancellationToken);
                exported++;
            }

            projected++;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new CaseProjectionResult(projected, exported);
    }

    private static async Task WriteCaseCorpusDocumentAsync(
        string corpusRootPath,
        RagCaseRecordEntity caseRecord,
        ParsedCaseResult parsed,
        CancellationToken cancellationToken)
    {
        var fileName = RagCorpusFileNamer.CreateCaseFileName(
            caseRecord.Engine,
            caseRecord.ProblemType,
            caseRecord.WorkflowSessionId.ToString("N"));
        var directory = Path.Combine(corpusRootPath, "cases", caseRecord.Engine);
        Directory.CreateDirectory(directory);
        var fullPath = Path.Combine(directory, fileName);
        var payload = new
        {
            source_title = $"workflow-case-{caseRecord.WorkflowSessionId}",
            source_url = $"/api/history/{caseRecord.WorkflowSessionId}",
            engine = caseRecord.Engine,
            problem_type = caseRecord.ProblemType,
            review_status = caseRecord.ReviewStatus,
            recommendation_type = caseRecord.RecommendationType,
            evidence_references = parsed.Recommendations.SelectMany(item => item.EvidenceReferences).Distinct(StringComparer.OrdinalIgnoreCase)
        };
        var content = $$"""
        ---
        source_title: {{payload.source_title}}
        source_url: {{payload.source_url}}
        engine: {{payload.engine}}
        problem_type: {{payload.problem_type}}
        review_status: {{payload.review_status}}
        recommendation_type: {{payload.recommendation_type}}
        evidence_references: {{JsonSerializer.Serialize(payload.evidence_references)}}
        ---

        # {{caseRecord.ProblemType}}

        {{caseRecord.Summary}}
        """;
        await File.WriteAllTextAsync(fullPath, content, cancellationToken);
    }

    private static string ResolveReviewStatus(ICollection<WorkflowReviewTaskEntity> reviewTasks)
    {
        if (reviewTasks.Count == 0)
        {
            return "NoReview";
        }

        return reviewTasks
            .OrderByDescending(item => item.UpdatedAt)
            .First()
            .Status
            .ToString();
    }

    private static string ParseEngine(string inputPayloadJson)
    {
        try
        {
            using var document = JsonDocument.Parse(inputPayloadJson);
            return document.RootElement.TryGetProperty("bundleId", out var bundleId) &&
                   bundleId.GetString()?.Contains("mysql", StringComparison.OrdinalIgnoreCase) == true
                ? "mysql"
                : "postgresql";
        }
        catch
        {
            return "unknown";
        }
    }

    private static ParsedCaseResult? TryParseResult(string resultPayloadJson)
    {
        try
        {
            using var document = JsonDocument.Parse(resultPayloadJson);
            var root = document.RootElement;
            if (root.ValueKind != JsonValueKind.Object)
            {
                return null;
            }

            var summary = root.TryGetProperty("summary", out var summaryProp) ? summaryProp.GetString() : null;
            var recommendations = root.TryGetProperty("recommendations", out var recommendationsProp) &&
                                  recommendationsProp.ValueKind == JsonValueKind.Array
                ? recommendationsProp.EnumerateArray()
                    .Select(item => new ParsedCaseRecommendation(
                        item.TryGetProperty("key", out var key) ? key.GetString() : null,
                        item.TryGetProperty("findingType", out var findingType) ? findingType.GetString() : null,
                        item.TryGetProperty("recommendationType", out var recommendationType) ? recommendationType.GetString() : null,
                        item.TryGetProperty("evidenceReferences", out var evidenceReferences) && evidenceReferences.ValueKind == JsonValueKind.Array
                            ? evidenceReferences.EnumerateArray()
                                .Where(value => value.ValueKind == JsonValueKind.String)
                                .Select(value => value.GetString() ?? string.Empty)
                                .Where(value => !string.IsNullOrWhiteSpace(value))
                                .ToArray()
                            : []))
                    .ToArray()
                : [];

            return new ParsedCaseResult(summary, recommendations);
        }
        catch
        {
            return null;
        }
    }

    private sealed record ParsedCaseResult(
        string? Summary,
        IReadOnlyList<ParsedCaseRecommendation> Recommendations);

    private sealed record ParsedCaseRecommendation(
        string? Key,
        string? FindingType,
        string? RecommendationType,
        IReadOnlyList<string> EvidenceReferences);
}

public sealed record CaseProjectionResult(
    int ProjectedCount,
    int ExportedDocumentCount);

namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagCaseRecordRepository
{
    Task<IReadOnlyList<RagCaseRecordRecord>> ListByEvidenceReferencesAsync(
        string engine,
        IReadOnlyCollection<string> evidenceReferences,
        int topK,
        CancellationToken cancellationToken = default);
}

public sealed record RagCaseRecordRecord(
    Guid Id,
    Guid WorkflowSessionId,
    string Engine,
    string ProblemType,
    string Outcome,
    string ReviewStatus,
    string RecommendationType,
    string Summary,
    IReadOnlyList<string> EvidenceReferences,
    DateTimeOffset CreatedAt);

namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagCaseAuditItemDto(
    string CaseRecordId,
    string WorkflowSessionId,
    string Engine,
    string ProblemType,
    string Outcome,
    string ReviewStatus,
    string RecommendationType,
    string Summary,
    int EvidenceLinkCount,
    DateTimeOffset CreatedAt);

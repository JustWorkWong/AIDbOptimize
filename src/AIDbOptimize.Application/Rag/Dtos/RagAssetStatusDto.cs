namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagAssetStatusDto(
    int FactDocumentCount,
    int CaseRecordCount,
    int ChunkCount,
    int RetrievalSnapshotCount,
    DateTimeOffset? LatestDocumentIngestedAt,
    DateTimeOffset? LatestCaseProjectedAt,
    DateTimeOffset? LatestSnapshotCreatedAt,
    double? DocumentFreshnessHours,
    double? CaseFreshnessHours,
    double? SnapshotFreshnessHours);

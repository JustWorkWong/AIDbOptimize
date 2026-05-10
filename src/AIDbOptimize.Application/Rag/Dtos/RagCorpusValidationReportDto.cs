namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagCorpusValidationReportDto(
    int ValidDocumentCount,
    int InvalidDocumentCount,
    IReadOnlyList<RagCoverageItemDto> Coverage,
    IReadOnlyList<RagCorpusValidationIssueDto> Issues);

namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagCorpusValidationIssueDto(
    string RelativePath,
    string Message,
    string Severity);

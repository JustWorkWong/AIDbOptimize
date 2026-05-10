namespace AIDbOptimize.Infrastructure.Rag.Corpus;

public sealed record RagSourceDocument(
    RagDocumentType DocumentType,
    string RelativePath,
    string FileName,
    string Engine,
    string? Vendor,
    string? Topic,
    string? ProblemType,
    string SourceTitle,
    string SourceUrl,
    IReadOnlyDictionary<string, string> Metadata);

using AIDbOptimize.Infrastructure.Rag.Corpus;

namespace AIDbOptimize.Infrastructure.Rag.Preprocess;

public sealed record PreprocessedCorpusDocument(
    RagSourceDocument SourceDocument,
    string Title,
    string CleanText);

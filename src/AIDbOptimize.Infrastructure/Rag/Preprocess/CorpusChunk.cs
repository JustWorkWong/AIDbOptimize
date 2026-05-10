using AIDbOptimize.Infrastructure.Rag.Corpus;

namespace AIDbOptimize.Infrastructure.Rag.Preprocess;

public sealed record CorpusChunk(
    string ChunkId,
    RagSourceDocument SourceDocument,
    string Title,
    string SectionPath,
    string Text);

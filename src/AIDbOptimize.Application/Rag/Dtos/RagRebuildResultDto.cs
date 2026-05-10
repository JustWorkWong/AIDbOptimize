namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagRebuildResultDto(
    string CorpusRootPath,
    int DocumentCount,
    int ChunkCount,
    int PreparedFileCount,
    int ProjectedCaseCount,
    IReadOnlyList<RagCoverageItemDto> Coverage);

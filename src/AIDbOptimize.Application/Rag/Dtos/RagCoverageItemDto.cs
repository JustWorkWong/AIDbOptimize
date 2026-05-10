namespace AIDbOptimize.Application.Rag.Dtos;

public sealed record RagCoverageItemDto(
    string SourceType,
    string Engine,
    string TopicOrProblemType,
    int Count);

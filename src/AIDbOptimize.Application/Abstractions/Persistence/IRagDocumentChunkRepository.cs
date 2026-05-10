namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagDocumentChunkRepository
{
    Task<IReadOnlyList<RagDocumentChunkRecord>> ListAsync(
        RagDocumentChunkFilter filter,
        int topK,
        CancellationToken cancellationToken = default);
}

public sealed record RagDocumentChunkFilter(
    string? Engine = null,
    string? Vendor = null,
    string? Topic = null,
    IReadOnlyList<string>? ParameterNames = null,
    bool RequireEmbedding = false);

public sealed record RagDocumentChunkRecord(
    Guid Id,
    Guid DocumentId,
    string Engine,
    string Vendor,
    string Topic,
    string Title,
    string SectionPath,
    string Text,
    string ProductVersion,
    string AppliesTo,
    string ParameterNamesJson,
    string KeywordsJson,
    bool HasEmbedding,
    string SourceUrl);

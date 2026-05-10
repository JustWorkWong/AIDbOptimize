using Pgvector;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

public sealed class RagDocumentChunkEntity
{
    public Guid Id { get; set; }

    public Guid DocumentId { get; set; }

    public string ChunkKey { get; set; } = string.Empty;

    public string Title { get; set; } = string.Empty;

    public string SectionPath { get; set; } = string.Empty;

    public string Text { get; set; } = string.Empty;

    public string ProductVersion { get; set; } = string.Empty;

    public string AppliesTo { get; set; } = string.Empty;

    public string ParameterNamesJson { get; set; } = "[]";

    public string KeywordsJson { get; set; } = "[]";

    public Vector? Embedding { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public RagDocumentEntity Document { get; set; } = null!;
}

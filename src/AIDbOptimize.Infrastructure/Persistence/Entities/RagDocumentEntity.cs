using AIDbOptimize.Infrastructure.Rag.Corpus;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

public sealed class RagDocumentEntity
{
    public Guid Id { get; set; }

    public RagDocumentType DocumentType { get; set; }

    public string Engine { get; set; } = string.Empty;

    public string Vendor { get; set; } = string.Empty;

    public string Topic { get; set; } = string.Empty;

    public string SourcePath { get; set; } = string.Empty;

    public string SourceUrl { get; set; } = string.Empty;

    public string SourceTitle { get; set; } = string.Empty;

    public string ContentHash { get; set; } = string.Empty;

    public DateTimeOffset CapturedAt { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }

    public ICollection<RagDocumentChunkEntity> Chunks { get; set; } = [];
}

using Pgvector;

namespace AIDbOptimize.Infrastructure.Rag.Embeddings;

public interface IRagEmbeddingService
{
    bool IsConfigured { get; }

    Task<Vector?> GenerateAsync(string text, CancellationToken cancellationToken = default);
}

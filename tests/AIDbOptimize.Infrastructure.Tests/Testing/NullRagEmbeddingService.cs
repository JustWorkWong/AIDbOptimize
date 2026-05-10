using AIDbOptimize.Infrastructure.Rag.Embeddings;
using Pgvector;

namespace AIDbOptimize.Infrastructure.Tests.Testing;

internal sealed class NullRagEmbeddingService : IRagEmbeddingService
{
    public bool IsConfigured => false;

    public Task<Vector?> GenerateAsync(string text, CancellationToken cancellationToken = default)
    {
        return Task.FromResult<Vector?>(null);
    }
}

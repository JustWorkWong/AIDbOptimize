using Microsoft.Extensions.AI;
using OpenAI;
using Pgvector;

namespace AIDbOptimize.Infrastructure.Rag.Embeddings;

public sealed class RagEmbeddingService(
    RagEmbeddingOptions options,
    IEmbeddingGenerator<string, Embedding<float>> embeddingGenerator) : IRagEmbeddingService
{
    private readonly RagEmbeddingOptions _options = options;
    private readonly IEmbeddingGenerator<string, Embedding<float>> _embeddingGenerator = embeddingGenerator;

    public bool IsConfigured => _options.IsConfigured;

    public async Task<Vector?> GenerateAsync(string text, CancellationToken cancellationToken = default)
    {
        if (!_options.IsConfigured || string.IsNullOrWhiteSpace(text))
        {
            return null;
        }

        var vector = await _embeddingGenerator.GenerateVectorAsync(
            text,
            new EmbeddingGenerationOptions
            {
                ModelId = _options.Model
            },
            cancellationToken);
        return new Vector(vector.ToArray());
    }
}

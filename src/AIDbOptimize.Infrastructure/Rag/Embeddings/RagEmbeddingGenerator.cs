using System.ClientModel;
using Microsoft.Extensions.AI;
using OpenAI;
using OpenAI.Embeddings;

namespace AIDbOptimize.Infrastructure.Rag.Embeddings;

public sealed class RagEmbeddingGenerator(RagEmbeddingOptions options) : IEmbeddingGenerator<string, Embedding<float>>
{
    private readonly RagEmbeddingOptions _options = options;
    private readonly EmbeddingClient? _client = options.IsConfigured
        ? new EmbeddingClient(
            options.Model,
            new ApiKeyCredential(options.ApiKey),
            new OpenAIClientOptions
            {
                Endpoint = new Uri(options.Endpoint),
                NetworkTimeout = TimeSpan.FromMinutes(3)
            })
        : null;

    public async Task<GeneratedEmbeddings<Embedding<float>>> GenerateAsync(
        IEnumerable<string> values,
        Microsoft.Extensions.AI.EmbeddingGenerationOptions? options = null,
        CancellationToken cancellationToken = default)
    {
        var results = new GeneratedEmbeddings<Embedding<float>>();
        if (_client is null)
        {
            return results;
        }

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            var embedding = await _client.GenerateEmbeddingAsync(
                value,
                new OpenAI.Embeddings.EmbeddingGenerationOptions
                {
                    EndUserId = null
                },
                cancellationToken);
            results.Add(new Embedding<float>(embedding.Value.ToFloats())
            {
                CreatedAt = DateTimeOffset.UtcNow,
                ModelId = _options.Model
            });
        }

        return results;
    }

    public object? GetService(Type serviceType, object? serviceKey = null)
    {
        if (serviceKey is not null)
        {
            return null;
        }

        if (serviceType == typeof(EmbeddingGeneratorMetadata))
        {
            return new EmbeddingGeneratorMetadata(
                "openai-compatible",
                new Uri(_options.Endpoint),
                _options.Model,
                null);
        }

        return null;
    }

    public void Dispose()
    {
    }
}

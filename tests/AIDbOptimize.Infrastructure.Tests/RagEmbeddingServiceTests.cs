using AIDbOptimize.Infrastructure.Rag.Embeddings;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagEmbeddingServiceTests
{
    [Fact]
    public async Task GenerateAsync_ReturnsNull_WhenApiKeyIsPlaceholder()
    {
        var options = new RagEmbeddingOptions
        {
            ApiKey = "xxx"
        };
        var service = new RagEmbeddingService(options, new RagEmbeddingGenerator(options));

        var vector = await service.GenerateAsync("shared_buffers");

        Assert.Null(vector);
        Assert.False(service.IsConfigured);
    }
}

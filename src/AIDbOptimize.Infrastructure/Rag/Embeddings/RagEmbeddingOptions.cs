namespace AIDbOptimize.Infrastructure.Rag.Embeddings;

public sealed class RagEmbeddingOptions
{
    public string Endpoint { get; init; } = "https://dashscope.aliyuncs.com/compatible-mode/v1";

    public string Model { get; init; } = "text-embedding-v3";

    public string ApiKey { get; init; } = string.Empty;

    public bool IsConfigured => !string.IsNullOrWhiteSpace(ApiKey) && !IsPlaceholder(ApiKey);

    private static bool IsPlaceholder(string apiKey)
    {
        return string.Equals(apiKey, "xxx", StringComparison.OrdinalIgnoreCase)
            || string.Equals(apiKey, "YOUR_DASHSCOPE_API_KEY", StringComparison.OrdinalIgnoreCase)
            || string.Equals(apiKey, "YOUR_API_KEY", StringComparison.OrdinalIgnoreCase);
    }
}

using Microsoft.Extensions.VectorData;

namespace AIDbOptimize.Infrastructure.Rag.VectorData;

public sealed class RagVectorStore(
    IServiceProvider serviceProvider) : VectorStore
{
    public const string KnowledgeCollectionName = "rag_knowledge";

    public override VectorStoreCollection<TKey, TRecord> GetCollection<TKey, TRecord>(
        string name,
        VectorStoreCollectionDefinition? definition = null)
    {
        if (name != KnowledgeCollectionName || typeof(TKey) != typeof(Guid) || typeof(TRecord) != typeof(RagKnowledgeVectorRecord))
        {
            throw new NotSupportedException($"Collection `{name}` with record `{typeof(TRecord).Name}` is not supported by RagVectorStore.");
        }

        return (VectorStoreCollection<TKey, TRecord>)(object)new RagVectorStoreCollection(serviceProvider);
    }

    public override VectorStoreCollection<object, Dictionary<string, object?>> GetDynamicCollection(
        string name,
        VectorStoreCollectionDefinition definition)
    {
        throw new NotSupportedException("Dynamic vector collections are not supported by RagVectorStore.");
    }

    public override async IAsyncEnumerable<string> ListCollectionNamesAsync([System.Runtime.CompilerServices.EnumeratorCancellation] CancellationToken cancellationToken = default)
    {
        yield return KnowledgeCollectionName;
        await Task.CompletedTask;
    }

    public override Task<bool> CollectionExistsAsync(string name, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(name == KnowledgeCollectionName);
    }

    public override Task EnsureCollectionDeletedAsync(string name, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("RagVectorStore is read-only.");
    }

    public override object? GetService(Type serviceType, object? serviceKey = null)
    {
        return serviceKey is null ? serviceProvider.GetService(serviceType) : null;
    }
}

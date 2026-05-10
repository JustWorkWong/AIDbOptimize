using Microsoft.Extensions.VectorData;

namespace AIDbOptimize.Infrastructure.Rag.VectorData;

public sealed class RagKnowledgeVectorRecord
{
    [VectorStoreKey]
    public Guid Key { get; set; }

    [VectorStoreData(IsIndexed = true)]
    public string SourceType { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string Engine { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string Vendor { get; set; } = string.Empty;

    [VectorStoreData(IsIndexed = true)]
    public string Topic { get; set; } = string.Empty;

    [VectorStoreData]
    public string SectionPath { get; set; } = string.Empty;

    [VectorStoreData]
    public string SourceTitle { get; set; } = string.Empty;

    [VectorStoreData]
    public string SourceUrl { get; set; } = string.Empty;

    [VectorStoreData]
    public string ParameterNamesJson { get; set; } = "[]";

    [VectorStoreData(IsFullTextIndexed = true)]
    public string Text { get; set; } = string.Empty;

    [VectorStoreData]
    public string Citation { get; set; } = string.Empty;

    [VectorStoreVector(1536, DistanceFunction = DistanceFunction.CosineSimilarity)]
    public ReadOnlyMemory<float> Vector { get; set; }
}

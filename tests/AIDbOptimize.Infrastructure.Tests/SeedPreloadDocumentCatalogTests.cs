using AIDbOptimize.Infrastructure.Rag.Corpus;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class SeedPreloadDocumentCatalogTests
{
    [Fact]
    public void GetDocuments_ReturnsFixedSeedCatalogWithUniqueUrlsAndPaths()
    {
        var documents = SeedPreloadDocumentCatalog.GetDocuments();

        Assert.NotEmpty(documents);
        Assert.Equal(documents.Count, documents.Select(item => item.SourceUrl).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.Equal(documents.Count, documents.Select(item => item.RelativePath).Distinct(StringComparer.OrdinalIgnoreCase).Count());
        Assert.Contains(documents, item => item.Vendor == "mysql");
        Assert.Contains(documents, item => item.Vendor == "postgresql");
        Assert.Contains(documents, item => item.Vendor == "aliyun");
        Assert.Contains(documents, item => item.Vendor == "aws");
        Assert.Contains(documents, item => item.Vendor == "azure");
        Assert.Contains(documents, item => item.Vendor == "gcp");
    }

    [Fact]
    public void GetDocuments_UsesOnlyFactDocumentsBackedByValidCorpusPaths()
    {
        var documents = SeedPreloadDocumentCatalog.GetDocuments();

        Assert.All(documents, item =>
        {
            Assert.Equal(RagDocumentType.Fact, item.DocumentType);
            Assert.StartsWith("facts/", item.RelativePath, StringComparison.Ordinal);
            Assert.False(string.IsNullOrWhiteSpace(item.SourceTitle));
            Assert.False(string.IsNullOrWhiteSpace(item.Metadata["seed_id"]));
            Assert.False(string.IsNullOrWhiteSpace(item.Metadata["vendor"]));
            Assert.False(string.IsNullOrWhiteSpace(item.Metadata["topic"]));

            var parsed = RagCorpusFileNamer.ParseRelativePath(item.RelativePath);

            Assert.Equal(item.Engine, parsed.Engine);
            Assert.Equal(item.Vendor, parsed.Vendor);
            Assert.Equal(item.Topic, parsed.Topic);
        });
    }
}

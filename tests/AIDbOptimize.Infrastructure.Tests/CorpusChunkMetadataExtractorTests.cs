using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class CorpusChunkMetadataExtractorTests
{
    [Fact]
    public void Extract_ReturnsParameterNamesKeywordsAndVersion()
    {
        var document = RagCorpusFileNamer.ParseRelativePath("facts/mysql/mysql__mysql__memory__server-system-variables.md") with
        {
            SourceTitle = "Server System Variables",
            SourceUrl = "https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html"
        };
        var chunk = new CorpusChunk(
            "chunk-1",
            document,
            "innodb_buffer_pool_size",
            "InnoDB > Memory",
            "Tune innodb_buffer_pool_size carefully and keep innodb_log_file_size aligned.");
        var extractor = new CorpusChunkMetadataExtractor();

        var metadata = extractor.Extract(document, chunk);

        Assert.Equal("8.4", metadata.ProductVersion);
        Assert.Equal("mysql:mysql", metadata.AppliesTo);
        Assert.Contains("innodb_buffer_pool_size", metadata.ParameterNames);
        Assert.Contains("memory", metadata.Keywords, StringComparer.OrdinalIgnoreCase);
    }
}

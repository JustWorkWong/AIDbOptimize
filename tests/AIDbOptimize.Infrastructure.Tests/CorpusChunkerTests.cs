using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class CorpusChunkerTests
{
    [Fact]
    public void Chunk_PrefersHeadingSectionsAndSplitsIntoStableChunks()
    {
        var rawContent = File.ReadAllText("Fixtures/Rag/MarkdownSeed.sample.md");
        var document = RagCorpusFileNamer.ParseRelativePath("facts/postgresql/postgresql__postgresql__memory__runtime-config-resource.md") with
        {
            SourceTitle = "Runtime Config Resource",
            SourceUrl = "https://example.com/postgresql"
        };
        var preprocessor = new CorpusPreprocessor();
        var chunker = new CorpusChunker();

        var preprocessed = preprocessor.Preprocess(document, rawContent);
        var chunks = chunker.Chunk(preprocessed, maxChars: 120, minChars: 40);

        Assert.True(chunks.Count >= 2);
        Assert.All(chunks, chunk =>
        {
            Assert.False(string.IsNullOrWhiteSpace(chunk.ChunkId));
            Assert.False(string.IsNullOrWhiteSpace(chunk.SectionPath));
            Assert.False(string.IsNullOrWhiteSpace(chunk.Text));
        });
        Assert.Contains(chunks, chunk => chunk.Title.Contains("Sizing", StringComparison.Ordinal));
    }
}

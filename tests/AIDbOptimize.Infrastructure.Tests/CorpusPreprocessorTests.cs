using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class CorpusPreprocessorTests
{
    [Fact]
    public void Preprocess_RemovesHtmlNavigationAndFooterNoise()
    {
        var rawContent = File.ReadAllText("Fixtures/Rag/HtmlSeed.sample.html");
        var document = RagCorpusFileNamer.ParseRelativePath("facts/mysql/mysql__mysql__memory__server-system-variables.md") with
        {
            SourceTitle = "MySQL Server System Variables",
            SourceUrl = "https://example.com/mysql"
        };
        var preprocessor = new CorpusPreprocessor();

        var result = preprocessor.Preprocess(document, rawContent);

        Assert.Equal("Buffer Pool Tuning", result.Title);
        Assert.DoesNotContain("Sign in", result.CleanText, StringComparison.OrdinalIgnoreCase);
        Assert.DoesNotContain("All rights reserved", result.CleanText, StringComparison.OrdinalIgnoreCase);
        Assert.Contains("Use a larger buffer pool", result.CleanText, StringComparison.Ordinal);
        Assert.Contains("Do not exceed available host memory", result.CleanText, StringComparison.Ordinal);
    }
}

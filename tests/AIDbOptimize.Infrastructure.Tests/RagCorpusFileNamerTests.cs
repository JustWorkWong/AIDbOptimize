using AIDbOptimize.Infrastructure.Rag.Corpus;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class RagCorpusFileNamerTests
{
    [Fact]
    public void CreateFactFileName_UsesPlannedConvention()
    {
        var fileName = RagCorpusFileNamer.CreateFactFileName(
            engine: "mysql",
            vendor: "aws",
            topic: "memory",
            shortTitle: "appendix mysql parameters");

        Assert.Equal("mysql__aws__memory__appendix-mysql-parameters.md", fileName);
    }

    [Fact]
    public void CreateCaseFileName_UsesPlannedConvention()
    {
        var fileName = RagCorpusFileNamer.CreateCaseFileName(
            engine: "postgresql",
            problemType: "checkpoint",
            sessionOrTicket: "wf-2026-05-09-001");

        Assert.Equal("postgresql__case__checkpoint__wf-2026-05-09-001.md", fileName);
    }

    [Theory]
    [InlineData("facts/mysql/mysql__mysql__memory__server-system-variables.md", RagDocumentType.Fact, "mysql", "mysql", "memory", null)]
    [InlineData("facts/cloud/postgresql__aws__storage__rds-parameters.md", RagDocumentType.Fact, "postgresql", "aws", "storage", null)]
    [InlineData("cases/postgresql/postgresql__case__locking__wf-2026-05-09-001.md", RagDocumentType.Case, "postgresql", null, null, "locking")]
    public void ParseRelativePath_ReturnsStructuredDocument(
        string relativePath,
        RagDocumentType expectedType,
        string expectedEngine,
        string? expectedVendor,
        string? expectedTopic,
        string? expectedProblemType)
    {
        var document = RagCorpusFileNamer.ParseRelativePath(relativePath);

        Assert.Equal(expectedType, document.DocumentType);
        Assert.Equal(expectedEngine, document.Engine);
        Assert.Equal(expectedVendor, document.Vendor);
        Assert.Equal(expectedTopic, document.Topic);
        Assert.Equal(expectedProblemType, document.ProblemType);
        Assert.Equal(relativePath.Replace('\\', '/'), document.RelativePath);
    }

    [Theory]
    [InlineData("facts/mysql/mysql__case__memory__wf-001.md")]
    [InlineData("cases/mysql/mysql__aws__memory__server-system-variables.md")]
    [InlineData("facts/mysql/mysql__aws__unknown__server-system-variables.md")]
    [InlineData("facts/mysql/postgresql__aws__memory__server-system-variables.md")]
    [InlineData("facts/mysql/mysql__aws__memory__.md")]
    public void ParseRelativePath_RejectsInvalidNames(string relativePath)
    {
        var error = Assert.Throws<InvalidOperationException>(() => RagCorpusFileNamer.ParseRelativePath(relativePath));

        Assert.Contains("corpus", error.Message, StringComparison.OrdinalIgnoreCase);
    }
}

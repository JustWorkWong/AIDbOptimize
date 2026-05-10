using System.Net;
using System.Text;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using Xunit;

namespace AIDbOptimize.Infrastructure.Tests;

public sealed class SeedPreloadCommandTests
{
    [Fact]
    public async Task ExecuteAsync_DownloadsCatalogDocumentsToExpectedPaths()
    {
        var rootPath = Path.Combine(Path.GetTempPath(), $"rag-seed-{Guid.NewGuid():N}");
        Directory.CreateDirectory(rootPath);

        try
        {
            using var httpClient = new HttpClient(new StubSeedMessageHandler());
            var command = new SeedPreloadCommand(httpClient);

            var result = await command.ExecuteAsync(rootPath);

            Assert.Equal(SeedPreloadDocumentCatalog.GetDocuments().Count, result.DownloadedCount);
            Assert.All(result.SavedPaths, path => Assert.True(File.Exists(path)));
            var samplePath = Path.Combine(rootPath, "facts", "mysql", "mysql__mysql__memory__server-system-variables.md");
            var content = await File.ReadAllTextAsync(samplePath, Encoding.UTF8);
            Assert.Contains("source_url: https://dev.mysql.com/doc/refman/8.4/en/server-system-variables.html", content, StringComparison.Ordinal);
            Assert.Contains("<html>", content, StringComparison.OrdinalIgnoreCase);
        }
        finally
        {
            if (Directory.Exists(rootPath))
            {
                Directory.Delete(rootPath, recursive: true);
            }
        }
    }

    private sealed class StubSeedMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var payload = """
            <html><body><main><h1>Downloaded Seed</h1><p>Example content.</p></main></body></html>
            """;
            return Task.FromResult(new HttpResponseMessage(HttpStatusCode.OK)
            {
                Content = new StringContent(payload, Encoding.UTF8, "text/html")
            });
        }
    }
}

using System.Text;

namespace AIDbOptimize.Infrastructure.Rag.Corpus;

public sealed class SeedPreloadCommand(HttpClient httpClient)
{
    public async Task<SeedPreloadCommandResult> ExecuteAsync(
        string corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(corpusRootPath);

        var savedPaths = new List<string>();
        foreach (var document in SeedPreloadDocumentCatalog.GetDocuments())
        {
            using var request = new HttpRequestMessage(HttpMethod.Get, document.SourceUrl);
            request.Headers.UserAgent.ParseAdd("AIDbOptimize-RagSeed/1.0");
            request.Headers.Accept.ParseAdd("text/html,application/xhtml+xml,application/xml;q=0.9,text/plain;q=0.8,*/*;q=0.7");
            request.Headers.AcceptLanguage.ParseAdd("en-US,en;q=0.9,zh-CN;q=0.8");
            using var response = await httpClient.SendAsync(request, cancellationToken);
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync(cancellationToken);
            var fullPath = Path.Combine(corpusRootPath, document.RelativePath.Replace('/', Path.DirectorySeparatorChar));
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);
            await File.WriteAllTextAsync(
                fullPath,
                BuildFileContent(document, content),
                Encoding.UTF8,
                cancellationToken);
            savedPaths.Add(fullPath);
        }

        return new SeedPreloadCommandResult(savedPaths.Count, savedPaths);
    }

    private static string BuildFileContent(RagSourceDocument document, string body)
    {
        var builder = new StringBuilder();
        builder.AppendLine("---");
        builder.Append("source_title: ").AppendLine(EscapeMetadataValue(document.SourceTitle));
        builder.Append("source_url: ").AppendLine(document.SourceUrl);
        builder.Append("engine: ").AppendLine(document.Engine);
        if (!string.IsNullOrWhiteSpace(document.Vendor))
        {
            builder.Append("vendor: ").AppendLine(document.Vendor);
        }

        if (!string.IsNullOrWhiteSpace(document.Topic))
        {
            builder.Append("topic: ").AppendLine(document.Topic);
        }

        if (document.Metadata.TryGetValue("seed_id", out var seedId))
        {
            builder.Append("seed_id: ").AppendLine(seedId);
        }

        builder.Append("captured_at_utc: ").AppendLine(DateTimeOffset.UtcNow.ToString("O"));
        builder.AppendLine("---");
        builder.AppendLine();
        builder.AppendLine(body.Replace("\r\n", "\n"));
        return builder.ToString();
    }

    private static string EscapeMetadataValue(string value)
    {
        return value.Contains(':', StringComparison.Ordinal) ? $"\"{value}\"" : value;
    }
}

public sealed record SeedPreloadCommandResult(
    int DownloadedCount,
    IReadOnlyList<string> SavedPaths);

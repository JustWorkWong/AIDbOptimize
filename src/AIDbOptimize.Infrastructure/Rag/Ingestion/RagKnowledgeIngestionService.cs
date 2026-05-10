using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Embeddings;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Rag.Ingestion;

public sealed class RagKnowledgeIngestionService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    CorpusPreprocessor preprocessor,
    CorpusChunker chunker,
    CorpusChunkMetadataExtractor metadataExtractor,
    IRagEmbeddingService embeddingService,
    RagPreparedArtifactWriter preparedArtifactWriter)
{
    public async Task<RagKnowledgeIngestionResult> IngestAsync(
        string corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(corpusRootPath);

        var sourceFiles = Directory.GetFiles(corpusRootPath, "*.md", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}prepared{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !string.Equals(Path.GetFileName(path), "README.md", StringComparison.OrdinalIgnoreCase))
            .Where(path => !string.Equals(Path.GetFileName(path), "CLAUDE.md", StringComparison.OrdinalIgnoreCase))
            .ToArray();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var ingestedDocuments = 0;
        var ingestedChunks = 0;
        var preparedFileCount = 0;

        foreach (var fullPath in sourceFiles)
        {
            var relativePath = Path.GetRelativePath(corpusRootPath, fullPath).Replace('\\', '/');
            if (!RagCorpusFileNamer.TryParseRelativePath(relativePath, out var sourceDocument) || sourceDocument is null)
            {
                continue;
            }

            var rawContent = await File.ReadAllTextAsync(fullPath, cancellationToken);
            sourceDocument = ApplyFrontMatter(sourceDocument, rawContent);
            var preprocessed = preprocessor.Preprocess(sourceDocument, rawContent);
            var chunks = chunker.Chunk(preprocessed);
            var contentHash = ComputeSha256(rawContent);
            var now = DateTimeOffset.UtcNow;

            var document = await dbContext.RagDocuments
                .Include(item => item.Chunks)
                .SingleOrDefaultAsync(item => item.SourcePath == relativePath, cancellationToken);
            if (document is null)
            {
                document = new RagDocumentEntity
                {
                    Id = Guid.NewGuid(),
                    CreatedAt = now
                };
                dbContext.RagDocuments.Add(document);
            }

            document.DocumentType = sourceDocument.DocumentType;
            document.Engine = sourceDocument.Engine;
            document.Vendor = sourceDocument.Vendor ?? "case";
            document.Topic = sourceDocument.Topic ?? sourceDocument.ProblemType ?? "case";
            document.SourcePath = relativePath;
            document.SourceUrl = sourceDocument.SourceUrl;
            document.SourceTitle = preprocessed.Title;
            document.ContentHash = contentHash;
            document.CapturedAt = now;
            document.IsActive = true;
            document.UpdatedAt = now;

            if (document.Chunks.Count > 0)
            {
                dbContext.RagDocumentChunks.RemoveRange(document.Chunks);
            }

            var preparedArtifacts = new List<PreparedChunkArtifact>();
            foreach (var chunk in chunks)
            {
                var metadata = metadataExtractor.Extract(sourceDocument, chunk);
                var embedding = await embeddingService.GenerateAsync($"{chunk.Title}\n{chunk.Text}", cancellationToken);
                preparedArtifacts.Add(new PreparedChunkArtifact(chunk, metadata));
                dbContext.RagDocumentChunks.Add(new RagDocumentChunkEntity
                {
                    Id = Guid.NewGuid(),
                    Document = document,
                    ChunkKey = chunk.ChunkId,
                    Title = chunk.Title,
                    SectionPath = chunk.SectionPath,
                    Text = chunk.Text,
                    ProductVersion = metadata.ProductVersion,
                    AppliesTo = metadata.AppliesTo,
                    ParameterNamesJson = SerializeList(metadata.ParameterNames),
                    KeywordsJson = SerializeList(metadata.Keywords),
                    Embedding = embedding,
                    CreatedAt = now
                });
            }

            preparedFileCount += await preparedArtifactWriter.WriteAsync(
                corpusRootPath,
                preprocessed,
                preparedArtifacts,
                cancellationToken);

            ingestedDocuments++;
            ingestedChunks += chunks.Count;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return new RagKnowledgeIngestionResult(ingestedDocuments, ingestedChunks, preparedFileCount);
    }

    private static string ComputeSha256(string content)
    {
        var bytes = Encoding.UTF8.GetBytes(content);
        var hash = SHA256.HashData(bytes);
        return Convert.ToHexString(hash);
    }

    private static string SerializeList(IReadOnlyList<string> values)
    {
        return System.Text.Json.JsonSerializer.Serialize(values);
    }

    private static RagSourceDocument ApplyFrontMatter(RagSourceDocument sourceDocument, string rawContent)
    {
        var match = Regex.Match(rawContent, @"(?is)\A---\s*\n(?<body>.*?)\n---\s*\n?");
        if (!match.Success)
        {
            return sourceDocument;
        }

        var values = match.Groups["body"].Value
            .Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Split(':', 2, StringSplitOptions.TrimEntries))
            .Where(parts => parts.Length == 2)
            .ToDictionary(parts => parts[0], parts => parts[1].Trim().Trim('"'), StringComparer.OrdinalIgnoreCase);

        return sourceDocument with
        {
            SourceTitle = values.TryGetValue("source_title", out var sourceTitle) && !string.IsNullOrWhiteSpace(sourceTitle)
                ? sourceTitle
                : sourceDocument.SourceTitle,
            SourceUrl = values.TryGetValue("source_url", out var sourceUrl) && !string.IsNullOrWhiteSpace(sourceUrl)
                ? sourceUrl
                : sourceDocument.SourceUrl
        };
    }
}

public sealed record RagKnowledgeIngestionResult(
    int DocumentCount,
    int ChunkCount,
    int PreparedFileCount);

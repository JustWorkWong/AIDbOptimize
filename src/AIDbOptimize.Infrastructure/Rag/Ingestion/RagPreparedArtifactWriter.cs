using System.Text;
using System.Text.Json;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Preprocess;

namespace AIDbOptimize.Infrastructure.Rag.Ingestion;

public sealed class RagPreparedArtifactWriter
{
    public async Task<int> WriteAsync(
        string corpusRootPath,
        PreprocessedCorpusDocument document,
        IReadOnlyList<PreparedChunkArtifact> chunks,
        CancellationToken cancellationToken = default)
    {
        var preparedRootPath = ResolvePreparedRootPath(corpusRootPath, document.SourceDocument);
        Directory.CreateDirectory(preparedRootPath);

        var writtenFiles = 0;
        var sourceStem = Path.GetFileNameWithoutExtension(document.SourceDocument.FileName);
        var metadataPath = Path.Combine(preparedRootPath, $"{sourceStem}__metadata.json");
        var metadataPayload = new
        {
            sourceType = document.SourceDocument.DocumentType == RagDocumentType.Fact ? "fact" : "case",
            engine = document.SourceDocument.Engine,
            vendor = document.SourceDocument.Vendor,
            topic = document.SourceDocument.Topic,
            problemType = document.SourceDocument.ProblemType,
            sourcePath = document.SourceDocument.RelativePath,
            sourceUrl = document.SourceDocument.SourceUrl,
            sourceTitle = document.Title,
            chunkCount = chunks.Count,
            chunks = chunks.Select(item => new
            {
                item.Chunk.ChunkId,
                item.Chunk.Title,
                item.Chunk.SectionPath,
                item.Metadata.ProductVersion,
                item.Metadata.AppliesTo,
                item.Metadata.ParameterNames,
                item.Metadata.Keywords
            })
        };
        await File.WriteAllTextAsync(
            metadataPath,
            JsonSerializer.Serialize(metadataPayload, new JsonSerializerOptions(JsonSerializerDefaults.Web) { WriteIndented = true }),
            Encoding.UTF8,
            cancellationToken);
        writtenFiles++;

        for (var index = 0; index < chunks.Count; index++)
        {
            var item = chunks[index];
            var chunkPath = Path.Combine(preparedRootPath, $"{sourceStem}__chunk-{index + 1:D4}.md");
            await File.WriteAllTextAsync(
                chunkPath,
                BuildChunkContent(document, item),
                Encoding.UTF8,
                cancellationToken);
            writtenFiles++;
        }

        return writtenFiles;
    }

    private static string ResolvePreparedRootPath(string corpusRootPath, RagSourceDocument sourceDocument)
    {
        var bucket = sourceDocument.DocumentType == RagDocumentType.Fact ? "facts" : "cases";
        return Path.Combine(corpusRootPath, "prepared", bucket, sourceDocument.Engine);
    }

    private static string BuildChunkContent(
        PreprocessedCorpusDocument document,
        PreparedChunkArtifact artifact)
    {
        var builder = new StringBuilder();
        builder.AppendLine("---");
        builder.Append("chunk_id: ").AppendLine(artifact.Chunk.ChunkId);
        builder.Append("source_path: ").AppendLine(document.SourceDocument.RelativePath);
        builder.Append("source_url: ").AppendLine(document.SourceDocument.SourceUrl);
        builder.Append("title: ").AppendLine(EscapeScalar(artifact.Chunk.Title));
        builder.Append("section_path: ").AppendLine(EscapeScalar(artifact.Chunk.SectionPath));
        builder.Append("product_version: ").AppendLine(artifact.Metadata.ProductVersion);
        builder.Append("applies_to: ").AppendLine(EscapeScalar(artifact.Metadata.AppliesTo));
        builder.Append("parameter_names: ").AppendLine(JsonSerializer.Serialize(artifact.Metadata.ParameterNames));
        builder.Append("keywords: ").AppendLine(JsonSerializer.Serialize(artifact.Metadata.Keywords));
        builder.AppendLine("---");
        builder.AppendLine();
        builder.AppendLine(artifact.Chunk.Text);
        return builder.ToString();
    }

    private static string EscapeScalar(string value)
    {
        return value.Contains(':', StringComparison.Ordinal) ? $"\"{value}\"" : value;
    }
}

public sealed record PreparedChunkArtifact(
    CorpusChunk Chunk,
    ExtractedChunkMetadata Metadata);

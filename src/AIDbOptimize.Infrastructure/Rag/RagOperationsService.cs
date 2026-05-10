using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;
using AIDbOptimize.Infrastructure.Rag.Corpus;
using AIDbOptimize.Infrastructure.Rag.Ingestion;
using AIDbOptimize.Infrastructure.Rag.Preprocess;
using AIDbOptimize.Infrastructure.Rag.Workflow;
using Microsoft.Extensions.Hosting;

namespace AIDbOptimize.Infrastructure.Rag;

public sealed class RagOperationsService(
    IHostEnvironment hostEnvironment,
    RagKnowledgeIngestionService ingestionService,
    HistoricalOptimizationCaseProjector caseProjector,
    CorpusPreprocessor preprocessor,
    CorpusChunker chunker,
    CorpusChunkMetadataExtractor metadataExtractor) : IRagOperationsService
{
    public Task<RagCorpusValidationReportDto> ValidateAsync(
        string? corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        var rootPath = ResolveCorpusRootPath(corpusRootPath);
        var issues = new List<RagCorpusValidationIssueDto>();
        var coverage = new Dictionary<(string SourceType, string Engine, string Topic), int>();
        var validCount = 0;
        var invalidCount = 0;

        foreach (var fullPath in EnumerateCorpusFiles(rootPath))
        {
            var relativePath = Path.GetRelativePath(rootPath, fullPath).Replace('\\', '/');
            if (!RagCorpusFileNamer.TryParseRelativePath(relativePath, out var document) || document is null)
            {
                invalidCount++;
                issues.Add(new RagCorpusValidationIssueDto(relativePath, "文件路径或命名不符合约定。", "error"));
                continue;
            }

            var content = File.ReadAllText(fullPath);
            var missingTitle = !content.Contains("source_title:", StringComparison.OrdinalIgnoreCase);
            var missingUrl = !content.Contains("source_url:", StringComparison.OrdinalIgnoreCase);
            if (missingTitle || missingUrl)
            {
                invalidCount++;
                issues.Add(new RagCorpusValidationIssueDto(
                    relativePath,
                    $"front matter 缺少{(missingTitle ? " source_title" : string.Empty)}{(missingTitle && missingUrl ? " 和" : string.Empty)}{(missingUrl ? " source_url" : string.Empty)}。",
                    "error"));
                continue;
            }

            var rawContent = File.ReadAllText(fullPath);
            var preprocessed = preprocessor.Preprocess(document, rawContent);
            var chunks = chunker.Chunk(preprocessed);
            if (chunks.Count == 0)
            {
                invalidCount++;
                issues.Add(new RagCorpusValidationIssueDto(relativePath, "预处理后没有生成有效 chunk。", "error"));
                continue;
            }

            var hasChunkError = false;
            foreach (var chunk in chunks)
            {
                var metadata = metadataExtractor.Extract(document, chunk);
                if (metadata.Keywords.Count == 0)
                {
                    issues.Add(new RagCorpusValidationIssueDto(relativePath, $"chunk `{chunk.ChunkId}` 缺少 keywords。", "warning"));
                }

                if (metadata.ParameterNames.Count == 0)
                {
                    issues.Add(new RagCorpusValidationIssueDto(relativePath, $"chunk `{chunk.ChunkId}` 缺少 parameter_names。", "warning"));
                }

                if (string.IsNullOrWhiteSpace(chunk.SectionPath))
                {
                    invalidCount++;
                    issues.Add(new RagCorpusValidationIssueDto(relativePath, $"chunk `{chunk.ChunkId}` 缺少 section_path。", "error"));
                    hasChunkError = true;
                    break;
                }
            }

            if (hasChunkError)
            {
                continue;
            }

            validCount++;
            var key = (
                document.DocumentType == RagDocumentType.Fact ? "fact" : "case",
                document.Engine,
                document.Topic ?? document.ProblemType ?? "unknown");
            coverage[key] = coverage.TryGetValue(key, out var count) ? count + 1 : 1;
        }

        var coverageItems = coverage
            .OrderBy(item => item.Key.SourceType, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Key.Engine, StringComparer.OrdinalIgnoreCase)
            .ThenBy(item => item.Key.Topic, StringComparer.OrdinalIgnoreCase)
            .Select(item => new RagCoverageItemDto(item.Key.SourceType, item.Key.Engine, item.Key.Topic, item.Value))
            .ToArray();

        return Task.FromResult(new RagCorpusValidationReportDto(validCount, invalidCount, coverageItems, issues));
    }

    public async Task<RagRebuildResultDto> RebuildAsync(
        string? corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        var rootPath = ResolveCorpusRootPath(corpusRootPath);
        var projectionResult = await caseProjector.ProjectAsync(rootPath, cancellationToken);
        var validation = await ValidateAsync(rootPath, cancellationToken);
        if (validation.InvalidDocumentCount > 0)
        {
            throw new InvalidOperationException("RAG corpus validation failed. Fix corpus issues before rebuild.");
        }

        var ingestionResult = await ingestionService.IngestAsync(rootPath, cancellationToken);
        return new RagRebuildResultDto(
            rootPath,
            ingestionResult.DocumentCount,
            ingestionResult.ChunkCount,
            ingestionResult.PreparedFileCount,
            projectionResult.ProjectedCount,
            validation.Coverage);
    }

    private string ResolveCorpusRootPath(string? configuredPath)
    {
        if (!string.IsNullOrWhiteSpace(configuredPath))
        {
            return Path.GetFullPath(configuredPath);
        }

        return Path.GetFullPath(Path.Combine(hostEnvironment.ContentRootPath, "..", "..", "docs", "rag"));
    }

    private static IEnumerable<string> EnumerateCorpusFiles(string rootPath)
    {
        return Directory.GetFiles(rootPath, "*.md", SearchOption.AllDirectories)
            .Where(path => !path.Contains($"{Path.DirectorySeparatorChar}prepared{Path.DirectorySeparatorChar}", StringComparison.OrdinalIgnoreCase))
            .Where(path => !string.Equals(Path.GetFileName(path), "README.md", StringComparison.OrdinalIgnoreCase))
            .Where(path => !string.Equals(Path.GetFileName(path), "CLAUDE.md", StringComparison.OrdinalIgnoreCase));
    }
}

using System.Text.RegularExpressions;
using AIDbOptimize.Infrastructure.Rag.Corpus;

namespace AIDbOptimize.Infrastructure.Rag.Preprocess;

public sealed partial class CorpusChunkMetadataExtractor
{
    public ExtractedChunkMetadata Extract(RagSourceDocument document, CorpusChunk chunk)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentNullException.ThrowIfNull(chunk);

        var parameterNames = ParameterNameRegex()
            .Matches($"{chunk.Title}\n{chunk.Text}")
            .Select(match => match.Value)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();

        var keywords = BuildKeywords(document, chunk)
            .Distinct(StringComparer.OrdinalIgnoreCase)
            .Take(12)
            .ToArray();

        return new ExtractedChunkMetadata(
            ResolveProductVersion(document.SourceUrl),
            ResolveAppliesTo(document),
            parameterNames,
            keywords);
    }

    private static IEnumerable<string> BuildKeywords(RagSourceDocument document, CorpusChunk chunk)
    {
        yield return document.Engine;

        if (!string.IsNullOrWhiteSpace(document.Vendor))
        {
            yield return document.Vendor;
        }

        if (!string.IsNullOrWhiteSpace(document.Topic))
        {
            yield return document.Topic;
        }

        if (!string.IsNullOrWhiteSpace(document.ProblemType))
        {
            yield return document.ProblemType;
        }

        foreach (Match match in KeywordRegex().Matches(chunk.Title))
        {
            yield return match.Value.ToLowerInvariant();
        }
    }

    private static string ResolveProductVersion(string sourceUrl)
    {
        if (sourceUrl.Contains("/current/", StringComparison.OrdinalIgnoreCase))
        {
            return "current";
        }

        var match = VersionRegex().Match(sourceUrl);
        return match.Success ? match.Groups[1].Value : string.Empty;
    }

    private static string ResolveAppliesTo(RagSourceDocument document)
    {
        return string.IsNullOrWhiteSpace(document.Vendor)
            ? document.Engine
            : $"{document.Vendor}:{document.Engine}";
    }

    [GeneratedRegex(@"\b[a-z][a-z0-9]+(?:_[a-z0-9]+)+\b", RegexOptions.IgnoreCase)]
    private static partial Regex ParameterNameRegex();

    [GeneratedRegex(@"\b[a-z]{4,}\b", RegexOptions.IgnoreCase)]
    private static partial Regex KeywordRegex();

    [GeneratedRegex(@"/(\d+\.\d+)(?:/|$)")]
    private static partial Regex VersionRegex();
}

public sealed record ExtractedChunkMetadata(
    string ProductVersion,
    string AppliesTo,
    IReadOnlyList<string> ParameterNames,
    IReadOnlyList<string> Keywords);

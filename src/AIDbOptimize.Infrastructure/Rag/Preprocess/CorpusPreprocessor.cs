using System.Net;
using System.Text.RegularExpressions;
using AIDbOptimize.Infrastructure.Rag.Corpus;

namespace AIDbOptimize.Infrastructure.Rag.Preprocess;

public sealed partial class CorpusPreprocessor
{
    public PreprocessedCorpusDocument Preprocess(RagSourceDocument document, string rawContent)
    {
        ArgumentNullException.ThrowIfNull(document);
        ArgumentException.ThrowIfNullOrWhiteSpace(rawContent);

        var withoutFrontMatter = StripFrontMatter(rawContent);
        var cleaned = LooksLikeHtml(withoutFrontMatter)
            ? ExtractFromHtml(withoutFrontMatter)
            : NormalizePlainText(withoutFrontMatter);
        var title = ResolveTitle(document, cleaned);
        return new PreprocessedCorpusDocument(document, title, cleaned);
    }

    private static string ResolveTitle(RagSourceDocument document, string cleaned)
    {
        var heading = cleaned.Split('\n', StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim().TrimStart('#').Trim())
            .FirstOrDefault(line => !string.IsNullOrWhiteSpace(line));
        return string.IsNullOrWhiteSpace(heading) ? document.SourceTitle : heading;
    }

    private static string StripFrontMatter(string rawContent)
    {
        return FrontMatterRegex().Replace(rawContent, string.Empty, 1).Trim();
    }

    private static bool LooksLikeHtml(string content)
    {
        return content.Contains("<html", StringComparison.OrdinalIgnoreCase)
            || content.Contains("<body", StringComparison.OrdinalIgnoreCase)
            || content.Contains("<div", StringComparison.OrdinalIgnoreCase);
    }

    private static string ExtractFromHtml(string html)
    {
        var content = NoiseElementRegex().Replace(html, string.Empty);
        content = BlockElementRegex().Replace(content, "\n");
        content = RemainingTagRegex().Replace(content, string.Empty);
        content = WebUtility.HtmlDecode(content);
        return NormalizePlainText(content);
    }

    private static string NormalizePlainText(string content)
    {
        var lines = content.Replace("\r\n", "\n")
            .Split('\n')
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .Where(line => !NoiseLineRegex().IsMatch(line))
            .ToArray();
        return string.Join("\n\n", lines);
    }

    [GeneratedRegex(@"(?is)\A---\s*\n.*?\n---\s*\n?")]
    private static partial Regex FrontMatterRegex();

    [GeneratedRegex(@"(?is)<(script|style|noscript|svg|nav|footer|header|aside|form|button).*?>.*?</\1>")]
    private static partial Regex NoiseElementRegex();

    [GeneratedRegex(@"(?is)</?(article|section|main|div|p|li|ul|ol|table|tr|td|h1|h2|h3|h4|h5|h6|br)[^>]*>")]
    private static partial Regex BlockElementRegex();

    [GeneratedRegex(@"(?is)<[^>]+>")]
    private static partial Regex RemainingTagRegex();

    [GeneratedRegex(@"(?i)^(sign in|cookie|privacy|all rights reserved|contact us|back to top)$")]
    private static partial Regex NoiseLineRegex();
}

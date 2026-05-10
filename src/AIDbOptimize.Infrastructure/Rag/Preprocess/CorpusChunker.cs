namespace AIDbOptimize.Infrastructure.Rag.Preprocess;

public sealed class CorpusChunker
{
    public IReadOnlyList<CorpusChunk> Chunk(
        PreprocessedCorpusDocument document,
        int maxChars = 1800,
        int minChars = 200)
    {
        ArgumentNullException.ThrowIfNull(document);
        var sections = SplitSections(document);
        var chunks = new List<CorpusChunk>();

        foreach (var section in sections)
        {
            foreach (var part in SplitLargeSection(section.Text, maxChars))
            {
                AppendChunk(document, section.Title, section.Path, part, minChars, chunks);
            }
        }

        return chunks;
    }

    private static IReadOnlyList<SectionBlock> SplitSections(PreprocessedCorpusDocument document)
    {
        var blocks = new List<SectionBlock>();
        var currentTitle = document.Title;
        var currentPath = document.Title;
        var buffer = new List<string>();

        foreach (var paragraph in document.CleanText.Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
        {
            var trimmed = paragraph.Trim();
            if (trimmed.StartsWith("#", StringComparison.Ordinal))
            {
                Flush(blocks, currentTitle, currentPath, buffer);
                currentTitle = trimmed.TrimStart('#').Trim();
                currentPath = currentTitle;
                continue;
            }

            buffer.Add(trimmed);
        }

        Flush(blocks, currentTitle, currentPath, buffer);
        return blocks;
    }

    private static void Flush(
        List<SectionBlock> blocks,
        string title,
        string path,
        List<string> buffer)
    {
        if (buffer.Count == 0)
        {
            return;
        }

        blocks.Add(new SectionBlock(title, path, string.Join("\n\n", buffer)));
        buffer.Clear();
    }

    private static IEnumerable<string> SplitLargeSection(string text, int maxChars)
    {
        if (text.Length <= maxChars)
        {
            yield return text;
            yield break;
        }

        var paragraphs = text.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
        var current = string.Empty;
        foreach (var paragraph in paragraphs)
        {
            var candidate = string.IsNullOrEmpty(current) ? paragraph : $"{current}\n\n{paragraph}";
            if (candidate.Length > maxChars && !string.IsNullOrEmpty(current))
            {
                yield return current;
                current = paragraph;
                continue;
            }

            current = candidate;
        }

        if (!string.IsNullOrEmpty(current))
        {
            yield return current;
        }
    }

    private static void AppendChunk(
        PreprocessedCorpusDocument document,
        string title,
        string path,
        string text,
        int minChars,
        List<CorpusChunk> chunks)
    {
        if (chunks.Count > 0 && text.Length < minChars)
        {
            var previous = chunks[^1];
            chunks[^1] = previous with
            {
                Text = $"{previous.Text}\n\n{text}"
            };
            return;
        }

        chunks.Add(new CorpusChunk(
            $"{document.SourceDocument.FileName}::chunk-{chunks.Count + 1:D4}",
            document.SourceDocument,
            title,
            path,
            text));
    }

    private sealed record SectionBlock(string Title, string Path, string Text);
}

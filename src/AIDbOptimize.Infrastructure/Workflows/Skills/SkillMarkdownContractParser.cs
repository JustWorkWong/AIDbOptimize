namespace AIDbOptimize.Infrastructure.Workflows.Skills;

/// <summary>
/// Minimal parser used by the skills contract spike.
/// </summary>
public sealed class SkillMarkdownContractParser
{
    public SkillMarkdownContractDocument ParseAndValidate(
        string markdown,
        IReadOnlyCollection<string> requiredMetadataKeys,
        IReadOnlyCollection<string> requiredSectionNames)
    {
        var document = Parse(markdown);

        foreach (var key in requiredMetadataKeys)
        {
            if (!document.Metadata.ContainsKey(key))
            {
                throw new InvalidOperationException($"Missing required metadata `{key}`.");
            }
        }

        foreach (var sectionName in requiredSectionNames)
        {
            if (!document.Sections.ContainsKey(sectionName))
            {
                throw new InvalidOperationException($"Missing required section `## {sectionName}`.");
            }
        }

        return document;
    }

    public SkillMarkdownContractDocument Parse(string markdown)
    {
        if (string.IsNullOrWhiteSpace(markdown))
        {
            throw new InvalidOperationException("Skill markdown cannot be empty.");
        }

        var lines = Normalize(markdown).Split('\n');
        if (lines.Length < 3 || lines[0] != "---")
        {
            throw new InvalidOperationException("Skill markdown must start with front matter delimited by `---`.");
        }

        var metadata = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
        var sections = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        var index = 1;
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            if (line == "---")
            {
                index++;
                break;
            }

            if (string.IsNullOrWhiteSpace(line))
            {
                continue;
            }

            var separatorIndex = line.IndexOf(':');
            if (separatorIndex <= 0 || separatorIndex == line.Length - 1)
            {
                throw new InvalidOperationException($"Invalid front matter line `{line}`.");
            }

            var key = line[..separatorIndex].Trim();
            var value = line[(separatorIndex + 1)..].Trim();
            metadata[key] = value;
        }

        if (index == lines.Length && lines[^1] != "---")
        {
            throw new InvalidOperationException("Skill markdown front matter is not closed.");
        }

        string? currentSection = null;
        var buffer = new List<string>();
        for (; index < lines.Length; index++)
        {
            var line = lines[index];
            if (line.StartsWith("## ", StringComparison.Ordinal))
            {
                FlushSection(sections, currentSection, buffer);
                currentSection = line[3..].Trim();
                buffer.Clear();
                continue;
            }

            if (currentSection is not null)
            {
                buffer.Add(line);
            }
        }

        FlushSection(sections, currentSection, buffer);
        return new SkillMarkdownContractDocument(metadata, sections);
    }

    private static void FlushSection(
        Dictionary<string, string> sections,
        string? sectionName,
        List<string> buffer)
    {
        if (string.IsNullOrWhiteSpace(sectionName))
        {
            return;
        }

        sections[sectionName] = string.Join('\n', buffer).Trim();
    }

    private static string Normalize(string markdown)
    {
        return markdown
            .Replace("\r\n", "\n", StringComparison.Ordinal)
            .Replace('\r', '\n');
    }
}

public sealed record SkillMarkdownContractDocument(
    IReadOnlyDictionary<string, string> Metadata,
    IReadOnlyDictionary<string, string> Sections);

namespace AIDbOptimize.Infrastructure.Rag.Corpus;

public static class RagCorpusFileNamer
{
    private static readonly HashSet<string> SupportedEngines = new(StringComparer.OrdinalIgnoreCase)
    {
        "mysql",
        "postgresql"
    };

    private static readonly HashSet<string> SupportedTopics = new(StringComparer.OrdinalIgnoreCase)
    {
        "memory",
        "connections",
        "cache",
        "checkpoint",
        "logging",
        "replication",
        "storage",
        "timeout",
        "locking",
        "observability"
    };

    public static string CreateFactFileName(string engine, string vendor, string topic, string shortTitle)
    {
        var normalizedEngine = NormalizeEngine(engine);
        var normalizedVendor = NormalizeRequiredSlug(vendor, nameof(vendor));
        var normalizedTopic = NormalizeTopic(topic);
        var normalizedShortTitle = NormalizeRequiredSlug(shortTitle, nameof(shortTitle));

        return $"{normalizedEngine}__{normalizedVendor}__{normalizedTopic}__{normalizedShortTitle}.md";
    }

    public static string CreateCaseFileName(string engine, string problemType, string sessionOrTicket)
    {
        var normalizedEngine = NormalizeEngine(engine);
        var normalizedProblemType = NormalizeTopic(problemType);
        var normalizedSessionOrTicket = NormalizeRequiredSlug(sessionOrTicket, nameof(sessionOrTicket));

        return $"{normalizedEngine}__case__{normalizedProblemType}__{normalizedSessionOrTicket}.md";
    }

    public static bool TryParseRelativePath(string relativePath, out RagSourceDocument? document)
    {
        try
        {
            document = ParseRelativePath(relativePath);
            return true;
        }
        catch
        {
            document = null;
            return false;
        }
    }

    public static RagSourceDocument ParseRelativePath(string relativePath)
    {
        var normalizedPath = NormalizeRelativePath(relativePath);
        var segments = normalizedPath.Split('/', StringSplitOptions.RemoveEmptyEntries);
        if (segments.Length != 3)
        {
            throw CreateCorpusError(relativePath, "relative path must use `<kind>/<bucket>/<file>.md`.");
        }

        return segments[0] switch
        {
            "facts" => ParseFactPath(relativePath, normalizedPath, segments[1], segments[2]),
            "cases" => ParseCasePath(relativePath, normalizedPath, segments[1], segments[2]),
            _ => throw CreateCorpusError(relativePath, "root folder must be `facts` or `cases`.")
        };
    }

    private static RagSourceDocument ParseFactPath(
        string originalPath,
        string normalizedPath,
        string bucket,
        string fileName)
    {
        if (!fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "fact file must use `.md` extension.");
        }

        var segments = SplitStem(originalPath, fileName);
        if (segments.Length != 4)
        {
            throw CreateCorpusError(originalPath, "fact file name must use `<engine>__<vendor>__<topic>__<short-title>.md`.");
        }

        var engine = NormalizeEngine(segments[0]);
        var vendor = NormalizeRequiredSlug(segments[1], "vendor");
        if (string.Equals(vendor, "case", StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "fact vendor segment cannot be `case`.");
        }

        var topic = NormalizeTopic(segments[2]);
        var shortTitle = NormalizeRequiredSlug(segments[3], "shortTitle");
        ValidateFactBucket(originalPath, bucket, engine);

        return new RagSourceDocument(
            RagDocumentType.Fact,
            normalizedPath,
            fileName,
            engine,
            vendor,
            topic,
            null,
            shortTitle,
            string.Empty,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
    }

    private static RagSourceDocument ParseCasePath(
        string originalPath,
        string normalizedPath,
        string bucket,
        string fileName)
    {
        if (!fileName.EndsWith(".md", StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "case file must use `.md` extension.");
        }

        var segments = SplitStem(originalPath, fileName);
        if (segments.Length != 4)
        {
            throw CreateCorpusError(originalPath, "case file name must use `<engine>__case__<problem-type>__<session-or-ticket>.md`.");
        }

        var engine = NormalizeEngine(segments[0]);
        if (!string.Equals(segments[1], "case", StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "case file name must use `case` as the second segment.");
        }

        var problemType = NormalizeTopic(segments[2]);
        var sessionOrTicket = NormalizeRequiredSlug(segments[3], "sessionOrTicket");
        if (!string.Equals(bucket, engine, StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "case folder must match file engine.");
        }

        return new RagSourceDocument(
            RagDocumentType.Case,
            normalizedPath,
            fileName,
            engine,
            null,
            null,
            problemType,
            sessionOrTicket,
            string.Empty,
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase));
    }

    private static string NormalizeRelativePath(string relativePath)
    {
        if (string.IsNullOrWhiteSpace(relativePath))
        {
            throw CreateCorpusError(relativePath, "relative path is required.");
        }

        return relativePath.Trim().Replace('\\', '/');
    }

    private static string[] SplitStem(string originalPath, string fileName)
    {
        var stem = Path.GetFileNameWithoutExtension(fileName);
        var segments = stem.Split("__", StringSplitOptions.None);
        if (segments.Any(string.IsNullOrWhiteSpace))
        {
            throw CreateCorpusError(originalPath, "file name segments cannot be empty.");
        }

        return segments;
    }

    private static void ValidateFactBucket(string originalPath, string bucket, string engine)
    {
        if (string.Equals(bucket, "cloud", StringComparison.OrdinalIgnoreCase))
        {
            return;
        }

        if (!SupportedEngines.Contains(bucket))
        {
            throw CreateCorpusError(originalPath, "fact folder must be `mysql`, `postgresql`, or `cloud`.");
        }

        if (!string.Equals(bucket, engine, StringComparison.OrdinalIgnoreCase))
        {
            throw CreateCorpusError(originalPath, "fact folder must match file engine unless it is `cloud`.");
        }
    }

    private static string NormalizeEngine(string engine)
    {
        var normalized = NormalizeRequiredSlug(engine, nameof(engine));
        if (!SupportedEngines.Contains(normalized))
        {
            throw new InvalidOperationException($"Invalid corpus engine `{engine}`.");
        }

        return normalized;
    }

    private static string NormalizeTopic(string topic)
    {
        var normalized = NormalizeRequiredSlug(topic, nameof(topic));
        if (!SupportedTopics.Contains(normalized))
        {
            throw new InvalidOperationException($"Invalid corpus topic `{topic}`.");
        }

        return normalized;
    }

    private static string NormalizeRequiredSlug(string value, string parameterName)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"Corpus `{parameterName}` is required.");
        }

        var normalized = value.Trim().ToLowerInvariant();
        normalized = normalized.Replace("_", "-").Replace(" ", "-").Replace("/", "-");

        var buffer = new char[normalized.Length];
        var length = 0;
        var previousDash = false;

        foreach (var character in normalized)
        {
            if (char.IsLetterOrDigit(character))
            {
                buffer[length++] = character;
                previousDash = false;
                continue;
            }

            if (character == '-' && !previousDash)
            {
                buffer[length++] = character;
                previousDash = true;
            }
        }

        normalized = new string(buffer, 0, length).Trim('-');
        if (string.IsNullOrWhiteSpace(normalized))
        {
            throw new InvalidOperationException($"Corpus `{parameterName}` must contain slug characters.");
        }

        return normalized;
    }

    private static InvalidOperationException CreateCorpusError(string? path, string message)
    {
        return new InvalidOperationException($"Invalid corpus path `{path}`: {message}");
    }
}

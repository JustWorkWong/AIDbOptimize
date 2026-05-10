namespace AIDbOptimize.DataInit.Options;

public sealed class RagSeedOptions
{
    public const string SectionName = "RagSeed";

    public bool Enabled { get; set; }

    public string? CorpusRootPath { get; set; }
}

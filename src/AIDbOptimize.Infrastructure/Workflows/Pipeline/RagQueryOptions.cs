namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed class RagQueryOptions
{
    public int FactTopK { get; init; } = 5;

    public int CaseTopK { get; init; } = 3;

    public int MaxInjectTotal { get; init; } = 8;
}

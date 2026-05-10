namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

public sealed record RetrievedKnowledgeItem(
    string KnowledgeId,
    string SourceType,
    string Reference,
    string Title,
    string Summary,
    string Snippet,
    string Url,
    string SectionPath,
    string Topic,
    IReadOnlyList<string> ParameterNames,
    double RetrievalScore,
    string Citation);

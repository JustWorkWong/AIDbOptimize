using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// Optional host-context collector hook for db config workflows.
/// </summary>
public interface IDbConfigHostContextCollector
{
    Task<DbConfigHostContextCollectionResult> CollectAsync(
        McpConnectionEntity connection,
        Guid? workflowSessionId,
        string workflowNodeName,
        string requestedBy,
        string? traceId,
        CancellationToken cancellationToken = default);
}

public sealed record DbConfigHostContextCollectionResult(
    string ResourceScope,
    IReadOnlyList<DbConfigEvidenceItem> Items,
    IReadOnlyList<DbConfigMissingContextItem> MissingContextItems,
    IReadOnlyList<DbConfigCollectionMetadataItem> CollectionMetadata,
    IReadOnlyList<string> Warnings);

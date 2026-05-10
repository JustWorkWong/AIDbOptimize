using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagCaseAuditQuery
{
    Task<IReadOnlyList<RagCaseAuditItemDto>> ListAsync(CancellationToken cancellationToken = default);
}

using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Rag.Services;

public sealed class RagCaseAuditAppService(IRagCaseAuditQuery query)
{
    public Task<IReadOnlyList<RagCaseAuditItemDto>> ListAsync(CancellationToken cancellationToken = default)
    {
        return query.ListAsync(cancellationToken);
    }
}

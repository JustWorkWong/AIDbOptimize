using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Rag.Services;

public sealed class RagAssetStatusAppService(IRagAssetStatusQuery query)
{
    public Task<RagAssetStatusDto> GetAsync(CancellationToken cancellationToken = default)
    {
        return query.GetAsync(cancellationToken);
    }
}

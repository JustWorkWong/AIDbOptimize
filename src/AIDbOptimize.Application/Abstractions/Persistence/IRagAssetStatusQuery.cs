using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagAssetStatusQuery
{
    Task<RagAssetStatusDto> GetAsync(CancellationToken cancellationToken = default);
}

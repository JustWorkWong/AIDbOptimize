using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Rag.Services;

public sealed class RagOperationsAppService(IRagOperationsService operationsService)
{
    public Task<RagCorpusValidationReportDto> ValidateAsync(
        string? corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        return operationsService.ValidateAsync(corpusRootPath, cancellationToken);
    }

    public Task<RagRebuildResultDto> RebuildAsync(
        string? corpusRootPath,
        CancellationToken cancellationToken = default)
    {
        return operationsService.RebuildAsync(corpusRootPath, cancellationToken);
    }
}

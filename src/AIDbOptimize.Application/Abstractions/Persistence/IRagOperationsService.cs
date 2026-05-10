using AIDbOptimize.Application.Rag.Dtos;

namespace AIDbOptimize.Application.Abstractions.Persistence;

public interface IRagOperationsService
{
    Task<RagCorpusValidationReportDto> ValidateAsync(string? corpusRootPath, CancellationToken cancellationToken = default);

    Task<RagRebuildResultDto> RebuildAsync(string? corpusRootPath, CancellationToken cancellationToken = default);
}

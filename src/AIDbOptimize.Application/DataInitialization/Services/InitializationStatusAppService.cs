using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.DataInitialization.Dtos;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.DataInitialization.Services;

/// <summary>
/// 对外提供初始化状态读取能力。
/// </summary>
public sealed class InitializationStatusAppService
{
    private readonly IDataInitializationRunRepository _dataInitializationRunRepository;

    public InitializationStatusAppService(IDataInitializationRunRepository dataInitializationRunRepository)
    {
        _dataInitializationRunRepository = dataInitializationRunRepository;
    }

    public Task<IReadOnlyCollection<DataInitializationStatusDto>> GetLatestAsync(
        CancellationToken cancellationToken = default)
    {
        return _dataInitializationRunRepository.GetLatestAsync(cancellationToken);
    }

    public Task<DataInitializationStatusDto?> GetLatestAsync(
        DatabaseEngine engine,
        CancellationToken cancellationToken = default)
    {
        return _dataInitializationRunRepository.GetLatestAsync(engine, cancellationToken);
    }
}

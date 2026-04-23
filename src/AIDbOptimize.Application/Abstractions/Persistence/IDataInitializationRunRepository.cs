using AIDbOptimize.Application.DataInitialization.Dtos;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Abstractions.Persistence;

/// <summary>
/// 数据初始化状态持久化抽象。
/// </summary>
public interface IDataInitializationRunRepository
{
    Task<IReadOnlyCollection<DataInitializationStatusDto>> GetLatestAsync(
        CancellationToken cancellationToken = default);

    Task<DataInitializationStatusDto?> GetLatestAsync(
        DatabaseEngine engine,
        CancellationToken cancellationToken = default);

    Task SaveAsync(
        DataInitializationStatusDto status,
        CancellationToken cancellationToken = default);
}

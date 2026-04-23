using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.DataInitialization.Dtos;
using AIDbOptimize.Domain.Mcp.Enums;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// 数据初始化状态仓储实现。
/// </summary>
public sealed class DataInitializationRunRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IDataInitializationRunRepository
{
    /// <summary>
    /// 读取全部引擎的最近一次初始化状态。
    /// </summary>
    public async Task<IReadOnlyCollection<DataInitializationStatusDto>> GetLatestAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.DataInitializationRuns
            .AsNoTracking()
            .OrderByDescending(x => x.StartedAt)
            .Select(x => new DataInitializationStatusDto(
                x.Engine,
                x.DatabaseName,
                x.SeedVersion,
                x.TargetOrderCount,
                x.State,
                x.StartedAt,
                x.CompletedAt,
                x.ErrorMessage))
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 读取指定引擎最近一次初始化状态。
    /// </summary>
    public async Task<DataInitializationStatusDto?> GetLatestAsync(DatabaseEngine engine, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.DataInitializationRuns
            .AsNoTracking()
            .Where(x => x.Engine == engine)
            .OrderByDescending(x => x.StartedAt)
            .Select(x => new DataInitializationStatusDto(
                x.Engine,
                x.DatabaseName,
                x.SeedVersion,
                x.TargetOrderCount,
                x.State,
                x.StartedAt,
                x.CompletedAt,
                x.ErrorMessage))
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 当前阶段不通过该仓储回写状态，交由 DataInit 内部服务直接控制。
    /// 先保留接口占位，避免 Application 侧依赖断开。
    /// </summary>
    public Task SaveAsync(DataInitializationStatusDto status, CancellationToken cancellationToken = default)
    {
        throw new NotSupportedException("当前版本通过 InitializationStateService 直接写入初始化状态。");
    }
}

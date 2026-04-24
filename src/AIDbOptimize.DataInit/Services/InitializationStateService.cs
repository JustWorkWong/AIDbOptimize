using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.DataInit.Services;

/// <summary>
/// 统一管理 `data_initialization_runs` 的读写。
/// 该表用于展示初始化状态，不作为幂等主判据。
/// </summary>
public sealed class InitializationStateService(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
{
    /// <summary>
    /// 读取指定数据库与种子版本最近一次初始化记录。
    /// </summary>
    public async Task<DataInitializationRunEntity?> GetLatestAsync(
        DatabaseEngine engine,
        string databaseName,
        string seedVersion,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.DataInitializationRuns
            .AsNoTracking()
            .Where(x => x.Engine == engine && x.DatabaseName == databaseName && x.SeedVersion == seedVersion)
            .OrderByDescending(x => x.StartedAt)
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 将当前初始化批次标记为进行中。
    /// </summary>
    public async Task<DataInitializationRunEntity> MarkInProgressAsync(
        DatabaseEngine engine,
        string databaseName,
        string seedVersion,
        long targetOrderCount,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        var entity = new DataInitializationRunEntity
        {
            Id = Guid.NewGuid(),
            Engine = engine,
            DatabaseName = databaseName,
            SeedVersion = seedVersion,
            TargetOrderCount = targetOrderCount,
            State = DataInitializationState.InProgress,
            StartedAt = DateTimeOffset.UtcNow
        };

        dbContext.DataInitializationRuns.Add(entity);
        await dbContext.SaveChangesAsync(cancellationToken);
        return entity;
    }

    /// <summary>
    /// 将初始化记录标记为已完成。
    /// </summary>
    public async Task MarkCompletedAsync(Guid runId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await GetRequiredEntityAsync(dbContext, runId, cancellationToken);
        entity.State = DataInitializationState.Completed;
        entity.CompletedAt = DateTimeOffset.UtcNow;
        entity.ErrorMessage = null;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 当迁移已完成但种子尚未落地时，将状态回退为未开始。
    /// 这样可以避免界面误报“已完成”，也不会让幂等逻辑被错误短路。
    /// </summary>
    public async Task MarkNotStartedAsync(Guid runId, string? message, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await GetRequiredEntityAsync(dbContext, runId, cancellationToken);
        entity.State = DataInitializationState.NotStarted;
        entity.CompletedAt = null;
        entity.ErrorMessage = message;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 将初始化记录标记为失败。
    /// </summary>
    public async Task MarkFailedAsync(Guid runId, string? errorMessage, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await GetRequiredEntityAsync(dbContext, runId, cancellationToken);
        entity.State = DataInitializationState.Failed;
        entity.CompletedAt = DateTimeOffset.UtcNow;
        entity.ErrorMessage = errorMessage;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 读取必定存在的初始化记录。
    /// </summary>
    private static async Task<DataInitializationRunEntity> GetRequiredEntityAsync(
        ControlPlaneDbContext dbContext,
        Guid runId,
        CancellationToken cancellationToken)
    {
        var entity = await dbContext.DataInitializationRuns
            .FirstOrDefaultAsync(x => x.Id == runId, cancellationToken);

        if (entity is null)
        {
            throw new InvalidOperationException($"未找到数据初始化记录：{runId}");
        }

        return entity;
    }
}

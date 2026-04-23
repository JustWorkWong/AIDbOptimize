using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.DataInit.Services;

/// <summary>
/// 统一管理 data_initialization_runs 的读写。
/// 这样 hosted service 不需要直接了解控制面表结构。
/// </summary>
public sealed class InitializationStateService(IDbContextFactory<ControlPlaneDbContext> dbContextFactory)
{
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

    public async Task MarkCompletedAsync(Guid runId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await GetRequiredEntityAsync(dbContext, runId, cancellationToken);
        entity.State = DataInitializationState.Completed;
        entity.CompletedAt = DateTimeOffset.UtcNow;
        entity.ErrorMessage = null;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task MarkFailedAsync(Guid runId, string? errorMessage, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await GetRequiredEntityAsync(dbContext, runId, cancellationToken);
        entity.State = DataInitializationState.Failed;
        entity.CompletedAt = DateTimeOffset.UtcNow;
        entity.ErrorMessage = errorMessage;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

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

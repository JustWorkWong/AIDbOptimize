using AIDbOptimize.DataInit.Abstractions;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.DataInit.Services;

/// <summary>
/// PostgreSQL 业务测试库初始化器。
/// 当前阶段只负责应用迁移；真实 10w 级数据种子会在后续迁移中补齐。
/// </summary>
public sealed class PostgreSqlLabInitializer(
    IDbContextFactory<PostgreSqlLabDbContext> dbContextFactory,
    ILogger<PostgreSqlLabInitializer> logger) : IDataInitializer
{
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public string DatabaseName => "aidbopt_lab_pg";

    public string SeedVersion => "v1";

    public long TargetOrderCount => 100_000;

    /// <summary>
    /// 应用 PostgreSQL 业务测试库迁移。
    /// 当前返回 false，表示真实数据种子尚未在迁移中落地。
    /// </summary>
    public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        logger.LogInformation("PostgreSQL 业务测试库迁移已完成，待后续补充 10w 级种子迁移。");
        return false;
    }
}

using AIDbOptimize.DataInit.Abstractions;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.DataInit.Services;

/// <summary>
/// MySQL 业务测试库初始化器。
/// 当前阶段只负责应用迁移；真实 10w 级数据种子会在后续迁移中补齐。
/// </summary>
public sealed class MySqlLabInitializer(
    IDbContextFactory<MySqlLabDbContext> dbContextFactory,
    ILogger<MySqlLabInitializer> logger) : IDataInitializer
{
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public string DatabaseName => "aidbopt_lab_mysql";

    public string SeedVersion => "v1";

    public long TargetOrderCount => 100_000;

    /// <summary>
    /// 应用 MySQL 业务测试库迁移。
    /// 当前版本的业务测试数据已经下沉到迁移文件中，因此迁移成功即可视为初始化完成。
    /// </summary>
    public async Task<bool> InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        logger.LogInformation("MySQL 业务测试库迁移与 10w 级种子已完成。");
        return true;
    }
}

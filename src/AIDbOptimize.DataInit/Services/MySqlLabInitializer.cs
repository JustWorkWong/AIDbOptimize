using AIDbOptimize.DataInit.Abstractions;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.DataInit.Services;

/// <summary>
/// MySQL 业务测试库初始化器骨架。
/// 当前先打通迁移入口，后续再补大规模造数逻辑。
/// </summary>
public sealed class MySqlLabInitializer(
    IDbContextFactory<MySqlLabDbContext> dbContextFactory,
    ILogger<MySqlLabInitializer> logger) : IDataInitializer
{
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public string DatabaseName => "aidbopt_lab_mysql";

    public string SeedVersion => "v1";

    public long TargetOrderCount => 10_000_000;

    public async Task InitializeAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);

        logger.LogInformation(
            "MySQL 初始化骨架已执行：迁移已应用，后续补充 1000 万级造数逻辑。");
    }
}

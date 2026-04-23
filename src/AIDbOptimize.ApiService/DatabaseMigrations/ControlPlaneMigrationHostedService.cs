using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.ApiService.DatabaseMigrations;

/// <summary>
/// 控制面数据库自动迁移服务。
/// API 启动时优先应用控制面迁移，确保后续 MCP 配置与初始化状态表可用。
/// </summary>
internal sealed class ControlPlaneMigrationHostedService(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    ILogger<ControlPlaneMigrationHostedService> logger) : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        await dbContext.Database.MigrateAsync(cancellationToken);
        logger.LogInformation("控制面数据库迁移完成。");
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

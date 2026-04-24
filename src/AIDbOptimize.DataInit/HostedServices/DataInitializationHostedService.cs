using AIDbOptimize.DataInit.Abstractions;
using AIDbOptimize.DataInit.Services;
using AIDbOptimize.Domain.Seed.Enums;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.DataInit.HostedServices;

/// <summary>
/// DataInit 的总编排入口。
/// 启动时顺序执行 PostgreSQL / MySQL 初始化器，并统一写入控制面状态。
/// </summary>
public sealed class DataInitializationHostedService(
    IEnumerable<IDataInitializer> initializers,
    InitializationStateService initializationStateService,
    IHostApplicationLifetime hostApplicationLifetime,
    ILogger<DataInitializationHostedService> logger) : IHostedService
{
    /// <summary>
    /// 启动时顺序执行业务测试库初始化。
    /// 所有初始化器完成后主动结束当前宿主进程，交还控制权给 Aspire。
    /// </summary>
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("开始执行业务测试库初始化流程。");

        try
        {
            foreach (var initializer in initializers.OrderBy(x => x.Engine))
            {
                await ExecuteInitializerAsync(initializer, cancellationToken);
            }

            logger.LogInformation("业务测试库初始化流程执行完成。");
        }
        finally
        {
            // DataInit 是一次性作业，执行完成后主动结束宿主，便于 Aspire 接续后续流程。
            hostApplicationLifetime.StopApplication();
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }

    /// <summary>
    /// 执行单个初始化器，并负责状态流转。
    /// </summary>
    private async Task ExecuteInitializerAsync(IDataInitializer initializer, CancellationToken cancellationToken)
    {
        var run = await initializationStateService.MarkInProgressAsync(
            initializer.Engine,
            initializer.DatabaseName,
            initializer.SeedVersion,
            initializer.TargetOrderCount,
            cancellationToken);

        try
        {
            var completed = await initializer.InitializeAsync(cancellationToken);

            if (completed)
            {
                await initializationStateService.MarkCompletedAsync(run.Id, cancellationToken);
                return;
            }

            await initializationStateService.MarkNotStartedAsync(
                run.Id,
                "已完成迁移，但真实种子迁移尚未落地。",
                cancellationToken);
        }
        catch (Exception exception)
        {
            await initializationStateService.MarkFailedAsync(run.Id, exception.Message, cancellationToken);
            logger.LogError(exception, "{Engine} 初始化失败。", initializer.Engine);
            throw;
        }
    }
}

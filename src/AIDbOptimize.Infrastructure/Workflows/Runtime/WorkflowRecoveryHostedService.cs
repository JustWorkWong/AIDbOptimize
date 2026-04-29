using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

/// <summary>
/// Recovers resumable workflow sessions on startup.
/// </summary>
public sealed class WorkflowRecoveryHostedService(
    IServiceScopeFactory scopeFactory,
    ILogger<WorkflowRecoveryHostedService> logger)
    : IHostedService
{
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        using var scope = scopeFactory.CreateScope();
        var dbContextFactory = scope.ServiceProvider.GetRequiredService<IDbContextFactory<ControlPlaneDbContext>>();
        var runtime = scope.ServiceProvider.GetRequiredService<IWorkflowRuntime>();

        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var candidateSessionIds = await dbContext.WorkflowSessions
            .AsNoTracking()
            .Where(x => x.Status == WorkflowSessionStatus.Running || x.Status == WorkflowSessionStatus.Recovering)
            .Select(x => x.Id)
            .ToListAsync(cancellationToken);

        logger.LogInformation(
            "Workflow recovery scan started. CandidateSessionCount={CandidateSessionCount}",
            candidateSessionIds.Count);

        foreach (var sessionId in candidateSessionIds)
        {
            try
            {
                await runtime.ResumeAsync(
                    sessionId,
                    new WorkflowResumeRequest("recovery"),
                    cancellationToken);
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "Workflow recovery failed for SessionId={SessionId}", sessionId);
            }
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}

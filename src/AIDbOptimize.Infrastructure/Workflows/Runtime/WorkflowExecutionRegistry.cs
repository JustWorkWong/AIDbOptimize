using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

public interface IWorkflowExecutionRegistry
{
    void StartOrReplace(Guid sessionId, Func<CancellationToken, Task> work);

    bool TryCancel(Guid sessionId);
}

/// <summary>
/// Tracks active workflow background executions so they can be cancelled by session id.
/// </summary>
public sealed class WorkflowExecutionRegistry(ILogger<WorkflowExecutionRegistry> logger) : IWorkflowExecutionRegistry
{
    private readonly ConcurrentDictionary<Guid, ExecutionRegistration> _registrations = new();

    public void StartOrReplace(Guid sessionId, Func<CancellationToken, Task> work)
    {
        var registration = new ExecutionRegistration(new CancellationTokenSource());
        var existing = _registrations.AddOrUpdate(
            sessionId,
            registration,
            (_, current) =>
            {
                current.CancellationTokenSource.Cancel();
                return registration;
            });

        if (!ReferenceEquals(existing, registration))
        {
            registration.CancellationTokenSource.Cancel();
            throw new InvalidOperationException($"Workflow execution registration collision for session {sessionId}.");
        }

        registration.Task = Task.Run(async () =>
        {
            try
            {
                await work(registration.CancellationTokenSource.Token);
            }
            catch (OperationCanceledException) when (registration.CancellationTokenSource.IsCancellationRequested)
            {
                logger.LogInformation("Workflow execution cancelled. SessionId={SessionId}", sessionId);
            }
            finally
            {
                _registrations.TryRemove(new KeyValuePair<Guid, ExecutionRegistration>(sessionId, registration));
                registration.CancellationTokenSource.Dispose();
            }
        });
    }

    public bool TryCancel(Guid sessionId)
    {
        if (!_registrations.TryGetValue(sessionId, out var registration))
        {
            return false;
        }

        registration.CancellationTokenSource.Cancel();
        return true;
    }

    private sealed class ExecutionRegistration(CancellationTokenSource cancellationTokenSource)
    {
        public CancellationTokenSource CancellationTokenSource { get; } = cancellationTokenSource;

        public Task? Task { get; set; }
    }
}

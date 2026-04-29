using System.Text.Json;
using Microsoft.Agents.AI.Workflows;
using Microsoft.Agents.AI.Workflows.Checkpointing;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

public static class MafWorkflowGraphFactory
{
    public static Workflow BuildMinimalWorkflow()
    {
        var startExecutor = new FunctionExecutor<MafWorkflowStartMessage, MafValidatedMessage>(
            "DbConfigInputValidationExecutor",
            (message, context, cancellationToken) =>
                ValueTask.FromResult(new MafValidatedMessage(message.SessionId, message.Command)));

        var nextExecutor = new FunctionExecutor<MafValidatedMessage, MafSnapshotMessage>(
            "DbConfigSnapshotCollectorExecutor",
            (message, context, cancellationToken) =>
                ValueTask.FromResult(new MafSnapshotMessage(
                    message.SessionId,
                    message.Command,
                    new Domain.DbConfigOptimization.Models.DbConfigSnapshot(
                        Domain.Mcp.Enums.DatabaseEngine.PostgreSql,
                        "graph-test",
                        "graph",
                        new Dictionary<string, string>(),
                        Array.Empty<string>()))));

        var startBinding = startExecutor.BindExecutor();
        var nextBinding = nextExecutor.BindExecutor();
        var builder = new WorkflowBuilder(startBinding)
            .WithName("DbConfigOptimizationGraphProbe")
            .WithDescription("Graph probe for db config workflow integration.");
        builder.AddEdge(startBinding, nextBinding);
        builder.WithOutputFrom(nextBinding);
        return builder.Build();
    }

    public static CheckpointManager CreateCheckpointManager(MafJsonCheckpointStore store)
    {
        return CheckpointManager.CreateJson(store, new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    public static async Task<(Run Run, RunStatus Status, CheckpointInfo? LastCheckpoint)> ExecuteMinimalAsync(
        MafJsonCheckpointStore store,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var workflow = BuildMinimalWorkflow();
        var checkpointManager = CreateCheckpointManager(store);
        var run = await InProcessExecution.RunAsync(
            workflow,
            new MafWorkflowStartMessage(
                sessionId,
                new DbConfigWorkflowCommand(
                    sessionId,
                    "probe",
                    null,
                    true,
                    false,
                    true)),
            checkpointManager,
            sessionId.ToString(),
            cancellationToken);
        var status = await run.GetStatusAsync(cancellationToken);
        return (run, status, run.LastCheckpoint);
    }

    public static Workflow BuildReviewProbeWorkflow()
    {
        var startExecutor = new FunctionExecutor<MafWorkflowStartMessage, MafReviewRequest>(
            "ReviewProbeStart",
            (message, context, cancellationToken) =>
                ValueTask.FromResult(new MafReviewRequest(
                    message.SessionId,
                    "probe-review",
                    "{}",
                    "workflow",
                    "prompt",
                    Guid.Empty,
                    Guid.Empty,
                    """{"totalTokens":0}""")));
        var reviewPort = RequestPort.Create<MafReviewRequest, MafReviewResponse>("workflow-review-port");
        var completionExecutor = new FunctionExecutor<MafReviewResponse, MafCompletionMessage>(
            "ReviewProbeCompletion",
            (response, context, cancellationToken) =>
                ValueTask.FromResult(new MafCompletionMessage(
                    Guid.Empty,
                    response.AdjustmentsJson,
                    string.Equals(response.Action, "reject", StringComparison.OrdinalIgnoreCase),
                    response.Comment,
                    response.Action,
                    response.Reviewer,
                    response.Comment,
                    response.AdjustmentsJson)));

        var startBinding = startExecutor.BindExecutor();
        var reviewBinding = reviewPort.BindAsExecutor(false);
        var completionBinding = completionExecutor.BindExecutor();
        var builder = new WorkflowBuilder(startBinding)
            .WithName("DbConfigReviewProbe")
            .WithDescription("Review port probe for db config workflow integration.");
        builder.AddEdge(startBinding, reviewBinding);
        builder.AddEdge(reviewBinding, completionBinding);
        builder.WithOutputFrom(completionBinding);
        return builder.Build();
    }
}

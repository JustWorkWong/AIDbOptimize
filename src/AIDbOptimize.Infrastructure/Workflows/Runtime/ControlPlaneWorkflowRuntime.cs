using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Nodes;
using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Workflows.Dtos;
using AIDbOptimize.Domain.DbConfigOptimization.Enums;
using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Infrastructure.Agents;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Services;
using Microsoft.Agents.AI.Workflows;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

/// <summary>
/// Unified control-plane workflow runtime.
/// </summary>
public sealed class ControlPlaneWorkflowRuntime(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    DbConfigInputValidationExecutor inputValidationExecutor,
    DbConfigSnapshotCollectorExecutor snapshotCollectorExecutor,
    DbConfigRuleAnalysisExecutor ruleAnalysisExecutor,
    IDbConfigDiagnosisAgentExecutor diagnosisAgentExecutor,
    RecommendationSchemaValidator schemaValidator,
    DbConfigGroundingExecutor groundingExecutor,
    DbConfigHumanReviewGateExecutor humanReviewGateExecutor,
    ReviewAdjustmentValidator reviewAdjustmentValidator,
    IAgentSessionPersistenceService agentSessionPersistenceService,
    IAgentSummaryService agentSummaryService)
    : IWorkflowRuntime
{
    private static readonly JsonSerializerOptions SerializerOptions = new(JsonSerializerDefaults.Web);

    public Task<WorkflowSessionDetailDto> StartDbConfigAsync(
        DbConfigWorkflowCommand command,
        CancellationToken cancellationToken = default)
    {
        return RunInternalAsync(
            new WorkflowRunDescriptor(WorkflowRunMode.Start, null, command, null),
            cancellationToken);
    }

    public Task<WorkflowSessionDetailDto> ResumeAsync(
        Guid sessionId,
        WorkflowResumeRequest request,
        CancellationToken cancellationToken = default)
    {
        var mode = string.Equals(request.Trigger, "recovery", StringComparison.OrdinalIgnoreCase)
            ? WorkflowRunMode.Recovery
            : WorkflowRunMode.Resume;
        return RunInternalAsync(
            new WorkflowRunDescriptor(mode, sessionId, null, request),
            cancellationToken);
    }

    public async Task<WorkflowCancellationResultDto> CancelAsync(
        Guid sessionId,
        CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleOrDefaultAsync(x => x.Id == sessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session not found: {sessionId}");

        session.Status = WorkflowSessionStatus.Cancelled;
        session.CurrentNodeKey = "Cancelled";
        session.UpdatedAt = DateTimeOffset.UtcNow;
        session.CompletedAt = session.UpdatedAt;
        session.ErrorMessage = null;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            currentNode = session.CurrentNodeKey,
            status = PublicStatus(session.Status),
            cancelledAt = session.CompletedAt
        });

        var nextSequence = await GetNextEventSequenceAsync(dbContext, sessionId, cancellationToken);
        AppendEvent(
            dbContext,
            sessionId,
            ref nextSequence,
            WorkflowEventType.WorkflowCancelled,
            "workflow.cancelled",
            new { sessionId },
            "Workflow was cancelled.");

        dbContext.WorkflowCheckpoints.Add(await CreateCheckpointAsync(dbContext, session, nextSequence, cancellationToken));
        await dbContext.SaveChangesAsync(cancellationToken);

        return new WorkflowCancellationResultDto(
            sessionId.ToString(),
            true,
            PublicStatus(session.Status));
    }

    private async Task<WorkflowSessionDetailDto> RunInternalAsync(
        WorkflowRunDescriptor descriptor,
        CancellationToken cancellationToken)
    {
        using var activity = AIDbOptimizeTelemetry.WorkflowActivitySource.StartActivity(
            descriptor.Mode == WorkflowRunMode.Start ? "workflow.start" : "workflow.resume");

        if (descriptor.SessionId.HasValue)
        {
            activity?.SetTag("workflow.session_id", descriptor.SessionId.Value);
        }

        var stopwatch = Stopwatch.StartNew();
        if (descriptor.Mode == WorkflowRunMode.Start)
        {
            AIDbOptimizeTelemetry.WorkflowStarted.Add(1);
        }
        else
        {
            AIDbOptimizeTelemetry.WorkflowResumed.Add(1);
        }

        try
        {
            var result = descriptor.Mode switch
            {
                WorkflowRunMode.Start => await RunStartAsync(descriptor.Command!, activity, cancellationToken),
                WorkflowRunMode.Resume => await RunResumeAsync(descriptor.SessionId!.Value, descriptor.ResumeRequest!, activity, cancellationToken),
                WorkflowRunMode.Recovery => await RunRecoveryAsync(descriptor.SessionId!.Value, descriptor.ResumeRequest!, activity, cancellationToken),
                _ => throw new InvalidOperationException($"Unsupported workflow mode: {descriptor.Mode}")
            };

            stopwatch.Stop();
            AIDbOptimizeTelemetry.WorkflowDurationMs.Record(stopwatch.Elapsed.TotalMilliseconds);
            return result;
        }
        catch
        {
            stopwatch.Stop();
            AIDbOptimizeTelemetry.WorkflowDurationMs.Record(stopwatch.Elapsed.TotalMilliseconds);
            AIDbOptimizeTelemetry.WorkflowFailed.Add(1);
            throw;
        }
    }

    private async Task<WorkflowSessionDetailDto> RunStartAsync(
        DbConfigWorkflowCommand command,
        Activity? activity,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var connection = await dbContext.McpConnections
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == command.ConnectionId, cancellationToken)
            ?? throw new InvalidOperationException($"Connection not found: {command.ConnectionId}");

        var now = DateTimeOffset.UtcNow;
        var session = new WorkflowSessionEntity
        {
            Id = Guid.NewGuid(),
            ConnectionId = command.ConnectionId,
            WorkflowName = "DbConfigOptimization",
            EngineType = "maf",
            Status = WorkflowSessionStatus.Running,
            RequestedBy = string.IsNullOrWhiteSpace(command.RequestedBy) ? "frontend" : command.RequestedBy,
            InputPayloadJson = JsonSerializer.Serialize(command, SerializerOptions),
            StateJson = "{}",
            ResultType = "db-config-optimization-report",
            ResultPayloadJson = "{}",
            CurrentNodeKey = "DbConfigInputValidationExecutor",
            EngineRunId = Guid.NewGuid().ToString("N"),
            CreatedAt = now,
            UpdatedAt = now
        };
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            connectionId = session.ConnectionId
        });

        dbContext.WorkflowSessions.Add(session);
        await dbContext.SaveChangesAsync(cancellationToken);

        var nextSequence = 0L;
        AppendEvent(
            dbContext,
            session.Id,
            ref nextSequence,
            WorkflowEventType.WorkflowStarted,
            "workflow.started",
            new { sessionId = session.Id, runId = session.EngineRunId },
            "Workflow started.");

        try
        {
            var graphResult = await ExecuteMafStartGraphAsync(session.Id, command, cancellationToken);
            dbContext.ChangeTracker.Clear();
            session = await dbContext.WorkflowSessions
                .SingleAsync(x => x.Id == session.Id, cancellationToken);
            connection = await dbContext.McpConnections
                .AsNoTracking()
                .SingleAsync(x => x.Id == session.ConnectionId, cancellationToken);
            MafReviewRequest? reviewPayload = null;
            var persistedDiagnosis = await TryBuildPersistedDiagnosisArtifactsAsync(dbContext, session, cancellationToken);
            if (graphResult.RequestInfo is not null &&
                graphResult.RequestInfo.Request.TryGetDataAs<MafReviewRequest>(out var reviewRequestData))
            {
                reviewPayload = reviewRequestData;
            }

            session.ResultPayloadJson = graphResult.Grounded?.ReportJson
                ?? reviewPayload?.PayloadJson
                ?? persistedDiagnosis?.ReportJson
                ?? session.ResultPayloadJson;
            session.AgentSessionId = graphResult.Grounded?.AgentSessionId
                ?? reviewPayload?.AgentSessionId
                ?? persistedDiagnosis?.AgentSessionId
                ?? session.AgentSessionId;
            session.TotalTokens = graphResult.Grounded is not null
                ? ExtractTokenCount(graphResult.Grounded.TokenUsageJson)
                : reviewPayload is not null
                    ? ExtractTokenCount(reviewPayload.TokenUsageJson)
                    : persistedDiagnosis is not null
                        ? ExtractTokenCount(persistedDiagnosis.TokenUsageJson)
                    : session.TotalTokens;
            session.CurrentNodeKey = graphResult.RequestInfo is null
                ? "DbConfigGroundingExecutor"
                : "DbConfigHumanReviewGateExecutor";
            session.EngineCheckpointRef ??= graphResult.LastCheckpointRef;
            session.UpdatedAt = DateTimeOffset.UtcNow;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                agentSessionId = session.AgentSessionId
            });
            await dbContext.SaveChangesAsync(cancellationToken);
            nextSequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);

            if (graphResult.RequestInfo is not null ||
                graphResult.RunStatus == RunStatus.PendingRequests ||
                (command.RequireHumanReview && (graphResult.Grounded is not null || persistedDiagnosis is not null)))
            {
                var reviewTask = await ExecuteReviewGateAsync(
                    dbContext,
                    session,
                    connection,
                    command,
                    new DiagnosisArtifacts(
                        reviewPayload?.PayloadJson ?? graphResult.Grounded?.ReportJson ?? persistedDiagnosis?.ReportJson ?? session.ResultPayloadJson,
                        reviewPayload?.Prompt ?? graphResult.Grounded?.Prompt ?? persistedDiagnosis?.Prompt ?? string.Empty,
                        reviewPayload?.AgentSessionId ?? graphResult.Grounded?.AgentSessionId ?? persistedDiagnosis?.AgentSessionId ?? session.AgentSessionId ?? Guid.Empty,
                        reviewPayload?.SummaryId ?? graphResult.Grounded?.SummaryId ?? persistedDiagnosis?.SummaryId ?? Guid.Empty,
                        reviewPayload?.TokenUsageJson ?? graphResult.Grounded?.TokenUsageJson ?? persistedDiagnosis?.TokenUsageJson ?? """{"promptTokens":0,"completionTokens":0,"totalTokens":0}"""),
                    nextSequence,
                    graphResult.RequestInfo,
                    graphResult.LastCheckpointRef,
                    cancellationToken);

                await dbContext.SaveChangesAsync(cancellationToken);
                return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
            }

            if (graphResult.Completion is null)
            {
                if (graphResult.Grounded is null && persistedDiagnosis is null)
                {
                    throw new InvalidOperationException("MAF graph finished without a completion output.");
                }

                graphResult = graphResult with
                {
                    Completion = new MafCompletionMessage(
                        graphResult.Grounded?.SessionId ?? session.Id,
                        graphResult.Grounded?.ReportJson ?? persistedDiagnosis?.ReportJson ?? session.ResultPayloadJson,
                        false,
                        null,
                        null,
                        null,
                        null,
                        null)
                };
            }

            await ExecuteCompletionAsync(
                dbContext,
                session,
                graphResult.Completion.ResultPayloadJson,
                nextSequence,
                "Workflow completed without human review.",
                cancellationToken);

            await dbContext.SaveChangesAsync(cancellationToken);
            return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
        }
        catch (Exception ex)
        {
            var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
            session.Status = WorkflowSessionStatus.Failed;
            session.CurrentNodeKey ??= "DbConfigCompletionExecutor";
            session.ErrorMessage = ex.Message;
            session.UpdatedAt = DateTimeOffset.UtcNow;
            session.CompletedAt = session.UpdatedAt;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                error = ex.Message
            });
            AppendEvent(
                dbContext,
                session.Id,
                ref sequence,
                WorkflowEventType.WorkflowFailed,
                "workflow.failed",
                new { sessionId = session.Id, error = ex.Message },
                "Workflow failed.");
            dbContext.WorkflowCheckpoints.Add(await CreateCheckpointAsync(dbContext, session, sequence, cancellationToken));
            await dbContext.SaveChangesAsync(cancellationToken);
            throw;
        }
    }

    private async Task<WorkflowSessionDetailDto> RunResumeAsync(
        Guid sessionId,
        WorkflowResumeRequest request,
        Activity? activity,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .Include(x => x.Connection)
            .SingleOrDefaultAsync(x => x.Id == sessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session not found: {sessionId}");

        var reviewTaskId = request.ReviewTaskId is not null && Guid.TryParse(request.ReviewTaskId, out var parsedTaskId)
            ? parsedTaskId
            : session.ActiveReviewTaskId ?? throw new InvalidOperationException("No active review task was found for this workflow.");

        var reviewTask = await dbContext.WorkflowReviewTasks
            .SingleOrDefaultAsync(x => x.Id == reviewTaskId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow review task not found: {reviewTaskId}");

        if (reviewTask.Status == WorkflowReviewTaskStatus.Pending)
        {
            throw new InvalidOperationException("The review task is still pending.");
        }

        if (!string.Equals(reviewTask.EngineRunId, session.EngineRunId, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Review task run id does not match the workflow session.");
        }

        if (!string.Equals(reviewTask.EngineCheckpointRef, session.EngineCheckpointRef, StringComparison.Ordinal))
        {
            throw new InvalidOperationException("Review task checkpoint ref does not match the latest session checkpoint.");
        }

        var graphResult = await ExecuteMafResumeGraphAsync(session.Id, reviewTask, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        AppendEvent(
            dbContext,
            session.Id,
            ref sequence,
            WorkflowEventType.ReviewResolved,
            "review.resolved",
            new
            {
                sessionId = session.Id,
                reviewTaskId = reviewTask.Id,
                action = request.Action
            },
            "Review decision received.",
            reviewTaskId: reviewTask.Id);

        var waitingNode = await dbContext.WorkflowNodeExecutions
            .Where(x => x.WorkflowSessionId == session.Id && x.Status == WorkflowNodeExecutionStatus.WaitingForReview)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (waitingNode is not null)
        {
            waitingNode.Status = reviewTask.Status == WorkflowReviewTaskStatus.Rejected
                ? WorkflowNodeExecutionStatus.Failed
                : WorkflowNodeExecutionStatus.Succeeded;
            waitingNode.CompletedAt = DateTimeOffset.UtcNow;
            waitingNode.ErrorMessage = reviewTask.Status == WorkflowReviewTaskStatus.Rejected
                ? reviewTask.DecisionNote ?? "Review rejected."
                : null;
            waitingNode.OutputPayloadJson = reviewTask.PayloadJson;
        }

        session.ActiveReviewTaskId = null;
        session.CurrentNodeKey = reviewTask.Status == WorkflowReviewTaskStatus.Rejected
            ? "Cancelled"
            : "DbConfigCompletionExecutor";
        session.UpdatedAt = DateTimeOffset.UtcNow;
        session.EngineCheckpointRef = graphResult.LastCheckpointRef ?? session.EngineCheckpointRef;

        if (graphResult.Completion?.Action == "adjust")
        {
            var normalized = reviewAdjustmentValidator.ValidateAndNormalize(
                ParseJson(graphResult.Completion.AdjustmentsJson ?? "{}"),
                graphResult.Completion.Comment);
            session.ResultPayloadJson = ApplyAdjustments(session.ResultPayloadJson, normalized, graphResult.Completion.Comment);
            reviewTask.PayloadJson = session.ResultPayloadJson;
            await UpdateAgentSummaryAsync(dbContext, session, graphResult.Completion.Comment, cancellationToken);
        }

        if (graphResult.Completion?.Cancelled == true || reviewTask.Status == WorkflowReviewTaskStatus.Rejected)
        {
            session.Status = WorkflowSessionStatus.Cancelled;
            session.CompletedAt = session.UpdatedAt;
            session.ErrorMessage = graphResult.Completion?.ErrorMessage ?? reviewTask.DecisionNote;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                reviewTaskId = reviewTask.Id
            });
            AppendEvent(
                dbContext,
                session.Id,
                ref sequence,
                WorkflowEventType.WorkflowCancelled,
                "workflow.cancelled",
                new { sessionId = session.Id, reviewTaskId = reviewTask.Id },
                "Workflow cancelled after review rejection.",
                reviewTaskId: reviewTask.Id);
            dbContext.WorkflowCheckpoints.Add(await CreateCheckpointAsync(dbContext, session, sequence, cancellationToken));
            await dbContext.SaveChangesAsync(cancellationToken);
            return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
        }

        session.Status = WorkflowSessionStatus.Running;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            reviewTaskId = reviewTask.Id
        });

        await ExecuteCompletionAsync(
            dbContext,
            session,
            session.ResultPayloadJson,
            sequence,
            "Workflow resumed after review and completed.",
            cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
    }

    private async Task<WorkflowSessionDetailDto> RunRecoveryAsync(
        Guid sessionId,
        WorkflowResumeRequest request,
        Activity? activity,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .Include(x => x.Connection)
            .SingleOrDefaultAsync(x => x.Id == sessionId, cancellationToken)
            ?? throw new InvalidOperationException($"Workflow session not found: {sessionId}");

        if (session.Status == WorkflowSessionStatus.WaitingForReview)
        {
            return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
        }

        if (string.IsNullOrWhiteSpace(session.EngineCheckpointRef))
        {
            throw new InvalidOperationException("Workflow recovery requires an engine checkpoint ref.");
        }

        session.Status = WorkflowSessionStatus.Recovering;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            trigger = request.Trigger
        });

        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        AppendEvent(
            dbContext,
            session.Id,
            ref sequence,
            WorkflowEventType.WorkflowRecovered,
            "workflow.recovered",
            new { sessionId = session.Id, checkpointRef = session.EngineCheckpointRef },
            "Workflow recovery resumed.");

        var graphResult = await ExecuteMafRecoveryGraphAsync(session.Id, session.EngineCheckpointRef, cancellationToken);
        session.EngineCheckpointRef = graphResult.LastCheckpointRef ?? session.EngineCheckpointRef;

        if (graphResult.RequestInfo is not null)
        {
            var reviewTask = await CreateOrUpdateReviewTaskFromMafRequestAsync(
                dbContext,
                session,
                graphResult.RequestInfo,
                sequence,
                cancellationToken);
            session.Status = WorkflowSessionStatus.WaitingForReview;
            session.CurrentNodeKey = "DbConfigHumanReviewGateExecutor";
            session.ActiveReviewTaskId = reviewTask.Id;
            session.CompletedAt = null;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                reviewTaskId = reviewTask.Id,
                recoveredAt = session.UpdatedAt
            });
            await dbContext.SaveChangesAsync(cancellationToken);
            return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
        }

        if (graphResult.Completion is null)
        {
            var pendingReviewTask = await dbContext.WorkflowReviewTasks
                .Where(x => x.WorkflowSessionId == session.Id && x.Status == WorkflowReviewTaskStatus.Pending)
                .OrderByDescending(x => x.CreatedAt)
                .FirstOrDefaultAsync(cancellationToken);

            if (pendingReviewTask is not null)
            {
                session.Status = WorkflowSessionStatus.WaitingForReview;
                session.CurrentNodeKey = "DbConfigHumanReviewGateExecutor";
                session.ActiveReviewTaskId = pendingReviewTask.Id;
                session.CompletedAt = null;
                session.UpdatedAt = DateTimeOffset.UtcNow;
                UpdateStateJson(session, new
                {
                    sessionId = session.Id,
                    status = PublicStatus(session.Status),
                    currentNode = session.CurrentNodeKey,
                    reviewTaskId = pendingReviewTask.Id,
                    recoveredAt = session.UpdatedAt
                });
                await dbContext.SaveChangesAsync(cancellationToken);
                return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
            }

            throw new InvalidOperationException("MAF recovery finished without a completion or pending request.");
        }

        session.ResultPayloadJson = string.IsNullOrWhiteSpace(graphResult.Completion.ResultPayloadJson)
            ? session.ResultPayloadJson
            : graphResult.Completion.ResultPayloadJson;
        session.CurrentNodeKey = graphResult.Completion.Cancelled
            ? "Cancelled"
            : "DbConfigCompletionExecutor";
        session.CompletedAt = DateTimeOffset.UtcNow;
        session.UpdatedAt = session.CompletedAt.Value;
        session.ActiveReviewTaskId = null;

        if (graphResult.Completion.Cancelled)
        {
            session.Status = WorkflowSessionStatus.Cancelled;
            session.ErrorMessage = graphResult.Completion.ErrorMessage;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                recoveredAt = session.CompletedAt
            });
            dbContext.WorkflowCheckpoints.Add(await CreateCheckpointAsync(dbContext, session, sequence, cancellationToken));
            AppendEvent(
                dbContext,
                session.Id,
                ref sequence,
                WorkflowEventType.WorkflowCancelled,
                "workflow.cancelled",
                new { sessionId = session.Id, trigger = "recovery" },
                "Workflow cancelled after recovery.");
        }
        else
        {
            session.Status = WorkflowSessionStatus.Succeeded;
            session.ErrorMessage = null;
            UpdateStateJson(session, new
            {
                sessionId = session.Id,
                status = PublicStatus(session.Status),
                currentNode = session.CurrentNodeKey,
                recoveredAt = session.CompletedAt
            });
            dbContext.WorkflowCheckpoints.Add(await CreateCheckpointAsync(dbContext, session, sequence, cancellationToken));
            AppendEvent(
                dbContext,
                session.Id,
                ref sequence,
                WorkflowEventType.WorkflowCompleted,
                "workflow.completed",
                new { sessionId = session.Id, trigger = "recovery" },
                "Workflow completed after recovery.");
        }

        await dbContext.SaveChangesAsync(cancellationToken);
        return await BuildDetailAsync(dbContext, session.Id, cancellationToken);
    }

    private async Task<MafGraphRunResult> ExecuteMafStartGraphAsync(
        Guid sessionId,
        DbConfigWorkflowCommand command,
        CancellationToken cancellationToken)
    {
        var store = new MafJsonCheckpointStore(dbContextFactory);
        var checkpointManager = MafWorkflowGraphFactory.CreateCheckpointManager(store);

        var validationExecutor = new FunctionExecutor<MafWorkflowStartMessage, MafValidatedMessage>(
            "DbConfigInputValidationExecutor",
            (message, context, ct) => HandleValidationMessageAsync(message, ct));
        var snapshotExecutor = new FunctionExecutor<MafValidatedMessage, MafSnapshotMessage>(
            "DbConfigSnapshotCollectorExecutor",
            (message, context, ct) => HandleSnapshotMessageAsync(message, ct));
        var ruleExecutor = new FunctionExecutor<MafSnapshotMessage, MafEvidenceMessage>(
            "DbConfigRuleAnalysisExecutor",
            (message, context, ct) => HandleRuleAnalysisMessageAsync(message, ct));
        var diagnosisExecutor = new FunctionExecutor<MafEvidenceMessage, MafDiagnosisMessage>(
            "DbConfigDiagnosisAgentExecutor",
            (message, context, ct) => HandleDiagnosisMessageAsync(message, ct));
        var groundingExecutorBinding = new FunctionExecutor<MafDiagnosisMessage, MafGroundedMessage>(
            "DbConfigGroundingExecutor",
            (message, context, ct) => HandleGroundingMessageAsync(message, ct));
        var reviewRequestExecutor = new FunctionExecutor<MafGroundedMessage, MafReviewRequest>(
            "DbConfigHumanReviewGateExecutor",
            (message, context, ct) => HandleReviewRequestMessageAsync(message, ct));
        var directCompletionExecutor = new FunctionExecutor<MafGroundedMessage, MafCompletionMessage>(
            "DbConfigCompletionDirectExecutor",
            (message, context, ct) => ValueTask.FromResult(new MafCompletionMessage(
                message.SessionId,
                message.ReportJson,
                false,
                null,
                null,
                null,
                null,
                null)));
        var reviewPort = RequestPort.Create<MafReviewRequest, MafReviewResponse>("workflow-review-port");
        var reviewResponseExecutor = new FunctionExecutor<MafReviewResponse, MafCompletionMessage>(
            "DbConfigReviewResponseExecutor",
            (response, context, ct) => ValueTask.FromResult(new MafCompletionMessage(
                Guid.Empty,
                string.Empty,
                string.Equals(response.Action, "reject", StringComparison.OrdinalIgnoreCase),
                response.Comment,
                response.Action,
                response.Reviewer,
                response.Comment,
                response.AdjustmentsJson)));

        var startBinding = validationExecutor.BindExecutor();
        var snapshotBinding = snapshotExecutor.BindExecutor();
        var ruleBinding = ruleExecutor.BindExecutor();
        var diagnosisBinding = diagnosisExecutor.BindExecutor();
        var groundingBinding = groundingExecutorBinding.BindExecutor();
        var reviewRequestBinding = reviewRequestExecutor.BindExecutor();
        var directCompletionBinding = directCompletionExecutor.BindExecutor();
        var reviewPortBinding = reviewPort.BindAsExecutor(false);
        var reviewResponseBinding = reviewResponseExecutor.BindExecutor();

        var workflow = new WorkflowBuilder(startBinding)
            .WithName("DbConfigOptimization")
            .WithDescription("Database configuration optimization workflow")
            .AddEdge(startBinding, snapshotBinding)
            .AddEdge(snapshotBinding, ruleBinding)
            .AddEdge(ruleBinding, diagnosisBinding)
            .AddEdge(diagnosisBinding, groundingBinding)
            .AddEdge<MafGroundedMessage>(groundingBinding, reviewRequestBinding, message => message.Command.RequireHumanReview)
            .AddEdge<MafGroundedMessage>(groundingBinding, directCompletionBinding, message => !message.Command.RequireHumanReview)
            .AddEdge(reviewRequestBinding, reviewPortBinding)
            .AddEdge(reviewPortBinding, reviewResponseBinding)
            .WithOutputFrom(directCompletionBinding, reviewResponseBinding)
            .Build();

        var run = await InProcessExecution.RunAsync(
            workflow,
            new MafWorkflowStartMessage(sessionId, command),
            checkpointManager,
            sessionId.ToString(),
            cancellationToken);

        var runStatus = await run.GetStatusAsync(cancellationToken);

        var grounded = run.OutgoingEvents
            .OfType<WorkflowOutputEvent>()
            .Select(workflowEvent => workflowEvent.As<MafGroundedMessage>())
            .FirstOrDefault(message => message is not null);
        var completion = run.OutgoingEvents
            .OfType<WorkflowOutputEvent>()
            .Select(workflowEvent => workflowEvent.As<MafCompletionMessage>())
            .FirstOrDefault(message => message is not null);
        var requestInfo = run.OutgoingEvents.OfType<RequestInfoEvent>().FirstOrDefault();

        return new MafGraphRunResult(
            grounded is null ? null : grounded with { LastCheckpointRef = run.LastCheckpoint?.CheckpointId },
            completion,
            requestInfo,
            run.LastCheckpoint?.CheckpointId,
            runStatus);
    }

    private async Task<MafGraphRunResult> ExecuteMafResumeGraphAsync(
        Guid sessionId,
        WorkflowReviewTaskEntity reviewTask,
        CancellationToken cancellationToken)
    {
        var store = new MafJsonCheckpointStore(dbContextFactory);
        var checkpointManager = MafWorkflowGraphFactory.CreateCheckpointManager(store);
        var command = await LoadCommandAsync(sessionId, cancellationToken);
        var workflow = BuildDbConfigMafWorkflow();
        var run = await InProcessExecution.ResumeAsync(
            workflow,
            new CheckpointInfo(sessionId.ToString(), reviewTask.EngineCheckpointRef ?? throw new InvalidOperationException("Missing checkpoint ref.")),
            checkpointManager,
            cancellationToken);

        var pendingRequest = run.OutgoingEvents.OfType<RequestInfoEvent>().FirstOrDefault();
        if (pendingRequest is null)
        {
            var runStatusWithoutRequest = await run.GetStatusAsync(cancellationToken);
            return new MafGraphRunResult(
                null,
                new MafCompletionMessage(
                    sessionId,
                    string.Empty,
                    reviewTask.Status == WorkflowReviewTaskStatus.Rejected,
                    reviewTask.DecisionNote,
                    reviewTask.Status == WorkflowReviewTaskStatus.Rejected
                        ? "reject"
                        : reviewTask.Status == WorkflowReviewTaskStatus.Adjusted
                            ? "adjust"
                            : "approve",
                    reviewTask.DecisionBy,
                    reviewTask.DecisionNote,
                    reviewTask.AdjustmentsJson),
                null,
                run.LastCheckpoint?.CheckpointId ?? reviewTask.EngineCheckpointRef,
                runStatusWithoutRequest);
        }
        var response = pendingRequest.Request.CreateResponse(new MafReviewResponse(
            reviewTask.Status == WorkflowReviewTaskStatus.Rejected ? "reject" : reviewTask.Status == WorkflowReviewTaskStatus.Adjusted ? "adjust" : "approve",
            reviewTask.DecisionBy,
            reviewTask.DecisionNote,
            reviewTask.AdjustmentsJson));

        await run.ResumeAsync(new[] { response }, cancellationToken);

        var runStatus = await run.GetStatusAsync(cancellationToken);

        var completion = run.OutgoingEvents
            .OfType<WorkflowOutputEvent>()
            .Select(workflowEvent => workflowEvent.As<MafCompletionMessage>())
            .FirstOrDefault(message => message is not null)
            ?? throw new InvalidOperationException("MAF resume finished without a completion output.");
        return new MafGraphRunResult(null, completion, null, run.LastCheckpoint?.CheckpointId, runStatus);
    }

    private async Task<MafGraphRunResult> ExecuteMafRecoveryGraphAsync(
        Guid sessionId,
        string checkpointRef,
        CancellationToken cancellationToken)
    {
        var store = new MafJsonCheckpointStore(dbContextFactory);
        var checkpointManager = MafWorkflowGraphFactory.CreateCheckpointManager(store);
        var workflow = BuildDbConfigMafWorkflow();
        var run = await InProcessExecution.ResumeAsync(
            workflow,
            new CheckpointInfo(sessionId.ToString(), checkpointRef),
            checkpointManager,
            cancellationToken);

        var runStatus = await run.GetStatusAsync(cancellationToken);

        var requestInfo = run.OutgoingEvents.OfType<RequestInfoEvent>().FirstOrDefault();
        var completion = run.OutgoingEvents
            .OfType<WorkflowOutputEvent>()
            .Select(workflowEvent => workflowEvent.As<MafCompletionMessage>())
            .FirstOrDefault(message => message is not null);
        return new MafGraphRunResult(null, completion, requestInfo, run.LastCheckpoint?.CheckpointId, runStatus);
    }

    private Workflow BuildDbConfigMafWorkflow()
    {
        var validationExecutor = new FunctionExecutor<MafWorkflowStartMessage, MafValidatedMessage>(
            "DbConfigInputValidationExecutor",
            (message, context, ct) => HandleValidationMessageAsync(message, ct));
        var snapshotExecutor = new FunctionExecutor<MafValidatedMessage, MafSnapshotMessage>(
            "DbConfigSnapshotCollectorExecutor",
            (message, context, ct) => HandleSnapshotMessageAsync(message, ct));
        var ruleExecutor = new FunctionExecutor<MafSnapshotMessage, MafEvidenceMessage>(
            "DbConfigRuleAnalysisExecutor",
            (message, context, ct) => HandleRuleAnalysisMessageAsync(message, ct));
        var diagnosisExecutor = new FunctionExecutor<MafEvidenceMessage, MafDiagnosisMessage>(
            "DbConfigDiagnosisAgentExecutor",
            (message, context, ct) => HandleDiagnosisMessageAsync(message, ct));
        var groundingExecutorBinding = new FunctionExecutor<MafDiagnosisMessage, MafGroundedMessage>(
            "DbConfigGroundingExecutor",
            (message, context, ct) => HandleGroundingMessageAsync(message, ct));
        var reviewRequestExecutor = new FunctionExecutor<MafGroundedMessage, MafReviewRequest>(
            "DbConfigHumanReviewGateExecutor",
            (message, context, ct) => HandleReviewRequestMessageAsync(message, ct));
        var directCompletionExecutor = new FunctionExecutor<MafGroundedMessage, MafCompletionMessage>(
            "DbConfigCompletionDirectExecutor",
            (message, context, ct) => ValueTask.FromResult(new MafCompletionMessage(
                message.SessionId,
                message.ReportJson,
                false,
                null,
                null,
                null,
                null,
                null)));
        var reviewPort = RequestPort.Create<MafReviewRequest, MafReviewResponse>("workflow-review-port");
        var reviewResponseExecutor = new FunctionExecutor<MafReviewResponse, MafCompletionMessage>(
            "DbConfigReviewResponseExecutor",
            (response, context, ct) => ValueTask.FromResult(new MafCompletionMessage(
                Guid.Empty,
                string.Empty,
                string.Equals(response.Action, "reject", StringComparison.OrdinalIgnoreCase),
                response.Comment,
                response.Action,
                response.Reviewer,
                response.Comment,
                response.AdjustmentsJson)));

        var startBinding = validationExecutor.BindExecutor();
        var snapshotBinding = snapshotExecutor.BindExecutor();
        var ruleBinding = ruleExecutor.BindExecutor();
        var diagnosisBinding = diagnosisExecutor.BindExecutor();
        var groundingBinding = groundingExecutorBinding.BindExecutor();
        var reviewRequestBinding = reviewRequestExecutor.BindExecutor();
        var directCompletionBinding = directCompletionExecutor.BindExecutor();
        var reviewPortBinding = reviewPort.BindAsExecutor(false);
        var reviewResponseBinding = reviewResponseExecutor.BindExecutor();

        return new WorkflowBuilder(startBinding)
            .WithName("DbConfigOptimization")
            .WithDescription("Database configuration optimization workflow")
            .AddEdge(startBinding, snapshotBinding)
            .AddEdge(snapshotBinding, ruleBinding)
            .AddEdge(ruleBinding, diagnosisBinding)
            .AddEdge(diagnosisBinding, groundingBinding)
            .AddEdge<MafGroundedMessage>(groundingBinding, reviewRequestBinding, message => message.Command.RequireHumanReview)
            .AddEdge<MafGroundedMessage>(groundingBinding, directCompletionBinding, message => !message.Command.RequireHumanReview)
            .AddEdge(reviewRequestBinding, reviewPortBinding)
            .AddEdge(reviewPortBinding, reviewResponseBinding)
            .WithOutputFrom(directCompletionBinding, reviewResponseBinding)
            .Build();
    }

    private async Task<WorkflowReviewTaskEntity> CreateOrUpdateReviewTaskFromMafRequestAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        RequestInfoEvent requestInfo,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        if (!requestInfo.Request.TryGetDataAs<MafReviewRequest>(out var reviewPayload) || reviewPayload is null)
        {
            throw new InvalidOperationException("Review request payload could not be deserialized from MAF RequestInfoEvent.");
        }

        var checkpoint = await dbContext.WorkflowCheckpoints
            .Where(x => x.WorkflowSessionId == session.Id && x.CheckpointRef == session.EngineCheckpointRef)
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MAF checkpoint ref not found in control plane: {session.EngineCheckpointRef}");

        var reviewTask = await dbContext.WorkflowReviewTasks
            .Where(x => x.WorkflowSessionId == session.Id && x.RequestId == requestInfo.Request.RequestId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (reviewTask is null)
        {
            reviewTask = new WorkflowReviewTaskEntity
            {
                Id = Guid.NewGuid(),
                WorkflowSessionId = session.Id,
                CheckpointId = checkpoint.Id,
                RequestId = requestInfo.Request.RequestId,
                EngineRunId = session.EngineRunId,
                EngineCheckpointRef = checkpoint.CheckpointRef,
                Title = string.IsNullOrWhiteSpace(reviewPayload.Title)
                    ? "Database config optimization review"
                    : reviewPayload.Title,
                PayloadJson = string.IsNullOrWhiteSpace(reviewPayload.PayloadJson)
                    ? session.ResultPayloadJson
                    : reviewPayload.PayloadJson,
                Status = WorkflowReviewTaskStatus.Pending,
                RequestedBy = string.IsNullOrWhiteSpace(reviewPayload.RequestedBy)
                    ? session.RequestedBy
                    : reviewPayload.RequestedBy,
                CreatedAt = DateTimeOffset.UtcNow,
                UpdatedAt = DateTimeOffset.UtcNow
            };
            dbContext.WorkflowReviewTasks.Add(reviewTask);
        }
        else
        {
            reviewTask.CheckpointId = checkpoint.Id;
            reviewTask.EngineCheckpointRef = checkpoint.CheckpointRef;
            reviewTask.EngineRunId = session.EngineRunId;
            reviewTask.PayloadJson = string.IsNullOrWhiteSpace(reviewPayload.PayloadJson)
                ? reviewTask.PayloadJson
                : reviewPayload.PayloadJson;
            reviewTask.Status = WorkflowReviewTaskStatus.Pending;
            reviewTask.DecisionBy = null;
            reviewTask.DecisionNote = null;
            reviewTask.CompletedAt = null;
            reviewTask.UpdatedAt = DateTimeOffset.UtcNow;
        }

        AppendEvent(
            dbContext,
            session.Id,
            ref nextSequence,
            WorkflowEventType.ReviewRequested,
            "review.requested",
            new
            {
                sessionId = session.Id,
                reviewTaskId = reviewTask.Id,
                requestId = reviewTask.RequestId
            },
            "Human review requested after recovery.",
            reviewTaskId: reviewTask.Id);

        return reviewTask;
    }

    private async ValueTask<MafValidatedMessage> HandleValidationMessageAsync(
        MafWorkflowStartMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        var connection = await dbContext.McpConnections
            .AsNoTracking()
            .SingleAsync(x => x.Id == session.ConnectionId, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        await ExecuteValidationAsync(dbContext, session, connection, sequence, cancellationToken);
        return new MafValidatedMessage(message.SessionId, message.Command);
    }

    private async ValueTask<MafSnapshotMessage> HandleSnapshotMessageAsync(
        MafValidatedMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        var connection = await dbContext.McpConnections
            .AsNoTracking()
            .SingleAsync(x => x.Id == session.ConnectionId, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        var snapshot = await ExecuteSnapshotCollectionAsync(dbContext, session, connection, sequence, Activity.Current, cancellationToken);
        return new MafSnapshotMessage(message.SessionId, message.Command, snapshot);
    }

    private async ValueTask<MafEvidenceMessage> HandleRuleAnalysisMessageAsync(
        MafSnapshotMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        var evidence = await ExecuteRuleAnalysisAsync(dbContext, session, message.Snapshot, sequence, cancellationToken);
        return new MafEvidenceMessage(message.SessionId, message.Command, evidence);
    }

    private async ValueTask<MafDiagnosisMessage> HandleDiagnosisMessageAsync(
        MafEvidenceMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        var connection = await dbContext.McpConnections
            .AsNoTracking()
            .SingleAsync(x => x.Id == session.ConnectionId, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        var diagnosis = await ExecuteDiagnosisAsync(dbContext, session, connection, message.Evidence, message.Command.Notes, sequence, cancellationToken);
        return new MafDiagnosisMessage(
            message.SessionId,
            message.Command,
            message.Evidence,
            diagnosis.Prompt,
            diagnosis.ReportJson,
            diagnosis.AgentSessionId,
            diagnosis.SummaryId,
            diagnosis.TokenUsageJson);
    }

    private async ValueTask<MafGroundedMessage> HandleGroundingMessageAsync(
        MafDiagnosisMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        var sequence = await GetNextEventSequenceAsync(dbContext, session.Id, cancellationToken);
        if (message.Command.EnableEvidenceGrounding)
        {
            await ExecuteGroundingAsync(dbContext, session, message.Evidence, message.ReportJson, sequence, cancellationToken);
        }

        return new MafGroundedMessage(
            message.SessionId,
            message.Command,
            message.Evidence,
            message.Prompt,
            message.ReportJson,
            message.AgentSessionId,
            message.SummaryId,
            message.TokenUsageJson,
            null);
    }

    private async ValueTask<MafReviewRequest> HandleReviewRequestMessageAsync(
        MafGroundedMessage message,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .SingleAsync(x => x.Id == message.SessionId, cancellationToken);
        session.CurrentNodeKey = "DbConfigHumanReviewGateExecutor";
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            agentSessionId = session.AgentSessionId
        });
        await dbContext.SaveChangesAsync(cancellationToken);

        return new MafReviewRequest(
            message.SessionId,
            "Database config optimization report",
            message.ReportJson,
            message.Command.RequestedBy,
            message.Prompt,
            message.AgentSessionId,
            message.SummaryId,
            message.TokenUsageJson);
    }

    private async Task<DbConfigWorkflowCommand> LoadCommandAsync(
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var session = await dbContext.WorkflowSessions
            .AsNoTracking()
            .SingleAsync(x => x.Id == sessionId, cancellationToken);
        return JsonSerializer.Deserialize<DbConfigWorkflowCommand>(session.InputPayloadJson, SerializerOptions)
            ?? throw new InvalidOperationException("Workflow input payload could not be deserialized.");
    }

    private async Task ExecuteValidationAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        McpConnectionEntity connection,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        var inputJson = JsonSerializer.Serialize(new
        {
            session.ConnectionId,
            connection.DatabaseName,
            connection.Engine
        }, SerializerOptions);

        var execution = CreateNodeExecution(session.Id, "DbConfigInputValidationExecutor", "deterministic", inputJson);
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Input validation started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        inputValidationExecutor.Validate(
            session.ConnectionId,
            connection.DatabaseName,
            connection.Engine,
            requireHumanReview: true);

        CompleteNodeExecution(execution, """{"validated":true}""");
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey
        });
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey
        }, "Input validation completed.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<DbConfigSnapshot> ExecuteSnapshotCollectionAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        McpConnectionEntity connection,
        long nextSequence,
        Activity? activity,
        CancellationToken cancellationToken)
    {
        var execution = CreateNodeExecution(session.Id, "DbConfigSnapshotCollectorExecutor", "deterministic", """{}""");
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Snapshot collection started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        var snapshot = await snapshotCollectorExecutor.CollectAsync(
            connection,
            session.Id,
            execution.NodeKey,
            session.RequestedBy,
            activity?.TraceId.ToString(),
            cancellationToken);

        CompleteNodeExecution(execution, JsonSerializer.Serialize(snapshot, SerializerOptions));
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            snapshotSource = snapshot.Source
        });
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey,
            snapshotSource = snapshot.Source
        }, "Snapshot collection completed.", execution.Id);
        nextSequence = await LinkLatestToolExecutionAsync(dbContext, session.Id, execution.Id, nextSequence, cancellationToken);
        await dbContext.SaveChangesAsync(cancellationToken);
        return snapshot;
    }

    private async Task<DbConfigEvidencePack> ExecuteRuleAnalysisAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        DbConfigSnapshot snapshot,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        var execution = CreateNodeExecution(session.Id, "DbConfigRuleAnalysisExecutor", "deterministic", JsonSerializer.Serialize(snapshot, SerializerOptions));
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Rule analysis started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        var evidence = ruleAnalysisExecutor.Analyze(snapshot);

        CompleteNodeExecution(execution, JsonSerializer.Serialize(evidence, SerializerOptions));
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            recommendationCount = evidence.Recommendations.Count
        });
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey,
            recommendationCount = evidence.Recommendations.Count
        }, "Rule analysis completed.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);
        return evidence;
    }

    private async Task<DiagnosisArtifacts> ExecuteDiagnosisAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        McpConnectionEntity connection,
        DbConfigEvidencePack evidence,
        string? notes,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        var prompt = diagnosisAgentExecutor.BuildPrompt(
            connection.DisplayName,
            connection.DatabaseName,
            connection.Engine.ToString(),
            notes ?? string.Empty,
            evidence);

        var execution = CreateNodeExecution(session.Id, "DbConfigDiagnosisAgentExecutor", "agent", JsonSerializer.Serialize(evidence, SerializerOptions));
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Diagnosis agent started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        var diagnosis = await diagnosisAgentExecutor.ExecuteAsync(evidence, notes, cancellationToken);
        schemaValidator.Validate(diagnosis.ReportJson);

        var agentSessionResult = await agentSessionPersistenceService.CreateSessionAsync(
            new AgentSessionCreateRequest(
                session.Id,
                "DbConfigDiagnosisAgent",
                diagnosisAgentExecutor.PromptVersion,
                diagnosisAgentExecutor.ModelId,
                JsonSerializer.Serialize(new
                {
                    prompt,
                    report = diagnosis.ReportJson,
                    evidenceSource = evidence.Source
                }, SerializerOptions),
                JsonSerializer.Serialize(new
                {
                    workflowSessionId = session.Id,
                    connectionId = session.ConnectionId,
                    databaseName = connection.DatabaseName
                }, SerializerOptions),
                DateTimeOffset.UtcNow),
            cancellationToken);

        await agentSessionPersistenceService.AppendMessageAsync(
            new AgentMessageCreateRequest(
                agentSessionResult.AgentSessionId,
                session.Id,
                1,
                "system",
                "PromptInput",
                prompt,
                JsonSerializer.Serialize(new { prompt }, SerializerOptions),
                Activity.Current?.TraceId.ToString(),
                DateTimeOffset.UtcNow),
            cancellationToken);

        await agentSessionPersistenceService.AppendMessageAsync(
            new AgentMessageCreateRequest(
                agentSessionResult.AgentSessionId,
                session.Id,
                2,
                "assistant",
                "FinalAnswer",
                diagnosis.ReportJson,
                diagnosis.ReportJson,
                Activity.Current?.TraceId.ToString(),
                DateTimeOffset.UtcNow),
            cancellationToken);

        var rollingSummaryJson = AgentSummaryService.BuildRollingSummaryJson(
            "DbConfigOptimization",
            connection.DatabaseName,
            diagnosis.ReportJson,
            notes,
            messageCount: 2);
        var summary = await agentSummaryService.CreateRollingSummaryAsync(
            new AgentSummaryCreateRequest(
                agentSessionResult.AgentSessionId,
                "rolling",
                rollingSummaryJson,
                1,
                2,
                DateTimeOffset.UtcNow),
            cancellationToken);

        await agentSessionPersistenceService.AttachSummaryAsync(
            agentSessionResult.AgentSessionId,
            summary.SummaryId,
            2,
            DateTimeOffset.UtcNow,
            JsonSerializer.Serialize(new
            {
                prompt,
                report = diagnosis.ReportJson,
                activeSummaryId = summary.SummaryId
            }, SerializerOptions),
            JsonSerializer.Serialize(new
            {
                workflowSessionId = session.Id,
                latestReport = diagnosis.ReportJson
            }, SerializerOptions),
            cancellationToken);

        execution.AgentSessionId = agentSessionResult.AgentSessionId;
        execution.TokenUsageJson = diagnosis.TokenUsageJson;
        CompleteNodeExecution(execution, diagnosis.ReportJson);

        session.AgentSessionId = agentSessionResult.AgentSessionId;
        session.ResultPayloadJson = diagnosis.ReportJson;
        session.TotalTokens = ExtractTokenCount(diagnosis.TokenUsageJson);
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            agentSessionId = session.AgentSessionId
        });
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey,
            agentSessionId = session.AgentSessionId
        }, "Diagnosis agent completed.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        return new DiagnosisArtifacts(
            diagnosis.ReportJson,
            prompt,
            agentSessionResult.AgentSessionId,
            summary.SummaryId,
            diagnosis.TokenUsageJson);
    }

    private async Task ExecuteGroundingAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        DbConfigEvidencePack evidence,
        string reportJson,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        var execution = CreateNodeExecution(session.Id, "DbConfigGroundingExecutor", "deterministic", reportJson);
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Grounding validation started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        groundingExecutor.Validate(evidence, reportJson);

        CompleteNodeExecution(execution, """{"grounded":true}""");
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey
        });
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey
        }, "Grounding validation completed.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private async Task<WorkflowReviewTaskEntity?> ExecuteReviewGateAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        McpConnectionEntity connection,
        DbConfigWorkflowCommand command,
        DiagnosisArtifacts diagnosis,
        long nextSequence,
        RequestInfoEvent? requestInfo,
        string? lastCheckpointRef,
        CancellationToken cancellationToken)
    {
        MafReviewRequest? reviewPayload = null;
        if (requestInfo is not null &&
            (!requestInfo.Request.TryGetDataAs<MafReviewRequest>(out reviewPayload) || reviewPayload is null))
        {
            throw new InvalidOperationException("Review request payload could not be deserialized from MAF RequestInfoEvent.");
        }

        reviewPayload ??= new MafReviewRequest(
            session.Id,
            $"Database config optimization report - {connection.DisplayName}",
            diagnosis.ReportJson,
            session.RequestedBy,
            diagnosis.Prompt,
            diagnosis.AgentSessionId,
            diagnosis.SummaryId,
            diagnosis.TokenUsageJson);

        var execution = CreateNodeExecution(session.Id, "DbConfigHumanReviewGateExecutor", "gate", session.ResultPayloadJson);
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Human review gate started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        session.Status = WorkflowSessionStatus.WaitingForReview;
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey
        });

        execution.Status = WorkflowNodeExecutionStatus.WaitingForReview;
        execution.OutputPayloadJson = session.ResultPayloadJson;

        var checkpoint = await dbContext.WorkflowCheckpoints
            .Where(x => x.WorkflowSessionId == session.Id && x.CheckpointRef == lastCheckpointRef)
            .OrderByDescending(x => x.Sequence)
            .FirstOrDefaultAsync(cancellationToken)
            ?? throw new InvalidOperationException($"MAF checkpoint ref not found in control plane: {lastCheckpointRef}");
        session.EngineCheckpointRef = checkpoint.CheckpointRef;
        session.EngineStateJson = JsonSerializer.Serialize(new
        {
            runId = session.EngineRunId,
            checkpointRef = checkpoint.CheckpointRef,
            pendingReview = true
        }, SerializerOptions);

        var reviewTask = new WorkflowReviewTaskEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = session.Id,
            CheckpointId = checkpoint.Id,
            NodeExecutionId = execution.Id,
            RequestId = requestInfo?.Request.RequestId,
            EngineRunId = session.EngineRunId,
            EngineCheckpointRef = checkpoint.CheckpointRef,
            Title = string.IsNullOrWhiteSpace(reviewPayload.Title)
                ? $"Review db config optimization report for {connection.DisplayName}"
                : reviewPayload.Title,
            PayloadJson = string.IsNullOrWhiteSpace(reviewPayload.PayloadJson)
                ? session.ResultPayloadJson
                : reviewPayload.PayloadJson,
            Status = WorkflowReviewTaskStatus.Pending,
            RequestedBy = string.IsNullOrWhiteSpace(reviewPayload.RequestedBy)
                ? session.RequestedBy
                : reviewPayload.RequestedBy,
            CreatedAt = DateTimeOffset.UtcNow,
            UpdatedAt = DateTimeOffset.UtcNow
        };
        dbContext.WorkflowReviewTasks.Add(reviewTask);
        session.ActiveReviewTaskId = reviewTask.Id;

        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ReviewRequested, "review.requested", new
        {
            sessionId = session.Id,
            reviewTaskId = reviewTask.Id,
            requestId = reviewTask.RequestId
        }, "Human review requested.", execution.Id, reviewTask.Id);
        await dbContext.SaveChangesAsync(cancellationToken);
        return reviewTask;
    }

    private async Task ExecuteCompletionAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        string resultPayloadJson,
        long nextSequence,
        string completionMessage,
        CancellationToken cancellationToken)
    {
        var execution = CreateNodeExecution(session.Id, "DbConfigCompletionExecutor", "projection", resultPayloadJson);
        dbContext.WorkflowNodeExecutions.Add(execution);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorStarted, "executor.started", new
        {
            nodeName = execution.NodeKey
        }, "Completion started.", execution.Id);
        await dbContext.SaveChangesAsync(cancellationToken);

        CompleteNodeExecution(execution, resultPayloadJson);
        session.Status = WorkflowSessionStatus.Succeeded;
        session.ResultPayloadJson = resultPayloadJson;
        session.CurrentNodeKey = execution.NodeKey;
        session.UpdatedAt = DateTimeOffset.UtcNow;
        session.CompletedAt = session.UpdatedAt;
        UpdateStateJson(session, new
        {
            sessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey
        });

        var checkpoint = await CreateCheckpointAsync(dbContext, session, nextSequence, cancellationToken);
        dbContext.WorkflowCheckpoints.Add(checkpoint);
        session.EngineCheckpointRef = checkpoint.CheckpointRef;
        session.EngineStateJson = JsonSerializer.Serialize(new
        {
            runId = session.EngineRunId,
            checkpointRef = checkpoint.CheckpointRef,
            pendingReview = false
        }, SerializerOptions);

        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.ExecutorCompleted, "executor.completed", new
        {
            nodeName = execution.NodeKey
        }, "Completion finished.", execution.Id);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.CheckpointSaved, "checkpoint.saved", new
        {
            sessionId = session.Id,
            checkpointId = checkpoint.Id,
            checkpointRef = checkpoint.CheckpointRef
        }, "Checkpoint saved.", execution.Id);
        AppendEvent(dbContext, session.Id, ref nextSequence, WorkflowEventType.WorkflowCompleted, "workflow.completed", new
        {
            sessionId = session.Id
        }, completionMessage, execution.Id);

        AIDbOptimizeTelemetry.WorkflowCompleted.Add(1);
        AIDbOptimizeTelemetry.CheckpointSaved.Add(1);
        AIDbOptimizeTelemetry.CheckpointSnapshotBytes.Record(checkpoint.PayloadSizeBytes);
    }

    private async Task UpdateAgentSummaryAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        string? comment,
        CancellationToken cancellationToken)
    {
        if (!session.AgentSessionId.HasValue)
        {
            return;
        }

        var summaryJson = AgentSummaryService.BuildRollingSummaryJson(
            session.WorkflowName,
            session.Connection.DatabaseName,
            session.ResultPayloadJson,
            comment,
            3);

        var summary = await agentSummaryService.CreateRollingSummaryAsync(
            new AgentSummaryCreateRequest(
                session.AgentSessionId.Value,
                "review-adjust",
                summaryJson,
                1,
                3,
                DateTimeOffset.UtcNow),
            cancellationToken);

        await agentSessionPersistenceService.AttachSummaryAsync(
            session.AgentSessionId.Value,
            summary.SummaryId,
            3,
            DateTimeOffset.UtcNow,
            JsonSerializer.Serialize(new
            {
                report = session.ResultPayloadJson,
                activeSummaryId = summary.SummaryId
            }, SerializerOptions),
            JsonSerializer.Serialize(new
            {
                workflowSessionId = session.Id,
                latestReport = session.ResultPayloadJson,
                reviewAdjustment = comment
            }, SerializerOptions),
            cancellationToken);
    }

    private async Task<long> LinkLatestToolExecutionAsync(
        ControlPlaneDbContext dbContext,
        Guid sessionId,
        Guid nodeExecutionId,
        long nextSequence,
        CancellationToken cancellationToken)
    {
        var latestToolExecution = await dbContext.McpToolExecutions
            .Where(x => x.WorkflowSessionId == sessionId)
            .OrderByDescending(x => x.CreatedAt)
            .FirstOrDefaultAsync(cancellationToken);

        if (latestToolExecution is null)
        {
            return nextSequence;
        }

        AppendEvent(dbContext, sessionId, ref nextSequence, WorkflowEventType.ToolExecutionLinked, "tool.execution_linked", new
        {
            sessionId,
            toolExecutionId = latestToolExecution.Id,
            workflowNodeName = latestToolExecution.WorkflowNodeName
        }, "Tool execution linked to workflow node.", nodeExecutionId, mcpToolExecutionId: latestToolExecution.Id);

        return nextSequence;
    }

    private static async Task<DiagnosisArtifacts?> TryBuildPersistedDiagnosisArtifactsAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(session.ResultPayloadJson) || string.Equals(session.ResultPayloadJson, "{}", StringComparison.Ordinal))
        {
            return null;
        }

        if (!session.AgentSessionId.HasValue)
        {
            return null;
        }

        var agentSession = await dbContext.AgentSessions
            .AsNoTracking()
            .SingleOrDefaultAsync(x => x.Id == session.AgentSessionId.Value, cancellationToken);
        if (agentSession is null)
        {
            return null;
        }

        return new DiagnosisArtifacts(
            session.ResultPayloadJson,
            string.Empty,
            session.AgentSessionId.Value,
            agentSession.ActiveSummaryId ?? Guid.Empty,
            JsonSerializer.Serialize(new
            {
                promptTokens = 0,
                completionTokens = 0,
                totalTokens = session.TotalTokens
            }, SerializerOptions));
    }

    private async Task<WorkflowCheckpointEntity> CreateCheckpointAsync(
        ControlPlaneDbContext dbContext,
        WorkflowSessionEntity session,
        long sequenceHint,
        CancellationToken cancellationToken)
    {
        var nextSequence = (await dbContext.WorkflowCheckpoints
            .Where(x => x.WorkflowSessionId == session.Id)
            .Select(x => (int?)x.Sequence)
            .MaxAsync(cancellationToken) ?? 0) + 1;

        var snapshotJson = JsonSerializer.Serialize(new
        {
            workflowSessionId = session.Id,
            status = PublicStatus(session.Status),
            currentNode = session.CurrentNodeKey,
            resultType = session.ResultType,
            resultPayloadJson = session.ResultPayloadJson,
            activeReviewTaskId = session.ActiveReviewTaskId,
            engineRunId = session.EngineRunId,
            engineCheckpointRef = session.EngineCheckpointRef,
            updatedAt = session.UpdatedAt,
            completedAt = session.CompletedAt
        }, SerializerOptions);
        var encoded = WorkflowCheckpointCodec.Encode(snapshotJson);

        return new WorkflowCheckpointEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = session.Id,
            Sequence = nextSequence,
            RunId = session.EngineRunId ?? string.Empty,
            CheckpointRef = $"{session.EngineRunId ?? "run"}:{nextSequence:D4}",
            Status = PublicStatus(session.Status),
            CurrentNodeKey = session.CurrentNodeKey,
            SnapshotJson = snapshotJson,
            PayloadCompressed = encoded.PayloadCompressed,
            PayloadEncoding = encoded.PayloadEncoding,
            PayloadSha256 = encoded.PayloadSha256,
            PayloadSizeBytes = encoded.PayloadSizeBytes,
            PendingRequestsJson = session.ActiveReviewTaskId.HasValue
                ? JsonSerializer.Serialize(new[]
                {
                    new { type = "review", reviewTaskId = session.ActiveReviewTaskId }
                }, SerializerOptions)
                : "[]",
            AgentStateRefsJson = JsonSerializer.Serialize(new
            {
                agentSessionId = session.AgentSessionId
            }, SerializerOptions),
            CreatedAt = DateTimeOffset.UtcNow
        };
    }

    private static WorkflowNodeExecutionEntity CreateNodeExecution(
        Guid sessionId,
        string nodeKey,
        string nodeType,
        string inputJson)
    {
        return new WorkflowNodeExecutionEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = sessionId,
            NodeKey = nodeKey,
            NodeType = nodeType,
            Status = WorkflowNodeExecutionStatus.Running,
            InputPayloadJson = inputJson,
            CreatedAt = DateTimeOffset.UtcNow,
            StartedAt = DateTimeOffset.UtcNow
        };
    }

    private static void CompleteNodeExecution(
        WorkflowNodeExecutionEntity execution,
        string outputJson)
    {
        execution.Status = WorkflowNodeExecutionStatus.Succeeded;
        execution.OutputPayloadJson = outputJson;
        execution.CompletedAt = DateTimeOffset.UtcNow;
    }

    private static void AppendEvent(
        ControlPlaneDbContext dbContext,
        Guid sessionId,
        ref long nextSequence,
        WorkflowEventType eventType,
        string eventName,
        object payload,
        string? message,
        Guid? nodeExecutionId = null,
        Guid? reviewTaskId = null,
        Guid? mcpToolExecutionId = null)
    {
        nextSequence++;
        dbContext.WorkflowEvents.Add(new WorkflowEventEntity
        {
            Id = Guid.NewGuid(),
            WorkflowSessionId = sessionId,
            SequenceNo = nextSequence,
            NodeExecutionId = nodeExecutionId,
            ReviewTaskId = reviewTaskId,
            McpToolExecutionId = mcpToolExecutionId,
            EventType = eventType,
            EventName = eventName,
            PayloadJson = JsonSerializer.Serialize(payload, SerializerOptions),
            Message = message,
            TraceId = Activity.Current?.TraceId.ToString(),
            SpanId = Activity.Current?.SpanId.ToString(),
            OccurredAt = DateTimeOffset.UtcNow
        });
    }

    private static void UpdateStateJson(WorkflowSessionEntity session, object state)
    {
        session.StateJson = JsonSerializer.Serialize(state, SerializerOptions);
    }

    private static string PublicStatus(WorkflowSessionStatus status)
    {
        return status switch
        {
            WorkflowSessionStatus.Succeeded => "Completed",
            WorkflowSessionStatus.Recovering => "Recovering",
            _ => status.ToString()
        };
    }

    private static long ExtractTokenCount(string tokenUsageJson)
    {
        try
        {
            using var document = JsonDocument.Parse(tokenUsageJson);
            return document.RootElement.TryGetProperty("totalTokens", out var totalTokens)
                ? totalTokens.GetInt64()
                : 0;
        }
        catch
        {
            return 0;
        }
    }

    private static JsonElement ParseJson(string json)
    {
        using var document = JsonDocument.Parse(string.IsNullOrWhiteSpace(json) ? "{}" : json);
        return document.RootElement.Clone();
    }

    private static string ApplyAdjustments(
        string payloadJson,
        WorkflowReviewAdjustment adjustment,
        string? comment)
    {
        var node = JsonNode.Parse(payloadJson)?.AsObject() ?? new JsonObject();
        if (node["recommendations"] is JsonArray recommendations)
        {
            foreach (var recommendationNode in recommendations.OfType<JsonObject>())
            {
                var key = recommendationNode["key"]?.GetValue<string>();
                if (key is null)
                {
                    continue;
                }

                if (adjustment.RiskLevelOverrides.TryGetValue(key, out var riskLevel))
                {
                    recommendationNode["severity"] = riskLevel;
                }
            }
        }

        if (!string.IsNullOrWhiteSpace(comment))
        {
            var summary = node["summary"]?.GetValue<string>() ?? string.Empty;
            node["summary"] = string.IsNullOrWhiteSpace(summary)
                ? $"Review adjustment: {comment}"
                : $"{summary} Review adjustment: {comment}";
        }

        return node.ToJsonString(new JsonSerializerOptions(JsonSerializerDefaults.Web));
    }

    private static async Task<long> GetNextEventSequenceAsync(
        ControlPlaneDbContext dbContext,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        return await dbContext.WorkflowEvents
            .Where(x => x.WorkflowSessionId == sessionId)
            .Select(x => (long?)x.SequenceNo)
            .MaxAsync(cancellationToken) ?? 0;
    }

    private async Task<WorkflowSessionDetailDto> BuildDetailAsync(
        ControlPlaneDbContext dbContext,
        Guid sessionId,
        CancellationToken cancellationToken)
    {
        var session = await dbContext.WorkflowSessions
            .AsNoTracking()
            .Include(x => x.Connection)
            .SingleAsync(x => x.Id == sessionId, cancellationToken);

        WorkflowReviewReferenceDto? review = null;
        if (session.ActiveReviewTaskId.HasValue)
        {
            var reviewTask = await dbContext.WorkflowReviewTasks
                .AsNoTracking()
                .Where(x => x.Id == session.ActiveReviewTaskId.Value)
                .Select(x => new WorkflowReviewReferenceDto(x.Id.ToString(), x.Status.ToString()))
                .FirstOrDefaultAsync(cancellationToken);
            review = reviewTask;
        }

        WorkflowSummaryReferenceDto? summary = null;
        if (session.AgentSessionId.HasValue)
        {
            var agentSession = await dbContext.AgentSessions
                .AsNoTracking()
                .Where(x => x.Id == session.AgentSessionId.Value)
                .Select(x => new WorkflowSummaryReferenceDto(x.Id.ToString(), x.UpdatedAt))
                .FirstOrDefaultAsync(cancellationToken);
            summary = agentSession;
        }

        return new WorkflowSessionDetailDto(
            session.Id.ToString(),
            session.WorkflowName,
            session.EngineType,
            PublicStatus(session.Status),
            session.CurrentNodeKey,
            WorkflowProgressCalculator.GetProgressPercent(session.CurrentNodeKey, PublicStatus(session.Status)),
            new WorkflowConnectionDto(
                session.ConnectionId.ToString(),
                session.Connection.DisplayName,
                session.Connection.Engine.ToString(),
                session.Connection.DatabaseName),
            review,
            string.IsNullOrWhiteSpace(session.ResultPayloadJson) || session.ResultPayloadJson == "{}"
                ? null
                : new WorkflowResultDto(
                    session.ResultType,
                    session.ResultPayloadJson,
                    WorkflowResultParser.TryParse(session.ResultPayloadJson)),
            summary,
            session.ErrorMessage,
            $"/api/workflows/{session.Id}/events",
            session.CreatedAt,
            session.UpdatedAt,
            session.CompletedAt);
    }

    private sealed record DiagnosisArtifacts(
        string ReportJson,
        string Prompt,
        Guid AgentSessionId,
        Guid SummaryId,
        string TokenUsageJson);
}

using AIDbOptimize.Domain.DbConfigOptimization.Models;
using Microsoft.Agents.AI.Workflows;

namespace AIDbOptimize.Infrastructure.Workflows.Runtime;

public sealed record MafWorkflowStartMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command);

public sealed record MafValidatedMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command);

public sealed record MafSnapshotMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command,
    DbConfigSnapshot Snapshot);

public sealed record MafEvidenceMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command,
    DbConfigEvidencePack Evidence);

public sealed record MafDiagnosisMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command,
    DbConfigEvidencePack Evidence,
    string Prompt,
    string ReportJson,
    Guid AgentSessionId,
    Guid SummaryId,
    string TokenUsageJson);

public sealed record MafGroundedMessage(
    Guid SessionId,
    DbConfigWorkflowCommand Command,
    DbConfigEvidencePack Evidence,
    string Prompt,
    string ReportJson,
    Guid AgentSessionId,
    Guid SummaryId,
    string TokenUsageJson,
    string? LastCheckpointRef);

public sealed record MafReviewRequest(
    Guid SessionId,
    string Title,
    string PayloadJson,
    string RequestedBy,
    string Prompt,
    Guid AgentSessionId,
    Guid SummaryId,
    string TokenUsageJson);

public sealed record MafReviewResponse(
    string Action,
    string? Reviewer,
    string? Comment,
    string AdjustmentsJson);

public sealed record MafCompletionMessage(
    Guid SessionId,
    string ResultPayloadJson,
    bool Cancelled,
    string? ErrorMessage,
    string? Action,
    string? Reviewer,
    string? Comment,
    string? AdjustmentsJson);

public sealed record MafGraphRunResult(
    MafGroundedMessage? Grounded,
    MafCompletionMessage? Completion,
    RequestInfoEvent? RequestInfo,
    string? LastCheckpointRef,
    RunStatus RunStatus);

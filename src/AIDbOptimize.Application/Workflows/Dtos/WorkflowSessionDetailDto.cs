namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Workflow session detail.
/// </summary>
public sealed record WorkflowSessionDetailDto(
    string SessionId,
    string WorkflowType,
    string EngineType,
    string Status,
    string? CurrentNode,
    int ProgressPercent,
    WorkflowConnectionDto Connection,
    WorkflowReviewReferenceDto? Review,
    WorkflowResultDto? Result,
    WorkflowSummaryReferenceDto? Summary,
    string? Error,
    string StreamUrl,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

public sealed record WorkflowConnectionDto(
    string ConnectionId,
    string DisplayName,
    string Engine,
    string DatabaseName);

public sealed record WorkflowReviewReferenceDto(
    string TaskId,
    string Status);

public sealed record WorkflowSummaryReferenceDto(
    string AgentSessionId,
    DateTimeOffset UpdatedAt);

public sealed record WorkflowResultDto(
    string ResultType,
    string PayloadJson);

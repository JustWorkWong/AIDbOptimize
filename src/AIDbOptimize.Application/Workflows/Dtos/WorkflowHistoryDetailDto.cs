namespace AIDbOptimize.Application.Workflows.Dtos;

/// <summary>
/// Workflow history detail.
/// </summary>
public sealed record WorkflowHistoryDetailDto(
    string SessionId,
    string WorkflowType,
    string EngineType,
    string Status,
    string? CurrentNode,
    WorkflowConnectionDto Connection,
    WorkflowResultDto? Result,
    WorkflowSummaryReferenceDto? Summary,
    WorkflowSkillSelectionDto? SkillSelection,
    string? Error,
    IReadOnlyList<WorkflowHistoryNodeExecutionDto> NodeExecutions,
    IReadOnlyList<WorkflowHistoryToolExecutionDto> ToolExecutions,
    IReadOnlyList<WorkflowHistoryReviewDto> Reviews,
    DateTimeOffset StartedAt,
    DateTimeOffset UpdatedAt,
    DateTimeOffset? CompletedAt);

public sealed record WorkflowHistoryNodeExecutionDto(
    string ExecutionId,
    string NodeName,
    string NodeType,
    string Status,
    string InputJson,
    string OutputJson,
    string? Error,
    string? AgentSessionId,
    string TokenUsageJson,
    DateTimeOffset StartedAt,
    DateTimeOffset? CompletedAt);

public sealed record WorkflowHistoryToolExecutionDto(
    string ExecutionId,
    string ToolId,
    string ToolName,
    string Status,
    string RequestJson,
    string ResponseJson,
    string? Error,
    string? WorkflowNodeName,
    string ExecutionScope,
    DateTimeOffset CreatedAt);

public sealed record WorkflowHistoryReviewDto(
    string TaskId,
    string Status,
    string PayloadJson,
    string? Reviewer,
    string? Comment,
    string? AdjustmentsJson,
    DateTimeOffset CreatedAt,
    DateTimeOffset? ReviewedAt);

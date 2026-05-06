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
    WorkflowSkillSelectionDto? SkillSelection,
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

public sealed record WorkflowSkillSelectionDto(
    string BundleId,
    string BundleVersion,
    string InvestigationSkillId,
    string InvestigationSkillVersion,
    string DiagnosisSkillId,
    string DiagnosisSkillVersion);

public sealed record WorkflowResultDto(
    string ResultType,
    string PayloadJson,
    WorkflowStructuredResultDto? ParsedReport = null);

public sealed record WorkflowStructuredResultDto(
    string Title,
    string Summary,
    IReadOnlyList<WorkflowRecommendationDto> Recommendations,
    IReadOnlyList<WorkflowEvidenceItemDto> EvidenceItems,
    IReadOnlyList<WorkflowMissingContextItemDto> MissingContextItems,
    IReadOnlyList<WorkflowCollectionMetadataDto> CollectionMetadata,
    IReadOnlyList<string> Warnings);

public sealed record WorkflowRecommendationDto(
    string Key,
    string Suggestion,
    string Severity,
    string FindingType,
    string Confidence,
    bool RequiresMoreContext,
    string? ImpactSummary,
    IReadOnlyList<string> EvidenceReferences,
    string RecommendationClass,
    string RecommendationType,
    string? AppliesWhen,
    string? RuleId,
    string? RuleVersion);

public sealed record WorkflowEvidenceItemDto(
    string SourceType,
    string Reference,
    string Description,
    string Category,
    string? RawValue,
    string? NormalizedValue,
    string? Unit,
    string SourceScope,
    DateTimeOffset? CapturedAt,
    DateTimeOffset? ExpiresAt,
    bool IsCached,
    string? CollectionMethod);

public sealed record WorkflowMissingContextItemDto(
    string Reference,
    string Description,
    string Reason,
    string SourceScope,
    string Severity);

public sealed record WorkflowCollectionMetadataDto(
    string Name,
    string Value,
    string? Description);

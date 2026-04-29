namespace AIDbOptimize.Application.Abstractions.Agents;

/// <summary>
/// Agent 会话持久化服务。
/// </summary>
public interface IAgentSessionPersistenceService
{
    Task<AgentSessionPersistenceResult> CreateSessionAsync(
        AgentSessionCreateRequest request,
        CancellationToken cancellationToken = default);

    Task AppendMessageAsync(
        AgentMessageCreateRequest request,
        CancellationToken cancellationToken = default);

    Task AttachSummaryAsync(
        Guid agentSessionId,
        Guid summaryId,
        int messageGroupCount,
        DateTimeOffset summarizedAt,
        string serializedSessionJson,
        string sessionStateJson,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// Agent summary 生成服务。
/// </summary>
public interface IAgentSummaryService
{
    Task<AgentSummaryResult> CreateRollingSummaryAsync(
        AgentSummaryCreateRequest request,
        CancellationToken cancellationToken = default);
}

/// <summary>
/// 创建 agent session 请求。
/// </summary>
public sealed record AgentSessionCreateRequest(
    Guid WorkflowSessionId,
    string AgentRole,
    string PromptVersion,
    string ModelId,
    string SerializedSessionJson,
    string SessionStateJson,
    DateTimeOffset CreatedAt);

/// <summary>
/// 创建 agent message 请求。
/// </summary>
public sealed record AgentMessageCreateRequest(
    Guid AgentSessionId,
    Guid WorkflowSessionId,
    long SequenceNo,
    string Role,
    string MessageKind,
    string Content,
    string RawPayloadJson,
    string? TraceId,
    DateTimeOffset CreatedAt);

/// <summary>
/// 创建 rolling summary 请求。
/// </summary>
public sealed record AgentSummaryCreateRequest(
    Guid AgentSessionId,
    string SummaryType,
    string SummaryJson,
    long SourceStartSequence,
    long SourceEndSequence,
    DateTimeOffset CreatedAt);

/// <summary>
/// Agent session 创建结果。
/// </summary>
public sealed record AgentSessionPersistenceResult(
    Guid AgentSessionId);

/// <summary>
/// Agent summary 创建结果。
/// </summary>
public sealed record AgentSummaryResult(
    Guid SummaryId,
    string SummaryJson);

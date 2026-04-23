namespace AIDbOptimize.Application.Abstractions.Agents;

/// <summary>
/// 提供给 Agent 装配层的最小工具描述。
/// </summary>
public sealed record AgentToolDescriptor(
    Guid ToolId,
    string ToolName,
    bool RequiresApproval);

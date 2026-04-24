namespace AIDbOptimize.Application.Abstractions.Agents;

/// <summary>
/// Agent 工具装配抽象。
/// </summary>
public interface IAgentToolAssemblyService
{
    /// <summary>
    /// 为指定连接装配一组可直接给 Agent 使用的工具。
    /// 调用方需要对返回结果执行释放，以关闭底层 MCP 会话。
    /// </summary>
    Task<AgentToolAssemblySession> AssembleAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default);
}

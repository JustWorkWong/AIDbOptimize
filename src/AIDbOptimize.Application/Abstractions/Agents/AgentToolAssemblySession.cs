using Microsoft.Extensions.AI;

namespace AIDbOptimize.Application.Abstractions.Agents;

/// <summary>
/// Agent 工具装配结果。
/// 既提供可直接给 Agent 使用的工具集合，也负责管理底层 MCP 会话的生命周期。
/// </summary>
public sealed class AgentToolAssemblySession(
    IReadOnlyCollection<AIFunction> tools,
    IAsyncDisposable lifetime) : IAsyncDisposable
{
    /// <summary>
    /// 已装配好的 Agent 工具集合。
    /// </summary>
    public IReadOnlyCollection<AIFunction> Tools { get; } = tools;

    public ValueTask DisposeAsync()
    {
        return lifetime.DisposeAsync();
    }
}

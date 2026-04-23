namespace AIDbOptimize.Application.Abstractions.Agents;

/// <summary>
/// Agent 工具装配抽象。
/// </summary>
public interface IAgentToolAssemblyService
{
    Task<IReadOnlyCollection<AgentToolDescriptor>> AssembleAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default);
}

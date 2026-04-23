using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Mcp.Services;

/// <summary>
/// Agent 工具装配服务。
/// 当前先输出最小描述模型，后续再在 Infrastructure 中接真实的 AIFunction 包装。
/// </summary>
public sealed class AgentToolAssemblyService(IMcpToolRepository toolRepository) : IAgentToolAssemblyService
{
    /// <summary>
    /// 读取连接下已启用工具，并转换为 Agent 侧可消费的描述对象。
    /// </summary>
    public async Task<IReadOnlyCollection<AgentToolDescriptor>> AssembleAsync(
        Guid connectionId,
        CancellationToken cancellationToken = default)
    {
        var tools = await toolRepository.ListByConnectionAsync(connectionId, cancellationToken);
        return tools
            .Where(tool => tool.IsEnabled)
            .Select(tool => new AgentToolDescriptor(
                tool.Id,
                tool.ToolName,
                tool.ApprovalMode == ToolApprovalMode.ApprovalRequired))
            .ToArray();
    }
}

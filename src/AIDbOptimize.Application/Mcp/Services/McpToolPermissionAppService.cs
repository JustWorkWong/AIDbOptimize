using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Mcp.Services;

/// <summary>
/// 工具权限配置应用服务。
/// 负责更新工具的审批模式。
/// </summary>
public sealed class McpToolPermissionAppService(IMcpToolRepository toolRepository)
{
    /// <summary>
    /// 更新指定工具的审批模式。
    /// </summary>
    public Task UpdateApprovalModeAsync(Guid toolId, ToolApprovalMode approvalMode, CancellationToken cancellationToken = default)
    {
        return toolRepository.UpdateApprovalModeAsync(toolId, approvalMode, cancellationToken);
    }
}

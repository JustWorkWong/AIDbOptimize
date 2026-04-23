using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Application.Mcp.Commands;

/// <summary>
/// 更新工具审批模式命令。
/// </summary>
public sealed record UpdateToolApprovalModeCommand(Guid ToolId, ToolApprovalMode ApprovalMode);

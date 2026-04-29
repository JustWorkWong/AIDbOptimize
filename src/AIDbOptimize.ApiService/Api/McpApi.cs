using System.Text.Json;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Mcp.Services;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.ApiService.Api;

/// <summary>
/// MCP 管理 API。
/// 当前提供连接查询、连接编辑、工具发现、工具执行和审批模式更新的最小可运行接口。
/// </summary>
internal static class McpApiRouteBuilderExtensions
{
    public static IEndpointRouteBuilder MapMcpApi(this IEndpointRouteBuilder endpoints)
    {
        var group = endpoints.MapGroup("/api/mcp");

        group.MapGet("/connections", HandleGetConnectionsAsync);
        group.MapPost("/connections", HandleCreateConnectionAsync);
        group.MapPut("/connections/{connectionId:guid}", HandleUpdateConnectionAsync);
        group.MapGet("/connections/{connectionId:guid}/tools", HandleGetToolsAsync);
        group.MapGet("/connections/{connectionId:guid}/agent-tools", HandleGetAgentToolsAsync);
        group.MapPost("/connections/{connectionId:guid}/discover-tools", HandleDiscoverToolsAsync);
        group.MapPut("/tools/{toolId:guid}/approval-mode", HandleUpdateApprovalModeAsync);
        group.MapPost("/tools/{toolId:guid}/execute", HandleExecuteToolAsync);
        group.MapGet("/executions", HandleGetExecutionsAsync);

        return endpoints;
    }

    /// <summary>
    /// 查询全部连接配置。
    /// </summary>
    private static async Task<IResult> HandleGetConnectionsAsync(
        McpDiscoveryAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.GetConnectionsAsync(cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// 新增连接配置。
    /// </summary>
    private static async Task<IResult> HandleCreateConnectionAsync(
        UpsertConnectionRequest request,
        IMcpConnectionRepository repository,
        CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var record = request.ToRecord(Guid.NewGuid(), now, now);
        await repository.AddAsync(record, cancellationToken);
        return Results.Ok(new { record.Id });
    }

    /// <summary>
    /// 更新连接配置。
    /// </summary>
    private static async Task<IResult> HandleUpdateConnectionAsync(
        Guid connectionId,
        UpsertConnectionRequest request,
        IMcpConnectionRepository repository,
        CancellationToken cancellationToken)
    {
        var existing = await repository.GetByIdAsync(connectionId, cancellationToken);
        if (existing is null)
        {
            return Results.NotFound();
        }

        var updated = request.ToRecord(connectionId, existing.CreatedAt, DateTimeOffset.UtcNow);
        await repository.UpdateAsync(updated, cancellationToken);

        return Results.Ok(new { updated.Id });
    }

    /// <summary>
    /// 查询连接下的工具列表。
    /// </summary>
    private static async Task<IResult> HandleGetToolsAsync(
        Guid connectionId,
        McpDiscoveryAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.GetToolsAsync(connectionId, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// 查询连接下可供 Agent 使用的工具集合。
    /// 该接口主要用于验证真实 `AIFunction / ApprovalRequiredAIFunction` 装配是否正确。
    /// </summary>
    private static async Task<IResult> HandleGetAgentToolsAsync(
        Guid connectionId,
        IAgentToolAssemblyService assemblyService,
        CancellationToken cancellationToken)
    {
        await using var session = await assemblyService.AssembleAsync(connectionId, cancellationToken);

        var payload = session.Tools.Select(tool => new
        {
            tool.Name,
            TypeName = tool.GetType().Name,
            Description = tool.Description
        });

        return Results.Ok(payload);
    }

    /// <summary>
    /// 触发工具发现。
    /// </summary>
    private static async Task<IResult> HandleDiscoverToolsAsync(
        Guid connectionId,
        McpDiscoveryAppService appService,
        CancellationToken cancellationToken)
    {
        var result = await appService.DiscoverToolsAsync(connectionId, cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// 更新工具审批模式。
    /// </summary>
    private static async Task<IResult> HandleUpdateApprovalModeAsync(
        Guid toolId,
        UpdateApprovalModeRequest request,
        McpToolPermissionAppService appService,
        CancellationToken cancellationToken)
    {
        await appService.UpdateApprovalModeAsync(toolId, request.ApprovalMode, cancellationToken);
        return Results.Ok(new
        {
            toolId,
            request.ApprovalMode
        });
    }

    /// <summary>
    /// 执行指定工具。
    /// </summary>
    private static async Task<IResult> HandleExecuteToolAsync(
        Guid toolId,
        ExecuteToolRequest request,
        IMcpToolExecutionService executionService,
        CancellationToken cancellationToken)
    {
        using var document = JsonDocument.Parse(request.PayloadJson);
        var result = await executionService.ExecuteAsync(
            new Application.Abstractions.Mcp.McpToolExecutionRequest(
                toolId,
                document.RootElement.Clone(),
                null,
                null,
                "frontend",
                "manual",
                null),
            cancellationToken);
        return Results.Ok(result);
    }

    /// <summary>
    /// 查询最近的工具执行记录。
    /// </summary>
    private static async Task<IResult> HandleGetExecutionsAsync(
        IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
        CancellationToken cancellationToken)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var executions = await dbContext.McpToolExecutions
            .AsNoTracking()
            .OrderByDescending(x => x.CreatedAt)
            .Take(50)
            .Select(x => new
            {
                x.Id,
                x.ConnectionId,
                x.ToolId,
                x.RequestedBy,
                x.Status,
                x.ErrorMessage,
                x.CreatedAt
            })
            .ToListAsync(cancellationToken);

        return Results.Ok(executions);
    }
}

/// <summary>
/// 更新工具审批模式请求。
/// </summary>
internal sealed record UpdateApprovalModeRequest(
    ToolApprovalMode ApprovalMode);

/// <summary>
/// 连接新增 / 更新请求。
/// </summary>
internal sealed record UpsertConnectionRequest(
    string Name,
    DatabaseEngine Engine,
    string DisplayName,
    string ServerCommand,
    string ServerArgumentsJson,
    string EnvironmentJson,
    string DatabaseConnectionString,
    string DatabaseName,
    bool IsDefault)
{
    /// <summary>
    /// 将请求转换为 Application 层轻量记录。
    /// </summary>
    public McpConnectionRecord ToRecord(Guid id, DateTimeOffset createdAt, DateTimeOffset updatedAt)
    {
        return new McpConnectionRecord(
            id,
            Name,
            Engine,
            DisplayName,
            ServerCommand,
            ServerArgumentsJson,
            EnvironmentJson,
            DatabaseConnectionString,
            DatabaseName,
            IsDefault,
            McpConnectionStatus.Draft,
            LastDiscoveredAt: null,
            createdAt,
            updatedAt);
    }
}

/// <summary>
/// 执行工具请求。
/// 当前用字符串承接原始 JSON，再由后端解析。
/// </summary>
internal sealed record ExecuteToolRequest(
    string PayloadJson);

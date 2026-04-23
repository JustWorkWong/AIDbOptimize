using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Mcp.Models;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// MCP 工具配置仓储实现。
/// </summary>
public sealed class McpToolRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IMcpToolRepository
{
    /// <summary>
    /// 读取连接下的工具列表。
    /// </summary>
    public async Task<IReadOnlyList<McpToolRecord>> ListByConnectionAsync(Guid connectionId, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.McpTools
            .AsNoTracking()
            .Where(x => x.ConnectionId == connectionId)
            .OrderBy(x => x.ToolName)
            .Select(ToRecordExpression())
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 按主键读取单个工具配置。
    /// </summary>
    public async Task<McpToolRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.McpTools
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ToRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 批量新增或更新工具定义。
    /// </summary>
    public async Task UpsertManyAsync(IReadOnlyCollection<McpToolRecord> tools, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);

        foreach (var record in tools)
        {
            var entity = await dbContext.McpTools
                .FirstOrDefaultAsync(
                    x => x.ConnectionId == record.ConnectionId && x.ToolName == record.ToolName,
                    cancellationToken);

            if (entity is null)
            {
                dbContext.McpTools.Add(ToEntity(record));
                continue;
            }

            entity.DisplayName = record.DisplayName;
            entity.Description = record.Description;
            entity.InputSchemaJson = record.InputSchemaJson;
            entity.ApprovalMode = record.ApprovalMode;
            entity.IsEnabled = record.IsEnabled;
            entity.IsWriteTool = record.IsWriteTool;
            entity.UpdatedAt = record.UpdatedAt;
        }

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 更新工具审批模式。
    /// </summary>
    public async Task UpdateApprovalModeAsync(Guid id, ToolApprovalMode approvalMode, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.McpTools.FirstOrDefaultAsync(x => x.Id == id, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 工具：{id}");

        entity.ApprovalMode = approvalMode;
        entity.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private static Expression<Func<McpToolEntity, McpToolRecord>> ToRecordExpression()
    {
        return entity => new McpToolRecord(
            entity.Id,
            entity.ConnectionId,
            entity.ToolName,
            entity.DisplayName,
            entity.Description,
            entity.InputSchemaJson,
            entity.ApprovalMode,
            entity.IsEnabled,
            entity.IsWriteTool,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    private static McpToolEntity ToEntity(McpToolRecord record)
    {
        return new McpToolEntity
        {
            Id = record.Id,
            ConnectionId = record.ConnectionId,
            ToolName = record.ToolName,
            DisplayName = record.DisplayName,
            Description = record.Description ?? string.Empty,
            InputSchemaJson = record.InputSchemaJson,
            ApprovalMode = record.ApprovalMode,
            IsEnabled = record.IsEnabled,
            IsWriteTool = record.IsWriteTool,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }
}

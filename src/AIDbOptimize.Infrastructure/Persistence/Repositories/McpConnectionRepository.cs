using System.Linq.Expressions;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Models;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// MCP 连接配置仓储实现。
/// </summary>
public sealed class McpConnectionRepository(IDbContextFactory<ControlPlaneDbContext> dbContextFactory) : IMcpConnectionRepository
{
    /// <summary>
    /// 读取全部连接配置。
    /// </summary>
    public async Task<IReadOnlyList<McpConnectionRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.McpConnections
            .AsNoTracking()
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Name)
            .Select(ToRecordExpression())
            .ToListAsync(cancellationToken);
    }

    /// <summary>
    /// 按主键读取单条连接配置。
    /// </summary>
    public async Task<McpConnectionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        return await dbContext.McpConnections
            .AsNoTracking()
            .Where(x => x.Id == id)
            .Select(ToRecordExpression())
            .FirstOrDefaultAsync(cancellationToken);
    }

    /// <summary>
    /// 新增连接配置。
    /// </summary>
    public async Task AddAsync(McpConnectionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.McpConnections.Add(ToEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 更新连接配置。
    /// </summary>
    public async Task UpdateAsync(McpConnectionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.McpConnections.FirstOrDefaultAsync(x => x.Id == record.Id, cancellationToken)
            ?? throw new InvalidOperationException($"未找到 MCP 连接：{record.Id}");

        entity.Name = record.Name;
        entity.Engine = record.Engine;
        entity.DisplayName = record.DisplayName;
        entity.ServerCommand = record.ServerCommand;
        entity.ServerArgumentsJson = record.ServerArgumentsJson;
        entity.EnvironmentJson = record.EnvironmentJson;
        entity.DatabaseConnectionString = record.DatabaseConnectionString;
        entity.DatabaseName = record.DatabaseName;
        entity.IsDefault = record.IsDefault;
        entity.Status = record.Status;
        entity.LastDiscoveredAt = record.LastDiscoveredAt;
        entity.UpdatedAt = record.UpdatedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    /// <summary>
    /// 将持久化实体投影为 Application 轻量记录。
    /// </summary>
    private static Expression<Func<McpConnectionEntity, McpConnectionRecord>> ToRecordExpression()
    {
        return entity => new McpConnectionRecord(
            entity.Id,
            entity.Name,
            entity.Engine,
            entity.DisplayName,
            entity.ServerCommand,
            entity.ServerArgumentsJson,
            entity.EnvironmentJson,
            entity.DatabaseConnectionString,
            entity.DatabaseName,
            entity.IsDefault,
            entity.Status,
            entity.LastDiscoveredAt,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    /// <summary>
    /// 将 Application 记录转换为持久化实体。
    /// </summary>
    private static McpConnectionEntity ToEntity(McpConnectionRecord record)
    {
        return new McpConnectionEntity
        {
            Id = record.Id,
            Name = record.Name,
            Engine = record.Engine,
            DisplayName = record.DisplayName,
            ServerCommand = record.ServerCommand,
            ServerArgumentsJson = record.ServerArgumentsJson,
            EnvironmentJson = record.EnvironmentJson,
            DatabaseConnectionString = record.DatabaseConnectionString,
            DatabaseName = record.DatabaseName,
            IsDefault = record.IsDefault,
            Status = record.Status,
            LastDiscoveredAt = record.LastDiscoveredAt,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }
}

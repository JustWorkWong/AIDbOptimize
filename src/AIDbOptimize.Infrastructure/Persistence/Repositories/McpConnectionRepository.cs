using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Domain.Mcp.Models;
using AIDbOptimize.Infrastructure.Persistence.Entities;
using AIDbOptimize.Infrastructure.Security;
using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence.Repositories;

/// <summary>
/// MCP connection repository implementation.
/// </summary>
public sealed class McpConnectionRepository(
    IDbContextFactory<ControlPlaneDbContext> dbContextFactory,
    IConnectionSecretProtector secretProtector) : IMcpConnectionRepository
{
    public async Task<IReadOnlyList<McpConnectionRecord>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entities = await dbContext.McpConnections
            .AsNoTracking()
            .OrderByDescending(x => x.IsDefault)
            .ThenBy(x => x.Name)
            .ToListAsync(cancellationToken);

        return entities.Select(ToRecord).ToArray();
    }

    public async Task<McpConnectionRecord?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.McpConnections
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        return entity is null ? null : ToRecord(entity);
    }

    public async Task AddAsync(McpConnectionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        dbContext.McpConnections.Add(ToEntity(record));
        await dbContext.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateAsync(McpConnectionRecord record, CancellationToken cancellationToken = default)
    {
        await using var dbContext = await dbContextFactory.CreateDbContextAsync(cancellationToken);
        var entity = await dbContext.McpConnections.FirstOrDefaultAsync(x => x.Id == record.Id, cancellationToken)
            ?? throw new InvalidOperationException($"MCP connection not found: {record.Id}");

        entity.Name = record.Name;
        entity.Engine = record.Engine;
        entity.DisplayName = record.DisplayName;
        entity.ServerCommand = record.ServerCommand;
        entity.ServerArgumentsJson = secretProtector.ProtectIfNeeded(record.ServerArgumentsJson);
        entity.EnvironmentJson = secretProtector.ProtectIfNeeded(record.EnvironmentJson);
        entity.DatabaseConnectionString = secretProtector.ProtectIfNeeded(record.DatabaseConnectionString);
        entity.DatabaseName = record.DatabaseName;
        entity.IsDefault = record.IsDefault;
        entity.Status = record.Status;
        entity.LastDiscoveredAt = record.LastDiscoveredAt;
        entity.UpdatedAt = record.UpdatedAt;

        await dbContext.SaveChangesAsync(cancellationToken);
    }

    private McpConnectionRecord ToRecord(McpConnectionEntity entity)
    {
        return new McpConnectionRecord(
            entity.Id,
            entity.Name,
            entity.Engine,
            entity.DisplayName,
            entity.ServerCommand,
            secretProtector.UnprotectIfNeeded(entity.ServerArgumentsJson),
            secretProtector.UnprotectIfNeeded(entity.EnvironmentJson),
            secretProtector.UnprotectIfNeeded(entity.DatabaseConnectionString),
            entity.DatabaseName,
            entity.IsDefault,
            entity.Status,
            entity.LastDiscoveredAt,
            entity.CreatedAt,
            entity.UpdatedAt);
    }

    private McpConnectionEntity ToEntity(McpConnectionRecord record)
    {
        return new McpConnectionEntity
        {
            Id = record.Id,
            Name = record.Name,
            Engine = record.Engine,
            DisplayName = record.DisplayName,
            ServerCommand = record.ServerCommand,
            ServerArgumentsJson = secretProtector.ProtectIfNeeded(record.ServerArgumentsJson),
            EnvironmentJson = secretProtector.ProtectIfNeeded(record.EnvironmentJson),
            DatabaseConnectionString = secretProtector.ProtectIfNeeded(record.DatabaseConnectionString),
            DatabaseName = record.DatabaseName,
            IsDefault = record.IsDefault,
            Status = record.Status,
            LastDiscoveredAt = record.LastDiscoveredAt,
            CreatedAt = record.CreatedAt,
            UpdatedAt = record.UpdatedAt
        };
    }
}

using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;

namespace AIDbOptimize.Application.DataInitialization.Dtos;

/// <summary>
/// 数据初始化状态查询结果。
/// </summary>
public sealed record DataInitializationStatusDto(
    DatabaseEngine Engine,
    string DatabaseName,
    string SeedVersion,
    long TargetOrderCount,
    DataInitializationState State,
    DateTimeOffset? StartedAt,
    DateTimeOffset? CompletedAt,
    string? ErrorMessage);

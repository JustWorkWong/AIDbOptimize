namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 采集元数据项。
/// </summary>
public sealed record DbConfigCollectionMetadataItem(
    string Name,
    string Value,
    string? Description = null);

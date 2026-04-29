using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 数据库配置采集快照。
/// </summary>
public sealed record DbConfigSnapshot(
    DatabaseEngine Engine,
    string DatabaseName,
    string Source,
    IReadOnlyDictionary<string, string> CollectedValues,
    IReadOnlyList<string> Warnings);

using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Domain.DbConfigOptimization.Models;

/// <summary>
/// 配置优化结构化证据包。
/// </summary>
public sealed record DbConfigEvidencePack(
    DatabaseEngine Engine,
    string DatabaseName,
    string Source,
    IReadOnlyList<DbConfigRecommendation> Recommendations,
    IReadOnlyList<DbConfigEvidenceItem> EvidenceItems,
    IReadOnlyList<string> Warnings);

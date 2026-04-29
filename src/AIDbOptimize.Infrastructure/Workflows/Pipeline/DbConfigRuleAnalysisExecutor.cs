using AIDbOptimize.Domain.DbConfigOptimization.Models;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 数据库配置规则分析执行器。
/// </summary>
public sealed class DbConfigRuleAnalysisExecutor
{
    public DbConfigEvidencePack Analyze(DbConfigSnapshot snapshot)
    {
        var recommendations = new List<DbConfigRecommendation>();
        var evidence = snapshot.ConfigurationItems
            .Concat(snapshot.RuntimeMetricItems)
            .Concat(snapshot.HostContextItems)
            .Concat(snapshot.ObservabilityItems)
            .ToArray();

        if (snapshot.Engine == Domain.Mcp.Enums.DatabaseEngine.MySql)
        {
            recommendations.Add(new DbConfigRecommendation(
                "innodb_buffer_pool_size",
                "建议结合实例内存重新评估 innodb_buffer_pool_size。",
                "medium",
                findingType: "cache",
                confidence: snapshot.HostContextItems.Count > 0 ? "medium" : "low",
                requiresMoreContext: snapshot.HostContextItems.Count == 0,
                impactSummary: "过小的 buffer pool 可能带来更高磁盘读取压力。",
                evidenceReferences: BuildEvidenceReferences(snapshot, "innodb_buffer_pool_size", "innodb_buffer_pool_reads", "innodb_buffer_pool_read_requests"),
                recommendationClass: snapshot.HostContextItems.Count > 0 ? "tuning" : "capacity-planning",
                appliesWhen: snapshot.HostContextItems.Count > 0
                    ? "实例内存边界可见，且当前 buffer pool 相对偏小。"
                    : "当前缺少宿主资源上下文，只能先给出保守评估建议。",
                ruleId: "mysql.buffer-pool.reassessment",
                ruleVersion: "2026-04-29"));
        }
        else
        {
            recommendations.Add(new DbConfigRecommendation(
                "shared_buffers",
                "建议结合内存和缓存命中率重新评估 shared_buffers。",
                "medium",
                findingType: "cache",
                confidence: snapshot.HostContextItems.Count > 0 ? "medium" : "low",
                requiresMoreContext: snapshot.HostContextItems.Count == 0,
                impactSummary: "shared_buffers 偏保守时，可能导致缓存命中率与 I/O 表现不理想。",
                evidenceReferences: BuildEvidenceReferences(snapshot, "shared_buffers", "blks_hit", "blks_read"),
                recommendationClass: snapshot.HostContextItems.Count > 0 ? "tuning" : "capacity-planning",
                appliesWhen: snapshot.HostContextItems.Count > 0
                    ? "实例资源边界可见，且缓存相关指标支持继续评估。"
                    : "当前缺少宿主资源上下文，只能先给出保守评估建议。",
                ruleId: "postgres.shared-buffers.reassessment",
                ruleVersion: "2026-04-29"));
        }

        if (snapshot.ObservabilityItems.Count == 0)
        {
            recommendations.Add(new DbConfigRecommendation(
                "observability-gap",
                "建议先补齐慢查询或语句统计观测能力，再继续细化参数调优。",
                "warning",
                findingType: "observability-gap",
                confidence: "high",
                requiresMoreContext: false,
                impactSummary: "缺少观测能力会限制后续建议的置信度与可解释性。",
                evidenceReferences: snapshot.MissingContextItems.Select(item => item.Reference).Distinct(StringComparer.OrdinalIgnoreCase).ToArray(),
                recommendationClass: "observability",
                appliesWhen: "当前缺少关键语句统计或慢查询背景。",
                ruleId: "generic.observability-gap",
                ruleVersion: "2026-04-29"));
        }

        return new DbConfigEvidencePack(
            snapshot.Engine,
            snapshot.DatabaseName,
            snapshot.Source,
            recommendations,
            evidence,
            snapshot.Warnings,
            snapshot.ConfigurationItems,
            snapshot.RuntimeMetricItems,
            snapshot.HostContextItems,
            snapshot.ObservabilityItems,
            snapshot.MissingContextItems,
            snapshot.CollectionMetadata);
    }

    private static IReadOnlyList<string> BuildEvidenceReferences(DbConfigSnapshot snapshot, params string[] preferredKeys)
    {
        var available = snapshot.ConfigurationItems
            .Concat(snapshot.RuntimeMetricItems)
            .Concat(snapshot.HostContextItems)
            .Concat(snapshot.ObservabilityItems)
            .Select(item => item.Reference)
            .ToHashSet(StringComparer.OrdinalIgnoreCase);

        return preferredKeys.Where(available.Contains).ToArray();
    }
}

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
        var evidence = new List<DbConfigEvidenceItem>();

        foreach (var pair in snapshot.CollectedValues)
        {
            evidence.Add(new DbConfigEvidenceItem(
                "config",
                pair.Key,
                $"采集到配置项 {pair.Key}={pair.Value}"));
        }

        if (snapshot.Engine == Domain.Mcp.Enums.DatabaseEngine.MySql)
        {
            recommendations.Add(new DbConfigRecommendation(
                "innodb_buffer_pool_size",
                "建议结合实例内存重新评估 innodb_buffer_pool_size。",
                "medium"));
        }
        else
        {
            recommendations.Add(new DbConfigRecommendation(
                "shared_buffers",
                "建议结合内存和缓存命中率重新评估 shared_buffers。",
                "medium"));
        }

        return new DbConfigEvidencePack(
            snapshot.Engine,
            snapshot.DatabaseName,
            snapshot.Source,
            recommendations,
            evidence,
            snapshot.Warnings);
    }
}

using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

/// <summary>
/// 数据库配置规则分析执行器。
/// </summary>
public sealed class DbConfigRuleAnalysisExecutor
{
    private readonly IReadOnlyList<IDbConfigRule> _rules;

    public DbConfigRuleAnalysisExecutor()
        : this(CreateDefaultRules())
    {
    }

    public DbConfigRuleAnalysisExecutor(IEnumerable<IDbConfigRule> rules)
    {
        _rules = rules.Any() ? rules.ToArray() : CreateDefaultRules();
    }

    public DbConfigEvidencePack Analyze(DbConfigSnapshot snapshot)
    {
        var context = new DbConfigCollectedContext(snapshot);
        var recommendations = _rules
            .Where(rule => rule.Engine == snapshot.Engine)
            .SelectMany(rule => rule.EvaluateAsync(context).GetAwaiter().GetResult())
            .GroupBy(item => item.Key, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.OrderByDescending(item => SeverityRank(item.Severity)).First())
            .ToArray();

        var evidence = snapshot.ConfigurationItems
            .Concat(snapshot.RuntimeMetricItems)
            .Concat(snapshot.HostContextItems)
            .Concat(snapshot.ObservabilityItems)
            .ToArray();

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

    private static IReadOnlyList<IDbConfigRule> CreateDefaultRules()
    {
        return
        [
            new MySqlBufferPoolRule(),
            new MySqlConnectionsRule(),
            new MySqlThreadingRule(),
            new MySqlTempTableRule(),
            new MySqlSlowQueryRule(),
            new MySqlObservabilityGapRule(),
            new PostgreSqlSharedBuffersRule(),
            new PostgreSqlCheckpointRule(),
            new PostgreSqlPlannerCostRule(),
            new PostgreSqlTempIoRule(),
            new PostgreSqlObservabilityGapRule()
        ];
    }

    private static int SeverityRank(string severity)
    {
        return severity.ToLowerInvariant() switch
        {
            "critical" => 4,
            "high" => 3,
            "warning" => 2,
            "medium" => 1,
            _ => 0
        };
    }
}

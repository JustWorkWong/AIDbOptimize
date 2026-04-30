using AIDbOptimize.Domain.DbConfigOptimization.Models;
using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.Infrastructure.Workflows.Pipeline;

internal static class DbConfigRuleVersions
{
    public const string Current = "2026-04-30";
}

public sealed class MySqlBufferPoolRule : IDbConfigRule
{
    public string RuleId => "mysql.buffer-pool.reassessment";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetBytes("innodb_buffer_pool_size", out var poolBytes))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        var evidenceRefs = context.BuildEvidenceReferences("innodb_buffer_pool_size", "memory_limit_bytes", "memory_total_bytes", "innodb_buffer_pool_reads", "innodb_buffer_pool_read_requests");
        if (!context.TryGetBytes("memory_limit_bytes", out var memoryLimitBytes) && !context.TryGetBytes("memory_total_bytes", out memoryLimitBytes))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "innodb_buffer_pool_size",
                    "建议结合实例可用内存继续评估 innodb_buffer_pool_size，当前宿主资源上下文不足。",
                    "medium",
                    findingType: "cache",
                    confidence: "low",
                    requiresMoreContext: true,
                    impactSummary: "buffer pool 偏小时可能带来额外磁盘读取和缓存压力。",
                    evidenceReferences: evidenceRefs,
                    recommendationClass: "capacity-planning",
                    appliesWhen: "当前尚未确认实例可用内存边界。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        var ratio = memoryLimitBytes > 0 ? (decimal)poolBytes / memoryLimitBytes : 0m;
        var missRatio = context.TryGetRatio("innodb_buffer_pool_reads", "innodb_buffer_pool_read_requests") ?? 0m;
        if (ratio <= 0.20m || missRatio >= 0.01m)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "innodb_buffer_pool_size",
                    "innodb_buffer_pool_size 相对实例可用内存偏保守，建议结合业务负载和缓存命中继续上调评估。",
                    missRatio >= 0.01m ? "high" : "medium",
                    findingType: "cache",
                    confidence: "high",
                    requiresMoreContext: false,
                    impactSummary: "buffer pool 过小会增加磁盘读取，影响吞吐和延迟稳定性。",
                    evidenceReferences: evidenceRefs,
                    recommendationClass: "tuning",
                    appliesWhen: "实例可用内存明显高于当前 buffer pool，或 buffer pool miss 已经可见。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class MySqlConnectionsRule : IDbConfigRule
{
    public string RuleId => "mysql.connections.capacity";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetInt64("max_connections", out var maxConnections))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        var refs = context.BuildEvidenceReferences("max_connections", "threads_connected", "threads_running", "connections", "wait_timeout");
        if (context.TryGetInt64("threads_connected", out var connected))
        {
            var ratio = maxConnections > 0 ? (decimal)connected / maxConnections : 0m;
            if (ratio >= 0.80m)
            {
                return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
                [
                    new DbConfigRecommendation(
                        "max_connections",
                        "当前连接高水位已接近 max_connections，建议结合连接池和超时配置评估容量风险。",
                        "high",
                        findingType: "concurrency",
                        confidence: "high",
                        requiresMoreContext: false,
                        impactSummary: "接近连接上限会放大排队、拒绝连接和恢复期波动风险。",
                        evidenceReferences: refs,
                        recommendationClass: "capacity-planning",
                        appliesWhen: "Threads_connected 已长期接近连接上限。",
                        ruleId: RuleId,
                        ruleVersion: RuleVersion)
                ]);
            }

            if (maxConnections >= 500 && connected < 50)
            {
                return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
                [
                    new DbConfigRecommendation(
                        "max_connections",
                        "max_connections 当前上限偏保守，建议结合真实并发与连接池策略评估是否收敛。",
                        "medium",
                        findingType: "concurrency",
                        confidence: "medium",
                        requiresMoreContext: false,
                        impactSummary: "过高的连接上限会提高内存预留和异常并发放大风险。",
                        evidenceReferences: refs,
                        recommendationClass: "hygiene",
                        appliesWhen: "连接上限显著高于实际并发高水位。",
                        ruleId: RuleId,
                        ruleVersion: RuleVersion)
                ]);
            }
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class MySqlThreadingRule : IDbConfigRule
{
    public string RuleId => "mysql.threading.cache";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetInt64("thread_cache_size", out var cacheSize) || !context.TryGetInt64("connections", out var connections))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        if (cacheSize <= 16 && connections >= 100)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "thread_cache_size",
                    "thread_cache_size 偏小，建议结合真实连接创建频率评估是否提升线程缓存。",
                    "medium",
                    findingType: "concurrency",
                    confidence: "medium",
                    requiresMoreContext: false,
                    impactSummary: "线程缓存不足会增加连接创建与销毁开销。",
                    evidenceReferences: context.BuildEvidenceReferences("thread_cache_size", "connections"),
                    recommendationClass: "tuning",
                    appliesWhen: "连接创建量较高且线程缓存偏小。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class MySqlTempTableRule : IDbConfigRule
{
    public string RuleId => "mysql.tmp-table.spill";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        var spillRatio = context.TryGetRatio("created_tmp_disk_tables", "created_tmp_tables");
        if (spillRatio is null || spillRatio < 0.25m)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
        [
            new DbConfigRecommendation(
                "tmp_table_size",
                "磁盘临时表占比偏高，建议结合 tmp_table_size 和 max_heap_table_size 评估临时表是否过早落盘。",
                "medium",
                findingType: "temp-io",
                confidence: "medium",
                requiresMoreContext: false,
                impactSummary: "过多的磁盘临时表通常意味着排序或聚合工作集超出内存阈值。",
                evidenceReferences: context.BuildEvidenceReferences("tmp_table_size", "max_heap_table_size", "created_tmp_disk_tables", "created_tmp_tables"),
                recommendationClass: "tuning",
                appliesWhen: "临时表溢写到磁盘的比例已经可见。",
                ruleId: RuleId,
                ruleVersion: RuleVersion)
        ]);
    }
}

public sealed class MySqlSlowQueryRule : IDbConfigRule
{
    public string RuleId => "mysql.slow-query.observability";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (context.TryGetBoolean("slow_query_log", out var enabled) && !enabled)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "slow_query_log",
                    "slow_query_log 当前关闭，建议先补齐慢查询观测后再继续细化参数调优。",
                    "warning",
                    findingType: "observability-gap",
                    confidence: "high",
                    requiresMoreContext: false,
                    impactSummary: "缺少慢查询背景会削弱对连接、缓存和 I/O 调优的判断依据。",
                    evidenceReferences: context.BuildEvidenceReferences("slow_query_log", "long_query_time"),
                    recommendationClass: "observability",
                    appliesWhen: "当前没有独立的慢查询观测能力。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class PostgreSqlSharedBuffersRule : IDbConfigRule
{
    public string RuleId => "postgres.shared-buffers.reassessment";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetBytes("shared_buffers", out var sharedBuffers))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        var refs = context.BuildEvidenceReferences("shared_buffers", "memory_limit_bytes", "memory_total_bytes", "blks_hit", "blks_read");
        if (!context.TryGetBytes("memory_limit_bytes", out var memoryLimitBytes) && !context.TryGetBytes("memory_total_bytes", out memoryLimitBytes))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "shared_buffers",
                    "建议结合实例可用内存和缓存命中继续评估 shared_buffers，当前宿主资源上下文不足。",
                    "medium",
                    findingType: "cache",
                    confidence: "low",
                    requiresMoreContext: true,
                    impactSummary: "shared_buffers 偏保守时可能导致缓存命中和 I/O 行为不理想。",
                    evidenceReferences: refs,
                    recommendationClass: "capacity-planning",
                    appliesWhen: "当前尚未确认实例可用内存边界。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        var ratio = memoryLimitBytes > 0 ? (decimal)sharedBuffers / memoryLimitBytes : 0m;
        var hitRatio = context.TryGetRatio("blks_hit", "blks_read") ?? 0m;
        if (ratio <= 0.15m || hitRatio <= 10m)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "shared_buffers",
                    "shared_buffers 当前偏保守，建议结合缓存命中和实例内存继续评估调优空间。",
                    "medium",
                    findingType: "cache",
                    confidence: "medium",
                    requiresMoreContext: false,
                    impactSummary: "shared_buffers 过小时，缓存命中和磁盘读放大会更明显。",
                    evidenceReferences: refs,
                    recommendationClass: "tuning",
                    appliesWhen: "缓存命中不足或 shared_buffers 相对实例资源偏小。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class PostgreSqlCheckpointRule : IDbConfigRule
{
    public string RuleId => "postgres.checkpoint.frequency";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetInt64("checkpoints_timed", out var timed) || !context.TryGetInt64("checkpoints_req", out var requested))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        if (requested > timed)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "checkpoint_timeout",
                    "请求触发的 checkpoint 已超过定时 checkpoint，建议检查 checkpoint_timeout、checkpoint_completion_target 与 WAL 写放大压力。",
                    "medium",
                    findingType: "checkpoint",
                    confidence: "high",
                    requiresMoreContext: false,
                    impactSummary: "过于频繁的请求型 checkpoint 会放大写入抖动和恢复期压力。",
                    evidenceReferences: context.BuildEvidenceReferences("checkpoint_timeout", "checkpoint_completion_target", "checkpoints_timed", "checkpoints_req"),
                    recommendationClass: "tuning",
                    appliesWhen: "请求型 checkpoint 已经多于定时 checkpoint。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class PostgreSqlPlannerCostRule : IDbConfigRule
{
    public string RuleId => "postgres.planner-cost.review";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetDecimal("random_page_cost", out var randomPageCost) ||
            !context.TryGetDecimal("seq_page_cost", out var seqPageCost))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        if (randomPageCost >= 4m && seqPageCost <= 1m)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "random_page_cost",
                    "planner cost 仍接近传统磁盘假设，建议结合当前存储类型和实际 I/O 观测评估 random_page_cost / seq_page_cost 是否过于保守。",
                    "medium",
                    findingType: "capacity",
                    confidence: "medium",
                    requiresMoreContext: false,
                    impactSummary: "过于保守的 planner cost 可能导致 PostgreSQL 偏向不理想的执行计划。",
                    evidenceReferences: context.BuildEvidenceReferences("random_page_cost", "seq_page_cost", "track_io_timing"),
                    recommendationClass: "tuning",
                    appliesWhen: "当前存储介质已经明显快于传统磁盘假设。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class PostgreSqlTempIoRule : IDbConfigRule
{
    public string RuleId => "postgres.temp-io.work-mem";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        if (!context.TryGetInt64("temp_files", out var tempFiles))
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
        }

        if (tempFiles >= 20)
        {
            return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(
            [
                new DbConfigRecommendation(
                    "work_mem",
                    "temp_files 已经可见，建议结合查询模式和排序/哈希场景评估 work_mem 是否偏保守。",
                    "medium",
                    findingType: "temp-io",
                    confidence: "medium",
                    requiresMoreContext: false,
                    impactSummary: "临时文件增多通常意味着排序或哈希溢写到磁盘。",
                    evidenceReferences: context.BuildEvidenceReferences("work_mem", "temp_files"),
                    recommendationClass: "tuning",
                    appliesWhen: "临时文件数量持续上升且 work_mem 保守。",
                    ruleId: RuleId,
                    ruleVersion: RuleVersion)
            ]);
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>([]);
    }
}

public sealed class MySqlObservabilityGapRule : IDbConfigRule
{
    public string RuleId => "generic.observability-gap";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.MySql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        var recommendations = new List<DbConfigRecommendation>();
        if (!context.TryGetBoolean("slow_query_log", out var slowQueryEnabled) || !slowQueryEnabled)
        {
            recommendations.Add(BuildRecommendation(
                "slow_query_log",
                "建议先开启 slow_query_log 或补齐等价慢查询观测能力。",
                context.BuildEvidenceReferences("slow_query_log", "long_query_time")));
        }

        if (!context.TryGetBoolean("performance_schema_enabled", out var performanceSchemaEnabled) || !performanceSchemaEnabled)
        {
            recommendations.Add(BuildRecommendation(
                "performance_schema_enabled",
                "performance_schema 当前不可用，建议先补齐基础观测能力。",
                context.BuildEvidenceReferences("performance_schema_enabled")));
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(recommendations);
    }

    private DbConfigRecommendation BuildRecommendation(string key, string suggestion, IReadOnlyList<string> refs)
    {
        return new DbConfigRecommendation(
            key,
            suggestion,
            "warning",
            findingType: "observability-gap",
            confidence: "high",
            requiresMoreContext: false,
            impactSummary: "缺少关键观测能力会削弱规则结论和后续人工 review 的可信度。",
            evidenceReferences: refs,
            recommendationClass: "observability",
            appliesWhen: "当前缺少关键语句统计、慢查询或 I/O 观测能力。",
            ruleId: RuleId,
            ruleVersion: RuleVersion);
    }
}

public sealed class PostgreSqlObservabilityGapRule : IDbConfigRule
{
    public string RuleId => "generic.observability-gap";
    public string RuleVersion => DbConfigRuleVersions.Current;
    public DatabaseEngine Engine => DatabaseEngine.PostgreSql;

    public ValueTask<IReadOnlyList<DbConfigRecommendation>> EvaluateAsync(DbConfigCollectedContext context, CancellationToken cancellationToken = default)
    {
        var recommendations = new List<DbConfigRecommendation>();
        if (!context.TryGetBoolean("pg_stat_statements_enabled", out var statementsEnabled) || !statementsEnabled)
        {
            recommendations.Add(BuildRecommendation(
                "pg_stat_statements_enabled",
                "pg_stat_statements 当前不可用，建议先补齐语句统计观测能力。",
                context.BuildEvidenceReferences("pg_stat_statements_enabled", "shared_preload_libraries")));
        }

        if (!context.TryGetBoolean("track_io_timing", out var trackIoTiming) || !trackIoTiming)
        {
            recommendations.Add(BuildRecommendation(
                "track_io_timing",
                "track_io_timing 当前关闭，建议先补齐 I/O 观测能力。",
                context.BuildEvidenceReferences("track_io_timing")));
        }

        return ValueTask.FromResult<IReadOnlyList<DbConfigRecommendation>>(recommendations);
    }

    private DbConfigRecommendation BuildRecommendation(string key, string suggestion, IReadOnlyList<string> refs)
    {
        return new DbConfigRecommendation(
            key,
            suggestion,
            "warning",
            findingType: "observability-gap",
            confidence: "high",
            requiresMoreContext: false,
            impactSummary: "缺少关键观测能力会削弱规则结论和后续人工 review 的可信度。",
            evidenceReferences: refs,
            recommendationClass: "observability",
            appliesWhen: "当前缺少关键语句统计、慢查询或 I/O 观测能力。",
            ruleId: RuleId,
            ruleVersion: RuleVersion);
    }
}

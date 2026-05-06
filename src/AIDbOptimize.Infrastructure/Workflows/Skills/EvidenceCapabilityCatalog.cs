namespace AIDbOptimize.Infrastructure.Workflows.Skills;

public sealed class EvidenceCapabilityCatalog
{
    private readonly IReadOnlyDictionary<string, EvidenceCapabilityDefinition> _byCapabilityId;
    private readonly IReadOnlyDictionary<string, IReadOnlyList<EvidenceCapabilityDefinition>> _bySkillReference;

    public EvidenceCapabilityCatalog()
    {
        var definitions = BuildDefinitions();
        _byCapabilityId = definitions.ToDictionary(item => item.CapabilityId, StringComparer.OrdinalIgnoreCase);
        _bySkillReference = definitions
            .GroupBy(item => item.SkillReference, StringComparer.OrdinalIgnoreCase)
            .ToDictionary(
                group => group.Key,
                group => (IReadOnlyList<EvidenceCapabilityDefinition>)group.ToArray(),
                StringComparer.OrdinalIgnoreCase);
    }

    public IReadOnlyCollection<EvidenceCapabilityDefinition> ResolveCapabilities(
        string engine,
        IReadOnlyCollection<string> skillReferences)
    {
        var resolved = new List<EvidenceCapabilityDefinition>();

        foreach (var skillReference in skillReferences)
        {
            if (!_bySkillReference.TryGetValue(skillReference, out var definitions))
            {
                throw new InvalidOperationException($"No capability mapping was defined for skill reference `{skillReference}`.");
            }

            var matches = definitions
                .Where(item => string.Equals(item.Engine, engine, StringComparison.OrdinalIgnoreCase))
                .ToArray();
            if (matches.Length == 0)
            {
                throw new InvalidOperationException(
                    $"Skill reference `{skillReference}` does not have a capability mapping for engine `{engine}`.");
            }

            resolved.AddRange(matches);
        }

        return resolved
            .GroupBy(item => item.CapabilityId, StringComparer.OrdinalIgnoreCase)
            .Select(group => group.First())
            .ToArray();
    }

    public IReadOnlyCollection<EvidenceCapabilityDefinition> GetBaselineCapabilities(string engine)
    {
        return ResolveCapabilities(
            engine,
            [
                "engine.version",
                "deployment.flavor",
                "parameter.source",
                "parameter.apply_scope",
                "parameter.normalized_unit"
            ]);
    }

    public EvidenceCapabilityDefinition GetByCapabilityId(string capabilityId)
    {
        if (!_byCapabilityId.TryGetValue(capabilityId, out var definition))
        {
            throw new InvalidOperationException($"Capability `{capabilityId}` was not defined.");
        }

        return definition;
    }

    private static IReadOnlyCollection<EvidenceCapabilityDefinition> BuildDefinitions()
    {
        return
        [
            new(
                "mysql.config.global-variables",
                "mysql.global_variables",
                "mysql",
                "db-config-snapshot",
                "mysql-global-variables",
                "engine-native",
                "missing-required-evidence",
                "db",
                ["max_connections", "innodb_buffer_pool_size", "thread_cache_size", "tmp_table_size", "max_heap_table_size", "slow_query_log", "long_query_time", "performance_schema_enabled"],
                []),
            new(
                "mysql.runtime.global-status",
                "mysql.global_status",
                "mysql",
                "db-config-snapshot",
                "mysql-global-status",
                "engine-native",
                "missing-runtime-evidence",
                "db",
                ["threads_connected", "threads_running", "slow_queries", "connections", "aborted_connects", "innodb_buffer_pool_reads", "innodb_buffer_pool_read_requests", "created_tmp_disk_tables", "created_tmp_tables"],
                []),
            new(
                "mysql.metadata.version-profile",
                "mysql.version_profile",
                "mysql",
                "db-config-snapshot",
                "version-profile",
                "version-string",
                "missing-required-evidence",
                "db",
                ["engine_version", "version_comment"],
                []),
            new(
                "mysql.capacity.storage",
                "mysql.storage_capacity",
                "mysql",
                "db-config-snapshot",
                "storage-capacity",
                "bytes",
                "missing-storage-context",
                "db+host",
                ["database_size_bytes"],
                []),
            new(
                "mysql.topology.replication",
                "mysql.replication_topology",
                "mysql",
                "db-config-snapshot",
                "replication-topology",
                "state",
                "missing-optional-evidence",
                "db",
                ["replication_role"],
                []),
            new(
                "mysql.cache.buffer-pool-detail",
                "mysql.buffer_pool_detail",
                "mysql",
                "db-config-snapshot",
                "buffer-pool-detail",
                "engine-native",
                "missing-optional-evidence",
                "db",
                ["innodb_buffer_pool_reads", "innodb_buffer_pool_read_requests"],
                []),
            new(
                "mysql.workload.window",
                "mysql.workload_window",
                "mysql",
                "db-config-snapshot",
                "workload-window",
                "window",
                "missing-optional-evidence",
                "db",
                ["workload_window_label"],
                []),
            new(
                "mysql.host.resource-snapshot",
                "mysql.host_resource_snapshot",
                "mysql",
                "host-context",
                "host-resource-snapshot",
                "bytes+cores",
                "missing-host-context",
                "host",
                ["memory_limit_bytes", "cpu_limit_cores", "disk_available_bytes"],
                []),
            new(
                "postgresql.config.settings",
                "postgresql.settings",
                "postgresql",
                "db-config-snapshot",
                "postgresql-settings",
                "engine-native",
                "missing-required-evidence",
                "db",
                ["shared_buffers", "work_mem", "maintenance_work_mem", "effective_cache_size", "max_connections", "checkpoint_timeout", "checkpoint_completion_target", "random_page_cost", "seq_page_cost", "track_io_timing", "shared_preload_libraries", "pg_stat_statements_enabled"],
                []),
            new(
                "postgresql.runtime.activity-snapshot",
                "postgresql.activity_snapshot",
                "postgresql",
                "db-config-snapshot",
                "postgresql-activity-snapshot",
                "engine-native",
                "missing-runtime-evidence",
                "db",
                ["blks_hit", "blks_read", "temp_files", "deadlocks", "checkpoints_timed", "checkpoints_req"],
                []),
            new(
                "postgresql.metadata.version-profile",
                "postgresql.version_profile",
                "postgresql",
                "db-config-snapshot",
                "version-profile",
                "version-string",
                "missing-required-evidence",
                "db",
                ["engine_version"],
                []),
            new(
                "postgresql.capacity.storage",
                "postgresql.storage_capacity",
                "postgresql",
                "db-config-snapshot",
                "storage-capacity",
                "bytes",
                "missing-storage-context",
                "db+host",
                ["database_size_bytes"],
                []),
            new(
                "postgresql.topology.replication",
                "postgresql.replication_topology",
                "postgresql",
                "db-config-snapshot",
                "replication-topology",
                "state",
                "missing-optional-evidence",
                "db",
                ["replication_role"],
                []),
            new(
                "postgresql.maintenance.autovacuum-health",
                "postgresql.autovacuum_health",
                "postgresql",
                "db-config-snapshot",
                "autovacuum-health",
                "engine-native",
                "missing-optional-evidence",
                "db",
                ["autovacuum_running", "autovacuum_workers"],
                []),
            new(
                "postgresql.workload.window",
                "postgresql.workload_window",
                "postgresql",
                "db-config-snapshot",
                "workload-window",
                "window",
                "missing-optional-evidence",
                "db",
                ["workload_window_label"],
                []),
            new(
                "postgresql.host.resource-snapshot",
                "postgresql.host_resource_snapshot",
                "postgresql",
                "host-context",
                "host-resource-snapshot",
                "bytes+cores",
                "missing-host-context",
                "host",
                ["memory_limit_bytes", "cpu_limit_cores", "disk_available_bytes"],
                []),
            new(
                "baseline.engine-version.mysql",
                "engine.version",
                "mysql",
                "db-config-snapshot",
                "baseline-metadata",
                "version-string",
                "missing-baseline-metadata",
                "db",
                ["engine_version"],
                []),
            new(
                "baseline.engine-version.postgresql",
                "engine.version",
                "postgresql",
                "db-config-snapshot",
                "baseline-metadata",
                "version-string",
                "missing-baseline-metadata",
                "db",
                ["engine_version"],
                []),
            new(
                "baseline.deployment-flavor.mysql",
                "deployment.flavor",
                "mysql",
                "host-context",
                "baseline-metadata",
                "resource-scope",
                "missing-baseline-metadata",
                "host",
                [],
                ["deployment.flavor"]),
            new(
                "baseline.deployment-flavor.postgresql",
                "deployment.flavor",
                "postgresql",
                "host-context",
                "baseline-metadata",
                "resource-scope",
                "missing-baseline-metadata",
                "host",
                [],
                ["deployment.flavor"]),
            new(
                "baseline.parameter-source.mysql",
                "parameter.source",
                "mysql",
                "db-config-snapshot",
                "baseline-metadata",
                "source",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.source"]),
            new(
                "baseline.parameter-source.postgresql",
                "parameter.source",
                "postgresql",
                "db-config-snapshot",
                "baseline-metadata",
                "source",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.source"]),
            new(
                "baseline.parameter-apply-scope.mysql",
                "parameter.apply_scope",
                "mysql",
                "db-config-snapshot",
                "baseline-metadata",
                "scope",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.apply_scope"]),
            new(
                "baseline.parameter-apply-scope.postgresql",
                "parameter.apply_scope",
                "postgresql",
                "db-config-snapshot",
                "baseline-metadata",
                "scope",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.apply_scope"]),
            new(
                "baseline.parameter-normalized-unit.mysql",
                "parameter.normalized_unit",
                "mysql",
                "db-config-snapshot",
                "baseline-metadata",
                "unit-map",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.normalized_unit"]),
            new(
                "baseline.parameter-normalized-unit.postgresql",
                "parameter.normalized_unit",
                "postgresql",
                "db-config-snapshot",
                "baseline-metadata",
                "unit-map",
                "missing-baseline-metadata",
                "workflow",
                [],
                ["parameter.normalized_unit"])
        ];
    }
}

---
skill_id: mysql-investigation
skill_type: investigation
engine: mysql
skill_version: 1.0.0
schema_version: 1.0.0
bundle_id: mysql-default
bundle_version: 1.0.0
workflow_type: db-config-optimization
status: active
rag_ready: true
rag_enabled: false
---

# MySQL Investigation Skill

## Scope

- workflow: `db-config-optimization`
- responsibility: collect the minimum evidence required before diagnosis may produce actionable recommendations
- focus: server-level configuration, workload pressure indicators, storage/runtime context

## Required Evidence

- `mysql.global_variables`: effective MySQL global configuration values for the optimization target.
- `mysql.global_status`: current server status counters needed to interpret configuration pressure.
- `mysql.version_profile`: engine version, edition, and relevant feature flags that affect rule compatibility.
- `mysql.storage_capacity`: current storage size, free space, and growth pressure needed for safety checks.

## Optional Evidence

- `mysql.replication_topology`: replica role, lag, and topology hints when the node is not standalone.
- `mysql.buffer_pool_detail`: deeper InnoDB cache signals when available.
- `mysql.workload_window`: recent workload shape or peak-period context when sampling is possible.
- `mysql.host_resource_snapshot`: CPU and memory capacity context when the collector can provide it.

## Blocking Rules

1. Block when `mysql.global_variables` is missing because no configuration recommendation can be grounded.
2. Block when `mysql.version_profile` is missing because rule compatibility cannot be established.
3. Degrade rather than block when optional evidence is missing; record the missing items explicitly.
4. Never treat future external knowledge as a substitute for missing required evidence.

## Investigation Questions

1. Which MySQL settings currently constrain throughput, latency, or safety margins?
2. Are the observed pressures configuration-driven, workload-driven, or environment-driven?
3. Which conclusions are safe with current evidence, and which require more context?
4. Is the node operating under standalone, replicated, or otherwise role-sensitive conditions?

## Collection Hints

- Prefer normalized key-value snapshots over prose summaries.
- Capture effective runtime values rather than static file intent whenever the two may differ.
- Preserve evidence references exactly as listed above so planner and gate semantics stay stable.
- When host-resource context is unavailable, keep the gap explicit instead of inferring hardware limits.

## Retrieval Hints

- Reserve this section for future diagnosis-side retrieval guidance such as MySQL version-specific notes or historical tuning cases.
- Workflow v1 does not execute real retrieval from this section.
- Any future retrieval result is additive context only; it cannot fulfill required evidence and cannot alter gate classification.

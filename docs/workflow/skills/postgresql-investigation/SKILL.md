---
skill_id: postgresql-investigation
skill_type: investigation
engine: postgresql
skill_version: 1.0.0
schema_version: 1.0.0
bundle_id: postgresql-default
bundle_version: 1.0.0
workflow_type: db-config-optimization
status: active
rag_ready: true
rag_enabled: false
---

# PostgreSQL Investigation Skill

## Scope

- workflow: `db-config-optimization`
- responsibility: collect the minimum evidence required before diagnosis may produce actionable recommendations
- focus: server settings, activity pressure, storage/runtime context, role-sensitive deployment facts

## Required Evidence

- `postgresql.settings`: effective PostgreSQL configuration values for the optimization target.
- `postgresql.activity_snapshot`: active workload and wait-state signals needed to interpret pressure.
- `postgresql.version_profile`: engine version and compatibility context that affect rule applicability.
- `postgresql.storage_capacity`: current storage size, free space, and growth pressure needed for safety checks.

## Optional Evidence

- `postgresql.replication_topology`: primary/replica role, lag, and failover context when available.
- `postgresql.autovacuum_health`: maintenance pressure indicators for vacuum and bloat interpretation.
- `postgresql.workload_window`: recent workload shape or peak-period context when sampling is possible.
- `postgresql.host_resource_snapshot`: CPU and memory capacity context when the collector can provide it.

## Blocking Rules

1. Block when `postgresql.settings` is missing because configuration recommendations cannot be grounded.
2. Block when `postgresql.version_profile` is missing because rule compatibility cannot be established.
3. Degrade rather than block when optional evidence is missing; record the missing items explicitly.
4. Never treat future external knowledge as a substitute for missing required evidence.

## Investigation Questions

1. Which PostgreSQL settings currently constrain throughput, latency, maintenance health, or safety margins?
2. Are the observed pressures configuration-driven, workload-driven, or environment-driven?
3. Which conclusions are safe with current evidence, and which require more context?
4. Is the node operating under primary, replica, or otherwise role-sensitive conditions?

## Collection Hints

- Prefer normalized key-value snapshots over prose summaries.
- Capture effective runtime values rather than static file intent whenever the two may differ.
- Preserve evidence references exactly as listed above so planner and gate semantics stay stable.
- When host-resource context is unavailable, keep the gap explicit instead of inferring hardware limits.

## Retrieval Hints

- Reserve this section for future diagnosis-side retrieval guidance such as PostgreSQL version-specific notes or historical tuning cases.
- Workflow v1 does not execute real retrieval from this section.
- Any future retrieval result is additive context only; it cannot fulfill required evidence and cannot alter gate classification.

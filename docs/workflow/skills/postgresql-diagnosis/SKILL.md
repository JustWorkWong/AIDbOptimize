---
skill_id: postgresql-diagnosis
skill_type: diagnosis
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

# PostgreSQL Diagnosis Skill

## Scope

- workflow: `db-config-optimization`
- responsibility: turn collected PostgreSQL evidence into grounded recommendations
- focus: recommendation shape, confidence semantics, safe refusal behavior when context is insufficient

## Output Contract

1. Output either `actionable_recommendation` or `request_more_context`.
2. Every actionable recommendation must identify the target configuration area, the expected benefit, the risk note, and supporting `evidenceReferences`.
3. Every request for more context must name the missing evidence and explain why the gap blocks certainty.
4. Output must stay consistent with the evidence reference names produced by `postgresql-investigation`.

## Recommendation Rules

1. Prefer the smallest safe change that addresses the observed bottleneck, maintenance risk, or stability concern.
2. Tie each recommendation to concrete evidence rather than generic best-practice language.
3. Separate workload interpretation from configuration advice; do not blame settings for evidence that points to environment limits.
4. When evidence is degraded, narrow the recommendation scope or switch to `request_more_context`.

## Confidence Rules

1. High confidence requires all required evidence plus direct support from at least one runtime pressure or maintenance-health signal.
2. Medium confidence is allowed when required evidence exists but optional context that could refine sizing is missing.
3. Low confidence must not produce precise target values; convert to guarded guidance or `request_more_context`.
4. Missing required evidence always prevents a confident actionable recommendation.

## Forbidden Patterns

1. Do not invent numeric target values without direct evidence and host-capacity context.
2. Do not restate generic PostgreSQL tuning folklore as if it were workload-specific diagnosis.
3. Do not cite future external knowledge as proof that required evidence was collected.
4. Do not collapse uncertainty into deterministic language.

## Citation Rules

1. Cite only collected evidence references or explicitly marked future external knowledge references.
2. If future RAG-backed context is ever present, label it as supplemental context rather than primary evidence.
3. Workflow v1 does not implement real RAG citation sources; this section is a protocol reservation only.
4. Supplemental knowledge must not override required-evidence grounding or gate outcomes.

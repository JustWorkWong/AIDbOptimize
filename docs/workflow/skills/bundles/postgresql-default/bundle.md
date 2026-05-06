---
bundle_id: postgresql-default
bundle_version: 1.0.0
workflow_type: db-config-optimization
engine: postgresql
investigation_skill_id: postgresql-investigation
investigation_skill_version: 1.0.0
diagnosis_skill_id: postgresql-diagnosis
diagnosis_skill_version: 1.0.0
schema_version: 1.0.0
status: active
rag_ready: true
rag_enabled: false
---

# PostgreSQL Default Bundle

## Scope

- workflow: `db-config-optimization`
- engine: `postgresql`
- purpose: default skill profile for PostgreSQL database configuration optimization in workflow v1

## Skill Mapping

- Investigation skill: `postgresql-investigation@1.0.0`
- Diagnosis skill: `postgresql-diagnosis@1.0.0`
- Workflow type: `db-config-optimization`
- Engine: `postgresql`

## Compatibility Contract

1. The investigation skill defines which evidence must be collected before diagnosis may proceed.
2. The diagnosis skill may only reason over collected evidence plus explicit degraded-mode context.
3. Both skills must keep evidence reference names consistent so planner, gate, diagnosis, and grounding can trace one shared vocabulary.

## Evidence Model

- Required evidence is defined only in `postgresql-investigation`.
- Recommendation shape, confidence semantics, and citation obligations are defined only in `postgresql-diagnosis`.
- The bundle does not duplicate the full body of either skill; it only records the compatible pairing.

## RAG Reserved Boundary

- `Retrieval Hints` and `Citation Rules` are reserved protocol slots for a future knowledge injection layer.
- Workflow v1 does not implement live `Fact RAG` or `Case RAG`.
- Future RAG context may enrich diagnosis context only; it must not satisfy required evidence and must not change `pass / degraded / blocked` gate outcomes.

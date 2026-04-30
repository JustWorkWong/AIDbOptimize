<script setup lang="ts">
import { computed } from 'vue'
import type {
  WorkflowCollectionMetadata,
  WorkflowEvidenceItem,
  WorkflowHistoryDetail,
  WorkflowSessionDetail,
  WorkflowStructuredResult,
} from '../../models/workflow'
import { extractWorkflowStructuredResult } from '../../models/workflow'

const props = defineProps<{
  session: WorkflowSessionDetail | null
  historyDetail: WorkflowHistoryDetail | null
}>()

const resultPreview = computed(() => {
  return props.session?.result?.payloadJson ?? 'No result yet.'
})

const parsedReport = computed<WorkflowStructuredResult | null>(() => {
  return props.session?.result?.parsedReport ?? extractWorkflowStructuredResult(props.session?.result) ?? null
})

const hostContextItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'hostContext') ?? [])

const configurationItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'configuration').slice(0, 8) ?? [])

const runtimeItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'runtimeMetric').slice(0, 8) ?? [])

const observabilityItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'observability').slice(0, 8) ?? [])

function displayValue(item: WorkflowEvidenceItem): string {
  return item.normalizedValue || item.rawValue || 'n/a'
}

function metadataValue(name: string): WorkflowCollectionMetadata | undefined {
  return parsedReport.value?.collectionMetadata.find(item => item.name === name)
}
</script>

<template>
  <article class="panel">
    <h2>Result</h2>
    <p v-if="!session" class="state-text">
      Select a workflow session to inspect the final result and audit detail.
    </p>

    <template v-else>
      <div v-if="parsedReport" class="structured-report">
        <div class="review-summary-card">
          <strong>{{ parsedReport.title }}</strong>
          <span>{{ parsedReport.summary }}</span>
        </div>

        <div class="review-detail-grid">
          <div class="tip-card">
            <strong>Recommendations</strong>
            <span>{{ parsedReport.recommendations.length }}</span>
          </div>
          <div class="tip-card">
            <strong>Missing Context</strong>
            <span>{{ parsedReport.missingContextItems.length }}</span>
          </div>
          <div class="tip-card">
            <strong>Host Scope</strong>
            <span>{{ metadataValue('resource_scope')?.value || 'unknown' }}</span>
          </div>
        </div>

        <section v-if="parsedReport.recommendations.length" class="structured-section">
          <h3>Recommendations</h3>
          <div class="structured-list">
            <article
              v-for="item in parsedReport.recommendations"
              :key="item.key + item.ruleId"
              class="structured-card"
            >
              <div class="structured-head">
                <strong>{{ item.key }}</strong>
                <span>{{ item.severity }} · {{ item.findingType }}</span>
              </div>
              <p>{{ item.suggestion }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip">confidence: {{ item.confidence }}</span>
                <span class="meta-chip">class: {{ item.recommendationClass }}</span>
                <span class="meta-chip">rule: {{ item.ruleId || 'n/a' }}@{{ item.ruleVersion || 'n/a' }}</span>
                <span v-if="item.requiresMoreContext" class="meta-chip warning">requires more context</span>
              </div>
              <p v-if="item.appliesWhen" class="structured-note">Applies when: {{ item.appliesWhen }}</p>
              <p v-if="item.impactSummary" class="structured-note">Impact: {{ item.impactSummary }}</p>
              <p v-if="item.evidenceReferences.length" class="structured-note">
                Evidence refs: {{ item.evidenceReferences.join(', ') }}
              </p>
            </article>
          </div>
        </section>

        <section v-if="hostContextItems.length" class="structured-section">
          <h3>Host Context</h3>
          <div class="structured-list compact-grid">
            <article v-for="item in hostContextItems" :key="item.reference" class="structured-card">
              <strong>{{ item.reference }}</strong>
              <p>{{ displayValue(item) }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip">{{ item.sourceScope }}</span>
                <span class="meta-chip">{{ item.isCached ? 'cached' : 'live' }}</span>
              </div>
              <p class="structured-note">captured: {{ item.capturedAt || 'n/a' }}</p>
              <p class="structured-note">expires: {{ item.expiresAt || 'n/a' }}</p>
            </article>
          </div>
        </section>

        <section v-if="parsedReport.missingContextItems.length" class="structured-section">
          <h3>Missing Context</h3>
          <div class="structured-list compact-grid">
            <article v-for="item in parsedReport.missingContextItems" :key="item.reference" class="structured-card">
              <strong>{{ item.reference }}</strong>
              <p>{{ item.description }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip warning">{{ item.reason }}</span>
                <span class="meta-chip">{{ item.sourceScope }}</span>
              </div>
            </article>
          </div>
        </section>

        <section class="structured-section">
          <h3>Collected Evidence</h3>
          <div class="structured-columns">
            <div>
              <strong>Configuration</strong>
              <ul class="mini-list">
                <li v-for="item in configurationItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? 'cached' : 'live' }} · {{ item.capturedAt || 'n/a' }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>Runtime</strong>
              <ul class="mini-list">
                <li v-for="item in runtimeItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? 'cached' : 'live' }} · {{ item.capturedAt || 'n/a' }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>Observability</strong>
              <ul class="mini-list">
                <li v-for="item in observabilityItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? 'cached' : 'live' }} · {{ item.capturedAt || 'n/a' }}</span>
                </li>
              </ul>
            </div>
          </div>
        </section>
      </div>

      <pre class="result-box">{{ resultPreview }}</pre>

      <div v-if="historyDetail" class="review-detail-grid">
        <div class="tip-card">
          <strong>Node executions</strong>
          <span>{{ historyDetail.nodeExecutions.length }}</span>
        </div>
        <div class="tip-card">
          <strong>Tool executions</strong>
          <span>{{ historyDetail.toolExecutions.length }}</span>
        </div>
        <div class="tip-card">
          <strong>Reviews</strong>
          <span>{{ historyDetail.reviews.length }}</span>
        </div>
      </div>

      <section v-if="historyDetail?.reviews.length" class="structured-section">
        <h3>Review Decisions</h3>
        <div class="structured-list compact-grid">
          <article v-for="item in historyDetail.reviews" :key="item.taskId" class="structured-card">
            <strong>{{ item.status }}</strong>
            <p>{{ item.comment || 'No comment' }}</p>
            <div class="meta-chip-row">
              <span class="meta-chip">{{ item.reviewer || 'n/a' }}</span>
              <span class="meta-chip">{{ item.reviewedAt || item.createdAt }}</span>
            </div>
          </article>
        </div>
      </section>
    </template>
  </article>
</template>

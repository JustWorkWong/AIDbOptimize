<script setup lang="ts">
import type { ReviewTaskDetail, WorkflowSessionDetail } from '../../models/workflow'
import { extractWorkflowStructuredResult } from '../../models/workflow'

defineProps<{
  session: WorkflowSessionDetail | null
  review: ReviewTaskDetail | null
  loading: boolean
  submitting: boolean
}>()

const emit = defineEmits<{
  submit: [action: 'approve' | 'reject' | 'adjust']
}>()

const reviewer = defineModel<string>('reviewer', { default: 'frontend-reviewer' })
const comment = defineModel<string>('comment', { default: '' })
const adjustmentsJson = defineModel<string>('adjustmentsJson', { default: '' })

function parsedReviewReport(review: ReviewTaskDetail | null) {
  return review?.parsedReport ?? extractWorkflowStructuredResult(review) ?? null
}
</script>

<template>
  <article class="panel">
    <h2>Review</h2>
    <p class="section-copy">
      Review the generated result and resume the workflow with approve, reject, or adjust.
    </p>

    <p v-if="!session" class="state-text">
      Select a workflow session first.
    </p>
    <p v-else-if="loading" class="state-text">
      Loading review task...
    </p>
    <p v-else-if="!review" class="state-text">
      No active review task for the selected session.
    </p>

    <template v-if="session && review">
      <div class="review-summary-card">
        <strong>{{ review.title }}</strong>
        <span>Status: {{ review.status }}</span>
        <span>Session: {{ session.sessionId }}</span>
      </div>

      <div v-if="parsedReviewReport(review)" class="structured-report">
        <div class="review-detail-grid">
          <div class="tip-card">
            <strong>Recommendations</strong>
            <span>{{ parsedReviewReport(review)?.recommendations.length }}</span>
          </div>
          <div class="tip-card">
            <strong>Missing Context</strong>
            <span>{{ parsedReviewReport(review)?.missingContextItems.length }}</span>
          </div>
          <div class="tip-card">
            <strong>Warnings</strong>
            <span>{{ parsedReviewReport(review)?.warnings.length }}</span>
          </div>
        </div>

        <div class="structured-list">
          <article
            v-for="item in parsedReviewReport(review)?.recommendations"
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
            </div>
            <p v-if="item.appliesWhen" class="structured-note">Applies when: {{ item.appliesWhen }}</p>
            <p v-if="item.evidenceReferences.length" class="structured-note">
              Evidence refs: {{ item.evidenceReferences.join(', ') }}
            </p>
          </article>
        </div>
      </div>

      <div class="result-box">
        {{ review.payloadJson }}
      </div>

      <div
        v-if="review.status === 'Pending'"
        class="review-action-box"
      >
        <label class="form-field">
          <span>Reviewer</span>
          <input v-model.trim="reviewer" class="text-input" type="text" placeholder="frontend-reviewer">
        </label>

        <label class="form-field">
          <span>Comment</span>
          <textarea
            v-model.trim="comment"
            class="json-editor"
            placeholder="Explain the decision or describe the adjustment"
          />
        </label>

        <label class="form-field">
          <span>Adjustments JSON</span>
          <textarea
            v-model.trim="adjustmentsJson"
            class="json-editor"
            placeholder='{"riskLevelOverrides":{"max_connections":"warning"}}'
          />
        </label>

        <div class="review-action-row">
          <button
            type="button"
            :disabled="submitting || !reviewer"
            @click="emit('submit', 'approve')"
          >
            {{ submitting ? 'Submitting...' : 'Approve' }}
          </button>
          <button
            type="button"
            class="secondary"
            :disabled="submitting || !reviewer"
            @click="emit('submit', 'reject')"
          >
            Reject
          </button>
          <button
            type="button"
            class="secondary"
            :disabled="submitting || !reviewer || !comment"
            @click="emit('submit', 'adjust')"
          >
            Adjust
          </button>
        </div>
      </div>
    </template>
  </article>
</template>

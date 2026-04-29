<script setup lang="ts">
import { computed } from 'vue'
import type { WorkflowHistoryDetail, WorkflowSessionDetail } from '../../models/workflow'

const props = defineProps<{
  session: WorkflowSessionDetail | null
  historyDetail: WorkflowHistoryDetail | null
}>()

const resultPreview = computed(() => {
  return props.session?.result?.payloadJson ?? 'No result yet.'
})
</script>

<template>
  <article class="panel">
    <h2>Result</h2>
    <p v-if="!session" class="state-text">
      Select a workflow session to inspect the final result and audit detail.
    </p>

    <template v-else>
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
    </template>
  </article>
</template>

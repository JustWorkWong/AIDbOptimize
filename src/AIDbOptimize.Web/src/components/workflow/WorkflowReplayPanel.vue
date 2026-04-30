<script setup lang="ts">
import type { WorkflowReplayEvent, WorkflowSessionDetail } from '../../models/workflow'
import { extractWorkflowStructuredResult } from '../../models/workflow'

defineProps<{
  session: WorkflowSessionDetail | null
  connected: boolean
  events: WorkflowReplayEvent[]
}>()

const emit = defineEmits<{
  refresh: []
}>()
</script>

<template>
  <article class="panel">
    <div class="panel-header">
      <div>
        <h2>回放</h2>
        <p class="section-copy">
          来自 SSE 流的实时与历史工作流事件。
        </p>
      </div>
      <button type="button" class="secondary" @click="emit('refresh')">
        重新加载
      </button>
    </div>

    <p v-if="!session" class="state-text">
      请先选择一个工作流会话。
    </p>
    <p v-else class="state-text">
      SSE 状态：{{ connected ? '已连接' : '未连接' }}
    </p>
    <div v-if="extractWorkflowStructuredResult(session?.result)" class="review-summary-card compact-list">
      <strong>{{ extractWorkflowStructuredResult(session?.result)?.title }}</strong>
      <span>Recommendations: {{ extractWorkflowStructuredResult(session?.result)?.recommendations.length }}</span>
      <span>Missing context: {{ extractWorkflowStructuredResult(session?.result)?.missingContextItems.length }}</span>
    </div>
    <p v-if="session && !events.length" class="state-text">
      暂无事件。
    </p>

    <ol v-if="events.length" class="replay-list">
      <li v-for="item in events" :key="`${item.sequence}-${item.eventName}`" class="replay-item">
        <div class="replay-head">
          <strong>#{{ item.sequence }} {{ item.eventName }}</strong>
          <span>{{ item.occurredAt || 'n/a' }}</span>
        </div>
        <p>{{ item.message || item.eventType }}</p>
        <pre>{{ JSON.stringify(item.payload, null, 2) }}</pre>
      </li>
    </ol>
  </article>
</template>

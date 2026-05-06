<script setup lang="ts">
import { computed } from 'vue'
import type { WorkflowReplayEvent, WorkflowSessionDetail } from '../../models/workflow'
import {
  extractWorkflowStructuredResult,
  summarizeWorkflowReplayEvent,
} from '../../models/workflow'

const props = defineProps<{
  session: WorkflowSessionDetail | null
  connected: boolean
  events: WorkflowReplayEvent[]
}>()

const emit = defineEmits<{
  refresh: []
}>()

const parsedReport = computed(() => extractWorkflowStructuredResult(props.session?.result) ?? null)
const eventSummaries = computed(() => props.events.map(item => summarizeWorkflowReplayEvent(item)))
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
    <div v-if="parsedReport" class="review-summary-card compact-list">
      <strong>{{ parsedReport.title }}</strong>
      <span>建议数：{{ parsedReport.recommendations.length }}</span>
      <span>缺失上下文：{{ parsedReport.missingContextItems.length }}</span>
    </div>
    <p v-if="session && !events.length" class="state-text">
      暂无事件。
    </p>

    <ol v-if="eventSummaries.length" class="replay-list">
      <li v-for="item in eventSummaries" :key="`${item.sequence}-${item.eventName}`" class="replay-item">
        <div class="replay-head">
          <strong>#{{ item.sequence }} {{ item.title }}</strong>
          <span>{{ item.occurredAt }}</span>
        </div>
        <p>{{ item.description }}</p>
        <div v-if="item.chips.length" class="meta-chip-row">
          <span
            v-for="chip in item.chips"
            :key="chip"
            class="meta-chip"
          >
            {{ chip }}
          </span>
        </div>
        <p
          v-for="detail in item.detailLines"
          :key="detail"
          class="structured-note"
        >
          {{ detail }}
        </p>
        <details class="command-details">
          <summary>查看事件负载</summary>
          <pre>{{ item.rawPayloadJson }}</pre>
        </details>
      </li>
    </ol>
  </article>
</template>

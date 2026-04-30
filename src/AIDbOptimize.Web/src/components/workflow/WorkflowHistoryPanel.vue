<script setup lang="ts">
import type { WorkflowSessionSummary } from '../../models/workflow'

defineProps<{
  items: WorkflowSessionSummary[]
  loading: boolean
  message: string
  selectedSessionId: string
}>()

const emit = defineEmits<{
  refresh: []
  select: [sessionId: string]
}>()
</script>

<template>
  <article class="panel">
    <div class="panel-header">
      <div>
        <h2>历史</h2>
        <p class="section-copy">
          浏览工作流会话并切换当前工作区上下文。
        </p>
      </div>
      <button type="button" class="secondary" @click="emit('refresh')">
        刷新
      </button>
    </div>

    <p v-if="loading" class="state-text">正在加载工作流会话……</p>
    <p v-else-if="message" class="state-text">{{ message }}</p>
    <p v-if="!loading && !items.length" class="state-text">
      当前还没有工作流会话。
    </p>

    <div v-if="items.length" class="workflow-list">
      <button
        v-for="item in items"
        :key="item.sessionId"
        type="button"
        class="workflow-list-item"
        :class="{ active: item.sessionId === selectedSessionId }"
        @click="emit('select', item.sessionId)"
      >
        <strong>{{ item.connection.displayName }}</strong>
        <span>{{ item.status }} · {{ item.currentNode || '无' }}</span>
        <em>{{ item.connection.engine }} · {{ item.connection.databaseName }} · {{ item.updatedAt }}</em>
      </button>
    </div>
  </article>
</template>

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
        <h2>History</h2>
        <p class="section-copy">
          Browse workflow sessions and switch the active workspace context.
        </p>
      </div>
      <button type="button" class="secondary" @click="emit('refresh')">
        Refresh
      </button>
    </div>

    <p v-if="loading" class="state-text">Loading workflow sessions...</p>
    <p v-else-if="message" class="state-text">{{ message }}</p>
    <p v-if="!loading && !items.length" class="state-text">
      No workflow sessions yet.
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
        <span>{{ item.status }} · {{ item.currentNode || 'n/a' }}</span>
        <em>{{ item.connection.engine }} · {{ item.connection.databaseName }} · {{ item.updatedAt }}</em>
      </button>
    </div>
  </article>
</template>

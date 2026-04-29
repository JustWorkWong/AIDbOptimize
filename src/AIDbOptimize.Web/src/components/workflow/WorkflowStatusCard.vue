<script setup lang="ts">
import type { WorkflowSessionDetail } from '../../models/workflow'

defineProps<{
  session: WorkflowSessionDetail | null
}>()
</script>

<template>
  <article class="panel">
    <h2>Status</h2>
    <p v-if="!session" class="state-text">
      Select a workflow session to inspect the latest status.
    </p>

    <div v-else class="review-detail-grid">
      <div class="tip-card">
        <strong>Status</strong>
        <span>{{ session.status }}</span>
      </div>
      <div class="tip-card">
        <strong>Current node</strong>
        <span>{{ session.currentNode || 'n/a' }}</span>
      </div>
      <div class="tip-card">
        <strong>Progress</strong>
        <span>{{ session.progressPercent }}%</span>
      </div>
      <div class="tip-card">
        <strong>Connection</strong>
        <span>{{ session.connection.displayName }}</span>
      </div>
      <div class="tip-card">
        <strong>Engine</strong>
        <span>{{ session.connection.engine }}</span>
      </div>
      <div class="tip-card">
        <strong>Database</strong>
        <span>{{ session.connection.databaseName }}</span>
      </div>
    </div>

    <p v-if="session?.error" class="state-text error">
      {{ session.error }}
    </p>
  </article>
</template>

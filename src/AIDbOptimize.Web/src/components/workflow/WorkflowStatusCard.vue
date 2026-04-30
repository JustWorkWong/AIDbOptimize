<script setup lang="ts">
import type { WorkflowSessionDetail } from '../../models/workflow'

defineProps<{
  session: WorkflowSessionDetail | null
}>()
</script>

<template>
  <article class="panel">
    <h2>状态</h2>
    <p v-if="!session" class="state-text">
      请选择一个工作流会话以查看最新状态。
    </p>

    <div v-else class="review-detail-grid">
      <div class="tip-card">
        <strong>状态</strong>
        <span>{{ session.status }}</span>
      </div>
      <div class="tip-card">
        <strong>当前节点</strong>
        <span>{{ session.currentNode || 'n/a' }}</span>
      </div>
      <div class="tip-card">
        <strong>进度</strong>
        <span>{{ session.progressPercent }}%</span>
      </div>
      <div class="tip-card">
        <strong>连接</strong>
        <span>{{ session.connection.displayName }}</span>
      </div>
      <div class="tip-card">
        <strong>引擎</strong>
        <span>{{ session.connection.engine }}</span>
      </div>
      <div class="tip-card">
        <strong>数据库</strong>
        <span>{{ session.connection.databaseName }}</span>
      </div>
    </div>

    <p v-if="session?.error" class="state-text error">
      {{ session.error }}
    </p>
  </article>
</template>

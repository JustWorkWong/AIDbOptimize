<script setup lang="ts">
import type { DataInitializationStatus } from '../../models/mcp'

defineProps<{
  statuses: DataInitializationStatus[]
  loading: boolean
}>()

function resolveEngineName(engine: number): string {
  return engine === 1 ? 'PostgreSQL' : 'MySQL'
}

function resolveStateName(state: number): string {
  if (state === 2) {
    return '进行中'
  }

  if (state === 3) {
    return '已完成'
  }

  if (state === 4) {
    return '失败'
  }

  return '未开始'
}
</script>

<template>
  <article class="panel">
    <h2>初始化状态</h2>
    <p class="state-text" v-if="loading">正在读取初始化状态...</p>
    <div v-else class="status-grid">
      <div v-for="status in statuses" :key="status.engine" class="tip-card">
        <strong>{{ resolveEngineName(status.engine) }}</strong>
        <span>数据库：{{ status.databaseName }}</span>
        <span>状态：{{ resolveStateName(status.state) }}</span>
        <span>目标订单数：{{ status.targetOrderCount.toLocaleString() }}</span>
        <span>版本：{{ status.seedVersion }}</span>
      </div>
    </div>
  </article>
</template>

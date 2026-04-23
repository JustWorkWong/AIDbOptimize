<script setup lang="ts">
import type { McpConnection } from '../../models/mcp'

defineProps<{
  connections: McpConnection[]
  loading: boolean
  currentConnectionId: string
}>()

const emit = defineEmits<{
  select: [connectionId: string]
  discover: [connectionId: string]
}>()

function resolveEngineName(engine: number): string {
  return engine === 1 ? 'PostgreSQL' : 'MySQL'
}

function resolveStatusName(status: number): string {
  if (status === 2) {
    return 'Ready'
  }

  if (status === 3) {
    return 'Unavailable'
  }

  return 'Draft'
}
</script>

<template>
  <article class="panel">
    <h2>MCP 连接</h2>
    <p class="state-text" v-if="loading">正在加载默认 MCP 连接...</p>
    <div v-else class="table-card">
      <table class="data-table">
        <thead>
          <tr>
            <th>展示名</th>
            <th>引擎</th>
            <th>数据库</th>
            <th>状态</th>
            <th>操作</th>
          </tr>
        </thead>
        <tbody>
          <tr
            v-for="connection in connections"
            :key="connection.id"
            :class="{ selected: connection.id === currentConnectionId }"
          >
            <td>
              <strong>{{ connection.displayName }}</strong>
              <span class="cell-sub">{{ connection.name }}</span>
            </td>
            <td>{{ resolveEngineName(connection.engine) }}</td>
            <td>{{ connection.databaseName }}</td>
            <td>{{ resolveStatusName(connection.status) }}</td>
            <td class="cell-actions">
              <button type="button" @click="emit('select', connection.id)">查看工具</button>
              <button type="button" class="secondary" @click="emit('discover', connection.id)">获取工具</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </article>
</template>

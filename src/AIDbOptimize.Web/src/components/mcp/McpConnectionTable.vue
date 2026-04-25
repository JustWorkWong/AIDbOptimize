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

function formatDiscoveredAt(value: string | null): string {
  if (!value) {
    return '未发现'
  }

  return new Date(value).toLocaleString('zh-CN', {
    hour12: false,
  })
}
</script>

<template>
  <article class="panel">
    <h2>MCP 连接</h2>
    <p class="state-text" v-if="loading">正在加载 MCP 连接...</p>
    <div v-else class="table-card">
      <table class="data-table connection-table">
        <thead>
          <tr>
            <th>显示名称</th>
            <th>引擎</th>
            <th>数据库</th>
            <th class="command-column">真实连接命令</th>
            <th>最近发现</th>
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
            <td class="command-cell">
              <code class="command-line">{{ connection.commandLine }}</code>
              <details v-if="connection.environmentEntries.length" class="command-details">
                <summary>环境变量 {{ connection.environmentEntries.length }} 项</summary>
                <div class="command-env-list">
                  <code
                    v-for="entry in connection.environmentEntries"
                    :key="entry"
                    class="command-env-item"
                  >
                    {{ entry }}
                  </code>
                </div>
              </details>
              <span v-else class="cell-sub">无需额外环境变量</span>
            </td>
            <td>{{ formatDiscoveredAt(connection.lastDiscoveredAt) }}</td>
            <td>{{ resolveStatusName(connection.status) }}</td>
            <td class="cell-actions">
              <button type="button" @click="emit('select', connection.id)">查询工具</button>
              <button type="button" class="secondary" @click="emit('discover', connection.id)">获取工具</button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </article>
</template>

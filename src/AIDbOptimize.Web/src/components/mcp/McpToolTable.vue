<script setup lang="ts">
import type { McpTool } from '../../models/mcp'

defineProps<{
  tools: McpTool[]
  loading: boolean
}>()

const emit = defineEmits<{
  changeApproval: [toolId: string, approvalMode: number]
}>()

function resolveApprovalName(value: number): string {
  return value === 2 ? '需要审核' : '不需要审核'
}
</script>

<template>
  <article class="panel">
    <h2>工具列表</h2>
    <p class="state-text" v-if="loading">正在加载工具列表...</p>
    <div v-else class="table-card">
      <table class="data-table">
        <thead>
          <tr>
            <th>工具名</th>
            <th>类型</th>
            <th>审批模式</th>
            <th>描述</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="tool in tools" :key="tool.id">
            <td>
              <strong>{{ tool.displayName }}</strong>
              <span class="cell-sub">{{ tool.toolName }}</span>
            </td>
            <td>{{ tool.isWriteTool ? '写工具' : '读工具' }}</td>
            <td>
              <select
                class="inline-select"
                :value="tool.approvalMode"
                @change="emit('changeApproval', tool.id, Number(($event.target as HTMLSelectElement).value))"
              >
                <option :value="1">不需要审核</option>
                <option :value="2">需要审核</option>
              </select>
              <span class="cell-sub">{{ resolveApprovalName(tool.approvalMode) }}</span>
            </td>
            <td>{{ tool.description ?? '暂无描述' }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </article>
</template>

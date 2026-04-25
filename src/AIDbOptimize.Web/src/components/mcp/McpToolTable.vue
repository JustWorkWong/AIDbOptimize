<script setup lang="ts">
import { computed, reactive, watch } from 'vue'
import type { McpTool } from '../../models/mcp'

const props = defineProps<{
  tools: McpTool[]
  loading: boolean
  savingToolId: string
}>()

const emit = defineEmits<{
  saveApproval: [toolId: string, approvalMode: number]
}>()

const draftApprovalModes = reactive<Record<string, number>>({})

watch(
  () => props.tools,
  (tools) => {
    const activeIds = new Set(tools.map((tool) => tool.id))

    for (const toolId of Object.keys(draftApprovalModes)) {
      if (!activeIds.has(toolId)) {
        delete draftApprovalModes[toolId]
      }
    }

    for (const tool of tools) {
      const draftValue = draftApprovalModes[tool.id]
      const isSavingCurrentTool = props.savingToolId === tool.id

      if (draftValue === undefined || draftValue === tool.approvalMode || isSavingCurrentTool) {
        draftApprovalModes[tool.id] = tool.approvalMode
      }
    }
  },
  { immediate: true },
)

const hasTools = computed(() => props.tools.length > 0)

function resolveApprovalName(value: number): string {
  return value === 2 ? '需要审批' : '无需审批'
}

function resolveToolKind(tool: McpTool): string {
  return tool.isWriteTool ? '写工具' : '读工具'
}

function hasPendingChange(tool: McpTool): boolean {
  return draftApprovalModes[tool.id] !== tool.approvalMode
}
</script>

<template>
  <article class="panel">
    <h2>工具列表</h2>
    <p class="state-text" v-if="loading">正在加载工具列表...</p>
    <p class="state-text" v-else-if="!hasTools">当前连接还没有工具，请先点击“获取工具”。</p>
    <div v-else class="table-card">
      <table class="data-table">
        <thead>
          <tr>
            <th>工具名称</th>
            <th>类型</th>
            <th>审批模式</th>
            <th>描述</th>
            <th>保存</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="tool in tools" :key="tool.id">
            <td>
              <strong>{{ tool.displayName }}</strong>
              <span class="cell-sub">{{ tool.toolName }}</span>
            </td>
            <td>{{ resolveToolKind(tool) }}</td>
            <td>
              <select
                class="inline-select"
                :value="draftApprovalModes[tool.id]"
                @change="draftApprovalModes[tool.id] = Number(($event.target as HTMLSelectElement).value)"
              >
                <option :value="1">无需审批</option>
                <option :value="2">需要审批</option>
              </select>
              <span class="cell-sub">{{ resolveApprovalName(draftApprovalModes[tool.id]) }}</span>
            </td>
            <td>{{ tool.description ?? '暂无描述' }}</td>
            <td class="cell-actions">
              <button
                type="button"
                :disabled="!hasPendingChange(tool) || savingToolId === tool.id"
                @click="emit('saveApproval', tool.id, draftApprovalModes[tool.id])"
              >
                {{ savingToolId === tool.id ? '保存中...' : '保存' }}
              </button>
            </td>
          </tr>
        </tbody>
      </table>
    </div>
  </article>
</template>

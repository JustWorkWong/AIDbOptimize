<script setup lang="ts">
import { computed, ref } from 'vue'
import { executeTool } from '../../api/mcp'
import type { McpTool } from '../../models/mcp'

const props = defineProps<{
  tools: McpTool[]
}>()

const selectedToolId = ref('')
const payloadJson = ref('{\n  "sql": "SELECT 1"\n}')
const executing = ref(false)
const executionResult = ref('')
const executionError = ref('')

const selectedTool = computed(() => {
  return props.tools.find((tool) => tool.id === selectedToolId.value) ?? null
})

async function handleExecute(): Promise<void> {
  if (!selectedToolId.value) {
    executionError.value = '请先选择一个工具。'
    return
  }

  executing.value = true
  executionError.value = ''

  try {
    const result = await executeTool(selectedToolId.value, payloadJson.value)
    executionResult.value = result.responsePayloadJson
    executionError.value = result.errorMessage ?? ''
  } catch (error) {
    executionError.value = error instanceof Error ? error.message : '执行失败'
  } finally {
    executing.value = false
  }
}
</script>

<template>
  <article class="panel">
    <h2>通用工具执行器</h2>
    <p class="state-text">
      当前版本优先支持一组只读数据库工具。
      你可以选择工具并输入 JSON 参数，快速验证执行链路是否打通。
    </p>

    <div class="executor-stack">
      <label class="form-field">
        <span>选择工具</span>
        <select v-model="selectedToolId" class="inline-select">
          <option value="">请选择</option>
          <option v-for="tool in tools" :key="tool.id" :value="tool.id">
            {{ tool.displayName }}
          </option>
        </select>
      </label>

      <label class="form-field">
        <span>参数 JSON</span>
        <textarea v-model="payloadJson" class="json-editor" spellcheck="false" />
      </label>

      <div class="executor-actions">
        <button type="button" :disabled="executing" @click="handleExecute">
          {{ executing ? '执行中...' : '执行工具' }}
        </button>
        <span class="cell-sub" v-if="selectedTool">
          当前工具：{{ selectedTool.toolName }}
        </span>
      </div>

      <p v-if="executionError" class="state-text error">{{ executionError }}</p>
      <pre v-if="executionResult" class="result-box">{{ executionResult }}</pre>
    </div>
  </article>
</template>

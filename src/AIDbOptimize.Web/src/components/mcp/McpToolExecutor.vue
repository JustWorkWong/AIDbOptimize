<script setup lang="ts">
import { computed, ref, watch } from 'vue'
import { executeTool } from '../../api/mcp'
import type { McpConnection, McpTool } from '../../models/mcp'

interface JsonSchema {
  type?: string | string[]
  properties?: Record<string, JsonSchema>
  required?: string[]
  default?: unknown
  enum?: unknown[]
  const?: unknown
  anyOf?: JsonSchema[]
  oneOf?: JsonSchema[]
  allOf?: JsonSchema[]
  items?: JsonSchema
  example?: unknown
  examples?: unknown[]
}

const props = defineProps<{
  tools: McpTool[]
  connection: McpConnection | null
}>()

const selectedToolId = ref('')
const payloadJson = ref('{}')
const executing = ref(false)
const executionResult = ref('')
const executionError = ref('')

const selectedTool = computed(() => {
  return props.tools.find((tool) => tool.id === selectedToolId.value) ?? null
})

watch(
  () => props.tools,
  (tools) => {
    if (tools.length === 0) {
      selectedToolId.value = ''
      payloadJson.value = '{}'
      return
    }

    const currentToolStillExists = tools.some((tool) => tool.id === selectedToolId.value)
    if (!currentToolStillExists) {
      selectedToolId.value = tools[0].id
      return
    }
  },
  { immediate: true },
)

watch(selectedToolId, () => {
  applyDefaultPayload()
  executionResult.value = ''
  executionError.value = ''
})

function applyDefaultPayload(): void {
  payloadJson.value = selectedTool.value
    ? JSON.stringify(buildDefaultPayload(selectedTool.value), null, 2)
    : '{}'
}

function buildDefaultPayload(tool: McpTool): Record<string, unknown> {
  const schema = parseSchema(tool.inputSchemaJson)
  if (!schema) {
    return buildFallbackPayload(tool)
  }

  const normalizedSchema = unwrapSchema(schema)
  const properties = normalizedSchema.properties ?? {}
  const propertyNames = Object.keys(properties)
  const requiredNames = new Set(normalizedSchema.required ?? [])
  const namesToInclude = propertyNames.filter((name) => {
    return requiredNames.has(name) || shouldIncludeOptionalProperty(name, propertyNames.length)
  })

  if (namesToInclude.length === 0) {
    return buildFallbackPayload(tool)
  }

  return namesToInclude.reduce<Record<string, unknown>>((result, propertyName) => {
    result[propertyName] = buildDefaultValue(propertyName, properties[propertyName])
    return result
  }, {})
}

function buildFallbackPayload(tool: McpTool): Record<string, unknown> {
  if (tool.toolName.toLowerCase().includes('query')) {
    return { sql: 'SELECT 1 AS value;' }
  }

  return {}
}

function parseSchema(input: string): JsonSchema | null {
  try {
    return JSON.parse(input) as JsonSchema
  } catch {
    return null
  }
}

function unwrapSchema(schema: JsonSchema): JsonSchema {
  if (schema.allOf?.length) {
    return schema.allOf.map(unwrapSchema).reduce<JsonSchema>((merged, current) => {
      return {
        ...merged,
        ...current,
        properties: {
          ...(merged.properties ?? {}),
          ...(current.properties ?? {}),
        },
        required: [...new Set([...(merged.required ?? []), ...(current.required ?? [])])],
      }
    }, {})
  }

  if (schema.anyOf?.length) {
    return unwrapSchema(schema.anyOf[0])
  }

  if (schema.oneOf?.length) {
    return unwrapSchema(schema.oneOf[0])
  }

  return schema
}

function shouldIncludeOptionalProperty(propertyName: string, propertyCount: number): boolean {
  if (propertyCount === 1) {
    return true
  }

  return /(sql|query|statement|schema|database|limit|size|count)$/i.test(propertyName)
}

function buildDefaultValue(propertyName: string, schema: JsonSchema | undefined): unknown {
  const normalizedSchema = schema ? unwrapSchema(schema) : undefined

  if (!normalizedSchema) {
    return buildStringHint(propertyName)
  }

  if (normalizedSchema.const !== undefined) {
    return normalizedSchema.const
  }

  if (normalizedSchema.default !== undefined) {
    return normalizedSchema.default
  }

  if (normalizedSchema.example !== undefined) {
    return normalizedSchema.example
  }

  if (normalizedSchema.examples?.length) {
    return normalizedSchema.examples[0]
  }

  if (normalizedSchema.enum?.length) {
    return normalizedSchema.enum[0]
  }

  const schemaType = normalizeType(normalizedSchema.type)

  if (schemaType === 'object') {
    return Object.entries(normalizedSchema.properties ?? {}).reduce<Record<string, unknown>>((result, [name, childSchema]) => {
      result[name] = buildDefaultValue(name, childSchema)
      return result
    }, {})
  }

  if (schemaType === 'array') {
    return []
  }

  if (schemaType === 'integer' || schemaType === 'number') {
    return buildNumericHint(propertyName)
  }

  if (schemaType === 'boolean') {
    return false
  }

  return buildStringHint(propertyName)
}

function normalizeType(value: JsonSchema['type']): string {
  if (Array.isArray(value)) {
    return value.find((item) => item !== 'null') ?? 'string'
  }

  return value ?? 'string'
}

function buildNumericHint(propertyName: string): number {
  return /(limit|size|count|top|pageSize)$/i.test(propertyName) ? 20 : 1
}

function buildStringHint(propertyName: string): string {
  if (/(sql|query|statement)$/i.test(propertyName)) {
    return 'SELECT 1 AS value;'
  }

  if (/(schema|schemaName)$/i.test(propertyName)) {
    return 'public'
  }

  if (/(database|databaseName|db)$/i.test(propertyName)) {
    return props.connection?.databaseName ?? ''
  }

  if (/(table|tableName|objectName|name)$/i.test(propertyName)) {
    return 'orders'
  }

  return ''
}

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
      默认参数会根据工具输入 schema 自动生成。常见的 `sql`、`schema`、`database`、`limit`
      字段会填入可直接尝试执行的值，推断不出的字段会保留为空字符串，便于你按实际场景补齐。
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
        <button type="button" class="secondary" :disabled="!selectedTool" @click="applyDefaultPayload">
          重置默认参数
        </button>
        <button type="button" :disabled="executing || !selectedTool" @click="handleExecute">
          {{ executing ? '执行中...' : '执行工具' }}
        </button>
        <span class="cell-sub" v-if="selectedTool">
          当前工具：{{ selectedTool.toolName }}
        </span>
      </div>

      <details v-if="selectedTool" class="schema-box">
        <summary>查看输入 schema</summary>
        <pre class="result-box">{{ selectedTool.inputSchemaJson }}</pre>
      </details>

      <p v-if="executionError" class="state-text error">{{ executionError }}</p>
      <pre v-if="executionResult" class="result-box">{{ executionResult }}</pre>
    </div>
  </article>
</template>

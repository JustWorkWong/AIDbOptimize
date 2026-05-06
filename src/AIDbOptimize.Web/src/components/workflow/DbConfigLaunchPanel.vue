<script setup lang="ts">
import { computed } from 'vue'
import type { McpConnection } from '../../models/mcp'
import type { DbConfigWorkflowRequest } from '../../models/workflow'

const props = defineProps<{
  connections: McpConnection[]
  form: DbConfigWorkflowRequest
  submitting: boolean
  message: string
}>()

const emit = defineEmits<{
  submit: []
}>()

const canSubmit = computed(() => {
  return Boolean(props.form.connectionId.trim())
})
</script>

<template>
  <article class="panel">
    <div class="panel-header">
      <div>
        <h2>启动</h2>
        <p class="section-copy">
          选择一个现有 MCP 连接并启动数据库配置优化工作流。
        </p>
      </div>
      <button
        type="button"
        :disabled="submitting || !canSubmit"
        @click="emit('submit')"
      >
        {{ submitting ? '启动中…' : '启动工作流' }}
      </button>
    </div>

    <div class="form-grid">
      <label class="form-field">
        <span>目标连接</span>
        <select v-model="form.connectionId" class="inline-select">
          <option value="">请选择连接</option>
          <option
            v-for="connection in connections"
            :key="connection.id"
            :value="connection.id"
          >
            {{ connection.displayName }} / {{ connection.engine }} / {{ connection.databaseName }}
          </option>
        </select>
      </label>

      <label class="form-field">
        <span>发起人</span>
        <input
          v-model.trim="form.requestedBy"
          class="text-input"
          type="text"
          placeholder="frontend"
        >
      </label>

      <label class="form-field">
        <span>Bundle ID</span>
        <input
          v-model.trim="form.bundleId"
          class="text-input"
          type="text"
          placeholder="留空时按引擎走默认 bundle"
        >
      </label>

      <label class="form-field">
        <span>Bundle Version</span>
        <input
          v-model.trim="form.bundleVersion"
          class="text-input"
          type="text"
          placeholder="例如 1.0.0"
        >
      </label>

      <label class="form-field form-field-wide">
        <span>备注</span>
        <textarea
          v-model.trim="form.notes"
          class="json-editor"
          placeholder="填写上下文、目标、限制条件或已知风险"
        />
      </label>
    </div>

    <div class="tips-grid">
      <label class="tip-card">
        <strong>允许降级快照</strong>
        <span>
          <input v-model="form.options.allowFallbackSnapshot" type="checkbox">
          当没有可用只读工具时，允许回退到 metadata 快照。
        </span>
      </label>
      <label class="tip-card">
        <strong>需要人工审核</strong>
        <span>
          <input v-model="form.options.requireHumanReview" type="checkbox">
          在完成前暂停工作流，并要求人工批准。
        </span>
      </label>
      <label class="tip-card">
        <strong>启用 grounding 校验</strong>
        <span>
          <input v-model="form.options.enableEvidenceGrounding" type="checkbox">
          校验每条建议都有证据支撑。
        </span>
      </label>
    </div>

    <p v-if="message" class="state-text">
      {{ message }}
    </p>
  </article>
</template>

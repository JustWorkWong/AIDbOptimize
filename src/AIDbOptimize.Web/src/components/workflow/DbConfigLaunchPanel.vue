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
        <h2>Launch</h2>
        <p class="section-copy">
          Select an existing MCP connection and start a db-config optimization workflow.
        </p>
      </div>
      <button
        type="button"
        :disabled="submitting || !canSubmit"
        @click="emit('submit')"
      >
        {{ submitting ? 'Submitting...' : 'Start workflow' }}
      </button>
    </div>

    <div class="form-grid">
      <label class="form-field">
        <span>Target connection</span>
        <select v-model="form.connectionId" class="inline-select">
          <option value="">Select a connection</option>
          <option
            v-for="connection in connections"
            :key="connection.id"
            :value="connection.id"
          >
            {{ connection.displayName }} · {{ connection.engine }} · {{ connection.databaseName }}
          </option>
        </select>
      </label>

      <label class="form-field">
        <span>Requested by</span>
        <input
          v-model.trim="form.requestedBy"
          class="text-input"
          type="text"
          placeholder="frontend"
        >
      </label>

      <label class="form-field form-field-wide">
        <span>Notes</span>
        <textarea
          v-model.trim="form.notes"
          class="json-editor"
          placeholder="Context, goals, constraints, or known risks"
        />
      </label>
    </div>

    <div class="tips-grid">
      <label class="tip-card">
        <strong>Allow fallback snapshot</strong>
        <span>
          <input v-model="form.options.allowFallbackSnapshot" type="checkbox">
          Use metadata fallback when no read-only tool is available.
        </span>
      </label>
      <label class="tip-card">
        <strong>Require human review</strong>
        <span>
          <input v-model="form.options.requireHumanReview" type="checkbox">
          Pause the workflow and require manual approval before completion.
        </span>
      </label>
      <label class="tip-card">
        <strong>Enable grounding</strong>
        <span>
          <input v-model="form.options.enableEvidenceGrounding" type="checkbox">
          Validate that each recommendation is backed by evidence.
        </span>
      </label>
    </div>

    <p v-if="message" class="state-text">
      {{ message }}
    </p>
  </article>
</template>

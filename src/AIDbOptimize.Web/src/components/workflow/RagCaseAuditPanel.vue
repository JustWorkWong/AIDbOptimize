<script setup lang="ts">
import type { RagCaseAuditItem } from '../../models/rag'
import { formatWorkflowDateTime, formatWorkflowRecommendationType } from '../../models/workflow'

defineProps<{
  items: RagCaseAuditItem[]
  loading: boolean
}>()
</script>

<template>
  <article class="panel">
    <h2>Case Audit</h2>
    <p v-if="loading" class="state-text">正在读取 case audit...</p>
    <p v-else-if="items.length === 0" class="state-text">当前还没有 case knowledge。</p>
    <div v-else class="structured-list compact-grid">
      <article v-for="item in items.slice(0, 8)" :key="item.caseRecordId" class="structured-card">
        <div class="structured-head">
          <strong>{{ item.problemType }}</strong>
          <span>{{ item.engine }} · {{ item.reviewStatus }}</span>
        </div>
        <p>{{ item.summary }}</p>
        <div class="meta-chip-row">
          <span class="meta-chip">类型：{{ formatWorkflowRecommendationType(item.recommendationType) }}</span>
          <span class="meta-chip">证据链：{{ item.evidenceLinkCount }}</span>
        </div>
        <p class="structured-note">workflow: {{ item.workflowSessionId }}</p>
        <p class="structured-note">创建时间：{{ formatWorkflowDateTime(item.createdAt) }}</p>
      </article>
    </div>
  </article>
</template>

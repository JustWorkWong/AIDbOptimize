<script setup lang="ts">
import type { RagAssetStatus } from '../../models/rag'
import { formatWorkflowDateTime } from '../../models/workflow'

defineProps<{
  status: RagAssetStatus | null
  loading: boolean
}>()
</script>

<template>
  <article class="panel">
    <h2>RAG 资产状态</h2>
    <p v-if="loading" class="state-text">正在读取 RAG 资产状态...</p>
    <p v-else-if="!status" class="state-text">当前还没有 RAG 资产状态。</p>
    <div v-else class="review-detail-grid">
      <div class="tip-card">
        <strong>facts 文档</strong>
        <span>{{ status.factDocumentCount }}</span>
      </div>
      <div class="tip-card">
        <strong>case 记录</strong>
        <span>{{ status.caseRecordCount }}</span>
      </div>
      <div class="tip-card">
        <strong>chunk 数量</strong>
        <span>{{ status.chunkCount }}</span>
      </div>
      <div class="tip-card">
        <strong>snapshot 数量</strong>
        <span>{{ status.retrievalSnapshotCount }}</span>
      </div>
      <div class="tip-card">
        <strong>最近入库</strong>
        <span>{{ formatWorkflowDateTime(status.latestDocumentIngestedAt) }}</span>
      </div>
      <div class="tip-card">
        <strong>最近 case 投影</strong>
        <span>{{ formatWorkflowDateTime(status.latestCaseProjectedAt) }}</span>
      </div>
      <div class="tip-card">
        <strong>最近 snapshot</strong>
        <span>{{ formatWorkflowDateTime(status.latestSnapshotCreatedAt) }}</span>
      </div>
    </div>
  </article>
</template>

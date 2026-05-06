<script setup lang="ts">
import type { ReviewTaskDetail, WorkflowSessionDetail } from '../../models/workflow'
import {
  extractWorkflowStructuredResult,
  formatWorkflowRecommendationType,
  formatWorkflowStatus,
} from '../../models/workflow'

defineProps<{
  session: WorkflowSessionDetail | null
  review: ReviewTaskDetail | null
  loading: boolean
  submitting: boolean
}>()

const emit = defineEmits<{
  submit: [action: 'approve' | 'reject' | 'adjust']
}>()

const reviewer = defineModel<string>('reviewer', { default: 'frontend-reviewer' })
const comment = defineModel<string>('comment', { default: '' })
const adjustmentsJson = defineModel<string>('adjustmentsJson', { default: '' })

function parsedReviewReport(review: ReviewTaskDetail | null) {
  return review?.parsedReport ?? extractWorkflowStructuredResult(review) ?? null
}
</script>

<template>
  <article class="panel">
    <h2>审核</h2>
    <p class="section-copy">
      审核生成结果，并通过批准、拒绝或调整来恢复工作流。
    </p>

    <p v-if="!session" class="state-text">
      请先选择一个工作流会话。
    </p>
    <p v-else-if="loading" class="state-text">
      正在加载审核任务……
    </p>
    <p v-else-if="!review" class="state-text">
      当前选中的工作流没有待处理审核任务。
    </p>

    <template v-if="session && review">
      <div class="review-summary-card">
        <strong>{{ review.title }}</strong>
        <span>状态：{{ formatWorkflowStatus(review.status) }}</span>
        <span>会话：{{ session.sessionId }}</span>
        <span v-if="session.skillSelection">
          {{ session.skillSelection.bundleId }}@{{ session.skillSelection.bundleVersion }}
        </span>
      </div>

      <div v-if="parsedReviewReport(review)" class="structured-report">
        <div class="review-detail-grid">
          <div class="tip-card">
            <strong>建议数</strong>
            <span>{{ parsedReviewReport(review)?.recommendations.length }}</span>
          </div>
          <div class="tip-card">
            <strong>缺失上下文</strong>
            <span>{{ parsedReviewReport(review)?.missingContextItems.length }}</span>
          </div>
          <div class="tip-card">
            <strong>警告</strong>
            <span>{{ parsedReviewReport(review)?.warnings.length }}</span>
          </div>
        </div>

        <div class="structured-list">
          <article
            v-for="item in parsedReviewReport(review)?.recommendations"
            :key="item.key + item.ruleId"
            class="structured-card"
          >
            <div class="structured-head">
              <strong>{{ item.key }}</strong>
              <span>{{ item.severity }} · {{ item.findingType }}</span>
            </div>
            <p>{{ item.suggestion }}</p>
            <div class="meta-chip-row">
              <span class="meta-chip">置信度：{{ item.confidence }}</span>
              <span class="meta-chip">分类：{{ item.recommendationClass }}</span>
              <span class="meta-chip">类型：{{ formatWorkflowRecommendationType(item.recommendationType) }}</span>
              <span class="meta-chip">规则：{{ item.ruleId || '无' }}@{{ item.ruleVersion || '无' }}</span>
              <span v-if="item.requiresMoreContext" class="meta-chip warning">需要更多上下文</span>
            </div>
            <p v-if="item.appliesWhen" class="structured-note">适用前提：{{ item.appliesWhen }}</p>
            <p v-if="item.evidenceReferences.length" class="structured-note">
              证据引用：{{ item.evidenceReferences.join(', ') }}
            </p>
          </article>
        </div>
      </div>

      <div class="result-box">
        {{ review.payloadJson }}
      </div>

      <div
        v-if="review.status === 'Pending'"
        class="review-action-box"
      >
        <label class="form-field">
          <span>审核人</span>
          <input v-model.trim="reviewer" class="text-input" type="text" placeholder="frontend-reviewer">
        </label>

        <label class="form-field">
          <span>审核说明</span>
          <textarea
            v-model.trim="comment"
            class="json-editor"
            placeholder="填写审核结论或调整说明"
          />
        </label>

        <label class="form-field">
          <span>调整 JSON</span>
          <textarea
            v-model.trim="adjustmentsJson"
            class="json-editor"
            placeholder='{"riskLevelOverrides":{"max_connections":"warning"}}'
          />
        </label>

        <div class="review-action-row">
          <button
            type="button"
            :disabled="submitting || !reviewer"
            @click="emit('submit', 'approve')"
          >
            {{ submitting ? '提交中…' : '批准' }}
          </button>
          <button
            type="button"
            class="secondary"
            :disabled="submitting || !reviewer"
            @click="emit('submit', 'reject')"
          >
            拒绝
          </button>
          <button
            type="button"
            class="secondary"
            :disabled="submitting || !reviewer || !comment"
            @click="emit('submit', 'adjust')"
          >
            调整
          </button>
        </div>
      </div>
    </template>
  </article>
</template>

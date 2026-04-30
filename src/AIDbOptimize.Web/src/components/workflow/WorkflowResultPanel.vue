<script setup lang="ts">
import { computed } from 'vue'
import type {
  WorkflowCollectionMetadata,
  WorkflowEvidenceItem,
  WorkflowHistoryDetail,
  WorkflowSessionDetail,
  WorkflowStructuredResult,
} from '../../models/workflow'
import { extractWorkflowStructuredResult } from '../../models/workflow'

const props = defineProps<{
  session: WorkflowSessionDetail | null
  historyDetail: WorkflowHistoryDetail | null
}>()

const resultPreview = computed(() => {
  return props.session?.result?.payloadJson ?? '当前还没有结果。'
})

const parsedReport = computed<WorkflowStructuredResult | null>(() => {
  return props.session?.result?.parsedReport ?? extractWorkflowStructuredResult(props.session?.result) ?? null
})

const hostContextItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'hostContext') ?? [])

const configurationItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'configuration').slice(0, 8) ?? [])

const runtimeItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'runtimeMetric').slice(0, 8) ?? [])

const observabilityItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'observability').slice(0, 8) ?? [])

function displayValue(item: WorkflowEvidenceItem): string {
  return item.normalizedValue || item.rawValue || 'n/a'
}

function metadataValue(name: string): WorkflowCollectionMetadata | undefined {
  return parsedReport.value?.collectionMetadata.find(item => item.name === name)
}
</script>

<template>
  <article class="panel">
    <h2>结果</h2>
    <p v-if="!session" class="state-text">
      请选择一个工作流会话以查看最终结果和审计详情。
    </p>

    <template v-else>
      <div v-if="parsedReport" class="structured-report">
        <div class="review-summary-card">
          <strong>{{ parsedReport.title }}</strong>
          <span>{{ parsedReport.summary }}</span>
        </div>

        <div class="review-detail-grid">
          <div class="tip-card">
            <strong>建议数</strong>
            <span>{{ parsedReport.recommendations.length }}</span>
          </div>
          <div class="tip-card">
            <strong>缺失上下文</strong>
            <span>{{ parsedReport.missingContextItems.length }}</span>
          </div>
          <div class="tip-card">
            <strong>宿主范围</strong>
            <span>{{ metadataValue('resource_scope')?.value || '未知' }}</span>
          </div>
        </div>

        <section v-if="parsedReport.recommendations.length" class="structured-section">
          <h3>建议详情</h3>
          <div class="structured-list">
            <article
              v-for="item in parsedReport.recommendations"
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
                <span class="meta-chip">规则：{{ item.ruleId || '无' }}@{{ item.ruleVersion || '无' }}</span>
                <span v-if="item.requiresMoreContext" class="meta-chip warning">需要更多上下文</span>
              </div>
              <p v-if="item.appliesWhen" class="structured-note">适用前提：{{ item.appliesWhen }}</p>
              <p v-if="item.impactSummary" class="structured-note">影响说明：{{ item.impactSummary }}</p>
              <p v-if="item.evidenceReferences.length" class="structured-note">
                证据引用：{{ item.evidenceReferences.join(', ') }}
              </p>
            </article>
          </div>
        </section>

        <section v-if="hostContextItems.length" class="structured-section">
          <h3>宿主上下文</h3>
          <div class="structured-list compact-grid">
            <article v-for="item in hostContextItems" :key="item.reference" class="structured-card">
              <strong>{{ item.reference }}</strong>
              <p>{{ displayValue(item) }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip">{{ item.sourceScope }}</span>
                <span class="meta-chip">{{ item.isCached ? '缓存' : '实时' }}</span>
              </div>
              <p class="structured-note">采集时间：{{ item.capturedAt || '无' }}</p>
              <p class="structured-note">过期时间：{{ item.expiresAt || '无' }}</p>
            </article>
          </div>
        </section>

        <section v-if="parsedReport.missingContextItems.length" class="structured-section">
          <h3>缺失上下文</h3>
          <div class="structured-list compact-grid">
            <article v-for="item in parsedReport.missingContextItems" :key="item.reference" class="structured-card">
              <strong>{{ item.reference }}</strong>
              <p>{{ item.description }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip warning">{{ item.reason }}</span>
                <span class="meta-chip">{{ item.sourceScope }}</span>
              </div>
            </article>
          </div>
        </section>

        <section class="structured-section">
          <h3>已采集证据</h3>
          <div class="structured-columns">
            <div>
              <strong>配置项</strong>
              <ul class="mini-list">
                <li v-for="item in configurationItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? '缓存' : '实时' }} · {{ item.capturedAt || '无' }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>运行指标</strong>
              <ul class="mini-list">
                <li v-for="item in runtimeItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? '缓存' : '实时' }} · {{ item.capturedAt || '无' }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>观测能力</strong>
              <ul class="mini-list">
                <li v-for="item in observabilityItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ item.isCached ? '缓存' : '实时' }} · {{ item.capturedAt || '无' }}</span>
                </li>
              </ul>
            </div>
          </div>
        </section>
      </div>

      <details class="command-details">
        <summary>查看原始结果 JSON</summary>
        <pre class="result-box">{{ resultPreview }}</pre>
      </details>

      <div v-if="historyDetail" class="review-detail-grid">
        <div class="tip-card">
          <strong>节点执行</strong>
          <span>{{ historyDetail.nodeExecutions.length }}</span>
        </div>
        <div class="tip-card">
          <strong>工具执行</strong>
          <span>{{ historyDetail.toolExecutions.length }}</span>
        </div>
        <div class="tip-card">
          <strong>审核记录</strong>
          <span>{{ historyDetail.reviews.length }}</span>
        </div>
      </div>

      <section v-if="historyDetail?.reviews.length" class="structured-section">
        <h3>审核决策</h3>
        <div class="structured-list compact-grid">
          <article v-for="item in historyDetail.reviews" :key="item.taskId" class="structured-card">
            <strong>{{ item.status }}</strong>
            <p>{{ item.comment || '无说明' }}</p>
            <div class="meta-chip-row">
              <span class="meta-chip">{{ item.reviewer || '无' }}</span>
              <span class="meta-chip">{{ item.reviewedAt || item.createdAt }}</span>
            </div>
          </article>
        </div>
      </section>
    </template>
  </article>
</template>

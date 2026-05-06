<script setup lang="ts">
import { computed } from 'vue'
import type {
  WorkflowCollectionMetadata,
  WorkflowEvidenceItem,
  WorkflowHistoryDetail,
  WorkflowSessionDetail,
  WorkflowStructuredResult,
} from '../../models/workflow'
import {
  extractWorkflowCollectionPlan,
  extractWorkflowStructuredResult,
  formatWorkflowDateTime,
  formatWorkflowGateStatus,
  formatWorkflowRecommendationType,
  formatWorkflowReviewStatus,
  formatWorkflowSourceQuality,
  formatWorkflowVersionTag,
  summarizeWorkflowExecutionOverview,
  summarizeWorkflowNodeExecution,
} from '../../models/workflow'

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

const collectionPlan = computed(() => extractWorkflowCollectionPlan(parsedReport.value))
const executionOverview = computed(() =>
  summarizeWorkflowExecutionOverview(parsedReport.value, props.historyDetail))
const skillSelection = computed(() => props.historyDetail?.skillSelection ?? props.session?.skillSelection ?? null)
const capabilityResults = computed(() => collectionPlan.value?.capabilityResults.slice(0, 8) ?? [])
const nodeExecutionSummaries = computed(() =>
  props.historyDetail?.nodeExecutions.map(item => summarizeWorkflowNodeExecution(item)) ?? [])

const hostContextItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'hostContext') ?? [])

const configurationItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'configuration').slice(0, 8) ?? [])

const runtimeItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'runtimeMetric').slice(0, 8) ?? [])

const observabilityItems = computed(() =>
  parsedReport.value?.evidenceItems.filter(item => item.category === 'observability').slice(0, 8) ?? [])

function displayValue(item: WorkflowEvidenceItem): string {
  return item.normalizedValue || item.rawValue || '无'
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
          <div class="tip-card">
            <strong>Gate</strong>
            <span>{{ formatWorkflowGateStatus(collectionPlan?.gateStatus) }}</span>
          </div>
          <div class="tip-card">
            <strong>计划完成度</strong>
            <span>{{ collectionPlan?.completeness || '未知' }}</span>
          </div>
          <div class="tip-card">
            <strong>计划 ID</strong>
            <span>{{ collectionPlan?.planId || '未知' }}</span>
          </div>
        </div>

        <section v-if="skillSelection" class="structured-section">
          <h3>规则选择</h3>
          <div class="review-detail-grid">
            <div class="tip-card">
              <strong>Bundle</strong>
              <span>{{ formatWorkflowVersionTag(skillSelection.bundleId, skillSelection.bundleVersion) }}</span>
            </div>
            <div class="tip-card">
              <strong>排查 skill</strong>
              <span>{{ formatWorkflowVersionTag(skillSelection.investigationSkillId, skillSelection.investigationSkillVersion) }}</span>
            </div>
            <div class="tip-card">
              <strong>诊断 skill</strong>
              <span>{{ formatWorkflowVersionTag(skillSelection.diagnosisSkillId, skillSelection.diagnosisSkillVersion) }}</span>
            </div>
          </div>
        </section>

        <section v-if="executionOverview" class="structured-section">
          <h3>执行轨迹摘要</h3>
          <div class="review-detail-grid">
            <div class="tip-card">
              <strong>计划 ID</strong>
              <span>{{ executionOverview.planId || 'n/a' }}</span>
            </div>
            <div class="tip-card">
              <strong>Gate 决策</strong>
              <span>{{ executionOverview.gateStatusLabel || 'n/a' }}</span>
            </div>
            <div class="tip-card">
              <strong>采集完成度</strong>
              <span>{{ executionOverview.collectionCompleteness || 'n/a' }}</span>
            </div>
          </div>
        </section>

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
                <span class="meta-chip">类型：{{ formatWorkflowRecommendationType(item.recommendationType) }}</span>
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

        <section v-if="collectionPlan" class="structured-section">
          <h3>采集计划与执行</h3>
          <div class="structured-columns">
            <div>
              <strong>计划证据引用</strong>
              <ul class="mini-list">
                <li v-for="item in collectionPlan.plannedSkillReferences" :key="item">
                  {{ item }}
                </li>
              </ul>
            </div>
            <div>
              <strong>缺失必要证据</strong>
              <ul class="mini-list">
                <li v-for="item in collectionPlan.missingRequiredEvidenceRefs" :key="item">
                  {{ item }}
                </li>
                <li v-if="!collectionPlan.missingRequiredEvidenceRefs.length">无</li>
              </ul>
            </div>
            <div>
              <strong>缺失可选证据</strong>
              <ul class="mini-list">
                <li v-for="item in collectionPlan.missingOptionalEvidenceRefs" :key="item">
                  {{ item }}
                </li>
                <li v-if="!collectionPlan.missingOptionalEvidenceRefs.length">无</li>
              </ul>
            </div>
          </div>
          <div v-if="capabilityResults.length" class="structured-list compact-grid">
            <article v-for="item in capabilityResults" :key="item.capabilityId" class="structured-card">
              <strong>{{ item.skillReference }}</strong>
              <p>{{ item.capabilityId }}</p>
              <div class="meta-chip-row">
                <span class="meta-chip">{{ item.isCollected ? '已采集' : '未采集' }}</span>
                <span class="meta-chip">{{ item.isNormalized ? '已归一化' : '未归一化' }}</span>
                <span class="meta-chip">{{ formatWorkflowSourceQuality(item.sourceQuality) }}</span>
              </div>
              <p class="structured-note">错误分类：{{ item.errorClassification || 'none' }}</p>
              <p class="structured-note">采集时间：{{ formatWorkflowDateTime(item.capturedAt) }}</p>
              <p class="structured-note">过期时间：{{ formatWorkflowDateTime(item.expiresAt) }}</p>
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
                <span class="meta-chip">{{ formatWorkflowSourceQuality(item.isCached ? 'cached' : 'live') }}</span>
              </div>
              <p class="structured-note">采集时间：{{ formatWorkflowDateTime(item.capturedAt) }}</p>
              <p class="structured-note">过期时间：{{ formatWorkflowDateTime(item.expiresAt) }}</p>
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
                  <span class="mini-note">· {{ formatWorkflowSourceQuality(item.isCached ? 'cached' : 'live') }} · {{ formatWorkflowDateTime(item.capturedAt) }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>运行指标</strong>
              <ul class="mini-list">
                <li v-for="item in runtimeItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ formatWorkflowSourceQuality(item.isCached ? 'cached' : 'live') }} · {{ formatWorkflowDateTime(item.capturedAt) }}</span>
                </li>
              </ul>
            </div>
            <div>
              <strong>观测能力</strong>
              <ul class="mini-list">
                <li v-for="item in observabilityItems" :key="item.reference">
                  {{ item.reference }} = {{ displayValue(item) }}
                  <span class="mini-note">· {{ formatWorkflowSourceQuality(item.isCached ? 'cached' : 'live') }} · {{ formatWorkflowDateTime(item.capturedAt) }}</span>
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

      <section v-if="nodeExecutionSummaries.length" class="structured-section">
        <h3>节点执行审计</h3>
        <div class="structured-list">
          <article v-for="item in nodeExecutionSummaries" :key="item.executionId" class="structured-card">
            <div class="structured-head">
              <strong>{{ item.nodeLabel }}</strong>
              <span>{{ item.statusLabel }}</span>
            </div>
            <p>{{ item.nodeDescription }}</p>
            <div class="meta-chip-row">
              <span v-for="chip in item.chips" :key="chip" class="meta-chip">
                {{ chip }}
              </span>
            </div>
            <p class="structured-note">开始时间：{{ item.startedAt }}</p>
            <p class="structured-note">完成时间：{{ item.completedAt || 'n/a' }}</p>
            <p v-for="detail in item.detailLines" :key="detail" class="structured-note">
              {{ detail }}
            </p>
          </article>
        </div>
      </section>

      <section v-if="historyDetail?.reviews.length" class="structured-section">
        <h3>审核决策</h3>
        <div class="structured-list compact-grid">
          <article v-for="item in historyDetail.reviews" :key="item.taskId" class="structured-card">
            <strong>{{ formatWorkflowReviewStatus(item.status) }}</strong>
            <p>{{ item.comment || '无说明' }}</p>
            <div class="meta-chip-row">
              <span class="meta-chip">{{ item.reviewer || '无' }}</span>
              <span class="meta-chip">{{ formatWorkflowDateTime(item.reviewedAt || item.createdAt) }}</span>
            </div>
          </article>
        </div>
      </section>
    </template>
  </article>
</template>

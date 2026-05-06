<script setup lang="ts">
import { computed } from 'vue'
import type { WorkflowHistoryDetail, WorkflowSessionDetail } from '../../models/workflow'
import {
  extractWorkflowStructuredResult,
  describeWorkflowNode,
  formatWorkflowGateStatus,
  formatWorkflowStatus,
  formatWorkflowVersionTag,
  summarizeWorkflowExecutionOverview,
} from '../../models/workflow'

const props = defineProps<{
  session: WorkflowSessionDetail | null
  historyDetail: WorkflowHistoryDetail | null
}>()

const emit = defineEmits<{
  cancel: []
}>()

const canCancel = computed(() => {
  return props.session !== null
    && !['Completed', 'Cancelled', 'Failed'].includes(props.session.status)
})

const parsedReport = computed(() => {
  return props.session?.result?.parsedReport ?? extractWorkflowStructuredResult(props.session?.result) ?? null
})

const currentNode = computed(() => describeWorkflowNode(props.session?.currentNode))
const executionOverview = computed(() => summarizeWorkflowExecutionOverview(parsedReport.value, props.historyDetail))
</script>

<template>
  <article class="panel">
    <div class="panel-header">
      <h2>状态</h2>
      <button
        v-if="canCancel"
        type="button"
        class="secondary"
        @click="emit('cancel')"
      >
        取消工作流
      </button>
    </div>
    <p v-if="!session" class="state-text">
      请选择一个工作流会话以查看最新状态。
    </p>

    <div v-else class="review-detail-grid">
      <div class="tip-card">
        <strong>状态</strong>
        <span>{{ formatWorkflowStatus(session.status) }}</span>
      </div>
      <div class="tip-card">
        <strong>当前阶段</strong>
        <span>{{ currentNode.label }}</span>
      </div>
      <div class="tip-card">
        <strong>阶段说明</strong>
        <span>{{ currentNode.description }}</span>
      </div>
      <div class="tip-card">
        <strong>进度</strong>
        <span>{{ session.progressPercent }}%</span>
      </div>
      <div class="tip-card">
        <strong>连接</strong>
        <span>{{ session.connection.displayName }}</span>
      </div>
      <div class="tip-card">
        <strong>引擎</strong>
        <span>{{ session.connection.engine }}</span>
      </div>
      <div class="tip-card">
        <strong>数据库</strong>
        <span>{{ session.connection.databaseName }}</span>
      </div>
      <div class="tip-card">
        <strong>Bundle</strong>
        <span>
          {{
            formatWorkflowVersionTag(
              session.skillSelection?.bundleId ?? executionOverview?.bundleId,
              session.skillSelection?.bundleVersion ?? executionOverview?.bundleVersion,
            )
          }}
        </span>
      </div>
      <div class="tip-card">
        <strong>排查 skill</strong>
        <span>
          {{
            formatWorkflowVersionTag(
              session.skillSelection?.investigationSkillId ?? executionOverview?.investigationSkillId,
              session.skillSelection?.investigationSkillVersion ?? executionOverview?.investigationSkillVersion,
            )
          }}
        </span>
      </div>
      <div class="tip-card">
        <strong>诊断 skill</strong>
        <span>
          {{
            formatWorkflowVersionTag(
              session.skillSelection?.diagnosisSkillId ?? executionOverview?.diagnosisSkillId,
              session.skillSelection?.diagnosisSkillVersion ?? executionOverview?.diagnosisSkillVersion,
            )
          }}
        </span>
      </div>
      <div class="tip-card">
        <strong>Gate</strong>
        <span>{{ formatWorkflowGateStatus(executionOverview?.gateStatus) }}</span>
      </div>
      <div class="tip-card">
        <strong>采集完成度</strong>
        <span>{{ executionOverview?.collectionCompleteness || 'n/a' }}</span>
      </div>
      <div class="tip-card">
        <strong>计划 ID</strong>
        <span>{{ executionOverview?.planId || 'n/a' }}</span>
      </div>
    </div>

    <p v-if="session?.error" class="state-text error">
      {{ session.error }}
    </p>
  </article>
</template>

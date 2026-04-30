<script setup lang="ts">
import { onBeforeUnmount, onMounted, ref, watch } from 'vue'
import { getWorkflowHistoryDetail } from '../../api/history'
import { getMcpConnections } from '../../api/mcp'
import { getReviewTask, submitWorkflowReview } from '../../api/review'
import {
  getWorkflowSession,
  getWorkflowSessions,
  startDbConfigWorkflow,
  subscribeWorkflowEvents,
} from '../../api/workflow'
import type { McpConnection } from '../../models/mcp'
import type {
  DbConfigWorkflowRequest,
  ReviewTaskDetail,
  WorkflowHistoryDetail,
  WorkflowReplayEvent,
  WorkflowSessionDetail,
  WorkflowSessionSummary,
} from '../../models/workflow'
import { emptyWorkflowRequest } from '../../models/workflow'
import DbConfigLaunchPanel from './DbConfigLaunchPanel.vue'
import WorkflowHistoryPanel from './WorkflowHistoryPanel.vue'
import WorkflowReplayPanel from './WorkflowReplayPanel.vue'
import WorkflowResultPanel from './WorkflowResultPanel.vue'
import WorkflowReviewPanel from './WorkflowReviewPanel.vue'
import WorkflowStatusCard from './WorkflowStatusCard.vue'

const connections = ref<McpConnection[]>([])
const sessions = ref<WorkflowSessionSummary[]>([])
const selectedSessionId = ref('')
const selectedSession = ref<WorkflowSessionDetail | null>(null)
const historyDetail = ref<WorkflowHistoryDetail | null>(null)
const selectedReview = ref<ReviewTaskDetail | null>(null)
const events = ref<WorkflowReplayEvent[]>([])
const message = ref('')
const submitting = ref(false)
const listLoading = ref(true)
const detailLoading = ref(false)
const reviewSubmitting = ref(false)
const reviewer = ref('frontend-reviewer')
const comment = ref('')
const adjustmentsJson = ref('{"riskLevelOverrides":{"max_connections":"warning"}}')
const sseConnected = ref(false)
const workflowForm = ref<DbConfigWorkflowRequest>(emptyWorkflowRequest())
let eventSource: EventSource | null = null

onMounted(async () => {
  await Promise.all([
    loadConnections(),
    loadSessions(),
  ])
})

onBeforeUnmount(() => {
  closeEventSource()
})

watch(selectedSessionId, async (sessionId) => {
  closeEventSource()
  events.value = []

  if (!sessionId) {
    selectedSession.value = null
    historyDetail.value = null
    selectedReview.value = null
    return
  }

  await refreshSelectedSession(sessionId)
  connectToEvents(sessionId)
})

async function loadConnections(): Promise<void> {
  connections.value = await getMcpConnections()
  if (!workflowForm.value.connectionId && connections.value.length > 0) {
    workflowForm.value.connectionId = connections.value[0].id
  }
}

async function loadSessions(): Promise<void> {
  listLoading.value = true
  try {
    sessions.value = await getWorkflowSessions()
    if (!selectedSessionId.value && sessions.value.length > 0) {
      selectedSessionId.value = sessions.value[0].sessionId
    }
  } catch (error) {
    message.value = error instanceof Error ? error.message : '加载工作流列表失败。'
  } finally {
    listLoading.value = false
  }
}

async function refreshSelectedSession(sessionId: string): Promise<void> {
  detailLoading.value = true
  try {
    const [session, detail] = await Promise.all([
      getWorkflowSession(sessionId),
      getWorkflowHistoryDetail(sessionId),
    ])
    selectedSession.value = session
    historyDetail.value = detail
    selectedReview.value = session.review?.taskId
      ? await getReviewTask(session.review.taskId)
      : null
  } catch (error) {
    message.value = error instanceof Error ? error.message : '加载工作流详情失败。'
  } finally {
    detailLoading.value = false
  }
}

function connectToEvents(sessionId: string): void {
  eventSource = subscribeWorkflowEvents(sessionId, {
    snapshot: async () => {
      sseConnected.value = true
      await refreshSelectedSession(sessionId)
    },
    event: async (event) => {
      events.value = [...events.value, event]
      sseConnected.value = true
      await refreshSelectedSession(sessionId)
      if (
        event.eventName === 'workflow.completed' ||
        event.eventName === 'workflow.failed' ||
        event.eventName === 'workflow.cancelled' ||
        event.eventName === 'review.requested' ||
        event.eventName === 'review.resolved' ||
        event.eventName === 'checkpoint.saved' ||
        event.eventName === 'executor.completed'
      ) {
        await loadSessions()
      }
    },
    error: () => {
      sseConnected.value = false
    },
  })
}

function closeEventSource(): void {
  if (eventSource) {
    eventSource.close()
    eventSource = null
  }
  sseConnected.value = false
}

async function handleWorkflowSubmit(): Promise<void> {
  submitting.value = true
  message.value = '正在启动工作流，请稍候……'

  try {
    const result = await startDbConfigWorkflow(workflowForm.value)
    message.value = `工作流已启动：${result.sessionId}`
    workflowForm.value = emptyWorkflowRequest()
    workflowForm.value.connectionId = connections.value[0]?.id ?? ''
    selectedSessionId.value = result.sessionId
    await loadSessions()
  } catch (error) {
    message.value = error instanceof Error ? error.message : '启动工作流失败。'
  } finally {
    submitting.value = false
  }
}

function handleSessionSelect(sessionId: string): void {
  selectedSessionId.value = sessionId
}

async function handleReviewSubmit(action: 'approve' | 'reject' | 'adjust'): Promise<void> {
  if (!selectedReview.value?.taskId) {
    message.value = '当前没有可提交的审核任务。'
    return
  }

  reviewSubmitting.value = true
  message.value = '正在提交审核结果，请稍候……'

  try {
    const payload = {
      action,
      reviewer: reviewer.value,
      comment: comment.value,
      adjustments: action === 'adjust' && adjustmentsJson.value.trim()
        ? JSON.parse(adjustmentsJson.value) as { riskLevelOverrides?: Record<string, string> }
        : undefined,
    }

    const result = await submitWorkflowReview(selectedReview.value.taskId, payload)
    message.value = `审核已提交：${result.reviewStatus} -> ${result.workflowStatus}`
    comment.value = ''
    await loadSessions()
    if (selectedSessionId.value) {
      await refreshSelectedSession(selectedSessionId.value)
    }
  } catch (error) {
    message.value = error instanceof Error ? error.message : '提交审核失败。'
  } finally {
    reviewSubmitting.value = false
  }
}

async function handleReplayRefresh(): Promise<void> {
  if (selectedSessionId.value) {
    await refreshSelectedSession(selectedSessionId.value)
  }
}
</script>

<template>
  <section class="panel">
    <h2>工作流工作区</h2>
    <p class="section-copy">
      启动数据库配置优化工作流，查看实时事件，处理待审核报告，并检查历史详情。
    </p>
  </section>

  <section class="panel-grid">
    <DbConfigLaunchPanel
      :connections="connections"
      :form="workflowForm"
      :submitting="submitting"
      :message="message"
      @submit="handleWorkflowSubmit"
    />
    <WorkflowStatusCard :session="selectedSession" />
  </section>

  <section class="panel-grid">
    <WorkflowHistoryPanel
      :items="sessions"
      :loading="listLoading"
      :message="message"
      :selected-session-id="selectedSessionId"
      @refresh="loadSessions"
      @select="handleSessionSelect"
    />
    <WorkflowResultPanel
      :session="selectedSession"
      :history-detail="historyDetail"
    />
  </section>

  <section class="panel-grid workflow-review-grid">
    <WorkflowReviewPanel
      :session="selectedSession"
      :review="selectedReview"
      :loading="detailLoading"
      :submitting="reviewSubmitting"
      v-model:reviewer="reviewer"
      v-model:comment="comment"
      v-model:adjustments-json="adjustmentsJson"
      @submit="handleReviewSubmit"
    />
    <WorkflowReplayPanel
      :session="selectedSession"
      :connected="sseConnected"
      :events="events"
      @refresh="handleReplayRefresh"
    />
  </section>
</template>

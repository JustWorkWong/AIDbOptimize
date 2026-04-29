export interface DbConfigWorkflowOptions {
  allowFallbackSnapshot: boolean
  requireHumanReview: boolean
  enableEvidenceGrounding: boolean
}

export interface DbConfigWorkflowRequest {
  connectionId: string
  requestedBy: string
  notes: string
  options: DbConfigWorkflowOptions
}

export interface WorkflowConnection {
  connectionId: string
  displayName: string
  engine: string
  databaseName: string
}

export interface WorkflowReviewReference {
  taskId: string
  status: string
}

export interface WorkflowSummaryReference {
  agentSessionId: string
  updatedAt: string
}

export interface WorkflowResult {
  resultType: string
  payloadJson: string
}

export interface WorkflowSessionDetail {
  sessionId: string
  workflowType: string
  engineType: string
  status: string
  currentNode: string | null
  progressPercent: number
  connection: WorkflowConnection
  review: WorkflowReviewReference | null
  result: WorkflowResult | null
  summary: WorkflowSummaryReference | null
  error: string | null
  streamUrl: string
  createdAt: string
  updatedAt: string
  completedAt: string | null
}

export interface WorkflowSessionSummary {
  sessionId: string
  workflowType: string
  engineType: string
  status: string
  currentNode: string | null
  progressPercent: number
  connection: WorkflowConnection
  activeReviewTaskId: string | null
  createdAt: string
  updatedAt: string
  completedAt: string | null
}

export interface WorkflowHistoryNodeExecution {
  executionId: string
  nodeName: string
  nodeType: string
  status: string
  inputJson: string
  outputJson: string
  error: string | null
  agentSessionId: string | null
  tokenUsageJson: string
  startedAt: string
  completedAt: string | null
}

export interface WorkflowHistoryToolExecution {
  executionId: string
  toolId: string
  toolName: string
  status: string
  requestJson: string
  responseJson: string
  error: string | null
  workflowNodeName: string | null
  executionScope: string
  createdAt: string
}

export interface WorkflowHistoryReview {
  taskId: string
  status: string
  payloadJson: string
  reviewer: string | null
  comment: string | null
  adjustmentsJson: string | null
  createdAt: string
  reviewedAt: string | null
}

export interface WorkflowHistoryDetail {
  sessionId: string
  workflowType: string
  engineType: string
  status: string
  currentNode: string | null
  connection: WorkflowConnection
  result: WorkflowResult | null
  summary: WorkflowSummaryReference | null
  error: string | null
  nodeExecutions: WorkflowHistoryNodeExecution[]
  toolExecutions: WorkflowHistoryToolExecution[]
  reviews: WorkflowHistoryReview[]
  startedAt: string
  updatedAt: string
  completedAt: string | null
}

export interface ReviewTaskSummary {
  taskId: string
  sessionId: string
  title: string
  status: string
  createdAt: string
  reviewedAt: string | null
}

export interface ReviewTaskDetail {
  taskId: string
  sessionId: string
  title: string
  status: string
  payloadJson: string
  createdAt: string
  reviewedAt: string | null
  reviewer: string | null
  decision: string | null
  comment: string | null
  adjustmentsJson: string | null
}

export interface WorkflowReplayEvent {
  sequence: number
  eventType: string
  eventName: string
  occurredAt: string | null
  message: string | null
  payload: unknown
}

export interface WorkflowReviewSubmission {
  action: 'approve' | 'reject' | 'adjust'
  reviewer: string
  comment: string
  adjustments?: {
    riskLevelOverrides?: Record<string, string>
  }
}

export const knownWorkflowEventNames = [
  'workflow.started',
  'executor.started',
  'executor.completed',
  'checkpoint.saved',
  'review.requested',
  'review.resolved',
  'workflow.completed',
  'workflow.failed',
  'workflow.cancelled',
  'workflow.recovered',
] as const

export const emptyWorkflowRequest = (): DbConfigWorkflowRequest => ({
  connectionId: '',
  requestedBy: 'frontend',
  notes: '',
  options: {
    allowFallbackSnapshot: true,
    requireHumanReview: true,
    enableEvidenceGrounding: true,
  },
})

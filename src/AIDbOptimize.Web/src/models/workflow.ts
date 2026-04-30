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
  parsedReport?: WorkflowStructuredResult | null
}

export interface WorkflowStructuredResult {
  title: string
  summary: string
  recommendations: WorkflowRecommendation[]
  evidenceItems: WorkflowEvidenceItem[]
  missingContextItems: WorkflowMissingContextItem[]
  collectionMetadata: WorkflowCollectionMetadata[]
  warnings: string[]
}

export interface WorkflowRecommendation {
  key: string
  suggestion: string
  severity: string
  findingType: string
  confidence: string
  requiresMoreContext: boolean
  impactSummary: string | null
  evidenceReferences: string[]
  recommendationClass: string
  appliesWhen: string | null
  ruleId: string | null
  ruleVersion: string | null
}

export interface WorkflowEvidenceItem {
  sourceType: string
  reference: string
  description: string
  category: string
  rawValue: string | null
  normalizedValue: string | null
  unit: string | null
  sourceScope: string
  capturedAt: string | null
  expiresAt: string | null
  isCached: boolean
  collectionMethod: string | null
}

export interface WorkflowMissingContextItem {
  reference: string
  description: string
  reason: string
  sourceScope: string
  severity: string
}

export interface WorkflowCollectionMetadata {
  name: string
  value: string
  description: string | null
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
  parsedReport: WorkflowStructuredResult | null
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

export interface WorkflowHostContextSummary {
  resourceScope: string | null
  status: string | null
  targetName: string | null
  targetId: string | null
  sourceTool: string | null
  topRowCount: string | null
  evidenceCount: number
  cachedEvidenceCount: number
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

const workflowPayloadKeys = ['parsedReport', 'payload', 'result', 'report', 'data', 'snapshot'] as const

function isRecord(value: unknown): value is Record<string, unknown> {
  return typeof value === 'object' && value !== null
}

function looksLikeStructuredWorkflowResult(value: Record<string, unknown>): boolean {
  return (
    'recommendations' in value ||
    'evidenceItems' in value ||
    'missingContextItems' in value ||
    'collectionMetadata' in value ||
    ('title' in value && 'summary' in value)
  )
}

function extractWorkflowStructuredResultInternal(
  source: unknown,
  visited: Set<unknown>,
  depth: number,
): WorkflowStructuredResult | null {
  if (depth > 5 || source === null || source === undefined) {
    return null
  }

  if (typeof source === 'string') {
    return extractWorkflowStructuredResultInternal(parseWorkflowJson(source), visited, depth + 1)
  }

  if (!isRecord(source) || visited.has(source)) {
    return null
  }

  visited.add(source)

  if (looksLikeStructuredWorkflowResult(source)) {
    return source as unknown as WorkflowStructuredResult
  }

  const payloadJson = source.payloadJson
  if (typeof payloadJson === 'string') {
    const parsedFromPayloadJson = extractWorkflowStructuredResultInternal(payloadJson, visited, depth + 1)
    if (parsedFromPayloadJson) {
      return parsedFromPayloadJson
    }
  }

  for (const key of workflowPayloadKeys) {
    const nested = source[key]
    const parsedNested = extractWorkflowStructuredResultInternal(nested, visited, depth + 1)
    if (parsedNested) {
      return parsedNested
    }
  }

  return null
}

export function parseWorkflowJson<T = unknown>(value: string | null | undefined): T | null {
  if (!value?.trim()) {
    return null
  }

  try {
    return JSON.parse(value) as T
  } catch {
    return null
  }
}

export function extractWorkflowStructuredResult(source: unknown): WorkflowStructuredResult | null {
  return extractWorkflowStructuredResultInternal(source, new Set<unknown>(), 0)
}

export function getWorkflowMetadataValue(
  metadata: WorkflowCollectionMetadata[],
  name: string,
): string | null {
  const item = metadata.find((entry) => entry.name === name)
  return item?.value ?? null
}

export function isHostContextEvidence(item: WorkflowEvidenceItem): boolean {
  return item.category === 'hostContext' || item.sourceScope.toLowerCase() !== 'db'
}

export function summarizeWorkflowHostContext(
  report: WorkflowStructuredResult | null | undefined,
): WorkflowHostContextSummary | null {
  if (!report) {
    return null
  }

  const hostEvidence = report.evidenceItems.filter(isHostContextEvidence)
  const resourceScope =
    getWorkflowMetadataValue(report.collectionMetadata, 'resource_scope') ??
    hostEvidence[0]?.sourceScope ??
    null
  const status = getWorkflowMetadataValue(report.collectionMetadata, 'host_context_status')
  const targetName = getWorkflowMetadataValue(report.collectionMetadata, 'target_name')
  const targetId = getWorkflowMetadataValue(report.collectionMetadata, 'target_id')
  const sourceTool = getWorkflowMetadataValue(report.collectionMetadata, 'topn_source_tool')
  const topRowCount = getWorkflowMetadataValue(report.collectionMetadata, 'topn_row_count')

  if (
    !resourceScope &&
    !status &&
    !targetName &&
    !targetId &&
    !sourceTool &&
    !topRowCount &&
    hostEvidence.length === 0
  ) {
    return null
  }

  return {
    resourceScope,
    status,
    targetName,
    targetId,
    sourceTool,
    topRowCount,
    evidenceCount: hostEvidence.length,
    cachedEvidenceCount: hostEvidence.filter((item) => item.isCached).length,
  }
}

export function formatWorkflowDateTime(value: string | null | undefined): string {
  if (!value) {
    return 'n/a'
  }

  const parsed = new Date(value)
  return Number.isNaN(parsed.getTime()) ? value : parsed.toLocaleString()
}

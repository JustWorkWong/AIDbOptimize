export interface DbConfigWorkflowOptions {
  allowFallbackSnapshot: boolean
  requireHumanReview: boolean
  enableEvidenceGrounding: boolean
}

export interface DbConfigWorkflowRequest {
  connectionId: string
  bundleId?: string
  bundleVersion?: string
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

export interface WorkflowSkillSelection {
  bundleId: string
  bundleVersion: string
  investigationSkillId: string
  investigationSkillVersion: string
  diagnosisSkillId: string
  diagnosisSkillVersion: string
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
  recommendationType: string
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

export interface WorkflowCapabilityResult {
  capabilityId: string
  skillReference: string
  isCollected: boolean
  isNormalized: boolean
  errorClassification: string
  capturedAt: string | null
  expiresAt: string | null
  sourceQuality: string
  producedEvidenceReferences: string[]
}

export interface WorkflowCollectionPlanSummary {
  planId: string | null
  plannedEvidenceCount: number
  collectedEvidenceCount: number
  missingRequiredEvidenceRefs: string[]
  missingOptionalEvidenceRefs: string[]
  plannedCapabilityIds: string[]
  plannedSkillReferences: string[]
  capabilityResults: WorkflowCapabilityResult[]
  missingContextPolicy: string | null
  gateStatus: string | null
  completeness: string | null
  bundleId: string | null
  bundleVersion: string | null
  investigationSkillId: string | null
  investigationSkillVersion: string | null
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
  skillSelection: WorkflowSkillSelection | null
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
  skillSelection: WorkflowSkillSelection | null
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
  skillSelection: WorkflowSkillSelection | null
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

export interface WorkflowNodePresentation {
  key: string | null
  label: string
  description: string
}

export interface WorkflowCapabilityResultSummary {
  capabilityId: string
  skillReference: string
  statusLabel: string
  sourceQualityLabel: string
  errorClassification: string
  producedEvidenceReferences: string[]
  capturedAt: string | null
  expiresAt: string | null
}

export interface WorkflowExecutionOverview {
  bundleId: string | null
  bundleVersion: string | null
  investigationSkillId: string | null
  investigationSkillVersion: string | null
  diagnosisSkillId: string | null
  diagnosisSkillVersion: string | null
  planId: string | null
  plannedEvidenceCount: number | null
  collectedEvidenceCount: number | null
  collectionCompleteness: string | null
  gateStatus: string | null
  gateStatusLabel: string | null
  missingRequiredEvidenceRefs: string[]
  missingOptionalEvidenceRefs: string[]
  plannedSkillReferences: string[]
  retrievalHints: string[]
  missingContextPolicy: string | null
  capabilityResults: WorkflowCapabilityResultSummary[]
}

export interface WorkflowNodeExecutionSummary {
  executionId: string
  nodeName: string
  nodeLabel: string
  nodeDescription: string
  statusLabel: string
  startedAt: string
  completedAt: string | null
  detailLines: string[]
  chips: string[]
}

export interface WorkflowReplayEventSummary {
  sequence: number
  eventName: string
  title: string
  description: string
  occurredAt: string
  detailLines: string[]
  chips: string[]
  rawPayloadJson: string
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
  'agent.report.ready',
  'workflow.completed',
  'workflow.failed',
  'workflow.cancelled',
  'workflow.recovered',
] as const

const workflowPayloadKeys = ['parsedReport', 'payload', 'result', 'report', 'data', 'snapshot'] as const

const workflowNodeCatalog: Record<string, Omit<WorkflowNodePresentation, 'key'>> = {
  DbConfigInputValidationExecutor: {
    label: '输入校验',
    description: '校验连接、bundle 与运行选项是否合法。',
  },
  InvestigationPlanner: {
    label: '调查规划',
    description: '根据 bundle 和调查 skill 生成证据采集计划。',
  },
  EvidenceCollectionSubworkflow: {
    label: '证据采集',
    description: '按计划收集配置、运行态与宿主上下文证据。',
  },
  SkillPolicyGate: {
    label: '策略门禁',
    description: '根据采集完整度判断通过、降级还是阻断。',
  },
  DbConfigDiagnosisAgentExecutor: {
    label: '诊断生成',
    description: '调用诊断 skill 产出结构化优化建议。',
  },
  DbConfigGroundingExecutor: {
    label: '结果校验',
    description: '用已采集证据校验诊断结论是否可信可落地。',
  },
  DbConfigHumanReviewGateExecutor: {
    label: '人工审核',
    description: '等待 reviewer 决定批准、驳回或调整后继续。',
  },
  DbConfigCompletionDirectExecutor: {
    label: '直接完成',
    description: 'grounding 通过后直接进入结果归档。',
  },
  DbConfigPolicyBlockedCompletionExecutor: {
    label: '阻断完成',
    description: '策略门禁阻断后输出保护性结果并结束流程。',
  },
  DbConfigCompletionExecutor: {
    label: '结果归档',
    description: '持久化最终结果、检查点与回放投影。',
  },
  DbConfigReviewResponseExecutor: {
    label: '审核恢复',
    description: '人工审核完成后恢复工作流并继续收尾。',
  },
  Cancelled: {
    label: '已取消',
    description: '工作流已停止，不会继续推进后续节点。',
  },
}

const workflowStatusCatalog: Record<string, string> = {
  Pending: '排队中',
  Running: '运行中',
  Recovering: '恢复中',
  WaitingForReview: '等待人工审核',
  Completed: '已完成',
  Cancelled: '已取消',
  Failed: '已失败',
}

const workflowExecutionStatusCatalog: Record<string, string> = {
  Pending: '等待中',
  Running: '执行中',
  Succeeded: '已完成',
  Completed: '已完成',
  Failed: '失败',
  Cancelled: '已取消',
}

const workflowReviewStatusCatalog: Record<string, string> = {
  Pending: '待审核',
  Approved: '已通过',
  Rejected: '已驳回',
  Adjusted: '已调整',
}

const workflowEventCatalog: Record<string, string> = {
  'workflow.started': '工作流启动',
  'executor.started': '阶段开始',
  'executor.completed': '阶段完成',
  'checkpoint.saved': '检查点已保存',
  'review.requested': '等待人工审核',
  'review.resolved': '人工审核已处理',
  'agent.report.ready': '诊断报告已生成',
  'workflow.completed': '工作流完成',
  'workflow.failed': '工作流失败',
  'workflow.cancelled': '工作流取消',
  'workflow.recovered': '工作流恢复',
}

const workflowRecommendationTypeCatalog: Record<string, string> = {
  actionableRecommendation: '可执行建议',
  requestMoreContext: '需补充上下文',
}

const workflowGateStatusCatalog: Record<string, string> = {
  Pass: '通过',
  Degraded: '降级放行',
  Blocked: '策略阻断',
  pass: '通过',
  degraded: '降级放行',
  blocked: '策略阻断',
}

const workflowReviewActionCatalog: Record<string, string> = {
  approve: '批准',
  reject: '驳回',
  adjust: '调整后继续',
}

const workflowNodeTypeCatalog: Record<string, string> = {
  deterministic: '确定性步骤',
  gate: '门禁步骤',
  agent: '智能体步骤',
  projection: '结果投影',
}

const workflowSourceQualityCatalog: Record<string, string> = {
  live: '实时采集',
  cached: '缓存复用',
  metadata: '元数据推导',
  derived: '派生结果',
  none: '无',
}

export const emptyWorkflowRequest = (): DbConfigWorkflowRequest => ({
  connectionId: '',
  bundleId: '',
  bundleVersion: '',
  requestedBy: 'frontend',
  notes: '',
  options: {
    allowFallbackSnapshot: true,
    requireHumanReview: true,
    enableEvidenceGrounding: true,
  },
})

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
    const parsedNested = extractWorkflowStructuredResultInternal(source[key], visited, depth + 1)
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

export function getWorkflowMetadataJson<T>(
  metadata: WorkflowCollectionMetadata[],
  name: string,
): T | null {
  return parseWorkflowJson<T>(getWorkflowMetadataValue(metadata, name))
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

export function extractWorkflowCollectionPlan(
  report: WorkflowStructuredResult | null | undefined,
): WorkflowCollectionPlanSummary | null {
  if (!report) {
    return null
  }

  const metadata = report.collectionMetadata
  const plannedEvidenceCount = Number.parseInt(getWorkflowMetadataValue(metadata, 'planned_evidence_count') ?? '', 10)
  const collectedEvidenceCount = Number.parseInt(getWorkflowMetadataValue(metadata, 'collected_evidence_count') ?? '', 10)

  return {
    planId: getWorkflowMetadataValue(metadata, 'plan_id'),
    plannedEvidenceCount: Number.isNaN(plannedEvidenceCount) ? 0 : plannedEvidenceCount,
    collectedEvidenceCount: Number.isNaN(collectedEvidenceCount) ? 0 : collectedEvidenceCount,
    missingRequiredEvidenceRefs: getWorkflowMetadataJson<string[]>(metadata, 'missing_required_evidence_refs') ?? [],
    missingOptionalEvidenceRefs: getWorkflowMetadataJson<string[]>(metadata, 'missing_optional_evidence_refs') ?? [],
    plannedCapabilityIds: getWorkflowMetadataJson<string[]>(metadata, 'planned_capability_ids_json') ?? [],
    plannedSkillReferences: getWorkflowMetadataJson<string[]>(metadata, 'planned_skill_references_json') ?? [],
    capabilityResults: getWorkflowMetadataJson<WorkflowCapabilityResult[]>(metadata, 'capability_results_json') ?? [],
    missingContextPolicy: getWorkflowMetadataValue(metadata, 'missing_context_policy'),
    gateStatus: getWorkflowMetadataValue(metadata, 'gate_status'),
    completeness: getWorkflowMetadataValue(metadata, 'collection_completeness'),
    bundleId: getWorkflowMetadataValue(metadata, 'bundle_id'),
    bundleVersion: getWorkflowMetadataValue(metadata, 'bundle_version'),
    investigationSkillId: getWorkflowMetadataValue(metadata, 'investigation_skill_id'),
    investigationSkillVersion: getWorkflowMetadataValue(metadata, 'investigation_skill_version'),
  }
}

export function describeWorkflowNode(nodeKey: string | null | undefined): WorkflowNodePresentation {
  if (!nodeKey?.trim()) {
    return {
      key: null,
      label: '未开始',
      description: '工作流尚未进入任何执行节点。',
    }
  }

  const trimmed = nodeKey.trim()
  const descriptor = workflowNodeCatalog[trimmed]
  if (descriptor) {
    return {
      key: trimmed,
      label: descriptor.label,
      description: descriptor.description,
    }
  }

  return {
    key: trimmed,
    label: trimmed,
    description: '暂未收录的工作流节点。',
  }
}

export function formatWorkflowNodeLabel(nodeKey: string | null | undefined): string {
  return describeWorkflowNode(nodeKey).label
}

export function formatWorkflowNodeName(value: string | null | undefined): string {
  return formatWorkflowNodeLabel(value)
}

export function formatWorkflowStatus(status: string | null | undefined): string {
  return formatCatalogValue(status, workflowStatusCatalog, '未知状态')
}

export function formatWorkflowExecutionStatus(status: string | null | undefined): string {
  return formatCatalogValue(status, workflowExecutionStatusCatalog, '未知执行状态')
}

export function formatWorkflowReviewStatus(status: string | null | undefined): string {
  return formatCatalogValue(status, workflowReviewStatusCatalog, '未知审核状态')
}

export function formatWorkflowEventLabel(eventName: string | null | undefined): string {
  return formatCatalogValue(eventName, workflowEventCatalog, '未知事件')
}

export function formatWorkflowEventName(value: string | null | undefined): string {
  return formatWorkflowEventLabel(value)
}

export function formatWorkflowRecommendationType(value: string | null | undefined): string {
  return formatCatalogValue(value, workflowRecommendationTypeCatalog, '未标注')
}

export function formatWorkflowGateStatus(status: string | null | undefined): string {
  return formatCatalogValue(status, workflowGateStatusCatalog, '未知 gate 状态')
}

export function formatWorkflowNodeType(nodeType: string | null | undefined): string {
  return formatCatalogValue(nodeType, workflowNodeTypeCatalog, '未知步骤类型')
}

export function formatWorkflowSourceQuality(value: string | null | undefined): string {
  return formatCatalogValue(value, workflowSourceQualityCatalog, '未知来源')
}

export function formatWorkflowVersionTag(
  id: string | null | undefined,
  version: string | null | undefined,
  fallback = 'n/a',
): string {
  if (!id?.trim()) {
    return fallback
  }

  return version?.trim()
    ? `${id.trim()}@${version.trim()}`
    : id.trim()
}

export function summarizeWorkflowExecutionOverview(
  report: WorkflowStructuredResult | null | undefined,
  historyDetail: WorkflowHistoryDetail | null | undefined,
): WorkflowExecutionOverview | null {
  const overview = createEmptyWorkflowExecutionOverview()

  if (report?.collectionMetadata.length) {
    applyOverviewFromMetadata(overview, report.collectionMetadata)
  }

  if (historyDetail) {
    const plannerExecution = findLatestNodeExecution(historyDetail, 'InvestigationPlanner')
    if (plannerExecution) {
      applyPlannerExecutionToOverview(overview, plannerExecution)
    }

    const collectionExecution = findLatestNodeExecution(historyDetail, 'EvidenceCollectionSubworkflow')
    if (collectionExecution) {
      applyCollectionExecutionToOverview(overview, collectionExecution)
    }

    const gateExecution = findLatestNodeExecution(historyDetail, 'SkillPolicyGate')
    if (gateExecution) {
      applyGateExecutionToOverview(overview, gateExecution)
    }

    const diagnosisExecution = findLatestNodeExecution(historyDetail, 'DbConfigDiagnosisAgentExecutor')
    if (diagnosisExecution) {
      applyOverviewFromMetadata(overview, extractWorkflowCollectionMetadata(diagnosisExecution.inputJson))
      applyOverviewFromMetadata(overview, extractWorkflowCollectionMetadata(diagnosisExecution.outputJson))
    }
  }

  if (
    !overview.collectionCompleteness &&
    overview.collectedEvidenceCount !== null &&
    overview.plannedEvidenceCount !== null
  ) {
    overview.collectionCompleteness = `${overview.collectedEvidenceCount}/${overview.plannedEvidenceCount}`
  }

  overview.gateStatusLabel = overview.gateStatus
    ? formatWorkflowGateStatus(overview.gateStatus)
    : null

  return hasWorkflowExecutionOverviewData(overview) ? overview : null
}

export function summarizeWorkflowNodeExecution(
  execution: WorkflowHistoryNodeExecution,
): WorkflowNodeExecutionSummary {
  const descriptor = describeWorkflowNode(execution.nodeName)
  const detailLines: string[] = []
  const chips: string[] = [formatWorkflowNodeType(execution.nodeType)]
  const input = parseWorkflowJson<unknown>(execution.inputJson)
  const output = parseWorkflowJson<unknown>(execution.outputJson)

  switch (execution.nodeName) {
    case 'InvestigationPlanner':
      appendPlannerDetails(detailLines, input, output)
      break
    case 'EvidenceCollectionSubworkflow':
      appendCollectionDetails(detailLines, output)
      break
    case 'SkillPolicyGate':
      appendGateDetails(detailLines, output)
      break
    case 'DbConfigDiagnosisAgentExecutor':
      appendDiagnosisDetails(detailLines, input, output)
      break
    case 'DbConfigGroundingExecutor':
      detailLines.push('已基于采集到的证据执行 grounding 校验。')
      break
    case 'DbConfigHumanReviewGateExecutor':
      detailLines.push('结构化结果已进入人工审核，等待 reviewer 决策。')
      break
    case 'DbConfigPolicyBlockedCompletionExecutor':
      detailLines.push('策略门禁阻断后输出保护性结果，避免给出看似确定的诊断结论。')
      break
    case 'DbConfigCompletionDirectExecutor':
      detailLines.push('已跳过人工审核，直接把 grounded 结果送往归档。')
      break
    case 'DbConfigCompletionExecutor':
      detailLines.push('最终结果、检查点和回放投影已写入控制面。')
      break
    default:
      break
  }

  if (execution.agentSessionId) {
    chips.push(`Agent Session: ${execution.agentSessionId}`)
  }

  const totalTokens = extractTokenCount(execution.tokenUsageJson)
  if (totalTokens !== null) {
    chips.push(`Token: ${totalTokens}`)
  }

  if (execution.error) {
    detailLines.push(`错误: ${execution.error}`)
  }

  return {
    executionId: execution.executionId,
    nodeName: execution.nodeName,
    nodeLabel: descriptor.label,
    nodeDescription: descriptor.description,
    statusLabel: formatWorkflowExecutionStatus(execution.status),
    startedAt: formatWorkflowDateTime(execution.startedAt),
    completedAt: execution.completedAt ? formatWorkflowDateTime(execution.completedAt) : null,
    detailLines: dedupeStrings(detailLines),
    chips: dedupeStrings(chips),
  }
}

export function summarizeWorkflowReplayEvent(event: WorkflowReplayEvent): WorkflowReplayEventSummary {
  const payload = isRecord(event.payload) ? event.payload : null
  const nodeDescriptor = describeWorkflowNode(getRecordString(payload, ['nodeName', 'currentNode']))
  const detailLines: string[] = []
  const chips: string[] = []

  const planId = getRecordString(payload, ['planId'])
  if (planId) {
    chips.push(`计划: ${planId}`)
  }

  const plannedEvidenceCount = getRecordNumber(payload, ['plannedEvidenceCount'])
  if (plannedEvidenceCount !== null) {
    chips.push(`计划证据: ${plannedEvidenceCount}`)
  }

  const collectedEvidenceCount = getRecordNumber(payload, ['collectedEvidenceCount'])
  if (collectedEvidenceCount !== null) {
    chips.push(`已采集: ${collectedEvidenceCount}`)
  }

  const missingRequiredEvidenceCount = getRecordNumber(payload, ['missingRequiredEvidenceCount'])
  if (missingRequiredEvidenceCount !== null) {
    chips.push(`缺失必需: ${missingRequiredEvidenceCount}`)
  }

  const recommendationCount = getRecordNumber(payload, ['recommendationCount'])
  if (recommendationCount !== null) {
    chips.push(`建议: ${recommendationCount}`)
  }

  const gateStatus = getRecordString(payload, ['gateStatus'])
  if (gateStatus) {
    chips.push(`Gate: ${formatWorkflowGateStatus(gateStatus)}`)
  }

  const reviewTaskId = getRecordString(payload, ['reviewTaskId'])
  if (reviewTaskId) {
    chips.push(`审核任务: ${reviewTaskId}`)
  }

  const action = getRecordString(payload, ['action'])
  if (action) {
    chips.push(`动作: ${formatCatalogValue(action, workflowReviewActionCatalog, action)}`)
  }

  const agentSessionId = getRecordString(payload, ['agentSessionId'])
  if (agentSessionId) {
    chips.push(`Agent Session: ${agentSessionId}`)
  }

  const checkpointRef = getRecordString(payload, ['checkpointRef'])
  if (checkpointRef) {
    chips.push(`Checkpoint: ${checkpointRef}`)
  }

  const title = getRecordString(payload, ['title'])
  if (title) {
    detailLines.push(`报告标题: ${title}`)
  }

  const summary = getRecordString(payload, ['summary'])
  if (summary) {
    detailLines.push(`摘要: ${summary}`)
  }

  const requestId = getRecordString(payload, ['requestId'])
  if (requestId) {
    detailLines.push(`请求 ID: ${requestId}`)
  }

  return {
    sequence: event.sequence,
    eventName: event.eventName,
    title: nodeDescriptor.key
      ? `${nodeDescriptor.label} · ${formatWorkflowEventLabel(event.eventName)}`
      : formatWorkflowEventLabel(event.eventName),
    description: buildWorkflowReplayDescription(event.eventName, nodeDescriptor.label, action),
    occurredAt: formatWorkflowDateTime(event.occurredAt),
    detailLines: dedupeStrings(detailLines),
    chips: dedupeStrings(chips),
    rawPayloadJson: JSON.stringify(event.payload, null, 2),
  }
}

export function formatWorkflowDateTime(value: string | null | undefined): string {
  if (!value) {
    return 'n/a'
  }

  const parsed = new Date(value)
  return Number.isNaN(parsed.getTime()) ? value : parsed.toLocaleString()
}

function createEmptyWorkflowExecutionOverview(): WorkflowExecutionOverview {
  return {
    bundleId: null,
    bundleVersion: null,
    investigationSkillId: null,
    investigationSkillVersion: null,
    diagnosisSkillId: null,
    diagnosisSkillVersion: null,
    planId: null,
    plannedEvidenceCount: null,
    collectedEvidenceCount: null,
    collectionCompleteness: null,
    gateStatus: null,
    gateStatusLabel: null,
    missingRequiredEvidenceRefs: [],
    missingOptionalEvidenceRefs: [],
    plannedSkillReferences: [],
    retrievalHints: [],
    missingContextPolicy: null,
    capabilityResults: [],
  }
}

function hasWorkflowExecutionOverviewData(overview: WorkflowExecutionOverview): boolean {
  return Boolean(
    overview.bundleId ||
    overview.investigationSkillId ||
    overview.diagnosisSkillId ||
    overview.planId ||
    overview.gateStatus ||
    overview.collectionCompleteness ||
    overview.plannedSkillReferences.length ||
    overview.retrievalHints.length ||
    overview.capabilityResults.length,
  )
}

function findLatestNodeExecution(
  historyDetail: WorkflowHistoryDetail,
  nodeName: string,
): WorkflowHistoryNodeExecution | null {
  const matches = historyDetail.nodeExecutions.filter(item => item.nodeName === nodeName)
  return matches.length > 0 ? matches[matches.length - 1] : null
}

function applyPlannerExecutionToOverview(
  overview: WorkflowExecutionOverview,
  execution: WorkflowHistoryNodeExecution,
): void {
  const input = parseWorkflowJson<Record<string, unknown>>(execution.inputJson)
  const output = parseWorkflowJson<Record<string, unknown>>(execution.outputJson)

  overview.bundleId ??= getRecordString(output, ['bundleId']) ?? getRecordString(input, ['bundleId'])
  overview.bundleVersion ??= getRecordString(output, ['bundleVersion']) ?? getRecordString(input, ['bundleVersion'])
  overview.investigationSkillId ??=
    getRecordString(output, ['skillId']) ??
    getRecordString(input, ['investigationSkill', 'investigationSkillId'])
  overview.investigationSkillVersion ??=
    getRecordString(output, ['skillVersion']) ??
    getRecordString(input, ['version', 'investigationSkillVersion'])
  overview.planId ??= getRecordString(output, ['planId'])
  overview.missingContextPolicy ??= getRecordString(output, ['missingContextPolicy'])

  const steps = getRecordArray(output, ['steps'])
  if (overview.plannedEvidenceCount === null && steps.length > 0) {
    overview.plannedEvidenceCount = steps.length
  }

  if (overview.plannedSkillReferences.length === 0 && steps.length > 0) {
    overview.plannedSkillReferences = dedupeStrings(
      steps
        .map(item => getRecordString(asRecord(item), ['skillReference']))
        .filter((item): item is string => Boolean(item)),
    )
  }

  if (overview.retrievalHints.length === 0) {
    overview.retrievalHints = toStringArray(output?.retrievalHints)
  }
}

function applyCollectionExecutionToOverview(
  overview: WorkflowExecutionOverview,
  execution: WorkflowHistoryNodeExecution,
): void {
  const output = parseWorkflowJson<Record<string, unknown>>(execution.outputJson)
  const summary = asRecord(output?.summary)

  overview.planId ??= getRecordString(summary, ['planId'])
  overview.plannedEvidenceCount ??= getRecordNumber(summary, ['plannedEvidenceCount'])
  overview.collectedEvidenceCount ??= getRecordNumber(summary, ['collectedEvidenceCount'])
  overview.missingRequiredEvidenceRefs =
    overview.missingRequiredEvidenceRefs.length > 0
      ? overview.missingRequiredEvidenceRefs
      : toStringArray(summary?.missingRequiredEvidenceRefs)
  overview.missingOptionalEvidenceRefs =
    overview.missingOptionalEvidenceRefs.length > 0
      ? overview.missingOptionalEvidenceRefs
      : toStringArray(summary?.missingOptionalEvidenceRefs)

  if (overview.capabilityResults.length === 0) {
    overview.capabilityResults = parseCapabilityResults(summary?.capabilityResults)
  }

  applyOverviewFromMetadata(overview, extractWorkflowCollectionMetadata(output))
}

function applyGateExecutionToOverview(
  overview: WorkflowExecutionOverview,
  execution: WorkflowHistoryNodeExecution,
): void {
  const output = parseWorkflowJson<Record<string, unknown>>(execution.outputJson)
  overview.gateStatus ??= getRecordString(output, ['decision'])
  applyOverviewFromMetadata(overview, extractWorkflowCollectionMetadata(output))
}

function applyOverviewFromMetadata(
  overview: WorkflowExecutionOverview,
  metadata: WorkflowCollectionMetadata[],
): void {
  if (metadata.length === 0) {
    return
  }

  const map = buildWorkflowMetadataMap(metadata)
  overview.bundleId ??= getMetadataValue(map, ['bundle_id'])
  overview.bundleVersion ??= getMetadataValue(map, ['bundle_version'])
  overview.investigationSkillId ??= getMetadataValue(map, ['investigation_skill_id', 'skill_id'])
  overview.investigationSkillVersion ??= getMetadataValue(map, ['investigation_skill_version', 'skill_version'])
  overview.diagnosisSkillId ??= getMetadataValue(map, ['diagnosis_skill_id'])
  overview.diagnosisSkillVersion ??= getMetadataValue(map, ['diagnosis_skill_version'])
  overview.planId ??= getMetadataValue(map, ['plan_id'])
  overview.collectionCompleteness ??= getMetadataValue(map, ['collection_completeness'])
  overview.gateStatus ??= getMetadataValue(map, ['gate_status'])
  overview.missingContextPolicy ??= getMetadataValue(map, ['missing_context_policy'])
  overview.plannedEvidenceCount ??= parseNumberValue(getMetadataValue(map, ['planned_evidence_count']))
  overview.collectedEvidenceCount ??= parseNumberValue(getMetadataValue(map, ['collected_evidence_count']))

  if (overview.missingRequiredEvidenceRefs.length === 0) {
    overview.missingRequiredEvidenceRefs = toStringArray(getMetadataValue(map, ['missing_required_evidence_refs']))
  }

  if (overview.missingOptionalEvidenceRefs.length === 0) {
    overview.missingOptionalEvidenceRefs = toStringArray(getMetadataValue(map, ['missing_optional_evidence_refs']))
  }

  if (overview.plannedSkillReferences.length === 0) {
    overview.plannedSkillReferences = toStringArray(getMetadataValue(map, ['planned_skill_references_json']))
  }

  if (overview.retrievalHints.length === 0) {
    overview.retrievalHints = toStringArray(getMetadataValue(map, ['retrieval_hints_json']))
  }

  if (overview.capabilityResults.length === 0) {
    overview.capabilityResults = parseCapabilityResults(getMetadataValue(map, ['capability_results_json']))
  }
}

function buildWorkflowMetadataMap(metadata: WorkflowCollectionMetadata[]): Record<string, string> {
  return metadata.reduce<Record<string, string>>((result, item) => {
    result[item.name.toLowerCase()] = item.value
    return result
  }, {})
}

function getMetadataValue(metadata: Record<string, string>, names: string[]): string | null {
  for (const name of names) {
    const value = metadata[name.toLowerCase()]
    if (value?.trim()) {
      return value
    }
  }

  return null
}

function extractWorkflowCollectionMetadata(source: unknown): WorkflowCollectionMetadata[] {
  return extractWorkflowCollectionMetadataInternal(source, new Set<unknown>(), 0)
}

function extractWorkflowCollectionMetadataInternal(
  source: unknown,
  visited: Set<unknown>,
  depth: number,
): WorkflowCollectionMetadata[] {
  if (depth > 6 || source === null || source === undefined) {
    return []
  }

  if (typeof source === 'string') {
    return extractWorkflowCollectionMetadataInternal(parseWorkflowJson(source), visited, depth + 1)
  }

  if (looksLikeWorkflowCollectionMetadataArray(source)) {
    return source
  }

  if (Array.isArray(source)) {
    for (const item of source) {
      const metadata = extractWorkflowCollectionMetadataInternal(item, visited, depth + 1)
      if (metadata.length > 0) {
        return metadata
      }
    }

    return []
  }

  if (!isRecord(source) || visited.has(source)) {
    return []
  }

  visited.add(source)

  for (const key of [
    'collectionMetadata',
    'evidence',
    'snapshot',
    'parsedReport',
    'payload',
    'result',
    'report',
    'data',
    'inputJson',
    'outputJson',
    'payloadJson',
  ]) {
    const metadata = extractWorkflowCollectionMetadataInternal(source[key], visited, depth + 1)
    if (metadata.length > 0) {
      return metadata
    }
  }

  return []
}

function looksLikeWorkflowCollectionMetadataArray(value: unknown): value is WorkflowCollectionMetadata[] {
  return Array.isArray(value) && value.every(item =>
    isRecord(item) &&
    typeof item.name === 'string' &&
    typeof item.value === 'string')
}

function appendPlannerDetails(detailLines: string[], input: unknown, output: unknown): void {
  const inputRecord = asRecord(input)
  const outputRecord = asRecord(output)
  const bundle = formatWorkflowVersionTag(
    getRecordString(outputRecord, ['bundleId']) ?? getRecordString(inputRecord, ['bundleId']),
    getRecordString(outputRecord, ['bundleVersion']) ?? getRecordString(inputRecord, ['bundleVersion']),
  )
  const skill = formatWorkflowVersionTag(
    getRecordString(outputRecord, ['skillId']) ?? getRecordString(inputRecord, ['investigationSkill']),
    getRecordString(outputRecord, ['skillVersion']) ?? getRecordString(inputRecord, ['version']),
  )
  const steps = getRecordArray(outputRecord, ['steps'])

  detailLines.push(`Bundle: ${bundle}`)
  detailLines.push(`调查 skill: ${skill}`)

  const planId = getRecordString(outputRecord, ['planId'])
  if (planId) {
    detailLines.push(`计划 ID: ${planId}`)
  }

  if (steps.length > 0) {
    detailLines.push(`计划步骤: ${steps.length}`)
  }
}

function appendCollectionDetails(detailLines: string[], output: unknown): void {
  const outputRecord = asRecord(output)
  const summary = asRecord(outputRecord?.summary)
  const plannedEvidenceCount = getRecordNumber(summary, ['plannedEvidenceCount'])
  const collectedEvidenceCount = getRecordNumber(summary, ['collectedEvidenceCount'])

  if (plannedEvidenceCount !== null && collectedEvidenceCount !== null) {
    detailLines.push(`采集覆盖: ${collectedEvidenceCount}/${plannedEvidenceCount}`)
  }

  const missingRequiredEvidenceRefs = toStringArray(summary?.missingRequiredEvidenceRefs)
  if (missingRequiredEvidenceRefs.length > 0) {
    detailLines.push(`缺失必需证据: ${missingRequiredEvidenceRefs.join(', ')}`)
  }

  const missingOptionalEvidenceRefs = toStringArray(summary?.missingOptionalEvidenceRefs)
  if (missingOptionalEvidenceRefs.length > 0) {
    detailLines.push(`缺失可选证据: ${missingOptionalEvidenceRefs.join(', ')}`)
  }
}

function appendGateDetails(detailLines: string[], output: unknown): void {
  const outputRecord = asRecord(output)
  const decision = getRecordString(outputRecord, ['decision'])
  if (decision) {
    detailLines.push(`Gate 决策: ${formatWorkflowGateStatus(decision)}`)
  }

  const metadata = extractWorkflowCollectionMetadata(outputRecord)
  const map = buildWorkflowMetadataMap(metadata)
  const completeness = getMetadataValue(map, ['collection_completeness'])
  if (completeness) {
    detailLines.push(`采集完整度: ${completeness}`)
  }
}

function appendDiagnosisDetails(detailLines: string[], input: unknown, output: unknown): void {
  const report = extractWorkflowStructuredResult(output)
  const metadata = extractWorkflowCollectionMetadata(input)
  const metadataMap = buildWorkflowMetadataMap(metadata)
  const diagnosisSkill = formatWorkflowVersionTag(
    getMetadataValue(metadataMap, ['diagnosis_skill_id']),
    getMetadataValue(metadataMap, ['diagnosis_skill_version']),
    '未解析',
  )

  detailLines.push(`诊断 skill: ${diagnosisSkill}`)

  if (report) {
    detailLines.push(`报告标题: ${report.title}`)
    detailLines.push(`建议数: ${report.recommendations.length}`)
    detailLines.push(`缺失上下文: ${report.missingContextItems.length}`)
  }
}

function buildWorkflowReplayDescription(
  eventName: string,
  nodeLabel: string,
  action: string | null,
): string {
  switch (eventName) {
    case 'workflow.started':
      return '工作流实例已创建，执行图开始运行。'
    case 'executor.started':
      return `${nodeLabel}开始执行。`
    case 'executor.completed':
      return `${nodeLabel}执行完成。`
    case 'checkpoint.saved':
      return '最终结果已经持久化为检查点，可用于恢复和回放。'
    case 'review.requested':
      return '结构化结果已进入人工审核队列。'
    case 'review.resolved':
      return action === 'adjust'
        ? '人工审核选择调整后继续，工作流将基于调整结果收尾。'
        : '人工审核已提交，工作流将继续推进。'
    case 'agent.report.ready':
      return '诊断 agent 已生成结构化报告。'
    case 'workflow.completed':
      return '工作流已经完成。'
    case 'workflow.failed':
      return '工作流执行失败，请结合错误与原始 payload 排查。'
    case 'workflow.cancelled':
      return '工作流已被取消，后续节点不会再继续执行。'
    case 'workflow.recovered':
      return '工作流从持久化状态恢复并重新接回执行路径。'
    default:
      return formatWorkflowEventLabel(eventName)
  }
}

function parseCapabilityResults(value: unknown): WorkflowCapabilityResultSummary[] {
  const source = typeof value === 'string'
    ? parseWorkflowJson<unknown>(value)
    : value

  if (!Array.isArray(source)) {
    return []
  }

  return source.flatMap(item => {
    const record = asRecord(item)
    if (!record) {
      return []
    }

    const capabilityId = getRecordString(record, ['capabilityId']) ?? 'unknown'
    const skillReference = getRecordString(record, ['skillReference']) ?? 'unknown'
    const isCollected = getRecordBoolean(record, ['isCollected']) ?? false
    const isNormalized = getRecordBoolean(record, ['isNormalized']) ?? false
    const errorClassification = getRecordString(record, ['errorClassification']) ?? 'none'

    return [{
      capabilityId,
      skillReference,
      statusLabel: isCollected
        ? isNormalized ? '已采集并归一化' : '已采集'
        : '未采集',
      sourceQualityLabel: formatWorkflowSourceQuality(getRecordString(record, ['sourceQuality'])),
      errorClassification,
      producedEvidenceReferences: toStringArray(record.producedEvidenceReferences),
      capturedAt: getRecordString(record, ['capturedAt']),
      expiresAt: getRecordString(record, ['expiresAt']),
    }]
  })
}

function extractTokenCount(tokenUsageJson: string | null | undefined): number | null {
  const tokenUsage = parseWorkflowJson<Record<string, unknown>>(tokenUsageJson)
  return getRecordNumber(tokenUsage, ['totalTokens'])
}

function formatCatalogValue(
  value: string | null | undefined,
  catalog: Record<string, string>,
  fallback: string,
): string {
  if (!value?.trim()) {
    return fallback
  }

  const trimmed = value.trim()
  const exactMatch = catalog[trimmed]
  if (exactMatch) {
    return exactMatch
  }

  const normalized = trimmed.toLowerCase()
  const matchedKey = Object.keys(catalog).find(key => key.toLowerCase() === normalized)
  return matchedKey ? catalog[matchedKey] : trimmed
}

function asRecord(value: unknown): Record<string, unknown> | null {
  return isRecord(value) ? value : null
}

function getRecordString(
  record: Record<string, unknown> | null | undefined,
  keys: string[],
): string | null {
  if (!record) {
    return null
  }

  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'string' && value.trim()) {
      return value.trim()
    }

    if (typeof value === 'number' || typeof value === 'boolean') {
      return String(value)
    }
  }

  return null
}

function getRecordNumber(
  record: Record<string, unknown> | null | undefined,
  keys: string[],
): number | null {
  if (!record) {
    return null
  }

  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'number' && Number.isFinite(value)) {
      return value
    }

    if (typeof value === 'string') {
      const parsed = Number(value)
      if (Number.isFinite(parsed)) {
        return parsed
      }
    }
  }

  return null
}

function getRecordBoolean(
  record: Record<string, unknown> | null | undefined,
  keys: string[],
): boolean | null {
  if (!record) {
    return null
  }

  for (const key of keys) {
    const value = record[key]
    if (typeof value === 'boolean') {
      return value
    }

    if (typeof value === 'string') {
      if (value.toLowerCase() === 'true') {
        return true
      }

      if (value.toLowerCase() === 'false') {
        return false
      }
    }
  }

  return null
}

function getRecordArray(
  record: Record<string, unknown> | null | undefined,
  keys: string[],
): unknown[] {
  if (!record) {
    return []
  }

  for (const key of keys) {
    const value = record[key]
    if (Array.isArray(value)) {
      return value
    }
  }

  return []
}

function parseNumberValue(value: string | null | undefined): number | null {
  if (!value?.trim()) {
    return null
  }

  const parsed = Number(value)
  return Number.isFinite(parsed) ? parsed : null
}

function toStringArray(value: unknown): string[] {
  if (Array.isArray(value)) {
    return dedupeStrings(
      value
        .map(item => {
          if (typeof item === 'string' && item.trim()) {
            return item.trim()
          }

          if (typeof item === 'number' || typeof item === 'boolean') {
            return String(item)
          }

          return null
        })
        .filter((item): item is string => Boolean(item)),
    )
  }

  if (typeof value === 'string') {
    const parsed = parseWorkflowJson<unknown>(value)
    if (parsed !== null) {
      const parsedValues = toStringArray(parsed)
      if (parsedValues.length > 0) {
        return parsedValues
      }
    }

    return value.trim() ? [value.trim()] : []
  }

  return []
}

function dedupeStrings(values: string[]): string[] {
  return [...new Set(values.filter(value => value.trim().length > 0))]
}

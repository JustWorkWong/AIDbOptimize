export interface RagAssetStatus {
  factDocumentCount: number
  caseRecordCount: number
  chunkCount: number
  retrievalSnapshotCount: number
  latestDocumentIngestedAt: string | null
  latestCaseProjectedAt: string | null
  latestSnapshotCreatedAt: string | null
}

export interface RagCaseAuditItem {
  caseRecordId: string
  workflowSessionId: string
  engine: string
  problemType: string
  outcome: string
  reviewStatus: string
  recommendationType: string
  summary: string
  evidenceLinkCount: number
  createdAt: string
}

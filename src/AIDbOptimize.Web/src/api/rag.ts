import type { RagAssetStatus, RagCaseAuditItem } from '../models/rag'

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || `请求失败，状态码：${response.status}`)
  }

  return (await response.json()) as T
}

export function getRagAssetStatus(): Promise<RagAssetStatus> {
  return readJson<RagAssetStatus>('/api/rag/assets/status')
}

export function getRagCaseAudit(): Promise<RagCaseAuditItem[]> {
  return readJson<RagCaseAuditItem[]>('/api/rag/cases/audit')
}

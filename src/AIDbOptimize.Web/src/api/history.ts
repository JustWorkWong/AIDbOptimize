import type { WorkflowHistoryDetail, WorkflowSessionSummary } from '../models/workflow'

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || `请求失败，状态码：${response.status}`)
  }

  return (await response.json()) as T
}

export function getWorkflowHistory(): Promise<WorkflowSessionSummary[]> {
  return readJson<WorkflowSessionSummary[]>('/api/history')
}

export function getWorkflowHistoryDetail(sessionId: string): Promise<WorkflowHistoryDetail> {
  return readJson<WorkflowHistoryDetail>(`/api/history/${sessionId}`)
}

import type {
  DbConfigWorkflowRequest,
  WorkflowReplayEvent,
  WorkflowSessionDetail,
  WorkflowSessionSummary,
} from '../models/workflow'
import { knownWorkflowEventNames } from '../models/workflow'

type WorkflowEventName = typeof knownWorkflowEventNames[number]

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || `请求失败，状态码：${response.status}`)
  }

  return (await response.json()) as T
}

export function startDbConfigWorkflow(payload: DbConfigWorkflowRequest): Promise<WorkflowSessionDetail> {
  return readJson<WorkflowSessionDetail>('/api/workflows/db-config-optimization', {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify(payload),
  })
}

export function getWorkflowSessions(): Promise<WorkflowSessionSummary[]> {
  return readJson<WorkflowSessionSummary[]>('/api/workflows')
}

export function getWorkflowSession(sessionId: string): Promise<WorkflowSessionDetail> {
  return readJson<WorkflowSessionDetail>(`/api/workflows/${sessionId}`)
}

export function cancelWorkflowSession(sessionId: string): Promise<{ sessionId: string; cancelled: boolean; status: string }> {
  return readJson<{ sessionId: string; cancelled: boolean; status: string }>(`/api/workflows/${sessionId}/cancel`, {
    method: 'POST',
  })
}

export function subscribeWorkflowEvents(
  sessionId: string,
  handlers: {
    snapshot?: (payload: unknown) => void
    event?: (event: WorkflowReplayEvent) => void
    heartbeat?: (payload: unknown) => void
    error?: () => void
  },
): EventSource {
  const source = new EventSource(`/api/workflows/${sessionId}/events`)

  source.addEventListener('snapshot', (event) => {
    handlers.snapshot?.(JSON.parse((event as MessageEvent).data))
  })

  ;(knownWorkflowEventNames as readonly WorkflowEventName[]).forEach((eventName) => {
    source.addEventListener(eventName, (event) => {
      handlers.event?.(JSON.parse((event as MessageEvent).data) as WorkflowReplayEvent)
    })
  })

  source.addEventListener('heartbeat', (event) => {
    handlers.heartbeat?.(JSON.parse((event as MessageEvent).data))
  })

  source.onerror = () => {
    handlers.error?.()
  }

  return source
}

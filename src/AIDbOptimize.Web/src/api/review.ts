import type { ReviewTaskDetail, ReviewTaskSummary, WorkflowReviewSubmission } from '../models/workflow'

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    const error = await readErrorMessage(response)
    throw new Error(error || `请求失败，状态码：${response.status}`)
  }

  return (await response.json()) as T
}

async function readErrorMessage(response: Response): Promise<string> {
  const contentType = response.headers.get('Content-Type') ?? ''
  if (contentType.includes('application/json')) {
    const payload = (await response.json()) as { error?: string }
    return payload.error ?? JSON.stringify(payload)
  }

  return await response.text()
}

export function getReviewTasks(): Promise<ReviewTaskSummary[]> {
  return readJson<ReviewTaskSummary[]>('/api/reviews')
}

export function getReviewTask(taskId: string): Promise<ReviewTaskDetail> {
  return readJson<ReviewTaskDetail>(`/api/reviews/${taskId}`)
}

export function submitWorkflowReview(
  taskId: string,
  payload: WorkflowReviewSubmission,
): Promise<{ taskId: string; reviewStatus: string; workflowStatus: string; submittedAt: string }> {
  return readJson<{ taskId: string; reviewStatus: string; workflowStatus: string; submittedAt: string }>(
    `/api/reviews/${taskId}/submit`,
    {
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
      },
      body: JSON.stringify(payload),
    },
  )
}

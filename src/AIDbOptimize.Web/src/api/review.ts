import type { ReviewTaskDetail, ReviewTaskSummary, WorkflowReviewSubmission } from '../models/workflow'

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    const errorText = await response.text()
    throw new Error(errorText || `请求失败，状态码：${response.status}`)
  }

  return (await response.json()) as T
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

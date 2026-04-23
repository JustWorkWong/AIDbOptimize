import type {
  DataInitializationStatus,
  McpConnection,
  McpTool,
} from '../models/mcp'

export interface ToolExecutionResponse {
  executionId: string
  status: string
  responsePayloadJson: string
  errorMessage: string | null
}

async function readJson<T>(input: RequestInfo | URL, init?: RequestInit): Promise<T> {
  const response = await fetch(input, init)
  if (!response.ok) {
    throw new Error(`请求失败：${response.status}`)
  }

  return (await response.json()) as T
}

export function getMcpConnections(): Promise<McpConnection[]> {
  return readJson<McpConnection[]>('/api/mcp/connections')
}

export function discoverConnectionTools(connectionId: string): Promise<McpTool[]> {
  return readJson<McpTool[]>(`/api/mcp/connections/${connectionId}/discover-tools`, {
    method: 'POST',
  })
}

export function getConnectionTools(connectionId: string): Promise<McpTool[]> {
  return readJson<McpTool[]>(`/api/mcp/connections/${connectionId}/tools`)
}

export function updateToolApprovalMode(toolId: string, approvalMode: number): Promise<{ toolId: string; approvalMode: number }> {
  return readJson<{ toolId: string; approvalMode: number }>(`/api/mcp/tools/${toolId}/approval-mode`, {
    method: 'PUT',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ approvalMode }),
  })
}

export function getDataInitializationStatus(): Promise<DataInitializationStatus[]> {
  return readJson<DataInitializationStatus[]>('/api/data-init/status')
}

export function executeTool(toolId: string, payloadJson: string): Promise<ToolExecutionResponse> {
  return readJson<ToolExecutionResponse>(`/api/mcp/tools/${toolId}/execute`, {
    method: 'POST',
    headers: {
      'Content-Type': 'application/json',
    },
    body: JSON.stringify({ payloadJson }),
  })
}

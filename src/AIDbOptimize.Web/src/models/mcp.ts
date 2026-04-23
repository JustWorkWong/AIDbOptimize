export interface McpConnection {
  id: string
  name: string
  engine: number
  displayName: string
  databaseName: string
  isDefault: boolean
  status: number
  lastDiscoveredAt: string | null
}

export interface McpTool {
  id: string
  connectionId: string
  toolName: string
  displayName: string
  description: string | null
  approvalMode: number
  isEnabled: boolean
  isWriteTool: boolean
}

export interface DataInitializationStatus {
  engine: number
  databaseName: string
  seedVersion: string
  targetOrderCount: number
  state: number
  startedAt: string | null
  completedAt: string | null
  errorMessage: string | null
}

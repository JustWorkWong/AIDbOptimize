<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import {
  discoverConnectionTools,
  getConnectionTools,
  getDataInitializationStatus,
  getMcpConnections,
  updateToolApprovalMode,
} from './api/mcp'
import DataInitStatusPanel from './components/mcp/DataInitStatusPanel.vue'
import McpConnectionTable from './components/mcp/McpConnectionTable.vue'
import McpToolExecutor from './components/mcp/McpToolExecutor.vue'
import McpToolTable from './components/mcp/McpToolTable.vue'
import WorkflowWorkspace from './components/workflow/WorkflowWorkspace.vue'
import type {
  DataInitializationStatus,
  McpConnection,
  McpTool,
} from './models/mcp'

interface ServiceStatus {
  name: string
  connectionName: string
  isConfigured: boolean
  preview: string
}

interface InfrastructureOverview {
  environmentName: string
  services: ServiceStatus[]
}

interface ResourceLink {
  title: string
  description: string
  href: string
}

const activeView = ref<'overview' | 'mcp' | 'workflow'>('overview')
const loading = ref(true)
const errorMessage = ref('')
const overview = ref<InfrastructureOverview | null>(null)

const mcpLoading = ref(true)
const mcpErrorMessage = ref('')
const mcpConnections = ref<McpConnection[]>([])
const selectedConnectionId = ref('')
const toolsLoading = ref(false)
const tools = ref<McpTool[]>([])
const savingApprovalToolId = ref('')
const initStatusesLoading = ref(true)
const initStatuses = ref<DataInitializationStatus[]>([])

const resourceLinks = computed<ResourceLink[]>(() => [
  {
    title: 'API Health',
    description: 'Check whether the backend API is reachable.',
    href: '/health',
  },
  {
    title: 'Swagger',
    description: 'Inspect the public HTTP surface.',
    href: '/swagger',
  },
  {
    title: 'pgAdmin',
    description: 'Open the PostgreSQL management console.',
    href: import.meta.env.VITE_PGADMIN_URL ?? '#',
  },
  {
    title: 'phpMyAdmin',
    description: 'Open the MySQL management console.',
    href: import.meta.env.VITE_PHPMYADMIN_URL ?? '#',
  },
])

const injectedServiceCount = computed(() => {
  return overview.value?.services.filter((service) => service.isConfigured).length ?? 0
})

const selectedConnection = computed(() => {
  return mcpConnections.value.find((connection) => connection.id === selectedConnectionId.value) ?? null
})

onMounted(async () => {
  await Promise.all([
    loadInfrastructureOverview(),
    loadMcpConnections(),
    loadInitializationStatuses(),
  ])
})

async function loadInfrastructureOverview(): Promise<void> {
  try {
    const response = await fetch('/api/infrastructure')
    if (!response.ok) {
      throw new Error(`Request failed with status ${response.status}`)
    }

    overview.value = (await response.json()) as InfrastructureOverview
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : 'Failed to load overview.'
  } finally {
    loading.value = false
  }
}

async function loadMcpConnections(): Promise<void> {
  mcpErrorMessage.value = ''

  try {
    const connections = await getMcpConnections()
    mcpConnections.value = connections

    const selectedStillExists = connections.some((connection) => connection.id === selectedConnectionId.value)
    if (!selectedStillExists) {
      selectedConnectionId.value = connections[0]?.id ?? ''
    }

    if (!selectedConnectionId.value) {
      tools.value = []
      return
    }

    await loadTools(selectedConnectionId.value)
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : 'Failed to load MCP connections.'
  } finally {
    mcpLoading.value = false
  }
}

async function loadTools(connectionId: string): Promise<void> {
  toolsLoading.value = true
  mcpErrorMessage.value = ''

  try {
    tools.value = await getConnectionTools(connectionId)
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : 'Failed to load MCP tools.'
  } finally {
    toolsLoading.value = false
  }
}

async function handleDiscover(connectionId: string): Promise<void> {
  toolsLoading.value = true
  mcpErrorMessage.value = ''
  selectedConnectionId.value = connectionId

  try {
    tools.value = await discoverConnectionTools(connectionId)
    mcpConnections.value = await getMcpConnections()
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : 'Failed to discover MCP tools.'
  } finally {
    toolsLoading.value = false
  }
}

async function handleSelectConnection(connectionId: string): Promise<void> {
  selectedConnectionId.value = connectionId
  await loadTools(connectionId)
}

async function handleApprovalSave(toolId: string, approvalMode: number): Promise<void> {
  savingApprovalToolId.value = toolId
  mcpErrorMessage.value = ''

  try {
    await updateToolApprovalMode(toolId, approvalMode)
    const tool = tools.value.find((item) => item.id === toolId)
    if (tool) {
      tool.approvalMode = approvalMode
    }
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : 'Failed to save approval mode.'
  } finally {
    savingApprovalToolId.value = ''
  }
}

async function loadInitializationStatuses(): Promise<void> {
  try {
    initStatuses.value = await getDataInitializationStatus()
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : 'Failed to load initialization status.'
  } finally {
    initStatusesLoading.value = false
  }
}
</script>

<template>
  <main class="page-shell">
    <section class="hero-card">
      <p class="eyebrow">Aspire Control Plane</p>
      <h1>AIDbOptimize</h1>
      <p class="hero-copy">
        This workspace combines infrastructure overview, MCP administration, and the
        database-configuration optimization workflow.
      </p>

      <div class="hero-stats">
        <div class="hero-stat">
          <span>Web</span>
          <strong>17101</strong>
        </div>
        <div class="hero-stat">
          <span>API</span>
          <strong>17100</strong>
        </div>
        <div class="hero-stat">
          <span>Configured services</span>
          <strong>{{ injectedServiceCount }}/4</strong>
        </div>
      </div>
    </section>

    <section class="panel nav-panel">
      <div class="view-switch">
        <button
          type="button"
          :class="{ active: activeView === 'overview' }"
          @click="activeView = 'overview'"
        >
          Overview
        </button>
        <button
          type="button"
          :class="{ active: activeView === 'mcp' }"
          @click="activeView = 'mcp'"
        >
          MCP
        </button>
        <button
          type="button"
          :class="{ active: activeView === 'workflow' }"
          @click="activeView = 'workflow'"
        >
          Workflow
        </button>
      </div>
    </section>

    <template v-if="activeView === 'overview'">
      <section class="panel-grid">
        <article class="panel">
          <h2>Quick links</h2>
          <div class="link-grid">
            <a
              v-for="link in resourceLinks"
              :key="link.title"
              class="resource-link"
              :href="link.href"
              target="_blank"
              rel="noreferrer"
            >
              <strong>{{ link.title }}</strong>
              <span>{{ link.description }}</span>
            </a>
          </div>
        </article>

        <article class="panel">
          <h2>Connection status</h2>
          <p v-if="loading" class="state-text">Loading infrastructure overview...</p>
          <p v-else-if="errorMessage" class="state-text error">{{ errorMessage }}</p>
          <template v-else-if="overview">
            <p class="state-text">
              Environment: <strong>{{ overview.environmentName }}</strong>
            </p>

            <ul class="status-list">
              <li
                v-for="service in overview.services"
                :key="service.connectionName"
                class="status-item"
              >
                <div>
                  <strong>{{ service.name }}</strong>
                  <span>{{ service.connectionName }}</span>
                </div>
                <div class="status-meta">
                  <em :class="service.isConfigured ? 'ok' : 'fail'">
                    {{ service.isConfigured ? 'configured' : 'missing' }}
                  </em>
                  <code>{{ service.preview }}</code>
                </div>
              </li>
            </ul>
          </template>
        </article>
      </section>
    </template>

    <template v-else-if="activeView === 'mcp'">
      <p v-if="mcpErrorMessage" class="state-text error">{{ mcpErrorMessage }}</p>

      <section class="panel-grid">
        <McpConnectionTable
          :connections="mcpConnections"
          :loading="mcpLoading"
          :current-connection-id="selectedConnectionId"
          @select="handleSelectConnection"
          @discover="handleDiscover"
        />
        <DataInitStatusPanel
          :statuses="initStatuses"
          :loading="initStatusesLoading"
        />
      </section>

      <section class="panel-grid">
        <McpToolTable
          :tools="tools"
          :loading="toolsLoading"
          :saving-tool-id="savingApprovalToolId"
          @save-approval="handleApprovalSave"
        />
        <McpToolExecutor
          :tools="tools"
          :connection="selectedConnection"
        />
      </section>
    </template>

    <WorkflowWorkspace v-else />
  </main>
</template>

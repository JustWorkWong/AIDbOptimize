<script setup lang="ts">
import { computed, onMounted, ref } from 'vue'
import {
  discoverConnectionTools,
  getConnectionTools,
  getDataInitializationStatus,
  getMcpConnections,
  updateToolApprovalMode,
} from './api/mcp'
import type {
  DataInitializationStatus,
  McpConnection,
  McpTool,
} from './models/mcp'
import DataInitStatusPanel from './components/mcp/DataInitStatusPanel.vue'
import McpConnectionTable from './components/mcp/McpConnectionTable.vue'
import McpToolExecutor from './components/mcp/McpToolExecutor.vue'
import McpToolTable from './components/mcp/McpToolTable.vue'

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

interface ResourceCard {
  name: string
  purpose: string
  servicePort: string
  managementPort?: string
  managementLabel?: string
}

const activeView = ref<'overview' | 'mcp'>('overview')
const loading = ref(true)
const errorMessage = ref('')
const overview = ref<InfrastructureOverview | null>(null)

const mcpLoading = ref(true)
const mcpErrorMessage = ref('')
const mcpConnections = ref<McpConnection[]>([])
const selectedConnectionId = ref('')
const toolsLoading = ref(false)
const tools = ref<McpTool[]>([])
const initStatusesLoading = ref(true)
const initStatuses = ref<DataInitializationStatus[]>([])

const resourceLinks = computed<ResourceLink[]>(() => [
  {
    title: 'API 健康检查',
    description: '确认后端 API 是否已经成功启动。',
    href: '/health',
  },
  {
    title: 'Swagger',
    description: '查看当前后端公开接口。',
    href: '/swagger',
  },
  {
    title: 'pgAdmin',
    description: '打开 PostgreSQL 管理面板。',
    href: import.meta.env.VITE_PGADMIN_URL ?? '#',
  },
  {
    title: 'phpMyAdmin',
    description: '打开 MySQL 管理面板。',
    href: import.meta.env.VITE_PHPMYADMIN_URL ?? '#',
  },
  {
    title: 'Redis Insight',
    description: '打开 Redis 可视化管理面板。',
    href: import.meta.env.VITE_REDIS_INSIGHT_URL ?? '#',
  },
  {
    title: 'RabbitMQ Management',
    description: '打开 RabbitMQ 管理后台。',
    href: import.meta.env.VITE_RABBITMQ_URL ?? '#',
  },
])

const resourceCards = computed<ResourceCard[]>(() => [
  {
    name: 'PostgreSQL',
    purpose: '主关系型数据库，用于保存结构化业务数据。',
    servicePort: '15432',
    managementPort: '15050',
    managementLabel: 'pgAdmin',
  },
  {
    name: 'MySQL',
    purpose: '辅助关系型数据库，用于兼容 MySQL 场景联调。',
    servicePort: '13306',
    managementPort: '15051',
    managementLabel: 'phpMyAdmin',
  },
  {
    name: 'Redis',
    purpose: '缓存与临时状态存储。',
    servicePort: '16379',
    managementPort: '15540',
    managementLabel: 'Redis Insight',
  },
  {
    name: 'RabbitMQ',
    purpose: '消息中间件，用于异步任务和事件解耦。',
    servicePort: '15672',
    managementPort: '15673',
    managementLabel: 'RabbitMQ Management',
  },
])

const injectedServiceCount = computed(() => {
  return overview.value?.services.filter((service) => service.isConfigured).length ?? 0
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
      throw new Error(`接口返回状态码 ${response.status}`)
    }

    overview.value = (await response.json()) as InfrastructureOverview
  } catch (error) {
    errorMessage.value = error instanceof Error ? error.message : '加载失败'
  } finally {
    loading.value = false
  }
}

async function loadMcpConnections(): Promise<void> {
  try {
    mcpConnections.value = await getMcpConnections()
    selectedConnectionId.value = mcpConnections.value[0]?.id ?? ''

    if (selectedConnectionId.value) {
      await loadTools(selectedConnectionId.value)
    }
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : '加载 MCP 连接失败'
  } finally {
    mcpLoading.value = false
  }
}

async function loadTools(connectionId: string): Promise<void> {
  toolsLoading.value = true

  try {
    tools.value = await getConnectionTools(connectionId)
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : '加载工具失败'
  } finally {
    toolsLoading.value = false
  }
}

async function handleDiscover(connectionId: string): Promise<void> {
  toolsLoading.value = true

  try {
    tools.value = await discoverConnectionTools(connectionId)
    selectedConnectionId.value = connectionId
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : '获取工具失败'
  } finally {
    toolsLoading.value = false
  }
}

async function handleSelectConnection(connectionId: string): Promise<void> {
  selectedConnectionId.value = connectionId
  await loadTools(connectionId)
}

async function handleApprovalChange(toolId: string, approvalMode: number): Promise<void> {
  try {
    await updateToolApprovalMode(toolId, approvalMode)
    const tool = tools.value.find((item) => item.id === toolId)
    if (tool) {
      tool.approvalMode = approvalMode
    }
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : '更新审批模式失败'
  }
}

async function loadInitializationStatuses(): Promise<void> {
  try {
    initStatuses.value = await getDataInitializationStatus()
  } catch (error) {
    mcpErrorMessage.value = error instanceof Error ? error.message : '加载初始化状态失败'
  } finally {
    initStatusesLoading.value = false
  }
}
</script>

<template>
  <main class="page-shell">
    <section class="hero-card">
      <p class="eyebrow">Aspire 中文开发骨架</p>
      <h1>AIDbOptimize</h1>
      <p class="hero-copy">
        当前页面分成“资源概览”和“MCP 管理”两部分。
        资源概览用于确认基础设施是否已编排完成，MCP 管理用于查看默认连接、发现工具、调整审批模式和观察初始化状态。
      </p>

      <div class="hero-stats">
        <div class="hero-stat">
          <span>前端端口</span>
          <strong>17101</strong>
        </div>
        <div class="hero-stat">
          <span>API 端口</span>
          <strong>17100</strong>
        </div>
        <div class="hero-stat">
          <span>已注入资源</span>
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
          资源概览
        </button>
        <button
          type="button"
          :class="{ active: activeView === 'mcp' }"
          @click="activeView = 'mcp'"
        >
          MCP 管理
        </button>
      </div>
    </section>

    <template v-if="activeView === 'overview'">
      <section class="panel-grid">
        <article class="panel">
          <h2>使用提示</h2>
          <div class="tips-grid">
            <div class="tip-card">
              <strong>启动入口</strong>
              <span>通过 AppHost 启动整个系统，而不是单独启动前后端。</span>
            </div>
            <div class="tip-card">
              <strong>固定端口</strong>
              <span>数据库、缓存、消息队列和管理面板都使用配置文件中的固定端口。</span>
            </div>
            <div class="tip-card">
              <strong>默认账号</strong>
              <span>默认账号和密码统一写在 README 中，页面不直接显示密码。</span>
            </div>
          </div>
        </article>

        <article class="panel">
          <h2>管理入口</h2>
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
      </section>

      <section class="panel">
        <h2>资源说明</h2>
        <div class="resource-card-grid">
          <article
            v-for="resource in resourceCards"
            :key="resource.name"
            class="resource-card"
          >
            <h3>{{ resource.name }}</h3>
            <p>{{ resource.purpose }}</p>
            <dl class="resource-meta">
              <div>
                <dt>服务端口</dt>
                <dd>{{ resource.servicePort }}</dd>
              </div>
              <div v-if="resource.managementPort && resource.managementLabel">
                <dt>{{ resource.managementLabel }}</dt>
                <dd>{{ resource.managementPort }}</dd>
              </div>
            </dl>
          </article>
        </div>
      </section>

      <section class="panel">
        <h2>连接状态</h2>
        <p v-if="loading" class="state-text">正在读取 Aspire 注入的连接字符串...</p>
        <p v-else-if="errorMessage" class="state-text error">{{ errorMessage }}</p>
        <template v-else-if="overview">
          <p class="state-text">
            当前环境：<strong>{{ overview.environmentName }}</strong>
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
                  {{ service.isConfigured ? '已注入' : '未注入' }}
                </em>
                <code>{{ service.preview }}</code>
              </div>
            </li>
          </ul>
        </template>
      </section>
    </template>

    <template v-else>
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
          @change-approval="handleApprovalChange"
        />
        <McpToolExecutor :tools="tools" />
      </section>
    </template>
  </main>
</template>

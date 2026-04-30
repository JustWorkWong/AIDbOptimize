#!/usr/bin/env node

import os from 'node:os'
import path from 'node:path'
import { spawnSync } from 'node:child_process'

const tools = [
  {
    name: 'resolve_runtime_target',
    description: 'Resolve the runtime target for a db connection into container, host, or managed-service scope.',
    inputSchema: {
      type: 'object',
      properties: {
        connectionId: { type: 'string' },
        engine: { type: 'string' },
        displayName: { type: 'string' },
        databaseName: { type: 'string' },
        host: { type: 'string' },
        port: { anyOf: [{ type: 'integer' }, { type: 'string' }] },
        metadata: { type: 'object' }
      }
    }
  },
  {
    name: 'get_container_limits',
    description: 'Read container resource limits and mount metadata for a runtime target.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  },
  {
    name: 'get_container_stats',
    description: 'Read current container runtime stats for a runtime target.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  },
  {
    name: 'get_disk_usage',
    description: 'Read disk usage for the database target or a fallback host drive.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' },
        mode: { type: 'string' }
      }
    }
  },
  {
    name: 'get_host_memory',
    description: 'Read host memory totals and availability.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  },
  {
    name: 'get_host_cpu',
    description: 'Read host CPU core count and optional current usage.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  },
  {
    name: 'get_process_limits',
    description: 'Read process-level context when container or host process data is available.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  },
  {
    name: 'get_managed_service_profile',
    description: 'Return a managed-service placeholder profile when no real host visibility exists.',
    inputSchema: {
      type: 'object',
      properties: {
        targetId: { type: 'string' },
        targetName: { type: 'string' }
      }
    }
  }
]

let inputBuffer = Buffer.alloc(0)

process.stdin.on('data', chunk => {
  inputBuffer = Buffer.concat([inputBuffer, chunk])
  parseIncoming()
})

process.stdin.on('error', error => {
  writeStderr(`stdin error: ${error.message}`)
})

function parseIncoming() {
  while (true) {
    const separatorIndex = inputBuffer.indexOf('\r\n\r\n')
    if (separatorIndex < 0) {
      return
    }

    const headerText = inputBuffer.slice(0, separatorIndex).toString('utf8')
    const contentLength = parseContentLength(headerText)
    if (contentLength == null) {
      writeStderr('missing content-length header')
      inputBuffer = Buffer.alloc(0)
      return
    }

    const totalLength = separatorIndex + 4 + contentLength
    if (inputBuffer.length < totalLength) {
      return
    }

    const payloadText = inputBuffer.slice(separatorIndex + 4, totalLength).toString('utf8')
    inputBuffer = inputBuffer.slice(totalLength)

    try {
      const message = JSON.parse(payloadText)
      handleMessage(message)
    } catch (error) {
      writeStderr(`invalid json payload: ${error.message}`)
    }
  }
}

function parseContentLength(headerText) {
  for (const line of headerText.split('\r\n')) {
    const [name, value] = line.split(':')
    if (name && value && name.trim().toLowerCase() === 'content-length') {
      const parsed = Number.parseInt(value.trim(), 10)
      return Number.isFinite(parsed) ? parsed : null
    }
  }

  return null
}

function handleMessage(message) {
  if (typeof message?.method !== 'string') {
    return
  }

  if (message.method === 'notifications/initialized') {
    return
  }

  if (message.method === 'initialize') {
    sendResponse(message.id, {
      protocolVersion: message.params?.protocolVersion ?? '2025-03-26',
      capabilities: {
        tools: {
          listChanged: false
        }
      },
      serverInfo: {
        name: 'aidbopt-host-context-mcp',
        version: '0.1.0'
      }
    })
    return
  }

  if (message.method === 'ping') {
    sendResponse(message.id, {})
    return
  }

  if (message.method === 'tools/list') {
    sendResponse(message.id, {
      tools: tools.map(tool => ({
        name: tool.name,
        description: tool.description,
        inputSchema: tool.inputSchema
      }))
    })
    return
  }

  if (message.method === 'tools/call') {
    const toolName = message.params?.name
    const args = message.params?.arguments ?? {}

    if (typeof toolName !== 'string') {
      sendResponse(message.id, toolError('Tool name is required.'))
      return
    }

    try {
      const result = callTool(toolName, args)
      sendResponse(message.id, toolSuccess(result))
    } catch (error) {
      sendResponse(message.id, toolError(error instanceof Error ? error.message : String(error)))
    }

    return
  }

  sendError(message.id, -32601, `Method not found: ${message.method}`)
}

function sendResponse(id, result) {
  writeMessage({
    jsonrpc: '2.0',
    id,
    result
  })
}

function sendError(id, code, message) {
  writeMessage({
    jsonrpc: '2.0',
    id,
    error: {
      code,
      message
    }
  })
}

function writeMessage(message) {
  const body = JSON.stringify(message)
  const header = `Content-Length: ${Buffer.byteLength(body, 'utf8')}\r\n\r\n`
  process.stdout.write(header)
  process.stdout.write(body)
}

function toolSuccess(result) {
  const text = JSON.stringify(result)
  return {
    content: [
      {
        type: 'text',
        text
      }
    ],
    structuredContent: result,
    isError: false
  }
}

function toolError(message) {
  const payload = { error: message }
  return {
    content: [
      {
        type: 'text',
        text: JSON.stringify(payload)
      }
    ],
    structuredContent: payload,
    isError: true
  }
}

function callTool(toolName, args) {
  switch (toolName) {
    case 'resolve_runtime_target':
      return resolveRuntimeTarget(args)
    case 'get_container_limits':
      return getContainerLimits(args)
    case 'get_container_stats':
      return getContainerStats(args)
    case 'get_disk_usage':
      return getDiskUsage(args)
    case 'get_host_memory':
      return getHostMemory()
    case 'get_host_cpu':
      return getHostCpu()
    case 'get_process_limits':
      return getProcessLimits(args)
    case 'get_managed_service_profile':
      return getManagedServiceProfile(args)
    default:
      throw new Error(`Unsupported tool: ${toolName}`)
  }
}

function resolveRuntimeTarget(args) {
  const host = `${args?.host ?? ''}`.trim()
  const port = Number.parseInt(`${args?.port ?? ''}`, 10)
  const localHost = isLocalHost(host)

  if (localHost && Number.isFinite(port)) {
    const containers = listDockerContainers()
    const match = containers.find(container => container.hostPorts.includes(port))
    if (match) {
      return {
        resourceScope: 'container',
        targetType: 'docker-container',
        targetId: match.id,
        targetName: match.name,
        confidence: 'high',
        matchedHostPort: port
      }
    }

    return {
      resourceScope: 'host',
      targetType: 'host-process',
      confidence: 'low',
      reason: 'No matching docker port mapping found for the current host/port.'
    }
  }

  return {
    resourceScope: 'unknown',
    targetType: 'unknown',
    confidence: 'low',
    reason: localHost
      ? 'Connection port is missing or invalid.'
      : 'Connection host is not local, so container resolution is not available.'
  }
}

function getContainerLimits(args) {
  const container = inspectContainer(args)
  const hostConfig = container.HostConfig ?? {}
  const memoryLimitBytes = numberOrNull(hostConfig.Memory)
  const cpuLimitCores = getCpuLimitCores(hostConfig)
  const mounts = Array.isArray(container.Mounts)
    ? container.Mounts.map(mount => ({
        path: mount.Destination ?? null,
        source: mount.Source ?? null,
        name: mount.Name ?? null,
        type: mount.Type ?? null
      }))
    : []

  return {
    containerId: container.Id,
    containerName: trimContainerName(container.Name),
    memoryLimitBytes,
    cpuLimitCores,
    mounts,
    capturedAt: new Date().toISOString()
  }
}

function getContainerStats(args) {
  const target = resolveTargetName(args)
  const result = runCommand('docker', ['stats', target, '--no-stream', '--format', '{{json .}}'])
  const line = firstNonEmptyLine(result.stdout)
  if (!line) {
    throw new Error('docker stats returned no output.')
  }

  const stats = JSON.parse(line)
  const [usageBytes, limitBytes] = parseUsagePair(stats.MemUsage)
  const cpuUsagePercent = parsePercent(stats.CPUPerc)

  return {
    memoryUsageBytes: usageBytes,
    memoryAvailableBytes: usageBytes != null && limitBytes != null ? Math.max(limitBytes - usageBytes, 0) : null,
    cpuUsagePercent,
    capturedAt: new Date().toISOString()
  }
}

function getDiskUsage(args) {
  const target = resolveTargetName(args, false)
  let sourcePath = null

  if (target) {
    try {
      const container = inspectContainer(args)
      const mounts = Array.isArray(container.Mounts) ? container.Mounts : []
      const preferredMount = mounts.find(mount =>
        `${mount.Destination ?? ''}`.includes('/var/lib/mysql') ||
        `${mount.Destination ?? ''}`.includes('/var/lib/postgresql') ||
        `${mount.Destination ?? ''}`.includes('/var/lib/postgresql/data')) ?? mounts[0]
      sourcePath = preferredMount?.Source ?? null
    } catch {
      sourcePath = null
    }
  }

  const disk = readDiskStats(sourcePath)
  return {
    diskTotalBytes: disk.totalBytes,
    diskAvailableBytes: disk.freeBytes,
    sourcePath: sourcePath,
    fallbackDrive: disk.drive,
    capturedAt: new Date().toISOString()
  }
}

function getHostMemory() {
  return {
    memoryTotalBytes: os.totalmem(),
    memoryAvailableBytes: os.freemem(),
    capturedAt: new Date().toISOString()
  }
}

function getHostCpu() {
  return {
    cpuTotalCores: os.cpus().length,
    cpuUsagePercent: readHostCpuUsagePercent(),
    capturedAt: new Date().toISOString()
  }
}

function getProcessLimits(args) {
  return {
    targetName: resolveTargetName(args, false),
    processScope: 'host',
    capturedAt: new Date().toISOString()
  }
}

function getManagedServiceProfile(args) {
  return {
    resourceScope: 'managed-service',
    profileName: `${args?.targetName ?? 'managed-service'}-unknown`,
    reason: 'No managed-service provider integration is configured.',
    capturedAt: new Date().toISOString()
  }
}

function listDockerContainers() {
  const result = runCommand('docker', ['ps', '--format', '{{json .}}'])
  return result.stdout
    .split(/\r?\n/)
    .map(line => line.trim())
    .filter(Boolean)
    .map(line => JSON.parse(line))
    .map(container => ({
      id: container.ID,
      name: container.Names,
      hostPorts: parseDockerPorts(container.Ports)
    }))
}

function inspectContainer(args) {
  const target = resolveTargetName(args)
  const result = runCommand('docker', ['inspect', target])
  const containers = JSON.parse(result.stdout)
  if (!Array.isArray(containers) || containers.length === 0) {
    throw new Error(`docker inspect returned no container for ${target}`)
  }

  return containers[0]
}

function resolveTargetName(args, required = true) {
  const target = `${args?.targetId ?? args?.targetName ?? ''}`.trim()
  if (!target && required) {
    throw new Error('targetId or targetName is required.')
  }

  return target
}

function parseDockerPorts(portsText) {
  if (!portsText) {
    return []
  }

  const matches = [...`${portsText}`.matchAll(/:(\d+)->\d+\/tcp/g)]
  return matches
    .map(match => Number.parseInt(match[1], 10))
    .filter(Number.isFinite)
}

function parseUsagePair(text) {
  if (!text || !`${text}`.includes('/')) {
    return [null, null]
  }

  const [usageText, limitText] = `${text}`.split('/')
  return [parseSize(usageText), parseSize(limitText)]
}

function parseSize(text) {
  if (!text) {
    return null
  }

  const match = `${text}`.trim().match(/^([\d.]+)\s*([KMGTPE]?i?)?B?$/i)
  if (!match) {
    return null
  }

  const value = Number.parseFloat(match[1])
  if (!Number.isFinite(value)) {
    return null
  }

  const unit = (match[2] ?? '').toUpperCase()
  const scale = {
    '': 1,
    K: 1_000,
    M: 1_000_000,
    G: 1_000_000_000,
    T: 1_000_000_000_000,
    KI: 1024,
    MI: 1024 ** 2,
    GI: 1024 ** 3,
    TI: 1024 ** 4
  }[unit] ?? 1

  return Math.round(value * scale)
}

function parsePercent(text) {
  if (!text) {
    return null
  }

  const parsed = Number.parseFloat(`${text}`.replace('%', '').trim())
  return Number.isFinite(parsed) ? parsed : null
}

function readDiskStats(sourcePath) {
  const drive = inferDrive(sourcePath)
  if (process.platform === 'win32') {
    const deviceId = drive.replace(/\\$/, '')
    const script = `$disk = Get-CimInstance Win32_LogicalDisk -Filter \\\"DeviceID='${deviceId}'\\\"; if ($null -eq $disk) { Write-Output '{}' } else { $disk | Select-Object Size,FreeSpace | ConvertTo-Json -Compress }`
    const result = runCommand('powershell', ['-NoProfile', '-Command', script])
    const payload = JSON.parse(firstNonEmptyLine(result.stdout) || '{}')
    return {
      drive,
      totalBytes: numberOrNull(payload.Size),
      freeBytes: numberOrNull(payload.FreeSpace)
    }
  }

  return {
    drive,
    totalBytes: null,
    freeBytes: null
  }
}

function inferDrive(sourcePath) {
  if (sourcePath && /^[A-Za-z]:/.test(sourcePath)) {
    return path.parse(sourcePath).root
  }

  const workspaceRoot = path.parse(process.cwd()).root
  return workspaceRoot || `${process.env.SystemDrive ?? 'C:'}\\`
}

function readHostCpuUsagePercent() {
  if (process.platform !== 'win32') {
    return null
  }

  try {
    const script = "(Get-Counter '\\Processor(_Total)\\% Processor Time').CounterSamples[0].CookedValue | ConvertTo-Json -Compress"
    const result = runCommand('powershell', ['-NoProfile', '-Command', script], { allowFailure: true })
    const line = firstNonEmptyLine(result.stdout)
    if (!line) {
      return null
    }

    const parsed = JSON.parse(line)
    return typeof parsed === 'number' ? parsed : null
  } catch {
    return null
  }
}

function getCpuLimitCores(hostConfig) {
  const nanoCpus = numberOrNull(hostConfig.NanoCpus)
  if (nanoCpus && nanoCpus > 0) {
    return nanoCpus / 1_000_000_000
  }

  const quota = numberOrNull(hostConfig.CpuQuota)
  const period = numberOrNull(hostConfig.CpuPeriod)
  if (quota && period && quota > 0 && period > 0) {
    return quota / period
  }

  return null
}

function numberOrNull(value) {
  const numeric = Number(value)
  return Number.isFinite(numeric) && numeric > 0 ? numeric : null
}

function trimContainerName(name) {
  return `${name ?? ''}`.replace(/^\//, '')
}

function firstNonEmptyLine(text) {
  return `${text ?? ''}`
    .split(/\r?\n/)
    .map(line => line.trim())
    .find(Boolean) ?? null
}

function isLocalHost(host) {
  const value = `${host ?? ''}`.trim().toLowerCase()
  return value === 'localhost' || value === '127.0.0.1' || value === '::1' || value === '.'
}

function runCommand(command, args, options = {}) {
  const result = spawnSync(command, args, {
    encoding: 'utf8',
    timeout: options.timeoutMs ?? 5000,
    windowsHide: true
  })

  if (result.error) {
    throw result.error
  }

  if (result.status !== 0 && !options.allowFailure) {
    throw new Error(`${command} ${args.join(' ')} failed: ${(result.stderr || result.stdout || '').trim()}`)
  }

  return {
    stdout: result.stdout ?? '',
    stderr: result.stderr ?? '',
    status: result.status ?? 0
  }
}

function writeStderr(message) {
  process.stderr.write(`[aidbopt-host-context] ${message}\n`)
}

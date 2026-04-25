$ErrorActionPreference = 'Stop'

$version = if ($env:POSTGRESQL_MCP_VERSION) { $env:POSTGRESQL_MCP_VERSION } else { 'v0.3.0' }
$normalizedVersion = $version.TrimStart('v')
$fileName = "postgresql-mcp_${normalizedVersion}_windows_amd64.exe"
$cacheRoot = Join-Path $env:LOCALAPPDATA 'AIDbOptimize\tools\postgresql-mcp'
$binaryPath = Join-Path $cacheRoot $fileName
$downloadUrl = "https://github.com/sgaunet/postgresql-mcp/releases/download/$version/$fileName"

if (-not $env:POSTGRES_URL -and -not $env:DATABASE_URL) {
    throw 'POSTGRES_URL or DATABASE_URL is required.'
}

if (-not (Test-Path -LiteralPath $cacheRoot)) {
    New-Item -ItemType Directory -Path $cacheRoot -Force | Out-Null
}

if (-not (Test-Path -LiteralPath $binaryPath)) {
    Write-Host "[postgresql-mcp-launcher] downloading $downloadUrl"
    Invoke-WebRequest -Uri $downloadUrl -OutFile $binaryPath
}

& $binaryPath
exit $LASTEXITCODE

using System.Reflection;
using System.Text.RegularExpressions;
using AIDbOptimize.ApiService.Api;
using AIDbOptimize.ApiService.Configuration;
using AIDbOptimize.ApiService.DatabaseMigrations;
using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.DataInitialization.Services;
using AIDbOptimize.Application.Mcp.Services;
using AIDbOptimize.Application.Workflows.Services;
using AIDbOptimize.Infrastructure.Agents;
using AIDbOptimize.Infrastructure.Mcp;
using AIDbOptimize.Infrastructure.Observability;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Repositories;
using AIDbOptimize.Infrastructure.Security;
using AIDbOptimize.Infrastructure.Workflows.Pipeline;
using AIDbOptimize.Infrastructure.Workflows.Runtime;
using AIDbOptimize.Infrastructure.Workflows.Services;
using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Metrics;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);

// 这个最小 API 当前主要承担三类职责：
// 1. 公开健康检查与基础设施状态；
// 2. 公开控制面 MCP 管理接口；
// 3. 负责控制面数据库迁移和默认连接种子。
var controlPlaneConnectionString = ResolveConnectionString(
    builder.Configuration,
    "aidbopt-control",
    "ControlPlane",
    "Host=localhost;Port=15432;Username=postgres;Password=Postgres123!;Database=aidbopt_control");
controlPlaneConnectionString = ControlPlaneConnectionStringResolver.Resolve(
    builder.Configuration,
    "aidbopt-control",
    "ControlPlane",
    controlPlaneConnectionString);
var serviceName = builder.Environment.ApplicationName;
var serviceVersion = ResolveServiceVersion();
var diagnosisAgentOptions = builder.Configuration
    .GetSection("AIDbOptimize:Agent:Diagnosis")
    .Get<DbConfigDiagnosisAgentOptions>() ?? new DbConfigDiagnosisAgentOptions();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDataProtection();
builder.Services.AddSingleton(diagnosisAgentOptions);
builder.Services.AddOpenTelemetry()
    .ConfigureResource(resource =>
    {
        resource.AddService(
            serviceName: serviceName,
            serviceVersion: serviceVersion,
            serviceInstanceId: Environment.MachineName);
    })
    .WithTracing(tracing =>
    {
        tracing
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddEntityFrameworkCoreInstrumentation()
            .AddSource(serviceName)
            .AddSource(AIDbOptimizeTelemetry.WorkflowName)
            .AddSource(AIDbOptimizeTelemetry.AgentName)
            .AddSource(AIDbOptimizeTelemetry.ReviewName)
            .AddSource(AIDbOptimizeTelemetry.McpName)
            .AddOtlpExporter();
    })
    .WithMetrics(metrics =>
    {
        metrics
            .AddAspNetCoreInstrumentation()
            .AddHttpClientInstrumentation()
            .AddRuntimeInstrumentation()
            .AddProcessInstrumentation()
            .AddMeter(serviceName)
            .AddMeter(AIDbOptimizeTelemetry.WorkflowName)
            .AddMeter(AIDbOptimizeTelemetry.AgentName)
            .AddMeter(AIDbOptimizeTelemetry.ReviewName)
            .AddMeter(AIDbOptimizeTelemetry.McpName)
            .AddOtlpExporter();
    });

builder.Services.AddDbContextFactory<ControlPlaneDbContext>(options =>
    options.UseNpgsql(controlPlaneConnectionString));

builder.Services.AddHostedService<ControlPlaneMigrationHostedService>();
builder.Services.AddHostedService<ControlPlaneDefaultSeedHostedService>();

builder.Services.AddSingleton<McpClientFactory>();
builder.Services.AddSingleton<IConnectionSecretProtector, ConnectionSecretProtector>();
builder.Services.AddScoped<IMcpConnectionRepository, McpConnectionRepository>();
builder.Services.AddScoped<IMcpToolRepository, McpToolRepository>();
builder.Services.AddScoped<IDataInitializationRunRepository, DataInitializationRunRepository>();
builder.Services.AddScoped<IWorkflowSessionRepository, WorkflowSessionRepository>();
builder.Services.AddScoped<IWorkflowCheckpointRepository, WorkflowCheckpointRepository>();
builder.Services.AddScoped<IWorkflowReviewTaskRepository, WorkflowReviewTaskRepository>();
builder.Services.AddScoped<IWorkflowHistoryRepository, WorkflowHistoryRepository>();
builder.Services.AddScoped<IAgentSessionPersistenceService, AgentSessionPersistenceService>();
builder.Services.AddScoped<IAgentSummaryService, AgentSummaryService>();
builder.Services.AddScoped<DbConfigInputValidationExecutor>();
builder.Services.AddScoped<IDbConfigHostContextCollector, McpDbConfigHostContextCollector>();
builder.Services.AddScoped<IDbConfigRule, MySqlBufferPoolRule>();
builder.Services.AddScoped<IDbConfigRule, MySqlConnectionsRule>();
builder.Services.AddScoped<IDbConfigRule, MySqlThreadingRule>();
builder.Services.AddScoped<IDbConfigRule, MySqlTempTableRule>();
builder.Services.AddScoped<IDbConfigRule, MySqlSlowQueryRule>();
builder.Services.AddScoped<IDbConfigRule, MySqlObservabilityGapRule>();
builder.Services.AddScoped<IDbConfigRule, PostgreSqlSharedBuffersRule>();
builder.Services.AddScoped<IDbConfigRule, PostgreSqlCheckpointRule>();
builder.Services.AddScoped<IDbConfigRule, PostgreSqlPlannerCostRule>();
builder.Services.AddScoped<IDbConfigRule, PostgreSqlTempIoRule>();
builder.Services.AddScoped<IDbConfigRule, PostgreSqlObservabilityGapRule>();
builder.Services.AddScoped<DbConfigSnapshotCollectorExecutor>();
builder.Services.AddScoped<DbConfigRuleAnalysisExecutor>();
builder.Services.AddScoped<DbConfigDiagnosisReportBuilder>();
builder.Services.AddScoped<IDbConfigDiagnosisAgentExecutor, DbConfigDiagnosisAgentExecutor>();
builder.Services.AddScoped<RecommendationSchemaValidator>();
builder.Services.AddScoped<DbConfigGroundingExecutor>();
builder.Services.AddScoped<ReviewAdjustmentValidator>();
builder.Services.AddScoped<IWorkflowRuntime, ControlPlaneWorkflowRuntime>();
builder.Services.AddSingleton<IWorkflowExecutionRegistry, WorkflowExecutionRegistry>();
builder.Services.AddScoped<IMcpDiscoveryService, McpDiscoveryService>();
builder.Services.AddScoped<IMcpToolExecutionService, McpToolExecutionService>();
builder.Services.AddScoped<McpDiscoveryAppService>();
builder.Services.AddScoped<McpToolPermissionAppService>();
builder.Services.AddScoped<InitializationStatusAppService>();
builder.Services.AddScoped<IAgentToolAssemblyService, AIDbOptimize.Infrastructure.Mcp.AgentToolAssemblyService>();
builder.Services.AddScoped<IDbConfigOptimizationWorkflowService, DbConfigOptimizationWorkflowService>();
builder.Services.AddScoped<IWorkflowReviewService, WorkflowReviewService>();
builder.Services.AddScoped<IWorkflowHistoryService, WorkflowHistoryService>();
builder.Services.AddHostedService<WorkflowRecoveryHostedService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("frontend-dev", policy =>
    {
        policy
            .AllowAnyHeader()
            .AllowAnyMethod()
            .SetIsOriginAllowed(origin =>
            {
                if (!Uri.TryCreate(origin, UriKind.Absolute, out var uri))
                {
                    return false;
                }

                return uri.IsLoopback;
            });
    });
});

var app = builder.Build();

app.UseCors("frontend-dev");
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    options.RoutePrefix = "swagger";
});

app.MapGet("/health", () => Results.Ok(new
{
    status = "ok",
    service = "AIDbOptimize.ApiService",
    time = DateTimeOffset.UtcNow
}));

app.MapGet("/api/infrastructure", (IConfiguration configuration, IWebHostEnvironment environment) =>
{
    // 这里只返回“连接串是否已注入”和脱敏预览，
    // 避免把默认密码直接暴露给前端页面。
    var response = new InfrastructureOverviewResponse(
        EnvironmentName: environment.EnvironmentName,
        Services:
        [
            BuildServiceStatus("PostgreSQL", "postgresdb", configuration),
            BuildServiceStatus("MySQL", "mysqldb", configuration),
            BuildServiceStatus("Redis", "redis", configuration),
            BuildServiceStatus("RabbitMQ", "rabbitmq", configuration)
        ]);

    return Results.Ok(response);
})
.WithName("GetInfrastructureOverview")
.WithSummary("返回 Aspire 注入的基础设施连接状态。")
.WithDescription("用于前端首页展示当前示例项目的基础设施接入情况。");

app.MapMcpApi();
app.MapDataInitializationApi();
app.MapWorkflowsApi();
app.MapWorkflowEventsApi();
app.MapReviewsApi();
app.MapHistoryApi();

app.Run();

return;

static ServiceStatusResponse BuildServiceStatus(
    string displayName,
    string connectionStringName,
    IConfiguration configuration)
{
    var raw = configuration.GetConnectionString(connectionStringName);

    return new ServiceStatusResponse(
        Name: displayName,
        ConnectionName: connectionStringName,
        IsConfigured: !string.IsNullOrWhiteSpace(raw),
        Preview: MaskConnectionString(raw));
}

static string MaskConnectionString(string? value)
{
    if (string.IsNullOrWhiteSpace(value))
    {
        return "未注入";
    }

    var masked = Regex.Replace(
        value,
        "(?i)(password|pwd)=([^;]+)",
        "$1=***");

    masked = Regex.Replace(
        masked,
        "(?i)(amqp://[^:]+:)([^@]+)(@)",
        "$1***$3");

    return masked;
}

static string ResolveServiceVersion()
{
    var assembly = typeof(Program).Assembly;
    var informationalVersion = assembly
        .GetCustomAttribute<AssemblyInformationalVersionAttribute>()
        ?.InformationalVersion;

    return informationalVersion?.Split('+')[0]
        ?? assembly.GetName().Version?.ToString()
        ?? "1.0.0";
}

static string ResolveConnectionString(
    IConfiguration configuration,
    string aspireName,
    string fallbackName,
    string defaultValue)
{
    return ControlPlaneConnectionStringResolver.Resolve(
        configuration,
        aspireName,
        fallbackName,
        defaultValue);
}

internal sealed record InfrastructureOverviewResponse(
    string EnvironmentName,
    IReadOnlyList<ServiceStatusResponse> Services);

internal sealed record ServiceStatusResponse(
    string Name,
    string ConnectionName,
    bool IsConfigured,
    string Preview);

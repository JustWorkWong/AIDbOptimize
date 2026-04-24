using System.Text.RegularExpressions;
using AIDbOptimize.ApiService.Api;
using AIDbOptimize.ApiService.DatabaseMigrations;
using AIDbOptimize.Application.Abstractions.Agents;
using AIDbOptimize.Application.Abstractions.Mcp;
using AIDbOptimize.Application.Abstractions.Persistence;
using AIDbOptimize.Application.DataInitialization.Services;
using AIDbOptimize.Application.Mcp.Services;
using AIDbOptimize.Infrastructure.Mcp;
using AIDbOptimize.Infrastructure.Persistence;
using AIDbOptimize.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// 这个最小 API 当前主要承担三类职责：
// 1. 公开健康检查与基础设施状态；
// 2. 公开控制面 MCP 管理接口；
// 3. 负责控制面数据库迁移和默认连接种子。
var controlPlaneConnectionString = ResolveConnectionString(
    builder.Configuration,
    "aidbopt-control",
    "ControlPlane",
    "Host=localhost;Port=15432;Username=postgres;Password=Postgres123!;Database=aidbopt_control");

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextFactory<ControlPlaneDbContext>(options =>
    options.UseNpgsql(controlPlaneConnectionString));

builder.Services.AddHostedService<ControlPlaneMigrationHostedService>();
builder.Services.AddHostedService<ControlPlaneDefaultSeedHostedService>();

builder.Services.AddScoped<IMcpConnectionRepository, McpConnectionRepository>();
builder.Services.AddScoped<IMcpToolRepository, McpToolRepository>();
builder.Services.AddScoped<IDataInitializationRunRepository, DataInitializationRunRepository>();
builder.Services.AddScoped<IMcpDiscoveryService, McpDiscoveryService>();
builder.Services.AddScoped<IMcpToolExecutionService, McpToolExecutionService>();
builder.Services.AddScoped<McpDiscoveryAppService>();
builder.Services.AddScoped<McpToolPermissionAppService>();
builder.Services.AddScoped<InitializationStatusAppService>();
builder.Services.AddScoped<IAgentToolAssemblyService, AgentToolAssemblyService>();

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

app.Run();

return;

/// <summary>
/// 构造单个基础设施资源的状态展示模型。
/// </summary>
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

/// <summary>
/// 对连接串做轻量脱敏，避免把默认密码直接回传到前端。
/// </summary>
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

/// <summary>
/// 解析必填连接串。
/// </summary>
static string ResolveConnectionString(
    IConfiguration configuration,
    string aspireName,
    string fallbackName,
    string defaultValue)
{
    return configuration.GetConnectionString(aspireName)
        ?? configuration.GetConnectionString(fallbackName)
        ?? defaultValue;
}

internal sealed record InfrastructureOverviewResponse(
    string EnvironmentName,
    IReadOnlyList<ServiceStatusResponse> Services);

internal sealed record ServiceStatusResponse(
    string Name,
    string ConnectionName,
    bool IsConfigured,
    string Preview);

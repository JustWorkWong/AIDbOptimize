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

// 这个最小 API 只做三件事：
// 1. 提供健康检查，确认 Aspire 编排的 API 已成功启动；
// 2. 返回 Aspire 注入的基础设施连接状态，给首页做可视化展示；
// 3. 暴露 Swagger，方便后续继续扩展接口时直接调试。
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
    // Aspire 会把 WithReference 的结果注入成连接字符串。
    // 这里不直接连接数据库，只返回“是否已注入连接字符串”和脱敏预览，
    // 既能满足首页展示，也能避免把默认密码直接暴露给前端。
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

    // 这里做轻量级脱敏，避免把本地开发密码直接返回给前端页面。
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

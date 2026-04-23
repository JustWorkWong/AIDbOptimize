using AIDbOptimize.DataInit.Abstractions;
using AIDbOptimize.DataInit.HostedServices;
using AIDbOptimize.DataInit.Services;
using AIDbOptimize.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

var builder = Host.CreateApplicationBuilder(args);

// DataInit 以一次性后台作业的形式运行。
// 这里统一开启简单控制台日志，便于在 Aspire 输出中观察初始化进度。
builder.Services.AddLogging(logging =>
{
    logging.AddSimpleConsole(options =>
    {
        options.SingleLine = true;
        options.TimestampFormat = "yyyy-MM-dd HH:mm:ss ";
    });
});

// 当前先通过连接字符串完成最小接线。
// 后续随着 AppHost 集成增强，可以继续补充强类型配置绑定。
var controlPlaneConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-control",
    "ControlPlane");

var postgreSqlLabConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-lab-pg",
    "PostgreSqlLab");

var mySqlLabConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-lab-mysql",
    "MySqlLab");

builder.Services.AddDbContextFactory<ControlPlaneDbContext>(options =>
    options.UseNpgsql(controlPlaneConnection));

builder.Services.AddDbContextFactory<PostgreSqlLabDbContext>(options =>
    options.UseNpgsql(postgreSqlLabConnection));

builder.Services.AddDbContextFactory<MySqlLabDbContext>(options =>
    options.UseMySql(mySqlLabConnection, ServerVersion.AutoDetect(mySqlLabConnection)));

builder.Services.AddSingleton<InitializationStateService>();
builder.Services.AddSingleton<IDataInitializer, PostgreSqlLabInitializer>();
builder.Services.AddSingleton<IDataInitializer, MySqlLabInitializer>();
builder.Services.AddHostedService<DataInitializationHostedService>();

await builder.Build().RunAsync();
return;

/// <summary>
/// 从配置中解析必填连接串。
/// 优先读取 Aspire 注入的连接名称，再读取本地备用名称。
/// </summary>
static string GetRequiredConnectionString(
    IConfiguration configuration,
    string primaryName,
    string secondaryName)
{
    return configuration.GetConnectionString(primaryName)
        ?? configuration.GetConnectionString(secondaryName)
        ?? throw new InvalidOperationException(
            $"缺少连接串：{primaryName}/{secondaryName}。请先完成 AppHost 与 DataInit 的连接配置。");
}

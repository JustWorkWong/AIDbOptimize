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
// 优先读取 Aspire 注入连接，缺省时回落本地开发默认值，便于单独调试 DataInit。
var controlPlaneConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-control",
    "ControlPlane",
    "Host=127.0.0.1;Port=15432;Username=postgres;Password=Postgres123!;Database=aidbopt_control");

var postgreSqlLabConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-lab-pg",
    "PostgreSqlLab",
    "Host=127.0.0.1;Port=15432;Username=postgres;Password=Postgres123!;Database=aidbopt_lab_pg");

var mySqlLabConnection = GetRequiredConnectionString(
    builder.Configuration,
    "aidbopt-lab-mysql",
    "MySqlLab",
    "Server=127.0.0.1;Port=13306;User ID=root;Password=MySql123!;Database=aidbopt_lab_mysql");

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
/// 解析必填连接串。
/// 优先读取 Aspire 注入的连接名称，再读取本地备用名称，最后回落默认值。
/// </summary>
static string GetRequiredConnectionString(
    IConfiguration configuration,
    string primaryName,
    string secondaryName,
    string defaultValue)
{
    return configuration.GetConnectionString(primaryName)
        ?? configuration.GetConnectionString(secondaryName)
        ?? defaultValue;
}

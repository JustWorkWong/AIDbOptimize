using AIDbOptimize.AppHost;
using Aspire.Hosting.ApplicationModel;
using Aspire.Hosting.MySql;
using Aspire.Hosting.Postgres;
using Aspire.Hosting.Redis;
using System.Globalization;

var builder = DistributedApplication.CreateBuilder(args);

// 先统一读取并校验配置，避免资源创建到一半时才因为端口冲突失败。
var settings = AppHostConfiguration.Load(
    builder.Configuration,
    builder.AppHostDirectory,
    builder.Environment.EnvironmentName);

// 资源凭据统一声明为 Aspire 参数，后续容器和项目都通过参数引用，避免散落硬编码。
var postgresUser = builder.AddParameter("postgres-user", settings.Infrastructure.PostgreSql.Username);
var postgresPassword = builder.AddParameter("postgres-password", settings.Infrastructure.PostgreSql.Password, secret: true);
var mySqlPassword = builder.AddParameter("mysql-password", settings.Infrastructure.MySql.Password, secret: true);
var rabbitMqUser = builder.AddParameter("rabbitmq-user", settings.Infrastructure.RabbitMq.Username);
var rabbitMqPassword = builder.AddParameter("rabbitmq-password", settings.Infrastructure.RabbitMq.Password, secret: true);

// PostgreSQL：固定端口 + 数据卷 + pgAdmin。
var postgres = builder.AddPostgres(
        name: "postgresql",
        userName: postgresUser,
        password: postgresPassword,
        port: settings.Infrastructure.PostgreSql.Port)
    .WithImage("pgvector/pgvector")
    .WithImageTag("pg17")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPgAdmin(pgAdmin =>
    {
        pgAdmin.WithHostPort(settings.Infrastructure.PostgreSql.ManagementPort);
        pgAdmin.WithLifetime(ContainerLifetime.Persistent);
    });

var controlPlaneDb = postgres.AddDatabase("aidbopt-control", settings.Infrastructure.PostgreSql.ControlPlaneDatabase);
var postgreSqlLabDb = postgres.AddDatabase("aidbopt-lab-pg", settings.Infrastructure.PostgreSql.LabDatabase);

// MySQL：固定端口 + 数据卷 + phpMyAdmin。
var mySql = builder.AddMySql(
        name: "mysql",
        password: mySqlPassword,
        port: settings.Infrastructure.MySql.Port)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithPhpMyAdmin(phpMyAdmin =>
    {
        phpMyAdmin.WithHostPort(settings.Infrastructure.MySql.ManagementPort);
        phpMyAdmin.WithLifetime(ContainerLifetime.Persistent);
    });

var mySqlLabDb = mySql.AddDatabase("aidbopt-lab-mysql", settings.Infrastructure.MySql.LabDatabase);

// Redis：固定端口 + 数据卷 + Redis Insight。
var redis = builder.AddRedis("redis", settings.Infrastructure.Redis.Port)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithRedisInsight(redisInsight =>
    {
        redisInsight.WithHostPort(settings.Infrastructure.Redis.ManagementPort);
        redisInsight.WithLifetime(ContainerLifetime.Persistent);
    });

// RabbitMQ：固定 AMQP 端口 + 管理后台端口 + 数据卷。
var rabbitMq = builder.AddRabbitMQ(
        name: "rabbitmq",
        userName: rabbitMqUser,
        password: rabbitMqPassword,
        port: settings.Infrastructure.RabbitMq.Port)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume()
    .WithManagementPlugin(port: settings.Infrastructure.RabbitMq.ManagementPort);

// API 统一等待所有依赖就绪，并由 Aspire 自动注入连接字符串。
// 这里显式关闭项目自身的 launch profile，避免和 AppHost 中声明的固定端口冲突。
var api = builder.AddProject<Projects.AIDbOptimize_ApiService>("api", options =>
    {
        options.ExcludeLaunchProfile = true;
    })
    .WithHttpEndpoint(
        port: settings.Endpoints.ApiPort,
        name: "http")
    .WithExternalHttpEndpoints()
    .WaitFor(controlPlaneDb)
    .WaitFor(postgreSqlLabDb)
    .WaitFor(mySqlLabDb)
    .WaitFor(redis)
    .WaitFor(rabbitMq)
    .WithReference(controlPlaneDb)
    .WithReference(postgreSqlLabDb)
    .WithReference(mySqlLabDb)
    .WithReference(redis)
    .WithReference(rabbitMq);

// 数据初始化项目负责业务测试库的迁移与首轮种子初始化。
builder.AddProject<Projects.AIDbOptimize_DataInit>("data-init", options =>
    {
        options.ExcludeLaunchProfile = true;
    })
    .WaitFor(controlPlaneDb)
    .WaitFor(postgreSqlLabDb)
    .WaitFor(mySqlLabDb)
    .WithReference(controlPlaneDb)
    .WithReference(postgreSqlLabDb)
    .WithReference(mySqlLabDb);

// 前端也交给 Aspire 托管，并显式要求启动前执行 npm install。
// 这里使用 AddJavaScriptApp + WithRunScript，而不是 AddViteApp，
// 目的是把 Vite 的真实监听端口固定到配置值，而不是由框架分配随机端口。
builder.AddJavaScriptApp("web", "../AIDbOptimize.Web")
    .WithNpm(install: true, installCommand: "install")
    .WithRunScript(
        "dev",
        [
            "--",
            "--host",
            "0.0.0.0",
            "--port",
            settings.Endpoints.WebPort.ToString(CultureInfo.InvariantCulture)
        ])
    .WithHttpEndpoint(
        port: settings.Endpoints.WebPort,
        targetPort: settings.Endpoints.WebPort,
        name: "web-http",
        isProxied: false)
    .WithExternalHttpEndpoints()
    .WaitFor(api)
    .WithReference(api)
    .WithEnvironment("VITE_API_BASE_URL", api.GetEndpoint("http"))
    .WithEnvironment("VITE_WEB_PORT", settings.Endpoints.WebPort.ToString(CultureInfo.InvariantCulture))
    .WithEnvironment("VITE_PGADMIN_URL", BuildLocalUrl(settings.Infrastructure.PostgreSql.ManagementPort))
    .WithEnvironment("VITE_PHPMYADMIN_URL", BuildLocalUrl(settings.Infrastructure.MySql.ManagementPort))
    .WithEnvironment("VITE_REDIS_INSIGHT_URL", BuildLocalUrl(settings.Infrastructure.Redis.ManagementPort))
    .WithEnvironment("VITE_RABBITMQ_URL", BuildLocalUrl(settings.Infrastructure.RabbitMq.ManagementPort));

builder.Build().Run();

return;

static string BuildLocalUrl(int port)
{
    return $"https://localhost:{port.ToString(CultureInfo.InvariantCulture)}/";
}

using Microsoft.Extensions.Configuration;

namespace AIDbOptimize.AppHost;

/// <summary>
/// 统一处理 AppHost 配置加载与校验。
/// 这里把固定端口、数据库名和本地开发口令全部收敛到配置文件中，
/// 避免后续扩展时继续在编排代码中散落硬编码。
/// </summary>
public static class AppHostConfiguration
{
    /// <summary>
    /// 加载 AppHost 所需配置，并在启动初期完成必要校验。
    /// </summary>
    public static AppHostSettings Load(
        ConfigurationManager configuration,
        string appHostDirectory,
        string environmentName)
    {
        AddConfigurationSources(configuration, appHostDirectory, environmentName);

        var settings = configuration.GetRequiredSection("AIDbOptimize").Get<AppHostSettings>();
        if (settings is null)
        {
            throw new InvalidOperationException("缺少 AIDbOptimize 根配置节。");
        }

        ValidatePorts(settings);
        ValidateText(settings);

        return settings;
    }

    /// <summary>
    /// 按固定顺序加载配置源。
    /// Local 文件优先级更高，便于开发者覆盖本地私有配置。
    /// </summary>
    public static void AddConfigurationSources(
        ConfigurationManager configuration,
        string appHostDirectory,
        string environmentName)
    {
        ArgumentNullException.ThrowIfNull(configuration);

        if (string.IsNullOrWhiteSpace(appHostDirectory))
        {
            throw new ArgumentException("AppHost 目录不能为空。", nameof(appHostDirectory));
        }

        if (string.IsNullOrWhiteSpace(environmentName))
        {
            throw new ArgumentException("环境名称不能为空。", nameof(environmentName));
        }

        configuration.SetBasePath(appHostDirectory);
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        configuration.AddJsonFile($"appsettings.{environmentName}.json", optional: true, reloadOnChange: true);
        configuration.AddJsonFile("appsettings.Local.json", optional: true, reloadOnChange: true);
        configuration.AddEnvironmentVariables();
    }

    /// <summary>
    /// 校验端口号是否为正整数，且不同资源之间不存在重复端口。
    /// </summary>
    private static void ValidatePorts(AppHostSettings settings)
    {
        var ports = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase)
        {
            ["ApiPort"] = settings.Endpoints.ApiPort,
            ["WebPort"] = settings.Endpoints.WebPort,
            ["PostgreSqlPort"] = settings.Infrastructure.PostgreSql.Port,
            ["PostgreSqlManagementPort"] = settings.Infrastructure.PostgreSql.ManagementPort,
            ["MySqlPort"] = settings.Infrastructure.MySql.Port,
            ["MySqlManagementPort"] = settings.Infrastructure.MySql.ManagementPort,
            ["RedisPort"] = settings.Infrastructure.Redis.Port,
            ["RedisManagementPort"] = settings.Infrastructure.Redis.ManagementPort,
            ["RabbitMqPort"] = settings.Infrastructure.RabbitMq.Port,
            ["RabbitMqManagementPort"] = settings.Infrastructure.RabbitMq.ManagementPort
        };

        foreach (var (name, port) in ports)
        {
            if (port <= 0)
            {
                throw new InvalidOperationException($"配置项 {name} 必须是正整数端口。");
            }
        }

        var duplicatePorts = ports
            .GroupBy(entry => entry.Value)
            .Where(group => group.Count() > 1)
            .ToArray();

        if (duplicatePorts.Length == 0)
        {
            return;
        }

        var duplicates = string.Join(
            "; ",
            duplicatePorts.Select(group => $"{group.Key} => {string.Join(", ", group.Select(item => item.Key))}"));

        throw new InvalidOperationException($"检测到重复端口配置：{duplicates}");
    }

    /// <summary>
    /// 校验关键配置值不能为空。
    /// </summary>
    private static void ValidateText(AppHostSettings settings)
    {
        EnsureRequired(settings.Infrastructure.PostgreSql.ControlPlaneDatabase, "PostgreSql.ControlPlaneDatabase");
        EnsureRequired(settings.Infrastructure.PostgreSql.LabDatabase, "PostgreSql.LabDatabase");
        EnsureRequired(settings.Infrastructure.PostgreSql.Username, "PostgreSql.Username");
        EnsureRequired(settings.Infrastructure.PostgreSql.Password, "PostgreSql.Password");

        EnsureRequired(settings.Infrastructure.MySql.LabDatabase, "MySql.LabDatabase");
        EnsureRequired(settings.Infrastructure.MySql.Password, "MySql.Password");

        EnsureRequired(settings.Infrastructure.RabbitMq.Username, "RabbitMq.Username");
        EnsureRequired(settings.Infrastructure.RabbitMq.Password, "RabbitMq.Password");
    }

    /// <summary>
    /// 统一处理字符串必填校验。
    /// </summary>
    private static void EnsureRequired(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"缺少必填配置：AIDbOptimize:Infrastructure:{key}");
        }
    }
}

/// <summary>
/// AppHost 顶层配置对象。
/// </summary>
public sealed class AppHostSettings
{
    /// <summary>
    /// 应用端点配置。
    /// </summary>
    public EndpointSettings Endpoints { get; init; } = new();

    /// <summary>
    /// 基础设施配置。
    /// </summary>
    public InfrastructureSettings Infrastructure { get; init; } = new();
}

/// <summary>
/// 需要直接暴露到宿主机的应用端口配置。
/// </summary>
public sealed class EndpointSettings
{
    /// <summary>
    /// API 固定端口。
    /// </summary>
    public int ApiPort { get; init; }

    /// <summary>
    /// Web 固定端口。
    /// </summary>
    public int WebPort { get; init; }
}

/// <summary>
/// 所有基础设施资源的配置集合。
/// </summary>
public sealed class InfrastructureSettings
{
    /// <summary>
    /// PostgreSQL 配置。
    /// </summary>
    public PostgreSqlSettings PostgreSql { get; init; } = new();

    /// <summary>
    /// MySQL 配置。
    /// </summary>
    public MySqlSettings MySql { get; init; } = new();

    /// <summary>
    /// Redis 配置。
    /// </summary>
    public RedisSettings Redis { get; init; } = new();

    /// <summary>
    /// RabbitMQ 配置。
    /// </summary>
    public RabbitMqSettings RabbitMq { get; init; } = new();
}

/// <summary>
/// PostgreSQL 资源配置。
/// </summary>
public sealed class PostgreSqlSettings
{
    /// <summary>
    /// PostgreSQL 对外服务端口。
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// pgAdmin 对外管理端口。
    /// </summary>
    public int ManagementPort { get; init; }

    /// <summary>
    /// 控制面数据库名。
    /// </summary>
    public string ControlPlaneDatabase { get; init; } = string.Empty;

    /// <summary>
    /// 业务测试库数据库名。
    /// </summary>
    public string LabDatabase { get; init; } = string.Empty;

    /// <summary>
    /// PostgreSQL 用户名。
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// PostgreSQL 密码。
    /// </summary>
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// MySQL 资源配置。
/// </summary>
public sealed class MySqlSettings
{
    /// <summary>
    /// MySQL 对外服务端口。
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// phpMyAdmin 对外管理端口。
    /// </summary>
    public int ManagementPort { get; init; }

    /// <summary>
    /// 业务测试库数据库名。
    /// </summary>
    public string LabDatabase { get; init; } = string.Empty;

    /// <summary>
    /// MySQL 密码。
    /// </summary>
    public string Password { get; init; } = string.Empty;
}

/// <summary>
/// Redis 资源配置。
/// </summary>
public sealed class RedisSettings
{
    /// <summary>
    /// Redis 对外服务端口。
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// Redis Insight 对外管理端口。
    /// </summary>
    public int ManagementPort { get; init; }
}

/// <summary>
/// RabbitMQ 资源配置。
/// </summary>
public sealed class RabbitMqSettings
{
    /// <summary>
    /// RabbitMQ 对外服务端口。
    /// </summary>
    public int Port { get; init; }

    /// <summary>
    /// RabbitMQ 管理后台对外端口。
    /// </summary>
    public int ManagementPort { get; init; }

    /// <summary>
    /// RabbitMQ 用户名。
    /// </summary>
    public string Username { get; init; } = string.Empty;

    /// <summary>
    /// RabbitMQ 密码。
    /// </summary>
    public string Password { get; init; } = string.Empty;
}

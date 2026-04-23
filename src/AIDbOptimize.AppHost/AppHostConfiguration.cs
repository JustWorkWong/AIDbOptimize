using Microsoft.Extensions.Configuration;

namespace AIDbOptimize.AppHost;

/// <summary>
/// 统一处理 AppHost 配置加载与校验。
/// 这里把固定端口、本地开发密码、数据库名等信息全部放进配置文件，
/// 后续扩展时只需要改配置，不需要在 AppHost 逻辑里到处找硬编码。
/// </summary>
public static class AppHostConfiguration
{
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
    /// 加载标准配置源。
    /// 这里让 Local 文件覆盖公共配置，便于保留一份可提交的默认配置，
    /// 同时又允许每位开发者在本地做个性化调整。
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

    private static void ValidateText(AppHostSettings settings)
    {
        EnsureRequired(settings.Infrastructure.PostgreSql.Database, "PostgreSql.Database");
        EnsureRequired(settings.Infrastructure.PostgreSql.Username, "PostgreSql.Username");
        EnsureRequired(settings.Infrastructure.PostgreSql.Password, "PostgreSql.Password");

        EnsureRequired(settings.Infrastructure.MySql.Database, "MySql.Database");
        EnsureRequired(settings.Infrastructure.MySql.Password, "MySql.Password");

        EnsureRequired(settings.Infrastructure.RabbitMq.Username, "RabbitMq.Username");
        EnsureRequired(settings.Infrastructure.RabbitMq.Password, "RabbitMq.Password");
    }

    private static void EnsureRequired(string? value, string key)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new InvalidOperationException($"缺少必填配置：AIDbOptimize:Infrastructure:{key}");
        }
    }
}

/// <summary>
/// AppHost 顶层配置。
/// </summary>
public sealed class AppHostSettings
{
    public EndpointSettings Endpoints { get; init; } = new();

    public InfrastructureSettings Infrastructure { get; init; } = new();
}

/// <summary>
/// 需要直接暴露到宿主机的应用端口。
/// </summary>
public sealed class EndpointSettings
{
    public int ApiPort { get; init; }

    public int WebPort { get; init; }
}

/// <summary>
/// 所有基础设施资源的固定端口与初始化参数。
/// </summary>
public sealed class InfrastructureSettings
{
    public PostgreSqlSettings PostgreSql { get; init; } = new();

    public MySqlSettings MySql { get; init; } = new();

    public RedisSettings Redis { get; init; } = new();

    public RabbitMqSettings RabbitMq { get; init; } = new();
}

public sealed class PostgreSqlSettings
{
    public int Port { get; init; }

    public int ManagementPort { get; init; }

    public string Database { get; init; } = string.Empty;

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

public sealed class MySqlSettings
{
    public int Port { get; init; }

    public int ManagementPort { get; init; }

    public string Database { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

public sealed class RedisSettings
{
    public int Port { get; init; }

    public int ManagementPort { get; init; }
}

public sealed class RabbitMqSettings
{
    public int Port { get; init; }

    public int ManagementPort { get; init; }

    public string Username { get; init; } = string.Empty;

    public string Password { get; init; } = string.Empty;
}

namespace AIDbOptimize.DataInit.Options;

/// <summary>
/// DataInit 的最小可配置选项。
/// 当前先覆盖种子版本、目标订单量和错误处理策略。
/// </summary>
public sealed class DataInitializationOptions
{
    public const string SectionName = "DataInitialization";

    /// <summary>
    /// 当前种子版本。
    /// 用于控制幂等判断和后续版本升级。
    /// </summary>
    public string SeedVersion { get; set; } = "v1";

    /// <summary>
    /// 目标订单数量。
    /// 当前按详细设计约定为 1000 万级。
    /// </summary>
    public long TargetOrderCount { get; set; } = 10_000_000;

    /// <summary>
    /// PostgreSQL 业务测试库名称。
    /// </summary>
    public string PostgreSqlDatabaseName { get; set; } = "aidbopt_lab_pg";

    /// <summary>
    /// MySQL 业务测试库名称。
    /// </summary>
    public string MySqlDatabaseName { get; set; } = "aidbopt_lab_mysql";

    /// <summary>
    /// 初始化器失败后是否继续执行剩余初始化器。
    /// 当前默认关闭，优先保证失败可见性。
    /// </summary>
    public bool ContinueOnInitializerError { get; set; }
}

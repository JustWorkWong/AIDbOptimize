using AIDbOptimize.Domain.Mcp.Enums;
using AIDbOptimize.Domain.Seed.Enums;

namespace AIDbOptimize.Infrastructure.Persistence.Entities;

/// <summary>
/// 数据初始化运行状态实体。
/// </summary>
public sealed class DataInitializationRunEntity
{
    /// <summary>
    /// 运行记录主键。
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// 目标数据库引擎。
    /// </summary>
    public DatabaseEngine Engine { get; set; }

    /// <summary>
    /// 目标数据库名称。
    /// </summary>
    public string DatabaseName { get; set; } = string.Empty;

    /// <summary>
    /// 种子版本号。
    /// </summary>
    public string SeedVersion { get; set; } = string.Empty;

    /// <summary>
    /// 目标订单数量。
    /// </summary>
    public long TargetOrderCount { get; set; }

    /// <summary>
    /// 当前初始化状态。
    /// </summary>
    public DataInitializationState State { get; set; }

    /// <summary>
    /// 开始时间。
    /// </summary>
    public DateTimeOffset? StartedAt { get; set; }

    /// <summary>
    /// 完成时间。
    /// </summary>
    public DateTimeOffset? CompletedAt { get; set; }

    /// <summary>
    /// 错误信息。
    /// </summary>
    public string? ErrorMessage { get; set; }
}

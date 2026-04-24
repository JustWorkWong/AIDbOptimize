using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.DataInit.Abstractions;

/// <summary>
/// 统一约束不同数据库引擎的数据初始化器。
/// HostedService 负责编排状态，具体初始化器只负责本引擎的迁移与种子执行。
/// </summary>
public interface IDataInitializer
{
    /// <summary>
    /// 当前初始化器对应的数据库引擎。
    /// </summary>
    DatabaseEngine Engine { get; }

    /// <summary>
    /// 当前初始化器对应的业务数据库名称。
    /// </summary>
    string DatabaseName { get; }

    /// <summary>
    /// 当前种子版本。
    /// </summary>
    string SeedVersion { get; }

    /// <summary>
    /// 目标订单数量。
    /// </summary>
    long TargetOrderCount { get; }

    /// <summary>
    /// 执行初始化。
    /// 返回 true 表示“迁移和种子均已完成”；
    /// 返回 false 表示“当前只完成了迁移准备，真实种子尚未执行”。
    /// </summary>
    Task<bool> InitializeAsync(CancellationToken cancellationToken = default);
}

using AIDbOptimize.Domain.Mcp.Enums;

namespace AIDbOptimize.DataInit.Abstractions;

/// <summary>
/// 统一约束不同数据库引擎的数据初始化器。
/// HostedService 负责状态编排，具体初始化器只负责本引擎的迁移和造数入口。
/// </summary>
public interface IDataInitializer
{
    DatabaseEngine Engine { get; }

    string DatabaseName { get; }

    string SeedVersion { get; }

    long TargetOrderCount { get; }

    Task InitializeAsync(CancellationToken cancellationToken = default);
}

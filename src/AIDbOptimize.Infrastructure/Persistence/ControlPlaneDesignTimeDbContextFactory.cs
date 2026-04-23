using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// 控制面数据库设计时工厂。
/// 优先读取环境变量，缺省时回落到本地开发默认连接，便于直接生成迁移。
/// </summary>
public sealed class ControlPlaneDesignTimeDbContextFactory : IDesignTimeDbContextFactory<ControlPlaneDbContext>
{
    public ControlPlaneDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AIDBOPT_CONTROL_CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__aidbopt_control")
            ?? "Host=127.0.0.1;Port=15432;Database=aidbopt_control;Username=postgres;Password=Postgres123!";

        var optionsBuilder = new DbContextOptionsBuilder<ControlPlaneDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new ControlPlaneDbContext(optionsBuilder.Options);
    }
}

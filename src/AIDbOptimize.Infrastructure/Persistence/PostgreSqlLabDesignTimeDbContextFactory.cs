using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// PostgreSQL 业务测试库设计时工厂。
/// 优先读取环境变量，缺省时使用本地开发默认连接。
/// </summary>
public sealed class PostgreSqlLabDesignTimeDbContextFactory : IDesignTimeDbContextFactory<PostgreSqlLabDbContext>
{
    public PostgreSqlLabDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AIDBOPT_PG_LAB_CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__aidbopt_lab_pg")
            ?? "Host=127.0.0.1;Port=15432;Database=aidbopt_lab_pg;Username=postgres;Password=Postgres123!";

        var optionsBuilder = new DbContextOptionsBuilder<PostgreSqlLabDbContext>();
        optionsBuilder.UseNpgsql(connectionString);
        return new PostgreSqlLabDbContext(optionsBuilder.Options);
    }
}

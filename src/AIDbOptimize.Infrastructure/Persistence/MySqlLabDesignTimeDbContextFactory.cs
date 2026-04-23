using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// MySQL 业务测试库设计时工厂。
/// 优先读取环境变量，缺省时使用本地开发默认连接。
/// </summary>
public sealed class MySqlLabDesignTimeDbContextFactory : IDesignTimeDbContextFactory<MySqlLabDbContext>
{
    public MySqlLabDbContext CreateDbContext(string[] args)
    {
        var connectionString =
            Environment.GetEnvironmentVariable("AIDBOPT_MYSQL_LAB_CONNECTION_STRING")
            ?? Environment.GetEnvironmentVariable("ConnectionStrings__aidbopt_lab_mysql")
            ?? "Server=127.0.0.1;Port=13306;User ID=root;Password=MySql123!;Database=aidbopt_lab_mysql";

        var optionsBuilder = new DbContextOptionsBuilder<MySqlLabDbContext>();
        optionsBuilder.UseMySql(connectionString, new MySqlServerVersion(new Version(8, 0, 36)));
        return new MySqlLabDbContext(optionsBuilder.Options);
    }
}

using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// MySQL 业务测试库上下文。
/// 与 PostgreSQL 使用相同的订单结构。
/// </summary>
public sealed class MySqlLabDbContext(DbContextOptions<MySqlLabDbContext> options) : DbContext(options)
{
    /// <summary>
    /// 订单表读取模型集合。
    /// </summary>
    public DbSet<OrderReadModel> Orders => Set<OrderReadModel>();

    /// <summary>
    /// 订单明细表读取模型集合。
    /// </summary>
    public DbSet<OrderItemReadModel> OrderItems => Set<OrderItemReadModel>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<OrderReadModel>();
        order.ToTable("orders");
        order.HasKey(x => x.Id);
        order.Property(x => x.OrderNumber).HasMaxLength(50).IsRequired();
        order.Property(x => x.Status).HasMaxLength(20).IsRequired();
        order.Property(x => x.TotalAmount).HasPrecision(18, 2);
        order.HasIndex(x => x.OrderNumber).IsUnique();
        order.HasIndex(x => x.CustomerId);
        order.HasIndex(x => x.Status);
        order.HasIndex(x => x.CreatedAt);

        var item = modelBuilder.Entity<OrderItemReadModel>();
        item.ToTable("order_items");
        item.HasKey(x => x.Id);
        item.Property(x => x.ProductName).HasMaxLength(200).IsRequired();
        item.Property(x => x.UnitPrice).HasPrecision(18, 2);
        item.Property(x => x.LineAmount).HasPrecision(18, 2);
        item.HasIndex(x => x.OrderId);
        item.HasIndex(x => x.ProductId);

        item.HasOne<OrderReadModel>()
            .WithMany(x => x.Items)
            .HasForeignKey(x => x.OrderId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}

using Microsoft.EntityFrameworkCore;

namespace AIDbOptimize.Infrastructure.Persistence;

/// <summary>
/// PostgreSQL 业务测试库上下文。
/// 当前只承载订单与订单明细的物理映射。
/// </summary>
public sealed class PostgreSqlLabDbContext(DbContextOptions<PostgreSqlLabDbContext> options) : DbContext(options)
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
        ConfigureOrderTables(modelBuilder);
    }

    /// <summary>
    /// 配置订单与订单明细的物理表结构。
    /// </summary>
    private static void ConfigureOrderTables(ModelBuilder modelBuilder)
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

/// <summary>
/// 订单读取模型。
/// 当前主要服务于 EF Core 建表、迁移和批量初始化。
/// </summary>
public sealed class OrderReadModel
{
    /// <summary>
    /// 订单主键。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 订单号。
    /// </summary>
    public string OrderNumber { get; set; } = string.Empty;

    /// <summary>
    /// 客户编号。
    /// </summary>
    public long CustomerId { get; set; }

    /// <summary>
    /// 订单状态文本。
    /// </summary>
    public string Status { get; set; } = string.Empty;

    /// <summary>
    /// 订单总金额。
    /// </summary>
    public decimal TotalAmount { get; set; }

    /// <summary>
    /// 订单创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }

    /// <summary>
    /// 订单更新时间。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; set; }

    /// <summary>
    /// 订单明细集合。
    /// </summary>
    public ICollection<OrderItemReadModel> Items { get; set; } = [];
}

/// <summary>
/// 订单明细读取模型。
/// </summary>
public sealed class OrderItemReadModel
{
    /// <summary>
    /// 明细主键。
    /// </summary>
    public long Id { get; set; }

    /// <summary>
    /// 所属订单主键。
    /// </summary>
    public long OrderId { get; set; }

    /// <summary>
    /// 商品编号。
    /// </summary>
    public long ProductId { get; set; }

    /// <summary>
    /// 商品名称。
    /// </summary>
    public string ProductName { get; set; } = string.Empty;

    /// <summary>
    /// 商品数量。
    /// </summary>
    public int Quantity { get; set; }

    /// <summary>
    /// 商品单价。
    /// </summary>
    public decimal UnitPrice { get; set; }

    /// <summary>
    /// 当前明细的小计金额。
    /// </summary>
    public decimal LineAmount { get; set; }

    /// <summary>
    /// 明细创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; set; }
}

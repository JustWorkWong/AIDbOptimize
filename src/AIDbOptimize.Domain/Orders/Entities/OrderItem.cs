using AIDbOptimize.Domain.Common;
using AIDbOptimize.Domain.Orders.ValueObjects;

namespace AIDbOptimize.Domain.Orders.Entities;

/// <summary>
/// 订单明细实体。
/// </summary>
public sealed class OrderItem
{
    private OrderItem(
        long id,
        long productId,
        string productName,
        int quantity,
        Money unitPrice,
        DateTimeOffset createdAt)
    {
        Id = id;
        ProductId = productId;
        ProductName = productName;
        Quantity = quantity;
        UnitPrice = unitPrice;
        LineAmount = unitPrice * quantity;
        CreatedAt = createdAt;
    }

    /// <summary>
    /// 明细主键。
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// 商品编号。
    /// </summary>
    public long ProductId { get; private set; }

    /// <summary>
    /// 商品名称。
    /// </summary>
    public string ProductName { get; private set; }

    /// <summary>
    /// 商品数量。
    /// </summary>
    public int Quantity { get; private set; }

    /// <summary>
    /// 商品单价。
    /// </summary>
    public Money UnitPrice { get; private set; }

    /// <summary>
    /// 当前明细的小计金额。
    /// </summary>
    public Money LineAmount { get; private set; }

    /// <summary>
    /// 明细创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// 创建合法的订单明细实体。
    /// </summary>
    public static OrderItem Create(
        long productId,
        string productName,
        int quantity,
        Money unitPrice,
        DateTimeOffset createdAt)
    {
        if (productId <= 0)
        {
            throw new DomainException("商品编号必须大于 0。");
        }

        if (string.IsNullOrWhiteSpace(productName))
        {
            throw new DomainException("商品名称不能为空。");
        }

        if (quantity <= 0)
        {
            throw new DomainException("商品数量必须大于 0。");
        }

        return new OrderItem(
            id: 0,
            productId: productId,
            productName: productName.Trim(),
            quantity: quantity,
            unitPrice: unitPrice,
            createdAt: createdAt);
    }
}

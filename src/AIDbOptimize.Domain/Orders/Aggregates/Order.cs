using AIDbOptimize.Domain.Common;
using AIDbOptimize.Domain.Orders.Entities;
using AIDbOptimize.Domain.Orders.Enums;
using AIDbOptimize.Domain.Orders.ValueObjects;

namespace AIDbOptimize.Domain.Orders.Aggregates;

/// <summary>
/// 订单聚合根。
/// 所有明细修改和总金额汇总都应通过该聚合完成。
/// </summary>
public sealed class Order
{
    private readonly List<OrderItem> _items = [];

    private Order(
        long id,
        OrderNumber orderNumber,
        long customerId,
        OrderStatus status,
        DateTimeOffset createdAt)
    {
        Id = id;
        OrderNumber = orderNumber;
        CustomerId = customerId;
        Status = status;
        CreatedAt = createdAt;
        UpdatedAt = createdAt;
        TotalAmount = Money.Zero;
    }

    /// <summary>
    /// 订单主键。
    /// </summary>
    public long Id { get; private set; }

    /// <summary>
    /// 订单号值对象。
    /// </summary>
    public OrderNumber OrderNumber { get; private set; }

    /// <summary>
    /// 客户编号。
    /// </summary>
    public long CustomerId { get; private set; }

    /// <summary>
    /// 当前订单状态。
    /// </summary>
    public OrderStatus Status { get; private set; }

    /// <summary>
    /// 当前订单总金额。
    /// </summary>
    public Money TotalAmount { get; private set; }

    /// <summary>
    /// 订单创建时间。
    /// </summary>
    public DateTimeOffset CreatedAt { get; private set; }

    /// <summary>
    /// 订单最后更新时间。
    /// </summary>
    public DateTimeOffset UpdatedAt { get; private set; }

    /// <summary>
    /// 订单明细集合，只允许通过聚合行为修改。
    /// </summary>
    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    /// <summary>
    /// 创建一个新的订单聚合根。
    /// </summary>
    public static Order Create(OrderNumber orderNumber, long customerId, DateTimeOffset createdAt)
    {
        if (customerId <= 0)
        {
            throw new DomainException("客户编号必须大于 0。");
        }

        return new Order(
            id: 0,
            orderNumber: orderNumber,
            customerId: customerId,
            status: OrderStatus.Pending,
            createdAt: createdAt);
    }

    /// <summary>
    /// 向订单中追加一条明细，并在内部重新汇总总金额。
    /// </summary>
    public void AddItem(long productId, string productName, int quantity, Money unitPrice, DateTimeOffset createdAt)
    {
        var item = OrderItem.Create(productId, productName, quantity, unitPrice, createdAt);
        _items.Add(item);
        RecalculateTotal();
        UpdatedAt = createdAt;
    }

    /// <summary>
    /// 修改订单状态。
    /// </summary>
    public void ChangeStatus(OrderStatus newStatus, DateTimeOffset updatedAt)
    {
        Status = newStatus;
        UpdatedAt = updatedAt;
    }

    /// <summary>
    /// 根据当前明细重新计算订单总金额。
    /// </summary>
    private void RecalculateTotal()
    {
        TotalAmount = _items.Aggregate(Money.Zero, static (current, item) => current + item.LineAmount);
    }
}

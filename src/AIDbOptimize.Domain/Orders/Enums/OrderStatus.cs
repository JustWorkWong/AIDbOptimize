namespace AIDbOptimize.Domain.Orders.Enums;

/// <summary>
/// 订单生命周期状态。
/// </summary>
public enum OrderStatus
{
    Pending = 1,
    Paid = 2,
    Shipped = 3,
    Completed = 4,
    Cancelled = 5
}

namespace AIDbOptimize.Application.Orders.Commands;

/// <summary>
/// 创建订单命令占位，后续可继续扩展明细参数。
/// </summary>
public sealed record CreateOrderCommand(
    string OrderNumber,
    long CustomerId,
    DateTimeOffset CreatedAt);

namespace AIDbOptimize.Application.Orders.Queries;

/// <summary>
/// 按订单号查询订单的最小查询对象。
/// </summary>
public sealed record GetOrderByNumberQuery(string OrderNumber);

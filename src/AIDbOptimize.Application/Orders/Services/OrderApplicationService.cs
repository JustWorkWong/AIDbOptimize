using AIDbOptimize.Application.Orders.Commands;
using AIDbOptimize.Application.Orders.Queries;
using AIDbOptimize.Domain.Orders.Aggregates;
using AIDbOptimize.Domain.Orders.Repositories;
using AIDbOptimize.Domain.Orders.ValueObjects;

namespace AIDbOptimize.Application.Orders.Services;

/// <summary>
/// 订单应用服务骨架，先承接最小创建和读取场景。
/// </summary>
public sealed class OrderApplicationService
{
    private readonly IOrderRepository _orderRepository;

    public OrderApplicationService(IOrderRepository orderRepository)
    {
        _orderRepository = orderRepository;
    }

    public async Task<Order> CreateAsync(CreateOrderCommand command, CancellationToken cancellationToken = default)
    {
        var order = Order.Create(new OrderNumber(command.OrderNumber), command.CustomerId, command.CreatedAt);
        await _orderRepository.AddAsync(order, cancellationToken);
        return order;
    }

    public Task<Order?> GetByOrderNumberAsync(
        GetOrderByNumberQuery query,
        CancellationToken cancellationToken = default)
    {
        return _orderRepository.GetByOrderNumberAsync(new OrderNumber(query.OrderNumber), cancellationToken);
    }
}

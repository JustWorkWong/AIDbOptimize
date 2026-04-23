using AIDbOptimize.Domain.Orders.Aggregates;
using AIDbOptimize.Domain.Orders.ValueObjects;

namespace AIDbOptimize.Domain.Orders.Repositories;

/// <summary>
/// 订单聚合仓储抽象。
/// </summary>
public interface IOrderRepository
{
    Task<Order?> GetByIdAsync(long id, CancellationToken cancellationToken = default);

    Task<Order?> GetByOrderNumberAsync(OrderNumber orderNumber, CancellationToken cancellationToken = default);

    Task AddAsync(Order order, CancellationToken cancellationToken = default);

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}

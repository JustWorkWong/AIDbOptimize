using AIDbOptimize.Domain.Common;

namespace AIDbOptimize.Domain.Orders.ValueObjects;

/// <summary>
/// 订单号值对象。
/// 统一约束订单号不能为空，并预留格式校验入口。
/// </summary>
public readonly record struct OrderNumber
{
    public OrderNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new DomainException("订单号不能为空。");
        }

        Value = value.Trim();
    }

    public string Value { get; }

    public override string ToString() => Value;
}

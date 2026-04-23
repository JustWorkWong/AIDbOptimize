using AIDbOptimize.Domain.Common;

namespace AIDbOptimize.Domain.Orders.ValueObjects;

/// <summary>
/// 金额值对象。
/// 统一约束金额精度，并封装基础运算。
/// </summary>
public readonly record struct Money
{
    public Money(decimal value)
    {
        if (value < 0)
        {
            throw new DomainException("金额不能为负数。");
        }

        Value = decimal.Round(value, 2, MidpointRounding.AwayFromZero);
    }

    public decimal Value { get; }

    public static Money Zero => new(0m);

    public static Money operator +(Money left, Money right) => new(left.Value + right.Value);

    public static Money operator *(Money left, int multiplier)
    {
        if (multiplier < 0)
        {
            throw new DomainException("数量不能为负数。");
        }

        return new Money(left.Value * multiplier);
    }

    public override string ToString() => Value.ToString("0.00");
}

namespace Bookify.Domain.Apartments;

public record Money(decimal Amount, Currency Currency)
{
    public static Money Zero() => new(0, Currency.None);

    public static Money Zero(Currency currency) => new(0, currency);
    
    public static Money operator +(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Cannot add money of different currencies");
        }

        return new Money(first.Amount + second.Amount, first.Currency);
    }

    public static Money operator -(Money first, Money second) =>
        CalcEnsuringSameCurrency(first, second, (f, s) => new Money(f.Amount - s.Amount, f.Currency));

    public static Money operator *(Money first, decimal multiplier) => new(first.Amount * multiplier, first.Currency);

    public static Money operator /(Money first, decimal divisor)
    {
        if (divisor == 0)
        {
            throw new InvalidOperationException("Cannot divide money by zero");
        }

        return CalcEnsuringSameCurrency(first, new Money(divisor, first.Currency),
            (f, s) => new Money(f.Amount / s.Amount, f.Currency));
    }

    public static bool operator <(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return first.Amount < second.Amount;
    }

    public static bool operator >(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return first.Amount > second.Amount;
    }
    
    public static bool operator <=(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return first.Amount <= second.Amount;
    }
    
    public static bool operator >=(Money first, Money second)
    {
        EnsureSameCurrency(first, second);
        return first.Amount >= second.Amount;
    }
    
    private static Money CalcEnsuringSameCurrency(Money first, Money second, Func<Money, Money, Money> calc)
    {
        EnsureSameCurrency(first, second);
        return calc(first, second);
    }

    private static void EnsureSameCurrency(Money first, Money second)
    {
        if (first.Currency != second.Currency)
        {
            throw new InvalidOperationException("Cannot compare money of different currencies");
        }
    }

    internal bool IsZero() => this == Zero(Currency);
}
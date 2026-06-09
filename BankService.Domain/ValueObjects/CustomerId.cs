namespace BankService.Domain.ValueObjects;

/// <summary>
/// Strongly-typed identifier for a customer. Using a value object instead of a raw
/// <see cref="Guid"/> prevents accidentally mixing identifiers of different concepts.
/// </summary>
public readonly record struct CustomerId(Guid Value)
{
    public static CustomerId New() => new(Guid.NewGuid());

    public static CustomerId From(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("CustomerId cannot be empty.", nameof(value));
        }

        return new CustomerId(value);
    }

    public override string ToString() => Value.ToString();
}

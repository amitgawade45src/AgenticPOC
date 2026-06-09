namespace BankService.Domain.ValueObjects;

/// <summary>
/// A value object describing a bank account number. It encapsulates the rules
/// for what a valid account number looks like, keeping that knowledge in the domain.
/// </summary>
public readonly record struct AccountNumber
{
    public string Value { get; }

    private AccountNumber(string value) => Value = value;

    /// <summary>
    /// Generates a new 10-digit account number. In a real bank this would come
    /// from a core-banking number range; here it is mocked.
    /// </summary>
    public static AccountNumber Generate()
    {
        var digits = Random.Shared.NextInt64(1_000_000_000, 9_999_999_999);
        return new AccountNumber(digits.ToString());
    }

    public static AccountNumber From(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 10 || !value.All(char.IsDigit))
        {
            throw new ArgumentException("Account number must be exactly 10 digits.", nameof(value));
        }

        return new AccountNumber(value);
    }

    public override string ToString() => Value;
}

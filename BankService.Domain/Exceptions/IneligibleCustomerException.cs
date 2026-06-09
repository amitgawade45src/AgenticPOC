namespace BankService.Domain.Exceptions;

/// <summary>
/// Thrown when a customer does not meet the domain rules required to open an account
/// (for example, an insufficient earning/credit score).
/// </summary>
public sealed class IneligibleCustomerException : Exception
{
    public int ActualScore { get; }
    public int RequiredScore { get; }

    public IneligibleCustomerException(int actualScore, int requiredScore)
        : base($"Customer is ineligible to open an account. Earning score {actualScore} is below the required minimum of {requiredScore}.")
    {
        ActualScore = actualScore;
        RequiredScore = requiredScore;
    }
}

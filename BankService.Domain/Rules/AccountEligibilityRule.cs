namespace BankService.Domain.Rules;

/// <summary>
/// Encapsulates the business rule that decides whether a customer is eligible
/// to open a bank account based on their earning/credit score.
/// Keeping this rule in the domain layer means the policy lives in one place
/// and is independent of how the score is obtained (mock API, bureau, etc.).
/// </summary>
public static class AccountEligibilityRule
{
    /// <summary>
    /// The minimum earning score a customer must have to be eligible.
    /// </summary>
    public const int MinimumEarningScore = 650;

    public static bool IsEligible(int earningScore) => earningScore >= MinimumEarningScore;
}

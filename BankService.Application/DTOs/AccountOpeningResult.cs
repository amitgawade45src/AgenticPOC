namespace BankService.Application.DTOs;

/// <summary>
/// Outcome of an account-opening attempt. Models both the eligible (success)
/// and ineligible (business-rule failure) paths without throwing across layers.
/// </summary>
public sealed record AccountOpeningResult
{
    public bool IsApproved { get; init; }
    public string Message { get; init; } = string.Empty;
    public AccountDto? Account { get; init; }

    public static AccountOpeningResult Approved(AccountDto account) => new()
    {
        IsApproved = true,
        Message = "Account opened successfully.",
        Account = account
    };

    public static AccountOpeningResult Rejected(string message) => new()
    {
        IsApproved = false,
        Message = message,
        Account = null
    };
}

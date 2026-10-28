namespace BankService.Models;

/// <summary>
/// Request body for repaying an instant loan.
/// </summary>
public sealed class RepayInstantLoanRequest
{
    public decimal Amount { get; set; }
}

namespace BankService.Models;

/// <summary>
/// Request body for paying a home-loan EMI.
/// </summary>
public sealed class PayEmiRequest
{
    public decimal Amount { get; set; }
}

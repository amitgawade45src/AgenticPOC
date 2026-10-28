namespace BankService.Models;

/// <summary>
/// Request body for applying for an instant loan.
/// </summary>
public sealed class ApplyForInstantLoanRequest
{
    public Guid CustomerId { get; set; }
    public decimal RequestedAmount { get; set; }
}

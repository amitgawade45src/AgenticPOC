namespace BankService.Models;

/// <summary>
/// Request body for applying for a home loan.
/// </summary>
public sealed class ApplyForHomeLoanRequest
{
    public Guid CustomerId { get; set; }
    public Guid AccountId { get; set; }
    public string PropertyAddress { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int LoanTermMonths { get; set; }
}

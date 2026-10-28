namespace BankService.Infrastructure.Persistence.Models;

/// <summary>
/// Persistence model for a home loan aggregate.
/// </summary>
public sealed class HomeLoanRecord
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public Guid AccountId { get; set; }
    public string PropertyAddress { get; set; } = string.Empty;
    public decimal LoanAmount { get; set; }
    public decimal OutstandingAmount { get; set; }
    public decimal InterestRate { get; set; }
    public int LoanTermMonths { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}

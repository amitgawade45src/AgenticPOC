namespace BankService.Infrastructure.Persistence.Models;

/// <summary>
/// Persistence model for an instant loan aggregate.
/// </summary>
public sealed class InstantLoanRecord
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public decimal RequestedAmount { get; set; }
    public decimal ApprovedAmount { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}

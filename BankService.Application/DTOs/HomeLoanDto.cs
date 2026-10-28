namespace BankService.Application.DTOs;

/// <summary>
/// Read model describing a home loan.
/// </summary>
public sealed record HomeLoanDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public Guid AccountId { get; init; }
    public string PropertyAddress { get; init; } = string.Empty;
    public decimal LoanAmount { get; init; }
    public decimal OutstandingAmount { get; init; }
    public decimal InterestRate { get; init; }
    public int LoanTermMonths { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
}

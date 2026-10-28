namespace BankService.Application.DTOs;

/// <summary>
/// Read model describing an instant loan.
/// </summary>
public sealed record InstantLoanDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public decimal RequestedAmount { get; init; }
    public decimal ApprovedAmount { get; init; }
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
}

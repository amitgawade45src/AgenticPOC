namespace BankService.Application.DTOs;

/// <summary>
/// A read model describing a bank account, returned to the presentation layer.
/// </summary>
public sealed record AccountDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string AccountNumber { get; init; } = string.Empty;
    public int EarningScore { get; init; }
    public string Status { get; init; } = string.Empty;
    public IReadOnlyList<DocumentDto> Documents { get; init; } = [];
    public DateTime CreatedOnUtc { get; init; }
}

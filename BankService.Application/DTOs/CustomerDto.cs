namespace BankService.Application.DTOs;

/// <summary>
/// Read model describing a customer profile.
/// </summary>
public sealed record CustomerDto
{
    public Guid Id { get; init; }
    public Guid CustomerId { get; init; }
    public string FirstName { get; init; } = string.Empty;
    public string LastName { get; init; } = string.Empty;
    public DateOnly DateOfBirth { get; init; }
    public string Email { get; init; } = string.Empty;
    public string MobileNumber { get; init; } = string.Empty;
    public string Address { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public DateTime CreatedOnUtc { get; init; }
}

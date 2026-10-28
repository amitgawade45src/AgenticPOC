namespace BankService.Infrastructure.Persistence.Models;

/// <summary>
/// Persistence model for a customer aggregate.
/// </summary>
public sealed class CustomerRecord
{
    public Guid Id { get; set; }
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateOnly DateOfBirth { get; set; }
    public string Email { get; set; } = string.Empty;
    public string MobileNumber { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedOnUtc { get; set; }
}

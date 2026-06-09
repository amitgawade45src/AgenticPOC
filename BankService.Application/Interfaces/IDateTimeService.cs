namespace BankService.Application.Interfaces;

/// <summary>
/// Abstraction over the system clock so handlers and the domain stay testable
/// and deterministic.
/// </summary>
public interface IDateTimeService
{
    DateTime UtcNow { get; }
}

using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when an existing bank account is closed.
/// </summary>
public sealed record AccountClosedEvent(
    Guid AccountId,
    CustomerId CustomerId,
    AccountNumber AccountNumber,
    DateTime OccurredOnUtc) : IDomainEvent;

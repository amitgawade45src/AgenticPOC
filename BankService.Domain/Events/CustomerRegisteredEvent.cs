using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a customer profile is first registered.
/// </summary>
public sealed record CustomerRegisteredEvent(
    CustomerId CustomerId,
    Guid AggregateId,
    DateTime OccurredOnUtc) : IDomainEvent;

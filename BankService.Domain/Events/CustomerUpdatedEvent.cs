using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a customer profile is updated.
/// </summary>
public sealed record CustomerUpdatedEvent(
    CustomerId CustomerId,
    Guid AggregateId,
    DateTime OccurredOnUtc) : IDomainEvent;

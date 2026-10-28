using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a customer profile is deactivated.
/// </summary>
public sealed record CustomerDeactivatedEvent(
    CustomerId CustomerId,
    Guid AggregateId,
    DateTime OccurredOnUtc) : IDomainEvent;

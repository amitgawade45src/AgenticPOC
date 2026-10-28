using BankService.Domain.Common;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a bank-account document is marked as verified.
/// </summary>
public sealed record DocumentVerifiedEvent(
    Guid AccountId,
    Guid DocumentId,
    DateTime OccurredOnUtc) : IDomainEvent;

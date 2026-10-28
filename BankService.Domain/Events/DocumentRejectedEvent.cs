using BankService.Domain.Common;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a bank-account document is marked as rejected.
/// </summary>
public sealed record DocumentRejectedEvent(
    Guid AccountId,
    Guid DocumentId,
    DateTime OccurredOnUtc) : IDomainEvent;

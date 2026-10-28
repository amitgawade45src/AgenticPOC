using BankService.Domain.Common;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when an onboarding document is attached to a bank account.
/// </summary>
public sealed record DocumentUploadedEvent(
    Guid AccountId,
    Guid DocumentId,
    string Uri,
    string FileName,
    DateTime OccurredOnUtc) : IDomainEvent;

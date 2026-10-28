using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a home loan is rejected.
/// </summary>
public sealed record HomeLoanRejectedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    Guid AccountId,
    DateTime OccurredOnUtc) : IDomainEvent;

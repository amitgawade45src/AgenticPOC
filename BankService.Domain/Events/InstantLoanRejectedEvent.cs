using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when an instant loan is rejected.
/// </summary>
public sealed record InstantLoanRejectedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    DateTime OccurredOnUtc) : IDomainEvent;

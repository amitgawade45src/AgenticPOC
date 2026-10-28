using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when an instant loan is disbursed.
/// </summary>
public sealed record InstantLoanDisbursedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    decimal Amount,
    DateTime OccurredOnUtc) : IDomainEvent;

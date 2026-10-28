using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a customer applies for an instant loan.
/// </summary>
public sealed record InstantLoanAppliedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    decimal RequestedAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

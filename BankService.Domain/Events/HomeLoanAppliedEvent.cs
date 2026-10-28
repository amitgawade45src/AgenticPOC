using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a customer applies for a home loan.
/// </summary>
public sealed record HomeLoanAppliedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    Guid AccountId,
    decimal LoanAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

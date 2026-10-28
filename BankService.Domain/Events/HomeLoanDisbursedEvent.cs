using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a home loan is disbursed.
/// </summary>
public sealed record HomeLoanDisbursedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    Guid AccountId,
    decimal LoanAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

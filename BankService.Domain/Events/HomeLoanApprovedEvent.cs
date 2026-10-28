using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a home loan is approved.
/// </summary>
public sealed record HomeLoanApprovedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    Guid AccountId,
    decimal LoanAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

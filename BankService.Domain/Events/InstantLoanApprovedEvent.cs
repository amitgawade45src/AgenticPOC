using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when an instant loan is approved.
/// </summary>
public sealed record InstantLoanApprovedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    decimal ApprovedAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

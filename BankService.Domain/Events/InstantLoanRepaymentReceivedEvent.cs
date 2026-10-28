using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a repayment is received for an instant loan.
/// </summary>
public sealed record InstantLoanRepaymentReceivedEvent(
    Guid LoanId,
    CustomerId CustomerId,
    decimal Amount,
    DateTime OccurredOnUtc) : IDomainEvent;

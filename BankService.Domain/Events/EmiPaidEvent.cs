using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised when a home-loan EMI payment is received.
/// </summary>
public sealed record EmiPaidEvent(
    Guid LoanId,
    CustomerId CustomerId,
    Guid AccountId,
    decimal PaidAmount,
    decimal OutstandingAmount,
    DateTime OccurredOnUtc) : IDomainEvent;

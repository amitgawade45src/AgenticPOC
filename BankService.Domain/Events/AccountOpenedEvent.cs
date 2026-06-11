using BankService.Domain.Common;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Events;

/// <summary>
/// Raised by the <see cref="Entities.BankAccount"/> aggregate when a new account
/// is successfully opened. Downstream handlers could send a welcome email,
/// notify compliance, etc.
/// </summary>
public sealed record AccountOpenedEvent(CustomerId CustomerId, AccountNumber AccountNumber, DateTime OccurredOnUtc) : IDomainEvent;

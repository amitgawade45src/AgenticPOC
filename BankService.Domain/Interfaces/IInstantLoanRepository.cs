using BankService.Domain.Entities;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Interfaces;

/// <summary>
/// Repository abstraction for persisting and retrieving <see cref="InstantLoan"/> aggregates.
/// </summary>
public interface IInstantLoanRepository : IRepository<InstantLoan>
{
    Task<IReadOnlyList<InstantLoan>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
}

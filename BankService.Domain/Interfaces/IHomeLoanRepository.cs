using BankService.Domain.Entities;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Interfaces;

/// <summary>
/// Repository abstraction for persisting and retrieving <see cref="HomeLoan"/> aggregates.
/// </summary>
public interface IHomeLoanRepository : IRepository<HomeLoan>
{
    Task<IReadOnlyList<HomeLoan>> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
}

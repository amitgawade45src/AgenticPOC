using BankService.Domain.Entities;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Interfaces;

/// <summary>
/// Repository abstraction for persisting and retrieving <see cref="Customer"/> aggregates.
/// </summary>
public interface ICustomerRepository : IRepository<Customer>
{
    Task<Customer?> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);

    Task<Customer?> GetByEmailAsync(string email, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Customer>> SearchAsync(string searchTerm, CancellationToken cancellationToken = default);
}

using BankService.Domain.Common;
using BankService.Domain.Entities;
using BankService.Domain.ValueObjects;

namespace BankService.Domain.Interfaces;

/// <summary>
/// Repository abstraction for persisting and retrieving the <see cref="BankAccount"/>
/// aggregate root. The interface lives in the domain layer; the implementation lives
/// in infrastructure (Dependency Inversion). Committing changes is the
/// responsibility of <see cref="IUnitOfWork"/>.
/// </summary>
public interface IAccountRepository : IRepository<BankAccount>
{
    Task<BankAccount?> GetByCustomerIdAsync(CustomerId customerId, CancellationToken cancellationToken = default);
}

/// <summary>
/// Generic repository contract constrained to <see cref="IAggregateRoot"/>, so only
/// aggregate roots (not arbitrary entities) can be persisted. Provided as a base
/// abstraction for repositories in this domain.
/// </summary>
public interface IRepository<T> where T : IAggregateRoot
{
    Task AddAsync(T aggregate, CancellationToken cancellationToken = default);

    Task UpdateAsync(T aggregate, CancellationToken cancellationToken = default);

    Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
}

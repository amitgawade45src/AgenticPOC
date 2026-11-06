using BankService.Domain.Entities;
using BankService.Domain.Interfaces;
using BankService.Domain.ValueObjects;
using BankService.Infrastructure.Persistence.Mapping;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of <see cref="IAccountRepository"/>. Reads come from
/// the committed <see cref="InMemoryBankStore"/> (plus anything staged in the
/// current unit of work); writes are staged into the <see cref="InMemoryChangeTracker"/>
/// and only become visible to other scopes once <see cref="UnitOfWork"/> commits.
/// The store works with the infrastructure <see cref="BankAccountRecord"/> model, so
/// the repository maps to/from the domain aggregate via <see cref="IBankAccountMapper"/>.
/// </summary>
public sealed class AccountRepository : IAccountRepository
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;
    private readonly IBankAccountMapper _mapper;

    public AccountRepository(
        InMemoryBankStore store,
        InMemoryChangeTracker changeTracker,
        IBankAccountMapper mapper)
    {
        _store = store;
        _changeTracker = changeTracker;
        _mapper = mapper;
    }

    public Task AddAsync(BankAccount account, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingAccounts.Add(_mapper.ToRecord(account));
        _changeTracker.Track(account);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(BankAccount account, CancellationToken cancellationToken = default)
    {
        _changeTracker.PendingAccounts.RemoveAll(a => a.Id == account.Id);
        _changeTracker.PendingAccounts.Add(_mapper.ToRecord(account));
        _changeTracker.Track(account);
        return Task.CompletedTask;
    }

    public Task<BankAccount?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingAccounts.FirstOrDefault(a => a.Id == id)
            ?? (_store.Accounts.TryGetValue(id, out var stored) ? stored : null);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }

    public Task<BankAccount?> GetByCustomerIdAsync(
        CustomerId customerId,
        CancellationToken cancellationToken = default)
    {
        var record = _changeTracker.PendingAccounts.FirstOrDefault(a => a.CustomerId == customerId.Value)
            ?? _store.Accounts.Values.FirstOrDefault(a => a.CustomerId == customerId.Value);

        return Task.FromResult(record is null ? null : _mapper.ToDomain(record));
    }
}

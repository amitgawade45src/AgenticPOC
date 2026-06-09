using BankService.Domain.Interfaces;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// In-memory implementation of <see cref="IUnitOfWork"/>. Commits the changes
/// staged in the <see cref="InMemoryChangeTracker"/> to the singleton
/// <see cref="InMemoryBankStore"/> as a single unit, mirroring how an EF Core
/// DbContext would flush its change tracker on SaveChanges.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;

    public UnitOfWork(InMemoryBankStore store, InMemoryChangeTracker changeTracker)
    {
        _store = store;
        _changeTracker = changeTracker;
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var pending = _changeTracker.PendingAccounts;

        foreach (var account in pending)
        {
            _store.Accounts[account.Id] = account;
        }

        var committed = pending.Count;
        pending.Clear();

        return Task.FromResult(committed);
    }
}

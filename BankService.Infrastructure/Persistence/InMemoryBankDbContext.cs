using BankService.Domain.Common;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// In-memory <see cref="IBankDbContext"/>. Commits the records staged in the scoped
/// <see cref="InMemoryChangeTracker"/> to the singleton <see cref="InMemoryBankStore"/>,
/// mirroring how an EF Core DbContext flushes its change tracker on SaveChanges.
/// </summary>
public sealed class InMemoryBankDbContext : IBankDbContext
{
    private readonly InMemoryBankStore _store;
    private readonly InMemoryChangeTracker _changeTracker;

    public InMemoryBankDbContext(InMemoryBankStore store, InMemoryChangeTracker changeTracker)
    {
        _store = store;
        _changeTracker = changeTracker;
    }

    public IReadOnlyCollection<AggregateRoot> TrackedAggregates => _changeTracker.TrackedAggregates;

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        foreach (var account in _changeTracker.PendingAccounts)
        {
            _store.Accounts[account.Id] = account;
        }

        foreach (var customer in _changeTracker.PendingCustomers)
        {
            _store.Customers[customer.Id] = customer;
        }

        foreach (var instantLoan in _changeTracker.PendingInstantLoans)
        {
            _store.InstantLoans[instantLoan.Id] = instantLoan;
        }

        foreach (var homeLoan in _changeTracker.PendingHomeLoans)
        {
            _store.HomeLoans[homeLoan.Id] = homeLoan;
        }

        var committed = _changeTracker.PendingAccounts.Count
            + _changeTracker.PendingCustomers.Count
            + _changeTracker.PendingInstantLoans.Count
            + _changeTracker.PendingHomeLoans.Count;

        _changeTracker.PendingAccounts.Clear();
        _changeTracker.PendingCustomers.Clear();
        _changeTracker.PendingInstantLoans.Clear();
        _changeTracker.PendingHomeLoans.Clear();

        return Task.FromResult(committed);
    }

    public void ClearTracking() => _changeTracker.Clear();
}

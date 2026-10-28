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
        var pendingAccounts = _changeTracker.PendingAccounts;
        var pendingCustomers = _changeTracker.PendingCustomers;
        var pendingInstantLoans = _changeTracker.PendingInstantLoans;
        var pendingHomeLoans = _changeTracker.PendingHomeLoans;

        foreach (var account in pendingAccounts)
        {
            _store.Accounts[account.Id] = account;
        }

        foreach (var customer in pendingCustomers)
        {
            _store.Customers[customer.Id] = customer;
        }

        foreach (var instantLoan in pendingInstantLoans)
        {
            _store.InstantLoans[instantLoan.Id] = instantLoan;
        }

        foreach (var homeLoan in pendingHomeLoans)
        {
            _store.HomeLoans[homeLoan.Id] = homeLoan;
        }

        var committed = pendingAccounts.Count + pendingCustomers.Count + pendingInstantLoans.Count + pendingHomeLoans.Count;

        pendingAccounts.Clear();
        pendingCustomers.Clear();
        pendingInstantLoans.Clear();
        pendingHomeLoans.Clear();

        return Task.FromResult(committed);
    }
}

using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// Holds the changes staged within a single unit of work (one request scope)
/// until <see cref="UnitOfWork"/> commits them to the <see cref="InMemoryBankStore"/>.
/// Shared (scoped) between the repository that stages changes and the unit of work
/// that commits them. Staged items are infrastructure <see cref="BankAccountRecord"/>
/// models.
/// </summary>
public sealed class InMemoryChangeTracker
{
    public List<BankAccountRecord> PendingAccounts { get; } = new();

    public List<CustomerRecord> PendingCustomers { get; } = new();

    public List<InstantLoanRecord> PendingInstantLoans { get; } = new();

    public List<HomeLoanRecord> PendingHomeLoans { get; } = new();
}

using BankService.Domain.Common;
using BankService.Infrastructure.Persistence.Models;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// Holds the changes staged within a single unit of work (one request scope)
/// until <see cref="InMemoryBankDbContext"/> commits them to the <see cref="InMemoryBankStore"/>.
/// Shared (scoped) between the repositories that stage changes and the context
/// that commits them. Staged items are infrastructure record models; the originating
/// aggregates are tracked too so their domain events can be dispatched after the commit,
/// mirroring EF Core's change tracker.
/// </summary>
public sealed class InMemoryChangeTracker
{
    private readonly List<AggregateRoot> _trackedAggregates = new();

    public List<BankAccountRecord> PendingAccounts { get; } = new();

    public List<CustomerRecord> PendingCustomers { get; } = new();

    public List<InstantLoanRecord> PendingInstantLoans { get; } = new();

    public List<HomeLoanRecord> PendingHomeLoans { get; } = new();

    /// <summary>Aggregates whose changes are staged in this unit of work.</summary>
    public IReadOnlyCollection<AggregateRoot> TrackedAggregates => _trackedAggregates.AsReadOnly();

    public void Track(AggregateRoot aggregate)
    {
        if (!_trackedAggregates.Contains(aggregate))
        {
            _trackedAggregates.Add(aggregate);
        }
    }

    public void Clear()
    {
        PendingAccounts.Clear();
        PendingCustomers.Clear();
        PendingInstantLoans.Clear();
        PendingHomeLoans.Clear();
        _trackedAggregates.Clear();
    }
}

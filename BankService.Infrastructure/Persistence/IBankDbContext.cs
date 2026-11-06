using BankService.Domain.Common;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// Infrastructure-level abstraction over the underlying data context (today an in-memory
/// store, tomorrow an EF Core <c>DbContext</c>). <see cref="UnitOfWork"/> depends on this
/// rather than on a concrete store, so swapping the persistence technology only requires
/// a new implementation of this interface.
/// </summary>
public interface IBankDbContext
{
    /// <summary>Aggregates with changes staged in the current context.</summary>
    IReadOnlyCollection<AggregateRoot> TrackedAggregates { get; }

    /// <summary>
    /// Flushes all staged changes to the backing store as a single atomic operation and
    /// returns the number of persisted aggregates. Does <b>not</b> dispatch domain events.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>Discards everything staged in the current context.</summary>
    void ClearTracking();
}

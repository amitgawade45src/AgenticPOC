namespace BankService.Domain.Interfaces;

/// <summary>
/// Coordinates the atomic commit of changes made through the repositories.
/// The interface lives in the domain layer; the implementation lives in
/// infrastructure (Dependency Inversion). Repositories stage changes; the unit
/// of work commits them as a single unit.
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Persists all staged changes as a single unit, then dispatches the domain events
    /// raised by the persisted aggregates and clears them. Returns the number of
    /// affected aggregates.
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

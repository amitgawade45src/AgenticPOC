using BankService.Domain.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BankService.Infrastructure.Persistence;

/// <summary>
/// <see cref="IUnitOfWork"/> implemented as an abstraction layer over <see cref="IBankDbContext"/>.
/// <see cref="SaveChangesAsync"/> commits the staged changes through the context and then
/// dispatches the domain events raised by every tracked aggregate, so command handlers
/// only need to call <c>SaveChangesAsync</c> and never deal with event publishing directly.
/// </summary>
public sealed class UnitOfWork : IUnitOfWork
{
    private readonly IBankDbContext _dbContext;
    private readonly IPublisher _publisher;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(IBankDbContext dbContext, IPublisher publisher, ILogger<UnitOfWork> logger)
    {
        _dbContext = dbContext;
        _publisher = publisher;
        _logger = logger;
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        // Snapshot the aggregates before the context clears its tracking.
        var aggregates = _dbContext.TrackedAggregates.ToList();

        var committed = await _dbContext.SaveChangesAsync(cancellationToken);

        // Events are dispatched only after the data is safely persisted, so handlers
        // (e.g. welcome e-mail) never react to state that was rolled back.
        await DispatchDomainEventsAsync(aggregates, cancellationToken);

        _dbContext.ClearTracking();

        return committed;
    }

    private async Task DispatchDomainEventsAsync(
        IEnumerable<Domain.Common.AggregateRoot> aggregates,
        CancellationToken cancellationToken)
    {
        foreach (var aggregate in aggregates)
        {
            var domainEvents = aggregate.DomainEvents.ToList();
            if (domainEvents.Count == 0)
            {
                continue;
            }

            foreach (var domainEvent in domainEvents)
            {
                _logger.LogDebug(
                    "Dispatching domain event {DomainEvent} for aggregate {AggregateType} {AggregateId}.",
                    domainEvent.GetType().Name, aggregate.GetType().Name, aggregate.Id);

                await _publisher.Publish(domainEvent, cancellationToken);
            }

            aggregate.ClearDomainEvents();
        }
    }
}

namespace BankService.Domain.Common;

/// <summary>
/// Base class for aggregate roots. An aggregate root is the consistency boundary
/// of an aggregate and the only object a repository persists or loads. It owns the
/// domain events raised while the aggregate's state changes; the application layer
/// dispatches and then clears them once the aggregate is safely persisted.
/// </summary>
public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<IDomainEvent> _domainEvents = new();

    /// <summary>
    /// The domain events raised by this aggregate that have not yet been dispatched.
    /// </summary>
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot(Guid id) : base(id)
    {
    }

    protected AggregateRoot()
    {
    }

    /// <summary>
    /// Records a domain event. Only the aggregate root may raise events, which is
    /// why this is protected.
    /// </summary>
    protected void RaiseDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}

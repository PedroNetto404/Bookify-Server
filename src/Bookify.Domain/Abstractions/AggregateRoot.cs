namespace Bookify.Domain.Abstractions;

public abstract class AggregateRoot<TId>
    : Entity<TId>,
        IAggregateRoot<TId>
    where TId : notnull
{
    protected AggregateRoot(TId id) : base(id)
    {
    }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();

    protected void RaiseDomainEvents(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);

    public void ClearDomainEvents() => _domainEvents.Clear();
}
namespace Bookify.Domain.Abstractions;

public abstract class Entity
{
    protected Entity(Guid identifier)
    {
        Identifier = identifier;
    }

    public Guid Identifier { get; private set; }

    private readonly List<IDomainEvent> _domainEvents = new();

    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents.ToList().AsReadOnly();

    protected void RaiseDomainEvents(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
namespace Bookify.Domain.Abstractions;

public interface IAggregateRoot<TId>
    where TId : notnull
{
    IReadOnlyCollection<IDomainEvent> DomainEvents { get; }

    void ClearDomainEvents();
}
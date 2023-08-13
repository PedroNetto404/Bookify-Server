using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record UserCreatedEvent(Guid userId) : IDomainEvent;

using Bookify.Domain.Abstractions;

namespace Bookify.Domain.Users.Events;

public sealed record TenantCreatedEvent(TenantId TenantId) : IDomainEvent;

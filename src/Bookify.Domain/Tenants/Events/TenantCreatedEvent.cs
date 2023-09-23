using Bookify.Domain.Abstractions;
using Bookify.Domain.Tenants.ValueObjects;

namespace Bookify.Domain.Tenants.Events;

public sealed record TenantCreatedEvent(TenantId TenantId) : IDomainEvent;

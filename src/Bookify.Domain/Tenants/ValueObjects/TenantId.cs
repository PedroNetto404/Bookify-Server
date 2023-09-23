namespace Bookify.Domain.Tenants.ValueObjects;

public record TenantId(Guid Value)
{
    public static TenantId New() => new(Guid.NewGuid());
}
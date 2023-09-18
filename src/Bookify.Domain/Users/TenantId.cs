using Bookify.Domain.Apartments;

namespace Bookify.Domain.Users;

public record TenantId(Guid Value)
{
    public static TenantId New() => new(Guid.NewGuid());
}
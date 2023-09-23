using Bookify.Domain.Tenants.ValueObjects;

namespace Bookify.Domain.Tenants;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(TenantId tenantId);

    Task AddAsync(Tenant tenant);
}
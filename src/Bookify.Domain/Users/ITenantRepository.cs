namespace Bookify.Domain.Users;

public interface ITenantRepository
{
    Task<Tenant?> GetByIdAsync(TenantId tenantId);

    Task AddAsync(Tenant tenant);
}
using Bookify.Domain.Tenants;
using Bookify.Domain.Tenants.ValueObjects;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Bookify.Infrastructure.Data.UnitOfWork;
using Dapper;

namespace Bookify.Infrastructure.Data.Repositories;

internal sealed class TenantRepository : BaseRepository, ITenantRepository
{
    public TenantRepository(IDbSession dbSession) : base(dbSession) { }

    public async Task<Tenant?> GetByIdAsync(TenantId tenantId)
    {
        const string query = $@"
        SELECT 
            tnt.TenantId AS {nameof(TenantSnapshot.TenantId)},
            tnt.FirstName AS {nameof(TenantSnapshot.FirstName)},
            tnt.LastName AS {nameof(TenantSnapshot.LastName)},
            tnt.Email AS {nameof(TenantSnapshot.Email)}
        FROM 
            Tenant AS tnt
        WHERE 
            tnt.TenantId = @TenantId";

        var tenantSnapshot = await Connection.QueryFirstOrDefaultAsync<TenantSnapshot>(query, new { TenantId = tenantId.Value });

        return tenantSnapshot is null ? null : Tenant.FromSnapshot(tenantSnapshot);
    }

    public async Task AddAsync(Tenant tenant)
    {
        const string query = $@"
        INSERT INTO Tenant (
            TenantId,
            FirstName,
            LastName,
            Email) 
        VALUES (
            @{nameof(TenantSnapshot.TenantId)}
            @{nameof(TenantSnapshot.FirstName)}
            @{nameof(TenantSnapshot.LastName)}
            @{nameof(TenantSnapshot.Email)});";

       await Connection.ExecuteAsync(
           query,
           tenant.ToSnapshot(),
           transaction: Transaction);
    }
}
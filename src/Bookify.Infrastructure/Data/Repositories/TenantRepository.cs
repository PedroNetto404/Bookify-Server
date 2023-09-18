using Bookify.Domain.Users;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Dapper;

namespace Bookify.Infrastructure.Data.Repositories;

internal sealed class TenantRepository : BaseRepository, ITenantRepository
{
    public TenantRepository(ISqlConnectionFactory sqlConnectionFactory) : base(sqlConnectionFactory)
    {
    }

    public async Task<Tenant?> GetByIdAsync(TenantId tenantId)
    {
        using var conn = GetOpeningConnection();

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

        var tenantSnapshot = await conn.QueryFirstOrDefaultAsync<TenantSnapshot>(query, new { TenantId = tenantId.Value });

        return tenantSnapshot is null ? null : Tenant.FromSnapshot(tenantSnapshot);
    }

    public async Task AddAsync(Tenant tenant)
    {
        using var conn = GetOpeningConnection();

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

        var rowsAffected = await conn.ExecuteAsync(query, tenant.ToSnapshot());

        if (rowsAffected != 1)
        {
            throw new InvalidOperationException("Tenant could not be added.");
        }
    }
}
using System.Data;
using Bookify.Infrastructure.Data.SqlConnectionFactory;

namespace Bookify.Infrastructure.Data.Repositories;

internal abstract class BaseRepository
{
    private readonly ISqlConnectionFactory _sqlConnectionFactory;

    protected BaseRepository(ISqlConnectionFactory sqlConnectionFactory)
    {
        _sqlConnectionFactory = sqlConnectionFactory;
    }

    protected IDbConnection GetOpeningConnection()
    {
        var connection = _sqlConnectionFactory.CreateConnection();
        connection.Open();
        return connection;
    }
}
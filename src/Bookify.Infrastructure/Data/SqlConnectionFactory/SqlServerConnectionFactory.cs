using Microsoft.Extensions.Options;
using System.Data;
using System.Data.SqlClient;

namespace Bookify.Infrastructure.Data.SqlConnectionFactory;

internal sealed class SqlServerConnectionFactory : ISqlConnectionFactory
{
    private readonly ConnectionString _connectionString;

    public SqlServerConnectionFactory(IOptions<ConnectionString> connectionString) => 
        _connectionString = connectionString.Value;

    public IDbConnection CreateConnection() => 
        new SqlConnection(_connectionString.Value);
}
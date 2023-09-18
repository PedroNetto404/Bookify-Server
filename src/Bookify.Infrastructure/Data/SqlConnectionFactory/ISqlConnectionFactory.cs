using System.Data;

namespace Bookify.Infrastructure.Data.SqlConnectionFactory;

public interface ISqlConnectionFactory
{
    IDbConnection CreateConnection();
}

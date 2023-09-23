using System.Data;
using Bookify.Infrastructure.Data.SqlConnectionFactory;

namespace Bookify.Infrastructure.Data.UnitOfWork;

public class DbSession : IDbSession, IDisposable
{
    public DbSession(ISqlConnectionFactory sqlConnectionFactory)
    {
        Connection = sqlConnectionFactory.CreateConnection();
        Connection.Open();
        Transaction = Connection.BeginTransaction();
    }

    public IDbConnection  Connection { get; }

    public IDbTransaction Transaction { get; }

    public void Dispose()
    {
        Connection.Dispose();
        Transaction.Dispose();
    }

    ~DbSession()
    {
        GC.SuppressFinalize(this);
        Dispose();
    }
}
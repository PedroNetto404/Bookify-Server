using System.Data;

namespace Bookify.Infrastructure.Data.UnitOfWork;

public interface IDbSession
{
    public IDbConnection Connection { get; }

    public IDbTransaction Transaction { get; }
}
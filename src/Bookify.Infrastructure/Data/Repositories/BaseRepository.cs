using System.Data;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Data.SqlConnectionFactory;
using Bookify.Infrastructure.Data.UnitOfWork;

namespace Bookify.Infrastructure.Data.Repositories;

internal abstract class BaseRepository
{
    private readonly IDbSession _dbSession;

    protected BaseRepository(IDbSession dbSession) =>
        _dbSession = dbSession;

    protected IDbConnection Connection => _dbSession.Connection;
    protected IDbTransaction Transaction => _dbSession.Transaction; 
}

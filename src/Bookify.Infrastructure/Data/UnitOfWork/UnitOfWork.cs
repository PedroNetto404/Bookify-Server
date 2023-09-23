using Bookify.Domain.Abstractions;

namespace Bookify.Infrastructure.Data.UnitOfWork;

internal class UnitOfWork : IUnitOfWork
{
    private readonly DbSession _dbSession;

    public UnitOfWork(DbSession dbSession)
    {
        _dbSession = dbSession;
    }

    public Task CommitAsync(CancellationToken cancellationToken = default)
    {
        try
        {
            _dbSession.Transaction.Commit();
            
            return Task.CompletedTask;
        }
        catch (Exception e)
        {
            _dbSession.Transaction.Rollback();
            throw;
        }
    }
}
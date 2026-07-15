using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Infrastructure.Db;

namespace Warehouse.Infrastructure.Db;

/// <summary>
/// Реализация единицы работы на основе EF Core DbContext
/// </summary>
public class EfUnitOfWork : IUnitOfWork
{
    private readonly PostgresDbContext _dbContext;

    public EfUnitOfWork(PostgresDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        var dbTransaction = await _dbContext.Database.BeginTransactionAsync(cancellationToken);
        return new EfTransaction(dbTransaction);
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return _dbContext.SaveChangesAsync(cancellationToken);
    }

    private class EfTransaction : ITransaction
    {
        private readonly Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction _transaction;

        public EfTransaction(Microsoft.EntityFrameworkCore.Storage.IDbContextTransaction transaction)
        {
            _transaction = transaction;
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return _transaction.CommitAsync(cancellationToken);
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return _transaction.RollbackAsync(cancellationToken);
        }

        public ValueTask DisposeAsync()
        {
            return _transaction.DisposeAsync();
        }
    }
}

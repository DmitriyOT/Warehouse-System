using Warehouse.Contracts.Infrastructure;

namespace Warehouse.Tests.TestInfrastructure;

public class TestUnitOfWork : IUnitOfWork
{
    public Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult<ITransaction>(new TestTransaction());
    }

    public Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return Task.FromResult(0);
    }

    private class TestTransaction : ITransaction
    {
        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public Task RollbackAsync(CancellationToken cancellationToken = default)
        {
            return Task.CompletedTask;
        }

        public ValueTask DisposeAsync()
        {
            return ValueTask.CompletedTask;
        }
    }
}

namespace Warehouse.Contracts.Infrastructure;

/// <summary>
/// Абстракция транзакции
/// </summary>
public interface ITransaction : IAsyncDisposable
{
    /// <summary>
    /// Зафиксировать транзакцию
    /// </summary>
    Task CommitAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Откатить транзакцию
    /// </summary>
    Task RollbackAsync(CancellationToken cancellationToken = default);
}

/// <summary>
/// Абстракция единицы работы для управления транзакциями
/// </summary>
public interface IUnitOfWork
{
    /// <summary>
    /// Начать транзакцию
    /// </summary>
    Task<ITransaction> BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Сохранить изменения
    /// </summary>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}

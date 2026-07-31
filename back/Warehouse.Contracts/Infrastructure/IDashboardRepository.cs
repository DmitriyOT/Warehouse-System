using Warehouse.Domain.Models;

namespace Warehouse.Contracts.Infrastructure;

/// <summary>
/// Репозиторий для агрегирующих запросов дашборда.
/// Отдаёт IQueryable, чтобы агрегация (GroupBy/Sum/Count) выполнялась на стороне БД
/// </summary>
public interface IDashboardRepository
{
    /// <summary>
    /// Остатки склада
    /// </summary>
    public IQueryable<BalanceEntity> Balances { get; }

    /// <summary>
    /// Документы поступления
    /// </summary>
    public IQueryable<IncomeEntity> Incomes { get; }

    /// <summary>
    /// Позиции поступлений
    /// </summary>
    public IQueryable<IncomeItemEntity> IncomeItems { get; }

    /// <summary>
    /// Документы отгрузки
    /// </summary>
    public IQueryable<ShipmentEntity> Shipments { get; }

    /// <summary>
    /// Позиции отгрузок
    /// </summary>
    public IQueryable<ShipmentItemEntity> ShipmentItems { get; }

    /// <summary>
    /// Клиенты
    /// </summary>
    public IQueryable<ClientEntity> Clients { get; }
}

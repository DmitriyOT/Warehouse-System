namespace Warehouse.Contracts.Api.Response.Dtos;

/// <summary>
/// Сводные данные для дашборда склада
/// </summary>
public class DashboardSummaryDto
{
    /// <summary>
    /// KPI-показатели
    /// </summary>
    public required DashboardKpisDto Kpis { get; set; }

    /// <summary>
    /// Движение товаров по дням за последние 7 дней (включая сегодня)
    /// </summary>
    public required List<DashboardDayDto> WeekMovement { get; set; }

    /// <summary>
    /// Последние 5 операций (приход с плюсом, подтверждённая отгрузка с минусом)
    /// </summary>
    public required List<DashboardOperationDto> LastOperations { get; set; }
}

/// <summary>
/// KPI-показатели дашборда
/// </summary>
public class DashboardKpisDto
{
    /// <summary>
    /// Сумма остатков по неархивным ресурсам и единицам измерения
    /// </summary>
    public decimal TotalBalance { get; set; }

    /// <summary>
    /// Изменение остатка в процентах относительно недели назад (приближённо,
    /// по движению за неделю: текущий остаток минус приходы недели плюс подтверждённые отгрузки недели)
    /// </summary>
    public decimal BalanceDeltaPercent { get; set; }

    /// <summary>
    /// Количество документов поступления за текущую неделю
    /// </summary>
    public int IncomeCount { get; set; }

    /// <summary>
    /// Разница количества поступлений с предыдущей неделей
    /// </summary>
    public int IncomeDelta { get; set; }

    /// <summary>
    /// Количество документов отгрузки за текущую неделю (все статусы)
    /// </summary>
    public int ShipmentCount { get; set; }

    /// <summary>
    /// Разница количества отгрузок с предыдущей неделей
    /// </summary>
    public int ShipmentDelta { get; set; }

    /// <summary>
    /// Количество активных (неархивных) клиентов
    /// </summary>
    public int ActiveClientCount { get; set; }

    /// <summary>
    /// Новые клиенты за неделю. У справочников нет даты создания, поэтому всегда 0
    /// </summary>
    public int ClientDelta { get; set; }
}

/// <summary>
/// Движение товаров за один день
/// </summary>
public class DashboardDayDto
{
    /// <summary>
    /// Дата
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Сумма поступлений за день
    /// </summary>
    public decimal Income { get; set; }

    /// <summary>
    /// Сумма подтверждённых отгрузок за день
    /// </summary>
    public decimal Shipment { get; set; }
}

/// <summary>
/// Последняя операция движения товара
/// </summary>
public class DashboardOperationDto
{
    /// <summary>
    /// Наименование ресурса
    /// </summary>
    public required string ResourceName { get; set; }

    /// <summary>
    /// Количество: положительное для прихода, отрицательное для отгрузки
    /// </summary>
    public decimal Quantity { get; set; }
}

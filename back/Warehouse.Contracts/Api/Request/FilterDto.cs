namespace Warehouse.Contracts.Api.Request;

/// <summary>
/// Класс для описания фильтрации в гриде
/// </summary>
public class FilterDto
{
    /// <summary>
    /// Имя поля по которому фильтруем
    /// </summary>
    public required string PropertyName { get; set; }
    /// <summary>
    /// Тип фильтрация
    /// </summary>
    public required string Type { get; set; }
    /// <summary>
    /// Аргумент фильтрации
    /// </summary>
    public required string Argument { get; set; }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Warehouse.Contracts.Exceptions;

/// <summary>
/// Ошибка, которую нужно отобразить для пользователя
/// </summary>
public class UserException : Exception
{
    /// <summary>
    /// Конструктор базовый
    /// </summary>
    /// <param name="message"></param>
    public UserException(string? message) : base(message)
    {
    }
}

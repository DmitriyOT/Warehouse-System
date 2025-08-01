using Microsoft.AspNetCore.Mvc;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Контроллер для клиентов
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClientController : BaseCrudController<ClientEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public ClientController(ILogger<ClientController> logger, ICrudService<ClientEntity> crudService)
        : base(logger, crudService)
    {
    }
}
using Microsoft.AspNetCore.Mvc;
using Warehouse.Api.Controllers.Base;
using Warehouse.Contracts.Application;
using Warehouse.Domain.Models;

namespace Warehouse.Api.Controllers;

/// <summary>
/// Контроллер для клиентов
/// </summary>
[ApiController]
[Route("[controller]")]
public class ClientController : BaseArchiveCrudController<ClientEntity>
{
    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="crudService"></param>
    public ClientController(ILogger<ClientController> logger, IArchiveCrudService<ClientEntity> crudService)
        : base(logger, crudService)
    {
    }
}
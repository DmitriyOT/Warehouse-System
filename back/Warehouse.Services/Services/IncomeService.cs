using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

public class IncomeService : CrudService<IncomeEntity>
{
    public IncomeService(IIncomeRepository repository) : base(repository)
    {
    }
}

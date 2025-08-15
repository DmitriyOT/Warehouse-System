using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Application.Services.Base;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models;

namespace Warehouse.Application.Services;

public class IncomeService : CrudService<IncomeEntity>
{
    public IncomeService(IIncomeRepository repository) : base(repository)
    {
    }

    public override Task<long> EditItem(IncomeEntity item)
    {
        if (item.IncomeItems != null)
        {
            foreach (var elem in item.IncomeItems)
            {
                if(elem.Quantity <= 0)
                {
                    throw new UserException("Ошибка. Количество ресурса должно быть положительным в документе поступления.");
                }
            }
        }

        return base.EditItem(item);
    }
}

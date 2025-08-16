using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Tests.TestInfrastructure;

public class TestCrudRepository<TEntity> : ICrudRepository<TEntity> where TEntity : BaseEntityWithId
{
    protected Dictionary<long, TEntity> _entities { get; set; }

    public TestCrudRepository()
    {
        _entities = new Dictionary<long, TEntity>();
    }

    public Task DeleteItem(long id)
    {
        _entities.Remove(id);
        return Task.CompletedTask;
    }

    public Task DeleteItems(IEnumerable<long> ids)
    {
        throw new NotImplementedException();
    }

    public Task<long> EditItem(TEntity item)
    {
        if(item.Id < 1)
        {
            item.Id = _entities.Keys.Max() + 1;
        }
        _entities[item.Id] = item;
        return Task.FromResult(item.Id);    
    }

    public Task<Tuple<List<TEntity>, long>> GetAll(GridOptionsDto options)
    {
        return Task.FromResult(Tuple.Create<List<TEntity>, long>(_entities.Values.ToList(), _entities.Values.Count));
    }

    public Task<TEntity> GetItem(long id)
    {
        return Task.FromResult(_entities[id]);
    }
}

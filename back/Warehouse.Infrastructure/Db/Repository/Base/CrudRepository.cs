using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Exceptions;
using Warehouse.Contracts.Infrastracture;
using Warehouse.Domain.Models.Base;

namespace Warehouse.Infrastructure.Db.Repository.Base;

/// <summary>
/// Базовый репозиторий поддерживающий CRUD операции
/// </summary>
/// <typeparam name="Entity"></typeparam>
public class CrudRepository<Entity> : ICrudRepository<Entity> where Entity : BaseEntityWithId
{
    /// <summary>
    /// Контект БД
    /// </summary>
    protected PostgresDbContext DB { get; private set; }

    /// <summary>
    /// Локальная переменная Dbset для удобства
    /// </summary>
    protected DbSet<Entity> entities { get; private set; }
    /// <summary>
    /// Конструктор класса
    /// </summary>
    public CrudRepository(PostgresDbContext db)
    {
        DB = db ?? throw new ArgumentNullException(nameof(db));
        entities = db.Set<Entity>();
    }

    /// <summary>
    /// Получить список элементов для грида
    /// </summary>
    /// <param name="options"></param>
    /// <returns></returns>
    public virtual async Task<Tuple<List<Entity>, long>> GetAll(GridOptionsDto options)
    {
        var query = GetQuery(options);
        return Tuple.Create(
            await query.OrderBy(x => x.Id)
                .Skip(options.GetSkip()).Take(options.GetTake())//Paginations
                .AsNoTracking().ToListAsync(),//To array (List)
            await query.LongCountAsync()
            );
    }

    protected virtual IQueryable<Entity> GetQuery(GridOptionsDto options)
    {
        var query = entities.AsQueryable();//Sorting base on ID
        if (options.Filters != null)
        {
            var restrictionParameter = Expression.Parameter(typeof(Entity), "p");
            Expression? whereExp = null;
            foreach (var filter in options.Filters)
            {
                var property = typeof(Entity).GetProperty(filter.PropertyName);
                if(property == null)
                {
                    continue; //exception ?
                }
                var propertyAccess = Expression.MakeMemberAccess(restrictionParameter, property);
                BinaryExpression? exp = null;
                switch (property.PropertyType.Name)
                {
                    case "Boolean":
                        {
                            exp = Expression.Equal(propertyAccess, Expression.Constant(bool.Parse(filter.Argument)));
                            break;
                        }
                    case "String":
                        {
                            if (filter.Type == "equal")
                            {
                                foreach(var item in filter.Argument.Split(','))
                                {
                                    var constExp = Expression.Constant(item);
                                    if (exp == null)
                                    {
                                        exp = Expression.Equal(propertyAccess, constExp);
                                    }
                                    else
                                    {
                                        exp = Expression.Or(exp, Expression.Equal(propertyAccess, constExp));
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                    case "Int64":
                        {
                            if (filter.Type == "equal")
                            {
                                foreach (var item in filter.Argument.Split(','))
                                {
                                    var constExp = Expression.Constant(long.Parse(item));
                                    if (exp == null)
                                    {
                                        exp = Expression.Equal(propertyAccess, constExp);
                                    }
                                    else
                                    {
                                        exp = Expression.Or(exp, Expression.Equal(propertyAccess, constExp));
                                    }
                                }
                            }
                            else
                            {
                                continue;
                            }
                            break;
                        }
                }
                if (whereExp == null)
                    whereExp = exp;
                else
                    whereExp = Expression.Add(whereExp, exp);
            }
            if(whereExp != null)
            {
                var lambdaExp = Expression.Lambda<Func<Entity, bool>>(whereExp, restrictionParameter);
                query = query.Where(lambdaExp);
            }
        }
        return query;
    }

    /// <summary>
    /// Получить элемент без отслеживания изменений
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task<Entity> GetItem(long id)
    {
        var item = await entities.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id);
        if(item == null)
        {
            throw new UserException($"Ошибка. Объект не найден в базе данных.");
        }
        else
        {
            return item;
        }
    }

    /// <summary>
    /// Отредактировать элемент или создать новый
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual async Task<long> EditItem(Entity item)
    {
        var itemDb = await entities
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == item.Id);

        if (itemDb == null)
        {//Create New
            if(item.Id < 0)//Отрицательные id не используем, 0 тогда создастся корректный id
                item.Id = 0;

            entities.Add(item);
        }
        else
        {//edit exist
            entities.Attach(item);
            DB.Entry(item).State = EntityState.Modified;
        }

        await DB.SaveChangesAsync();

        DB.Entry(item).State = EntityState.Detached;

        return item.Id;
    }

    /// <summary>
    /// Удалить элемент
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public virtual async Task DeleteItem(long id)
    {
        var item = await entities
            .FirstAsync(x => x.Id == id);
        entities.Remove(item);
        await DB.SaveChangesAsync();
    }

    /// <summary>
    /// Удалить список элементов
    /// </summary>
    /// <param name="ids"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public async Task DeleteItems(IEnumerable<long> ids)
    {
        var items = await entities
            .Where(x => ids.Contains(x.Id)).ToListAsync();
        entities.RemoveRange(items);
        await DB.SaveChangesAsync();
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Options;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Warehouse.Contracts.Api.Request;
using Warehouse.Contracts.Exceptions;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Warehouse.Infrastructure.Extentions;

internal static class IQueryableExtensions
{
    public static IQueryable<TEntity> ApplyFilters<TEntity>(this IQueryable<TEntity> query, List<FilterDto> filters)
    {
        var restrictionParameter = Expression.Parameter(typeof(TEntity), "p");
        Expression? whereExp = null;
        foreach (var filter in filters)
        {
            Expression? propertyExpression;
            PropertyInfo? property;

            var result = MakePropertyExpression<TEntity>(filter.PropertyName, restrictionParameter, typeof(TEntity), filter);

            propertyExpression = result.Item1;
            property = result.Item2;

            if (propertyExpression == null)
            {
                continue;
            }

            if (whereExp == null)
                whereExp = propertyExpression;
            else
                whereExp = Expression.And(whereExp, propertyExpression);
        }
        if (whereExp != null)
        {
            var lambdaExp = Expression.Lambda<Func<TEntity, bool>>(whereExp, restrictionParameter);
            query = query.Where(lambdaExp);
        }
        return query;
    }

    private static Tuple<Expression?, PropertyInfo?> MakePropertyExpression<TEntity>(string propertyPath, Expression propertyAccess, Type currentType, FilterDto filter)
    {
        PropertyInfo? property = null;
        foreach (var propertyName in propertyPath.Split('.'))
        {
            property = currentType.GetProperty(propertyName);
            if (property == null)
            {
                return Tuple.Create<Expression?, PropertyInfo?>(null, null);
            }

            propertyAccess = Expression.MakeMemberAccess(propertyAccess, property);
            currentType = property.PropertyType;

            if (currentType.GetInterface(nameof(IEnumerable)) != null && currentType != typeof(String))
            {
                Type genericArgument = currentType.GetGenericArguments().First();
                MethodInfo anyMethod = typeof(Enumerable).GetMethods()
                    .Where(m => m.Name == "Any" && m.GetParameters().Length == 2)
                    .Single().MakeGenericMethod(genericArgument);

                ParameterExpression elementParam = Expression.Parameter(genericArgument, "e");
                int nextDot = propertyPath.IndexOf('.', propertyPath.IndexOf(propertyName) + 1);
                var result = MakePropertyExpression<TEntity>(propertyPath.Substring(nextDot + 1), elementParam, genericArgument, filter);
                if (result.Item1 == null)
                {
                    return Tuple.Create<Expression?, PropertyInfo?>(null, null);
                }
                Expression innerCondition = result.Item1;

                LambdaExpression lambda = Expression.Lambda(innerCondition, elementParam);
                propertyAccess = Expression.Call(anyMethod, propertyAccess, lambda);

                return Tuple.Create(propertyAccess, result.Item2);
            }
        }
        Expression? exp = MakeEndExpression(filter.Argument, filter.Type, propertyAccess, property);
        return Tuple.Create(exp, property);
    }

    private static Expression? MakeEndExpression(string argument, string type, Expression propertyAccess, PropertyInfo? property)
    {
        Expression? exp = null;
        if (type == "equal")
        {
            foreach (var item in argument.Split(','))
            {
                //Значение фильтра приходит от клиента строкой: некорректный ввод
                //превращаем в понятную 400-ошибку, а не в FormatException -> 500
                object value;
                try
                {
                    value = Convert.ChangeType(item, propertyAccess.Type);
                }
                catch (Exception ex) when (ex is FormatException or OverflowException or InvalidCastException)
                {
                    throw new UserException(
                        $"Ошибка. Некорректное значение фильтра <{item}> для поля <{property?.Name}>.");
                }
                var constExp = Expression.Constant(value);
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
        else if (type == "dateRange")
        {
            var args = argument.Split(',');
            if (args.Length < 2)
            {
                throw new UserException(
                    $"Ошибка. Некорректное значение фильтра <{argument}> для поля <{property?.Name}>: ожидаются две даты через запятую.");
            }
            Expression? startExp = null;
            Expression? endExp = null;
            if (args[0] != "undefined")
            {
                DateOnly start = ParseFilterDate(args[0], property);
                startExp = Expression.GreaterThanOrEqual(propertyAccess, Expression.Constant(start));
            }
            if (args[1] != "undefined")
            {
                DateOnly end = ParseFilterDate(args[1], property);
                endExp = Expression.LessThanOrEqual(propertyAccess, Expression.Constant(end));
            }

            if (startExp != null && endExp != null)
                exp = Expression.And(startExp, endExp);
            else if (startExp != null)
            {
                exp = startExp;
            }
            else if (endExp != null)
            {
                exp = endExp;
            }
        }
        return exp;
    }

    private static DateOnly ParseFilterDate(string value, PropertyInfo? property)
    {
        //Аналогично: дата от клиента парсится строго, ошибка ввода -> 400
        if (!DateOnly.TryParse(value, out var date))
        {
            throw new UserException(
                $"Ошибка. Некорректная дата <{value}> в фильтре по полю <{property?.Name}>.");
        }
        return date;
    }
}

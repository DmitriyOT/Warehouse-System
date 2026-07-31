using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Tests.TestInfrastructure;

public class TestDashboardRepository : IDashboardRepository
{
    public List<BalanceEntity> BalanceList { get; } = new List<BalanceEntity>();

    public List<IncomeEntity> IncomeList { get; } = new List<IncomeEntity>();

    public List<ShipmentEntity> ShipmentList { get; } = new List<ShipmentEntity>();

    public List<ClientEntity> ClientList { get; } = new List<ClientEntity>();

    //IQueryable оборачиваются в TestAsyncEnumerable, чтобы в тестах работали
    //async-расширения EF Core (ToListAsync/CountAsync/SumAsync) поверх списков в памяти
    public IQueryable<BalanceEntity> Balances => new TestAsyncEnumerable<BalanceEntity>(BalanceList);

    public IQueryable<IncomeEntity> Incomes => new TestAsyncEnumerable<IncomeEntity>(IncomeList);

    public IQueryable<IncomeItemEntity> IncomeItems => new TestAsyncEnumerable<IncomeItemEntity>(IncomeList
        .SelectMany(x => x.IncomeItems ?? new List<IncomeItemEntity>()));

    public IQueryable<ShipmentEntity> Shipments => new TestAsyncEnumerable<ShipmentEntity>(ShipmentList);

    public IQueryable<ShipmentItemEntity> ShipmentItems => new TestAsyncEnumerable<ShipmentItemEntity>(ShipmentList
        .SelectMany(x => x.ShipmentItems ?? new List<ShipmentItemEntity>()));

    public IQueryable<ClientEntity> Clients => new TestAsyncEnumerable<ClientEntity>(ClientList);
}

/// <summary>
/// Обёртка над перечислением в памяти с поддержкой IAsyncEnumerable:
/// позволяет async-расширениям EF Core выполняться над тестовыми списками
/// </summary>
internal class TestAsyncEnumerable<T> : EnumerableQuery<T>, IAsyncEnumerable<T>, IQueryable<T>
{
    public TestAsyncEnumerable(IEnumerable<T> enumerable) : base(enumerable)
    {
    }

    public TestAsyncEnumerable(Expression expression) : base(expression)
    {
    }

    public IAsyncEnumerator<T> GetAsyncEnumerator(CancellationToken cancellationToken = default)
        => new TestAsyncEnumerator<T>(this.AsEnumerable().GetEnumerator());

    IQueryProvider IQueryable.Provider => new TestAsyncQueryProvider<T>(this);
}

/// <summary>
/// Синхронный перечислитель с асинхронной обёрткой для тестов
/// </summary>
internal class TestAsyncEnumerator<T> : IAsyncEnumerator<T>
{
    private readonly IEnumerator<T> _inner;

    public TestAsyncEnumerator(IEnumerator<T> inner)
    {
        _inner = inner;
    }

    public T Current => _inner.Current;

    public ValueTask DisposeAsync()
    {
        _inner.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask<bool> MoveNextAsync()
        => new ValueTask<bool>(_inner.MoveNext());
}

/// <summary>
/// Провайдер запросов, выполняющий async-операции EF Core синхронно в памяти
/// </summary>
internal class TestAsyncQueryProvider<TEntity> : IAsyncQueryProvider
{
    private readonly IQueryProvider _inner;

    public TestAsyncQueryProvider(IQueryProvider inner)
    {
        _inner = inner;
    }

    public IQueryable CreateQuery(Expression expression)
        => new TestAsyncEnumerable<TEntity>(expression);

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
        => new TestAsyncEnumerable<TElement>(expression);

    public object? Execute(Expression expression)
        => _inner.Execute(expression);

    public TResult Execute<TResult>(Expression expression)
        => _inner.Execute<TResult>(expression);

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        //Выполняем запрос синхронно и заворачиваем результат в Task<T>,
        //который ожидают async-расширения EF Core
        var expectedResultType = typeof(TResult).GetGenericArguments()[0];
        var executionResult = typeof(IQueryProvider)
            .GetMethod(nameof(IQueryProvider.Execute), genericParameterCount: 1, types: new[] { typeof(Expression) })!
            .MakeGenericMethod(expectedResultType)
            .Invoke(this, new object?[] { expression });

        return (TResult)typeof(Task).GetMethod(nameof(Task.FromResult))!
            .MakeGenericMethod(expectedResultType)
            .Invoke(null, new[] { executionResult })!;
    }
}

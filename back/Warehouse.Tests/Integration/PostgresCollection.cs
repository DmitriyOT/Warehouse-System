namespace Warehouse.Tests.Integration;

/// <summary>
/// Коллекция тестов, использующих общий контейнер PostgreSQL.
/// Тесты внутри коллекции выполняются последовательно, контейнер поднимается один раз.
/// </summary>
[CollectionDefinition(Name)]
public class PostgresCollection : ICollectionFixture<PostgresFixture>
{
    public const string Name = "Postgres";
}

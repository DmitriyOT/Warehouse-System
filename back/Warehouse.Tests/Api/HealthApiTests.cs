using System.Net;
using Warehouse.Tests.Integration;

namespace Warehouse.Tests.Api;

/// <summary>
/// API-тесты health-эндпоинтов (доступны без токена).
/// </summary>
[Collection(PostgresCollection.Name)]
public class HealthApiTests : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;

    public HealthApiTests(ApiFixture fixture)
    {
        _fixture = fixture;
    }

    [SkippableFact]
    public async Task Health_WithoutToken_Returns200()
    {
        Skip.IfNot(_fixture.IsAvailable, _fixture.UnavailableReason);

        var response = await _fixture.Client!.GetAsync("/health");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [SkippableFact]
    public async Task HealthReady_WithoutToken_Returns200()
    {
        Skip.IfNot(_fixture.IsAvailable, _fixture.UnavailableReason);

        var response = await _fixture.Client!.GetAsync("/health/ready");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }
}

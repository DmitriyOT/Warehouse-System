using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using Warehouse.Tests.Integration;

namespace Warehouse.Tests.Api;

/// <summary>
/// API-тесты аутентификации: POST /Auth/login.
/// </summary>
[Collection(PostgresCollection.Name)]
public class AuthApiTests : IClassFixture<ApiFixture>
{
    private readonly ApiFixture _fixture;

    public AuthApiTests(ApiFixture fixture)
    {
        _fixture = fixture;
    }

    [SkippableFact]
    public async Task Login_ValidAdminCredentials_Returns200AndToken()
    {
        Skip.IfNot(_fixture.IsAvailable, _fixture.UnavailableReason);

        var token = await LoginAndGetToken(WarehouseApiFactory.AdminLogin, WarehouseApiFactory.AdminPassword);

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [SkippableFact]
    public async Task Login_WrongPassword_Returns401()
    {
        Skip.IfNot(_fixture.IsAvailable, _fixture.UnavailableReason);

        var response = await _fixture.Client!.PostAsJsonAsync("/Auth/login", new
        {
            login = WarehouseApiFactory.AdminLogin,
            password = "wrong-password"
        });

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    /// <summary>
    /// Выполнить вход и извлечь JWT-токен из ResponseDto&lt;LoginResponseDto&gt;
    /// </summary>
    internal static async Task<string> LoginAndGetToken(HttpClient client, string login, string password)
    {
        var response = await client.PostAsJsonAsync("/Auth/login", new { login, password });
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var json = await response.Content.ReadAsStringAsync();
        using var document = JsonDocument.Parse(json);
        var root = document.RootElement;
        var responseElement = root.TryGetProperty("response", out var camel)
            ? camel
            : root.GetProperty("Response");
        var token = responseElement.TryGetProperty("token", out var tokenElement)
            ? tokenElement.GetString()
            : responseElement.GetProperty("Token").GetString();

        return token ?? string.Empty;
    }

    private async Task<string> LoginAndGetToken(string login, string password)
    {
        return await LoginAndGetToken(_fixture.Client!, login, password);
    }
}

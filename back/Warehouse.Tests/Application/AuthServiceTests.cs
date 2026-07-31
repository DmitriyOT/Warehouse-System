using Microsoft.Extensions.Configuration;
using Warehouse.Application.Services;
using Warehouse.Contracts.Exceptions;
using Warehouse.Tests.TestInfrastructure;

namespace Warehouse.Tests.Application;

public class AuthServiceTests
{
    private static AuthService CreateService(TestUserRepository userRepository)
    {
        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string?>
            {
                ["Jwt:Key"] = "unit-test-jwt-key-at-least-32-characters-long",
                ["Jwt:Issuer"] = "Warehouse.Api",
                ["Jwt:Audience"] = "Warehouse.Client",
                ["Jwt:ExpiresMinutes"] = "60"
            })
            .Build();
        return new AuthService(userRepository, configuration);
    }

    [Fact]
    public async Task Login_ValidCredentials_ReturnsToken()
    {
        var userRepository = new TestUserRepository();
        var authService = CreateService(userRepository);
        await authService.SeedAdmin("admin", "secret");

        var token = await authService.Login("admin", "secret");

        Assert.False(string.IsNullOrWhiteSpace(token));
    }

    [Fact]
    public async Task Login_WrongPassword_ThrowsUserException()
    {
        var userRepository = new TestUserRepository();
        var authService = CreateService(userRepository);
        await authService.SeedAdmin("admin", "secret");

        await Assert.ThrowsAsync<UserException>(() => authService.Login("admin", "wrong"));
    }

    [Fact]
    public async Task Login_UnknownLogin_ThrowsUserException()
    {
        var userRepository = new TestUserRepository();
        var authService = CreateService(userRepository);
        await authService.SeedAdmin("admin", "secret");

        await Assert.ThrowsAsync<UserException>(() => authService.Login("unknown", "secret"));
    }

    [Fact]
    public async Task SeedAdmin_UsersExist_DoesNotAddSecondUser()
    {
        var userRepository = new TestUserRepository();
        var authService = CreateService(userRepository);
        await authService.SeedAdmin("admin", "secret");
        await authService.SeedAdmin("admin2", "secret2");

        // Второй пользователь не создан: вход под admin2 невозможен
        await Assert.ThrowsAsync<UserException>(() => authService.Login("admin2", "secret2"));
    }
}

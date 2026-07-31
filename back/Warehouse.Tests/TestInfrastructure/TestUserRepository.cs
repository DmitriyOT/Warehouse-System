using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Tests.TestInfrastructure;

public class TestUserRepository : IUserRepository
{
    private readonly List<UserEntity> _users = new();

    public Task<UserEntity?> GetByLogin(string login)
    {
        return Task.FromResult(_users.FirstOrDefault(u => u.Login == login));
    }

    public Task<bool> HasAnyUser()
    {
        return Task.FromResult(_users.Count > 0);
    }

    public Task Add(UserEntity user)
    {
        user.Id = _users.Count == 0 ? 1 : _users.Max(u => u.Id) + 1;
        _users.Add(user);
        return Task.CompletedTask;
    }
}

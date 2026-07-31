using Microsoft.EntityFrameworkCore;
using Warehouse.Contracts.Infrastructure;
using Warehouse.Domain.Models;

namespace Warehouse.Infrastructure.Db.Repository;

/// <summary>
/// Репозиторий пользователей на EF Core
/// </summary>
public class UserRepository : IUserRepository
{
    private readonly PostgresDbContext _context;

    /// <summary>
    /// Конструктор
    /// </summary>
    /// <param name="context"></param>
    public UserRepository(PostgresDbContext context)
    {
        _context = context;
    }

    /// <inheritdoc />
    public Task<UserEntity?> GetByLogin(string login)
    {
        return _context.Users.FirstOrDefaultAsync(u => u.Login == login);
    }

    /// <inheritdoc />
    public Task<bool> HasAnyUser()
    {
        return _context.Users.AnyAsync();
    }

    /// <inheritdoc />
    public async Task Add(UserEntity user)
    {
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }
}

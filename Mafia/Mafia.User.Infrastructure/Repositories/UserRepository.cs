using DomainUser = Mafia.User.Domain.Aggregates.User;
using Mafia.User.Application.Contracts;

namespace Mafia.User.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly List<DomainUser> _users = new(); // Для примера, вместо БД

    public Task<DomainUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        => Task.FromResult(_users.FirstOrDefault(u => u.Id == id));

    public Task AddAsync(DomainUser user, CancellationToken cancellationToken = default)
    {
        _users.Add(user);
        return Task.CompletedTask;
    }

    public Task UpdateAsync(DomainUser user, CancellationToken cancellationToken = default)
        => Task.CompletedTask; // В памяти ничего не делаем

    public Task DeleteAsync(DomainUser user, CancellationToken cancellationToken = default)
    {
        _users.Remove(user);
        return Task.CompletedTask;
    }

    public Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        return Task.FromResult(_users.Any(u => u.Email.Value == email));
    }

    public Task<DomainUser?> GetByEmailAsync(string email, CancellationToken cancellationToken)
    {
        var user = _users.FirstOrDefault(u => u.Email.Value == email);
        return Task.FromResult(user);
    }
}

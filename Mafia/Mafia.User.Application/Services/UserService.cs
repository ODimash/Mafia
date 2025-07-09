using System;
using DomainUser = Mafia.User.Domain.Aggregates.User;
using Mafia.User.Application.Contracts;

namespace Mafia.User.Application.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }

    public async Task<DomainUser?> GetUserAsync(Guid id, CancellationToken cancellationToken = default)
        => await _repository.GetByIdAsync(id, cancellationToken);

    public async Task CreateUserAsync(string userName, string email, CancellationToken cancellationToken = default)
    {
        var user = new DomainUser(Guid.NewGuid(), userName, email);
        await _repository.AddAsync(user, cancellationToken);
    }

    public async Task ChangeEmailAsync(Guid id, string newEmail, CancellationToken cancellationToken = default)
    {
        var user = await _repository.GetByIdAsync(id, cancellationToken);
        if (user is null) throw new Exception("User not found");
        user.ChangeEmail(newEmail);
        await _repository.UpdateAsync(user, cancellationToken);
    }
}

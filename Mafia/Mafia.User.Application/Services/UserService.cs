using System;
using DomainUser = Mafia.User.Domain.Aggregates.User;
using Mafia.User.Application.Contracts;
using FluentResults;
using Mafia.User.Domain.ValueObjects;

namespace Mafia.User.Application.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }
    public async Task<Result<DomainUser>> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess)
            return Result.Fail<DomainUser>(emailResult.Errors);

        var passwordResult = PasswordHash.Create(password);
        if (!passwordResult.IsSuccess)
            return Result.Fail<DomainUser>(passwordResult.Errors);

        var user = await _repository.GetByEmailAsync(emailResult.Value.Value, cancellationToken);
        if (user is null)
            return Result.Fail<DomainUser>("Пользователь с таким email не найден.");

        if (user.PasswordHash != passwordResult.Value)
            return Result.Fail<DomainUser>("Неверный пароль.");

        return Result.Ok(user);
    }
    public async Task<Result> RegistrationUser(string userName, string gender, string email, string password, CancellationToken cancellationToken)
    {

        return Result.Ok();
    }

}

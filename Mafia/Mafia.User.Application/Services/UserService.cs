using System;
using DomainUser = Mafia.User.Domain.Aggregates.User;
using Mafia.User.Application.Contracts;
using FluentResults;
using Mafia.User.Domain.ValueObjects;
using Mafia.User.Application.DTOs;

namespace Mafia.User.Application.Services;

public class UserService
{
    private readonly IUserRepository _repository;

    public UserService(IUserRepository repository)
    {
        _repository = repository;
    }


    public async Task<Result<Guid>> CreateUserAsync(string userName, string email, string password,  CancellationToken cancellationToken = default)
    {
        var userResult = DomainUser.Register(userName, email, password);
        if (!userResult.IsSuccess)
            return Result.Fail<Guid>(userResult.Errors);

        var user = userResult.Value;

        await _repository.AddAsync(user, cancellationToken);

        return Result.Ok(user.Id);
    }

    public async Task<Result<UserDto>> GetUserAsync(Guid id)
    {
       
        var user = await _repository.GetByIdAsync(id);
        if (user == null)
        {
            return Result.Fail("Пользователь не найден.");
        }
        return Result.Ok(new UserDto
        {
            Email = user.Email.Value,
            UserName = user.Username.Value,
        });
    }

    public async Task<Result> LoginAsync(string email, string password, CancellationToken cancellationToken = default)
    {
        var emailResult = Email.Create(email);
        if (!emailResult.IsSuccess)
            return Result.Fail(emailResult.Errors);

        var passwordResult = PasswordHash.Create(password);
        if (!passwordResult.IsSuccess)
            return Result.Fail(passwordResult.Errors);

        var user = await _repository.GetByEmailAsync(emailResult.Value.Value, cancellationToken);
        if (user is null)
            return Result.Fail("Пользователь с таким email не найден.");

        if (user.PasswordHash != passwordResult.Value)
            return Result.Fail("Неверный пароль.");

        return Result.Ok();
    }
   

}

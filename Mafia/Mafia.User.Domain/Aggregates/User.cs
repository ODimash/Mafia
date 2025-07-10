using FluentResults;
using Mafia.Shared.Kernel;
using Mafia.User.Domain.ValueObjects;
using System.Collections.Generic;

namespace Mafia.User.Domain.Aggregates
{
    public class User : AggregateRoot<Guid>
    {
        public Username Username { get; private set; }
        public Email Email { get; }
        public bool EmailConfirmed { get; private set; }
        public PasswordHash PasswordHash { get; private set; }
        public UserStats Stats { get; private set; }


        private User(Guid id, Username username, Email email, PasswordHash passwordHash)
        {
            Id = id;
            Username = username;
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Email = email;
            EmailConfirmed = false;
            Stats = new UserStats();
            AddDomainEvent(new UserRegisteredDomainEvent(Id, Username, email)); // Use a protected method to add domain events  
        }

        public static Result<User> Register(string username, string email, string password, string confirmPassword)
        {
            List<Error> errors = new();
            if (password != confirmPassword)
                errors.Add(new Error("Password and confirmation password do not match.").WithMetadata("ErrorCode", "NotMatch").WithMetadata("Field", "confirmPassword"));

            var createdEmail = Email.Create(email);
            var createdUsername = Username.Create(username);
            var createdPassword = PasswordHash.Create(password);

            if (!createdEmail.IsSuccess)
                errors.AddRange(ConvertErrors(createdEmail.Errors));
            if (!createdPassword.IsSuccess)
                errors.AddRange(ConvertErrors(createdPassword.Errors));
            if (!createdUsername.IsSuccess)
                errors.AddRange(ConvertErrors(createdUsername.Errors));

            if (errors.Count > 0)
                return Result.Fail<User>(errors);

            var user = new User(
              Guid.NewGuid(),
              createdUsername.Value,
              createdEmail.Value,
              createdPassword.Value);

            return Result.Ok(user);
        }
        public void RegisterGame(bool isWin)
        {
            Stats.RegisterGame(isWin);
        }

        private static IEnumerable<Error> ConvertErrors(IEnumerable<IError> iErrors)
        {
            foreach (var iError in iErrors)
            {
                if (iError is Error error)
                {
                    yield return error;
                }
                else
                {
                    yield return new Error(iError.Message).WithMetadata(iError.Metadata);
                }
            }
        }
    }
}
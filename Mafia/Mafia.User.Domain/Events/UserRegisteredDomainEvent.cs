using Mafia.User.Domain.ValueObjects;
using Mafia.Shared.Kernel;

namespace Mafia.User.Domain.Aggregates;

public class UserRegisteredDomainEvent : IDomainEvent
{
    public Guid UserId { get; }
    public Username Username { get; }
    public Email Email { get; }
    public DateTime OccurredOn { get; }

    public UserRegisteredDomainEvent(Guid userId, Username username, Email email)
    {
        UserId = userId;
        Username = username;
        Email = email;
        OccurredOn = DateTime.UtcNow;
    }
}
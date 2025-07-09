//using Mafia.Shared.Kernel;

using Mafia.Shared.Kernel;

//namespace Mafia.User.Domain.Aggregates;
namespace Mafia.User.Domain.Aggregates
{
    public class User : AggregateRoot<Guid>
    {
        public string UserName { get; private set; }
        public string Email { get; private set; }

        // Для EF/ORM
        private User() { }

        public User(Guid id, string userName, string email)
        {
            Id = id;
            UserName = userName;
            Email = email;
            CreatedAt = DateTime.UtcNow;
        }

        public void ChangeEmail(string newEmail)
        {
            Email = newEmail;
            // AddDomainEvent(new UserEmailChangedEvent(Id, newEmail)); // пример доменного события
        }
    }
}
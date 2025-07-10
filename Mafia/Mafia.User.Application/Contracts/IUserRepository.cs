using DomainUser = Mafia.User.Domain.Aggregates.User;

namespace Mafia.User.Application.Contracts;

public interface IUserRepository
{
    Task<DomainUser?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default); // Fully qualify the User type
    Task AddAsync(DomainUser user, CancellationToken cancellationToken = default); // Fully qualify the User type
    Task UpdateAsync(DomainUser user, CancellationToken cancellationToken = default); // Fully qualify the User type
    Task DeleteAsync(DomainUser user, CancellationToken cancellationToken = default); // Fully qualify the User type
    Task<bool> ExistsByEmailAsync(string email, CancellationToken cancellationToken = default); // Fully qualify the User type
}

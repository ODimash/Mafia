using Mafia.Games.Domain.Entities;

namespace Mafia.Games.Abstraction.Repositories;
public interface IGameQueryRepository
{
    Task<Game?> GetGameById(Guid id);
}

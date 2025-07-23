using Mafia.Games.Domain.Models;

namespace Mafia.Games.Abstraction.Repositories;

public interface IGameCommandRepository
{
    Task AddGame(Game value, CancellationToken cancellationToken);
    Task<Game?> GetGameById(Guid requestGameId, CancellationToken cancellationToken);
    Task Update(Game game, CancellationToken cancellationToken);
}
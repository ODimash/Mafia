


using Mafia.Game.Domain.Entities;

namespace Mafia.Game.Abstraction.Repositories
{
    public interface IGameSessionCommandRepository
    {
        Task AddGame(GameSession value, CancellationToken cancellationToken);
        Task<GameSession?> GetGameById(Guid requestGameId, CancellationToken cancellationToken);
        Task Update(GameSession game, CancellationToken cancellationToken);
    }
}

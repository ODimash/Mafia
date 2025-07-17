


using Mafia.Game.Domain.Entities;

namespace Mafia.Game.Abstraction.Repositories
{
    public interface IGameSessionCommandRepository
    {
        Task AddGameAsync(GameSession value, CancellationToken cancellationToken);
    }
}

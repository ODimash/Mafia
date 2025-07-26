using Mafia.Games.Domain.Models;

namespace Mafia.Games.Abstraction.Repositories;

public interface IGameQueryRepository
{
	Task<Game?> GetGameById(Guid id, CancellationToken cancellationToken = default);
}

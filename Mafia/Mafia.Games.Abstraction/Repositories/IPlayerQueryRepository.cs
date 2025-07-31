
using Mafia.Games.Contracts.DTOs;

namespace Mafia.Games.Abstraction.Repositories;

public interface IPlayerQueryRepository
{
	Task<List<PlayerDto>> GetPlayersByGameId(Guid gameId, CancellationToken cancellationToken = default);
}
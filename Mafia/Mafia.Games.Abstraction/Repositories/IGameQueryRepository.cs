using Mafia.Games.Domain.Models;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;

namespace Mafia.Games.Abstraction.Repositories;

public interface IGameQueryRepository
{
	// Task<Game?> GetGameById(Guid id, CancellationToken cancellationToken = default);
	
	Task<List<PlayerDto>> GetPlayersByGameId(Guid gameId, CancellationToken cancellationToken = default);
	Task<Guid> GetPlayerIdByIdentityId(Guid identityId, Guid gameId, CancellationToken cancellationToken = default);
}

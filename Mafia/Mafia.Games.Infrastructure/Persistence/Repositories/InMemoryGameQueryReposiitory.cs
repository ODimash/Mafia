using AutoMapper;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;

namespace Mafia.Games.Infrastructure.Persistence.Repositories;

public class InMemoryGameQueryReposiitory : IGameQueryRepository
{
	private readonly IGameCommandRepository _gameCommandRepository;
	private readonly IMapper  _mapper;
	public InMemoryGameQueryReposiitory(IGameCommandRepository gameCommandRepository, IMapper mapper)
	{
		_gameCommandRepository = gameCommandRepository;
		_mapper = mapper;
	}

	public async Task<List<PlayerDto>> GetPlayersByGameId(Guid gameId, CancellationToken cancellationToken = default)
	{
		var game = await _gameCommandRepository.GetGameById(gameId, cancellationToken);
		if (game == null)
			return [];
		
		return _mapper.Map<List<PlayerDto>>(game.Players);
	}
	
	public async Task<Guid> GetPlayerIdByIdentityId(Guid identityId, Guid gameId, CancellationToken cancellationToken = default)
	{
		var game = await _gameCommandRepository.GetGameById(gameId, cancellationToken);
		if (game == null)
			return Guid.Empty;

		var player = game.Players.FirstOrDefault(p => p.IdentityId == identityId);
		if (player == null)
			return Guid.Empty;
		
		return player.Id;
	}
}

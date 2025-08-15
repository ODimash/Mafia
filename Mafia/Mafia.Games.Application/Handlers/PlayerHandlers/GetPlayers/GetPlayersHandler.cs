using AutoMapper;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayers;

public class GetPlayersHandler : IQueryHandler<GetPlayersQuery,  List<PlayerDto>>
{
	private readonly IGameQueryRepository  _repository;
	
	public GetPlayersHandler(IGameQueryRepository repository)
	{
		_repository = repository;
	}

	public async Task<List<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
	{
		return await _repository.GetPlayersByGameId(request.GameId, cancellationToken);
	}
}

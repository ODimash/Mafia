using AutoMapper;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayers;

public class GetPlayersHandler : IQueryHandler<GetPlayersQuery,  List<PlayerDto>>
{
	private readonly IPlayerQueryRepository  _repository;
	private readonly IMapper  _mapper;
	
	public GetPlayersHandler(IPlayerQueryRepository repository, IMapper mapper)
	{
		_repository = repository;
		_mapper = mapper;
	}

	public async Task<List<PlayerDto>> Handle(GetPlayersQuery request, CancellationToken cancellationToken)
	{
		return await _repository.GetPlayersByGameId(request.GameId, cancellationToken);
	}
}

using Mafia.Games.Abstraction.Repositories;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayerIdByIdentityId;

public class GetPlayerIdByIdentityIdHandler : IQueryHandler<GetPlayerIdByIdentityIdQuery, Guid>
{
	private readonly IGameQueryRepository  _playerQueryRepository;
	
	
	public GetPlayerIdByIdentityIdHandler(IGameQueryRepository playerQueryRepository)
	{
		_playerQueryRepository = playerQueryRepository;
	}

	public Task<Guid> Handle(GetPlayerIdByIdentityIdQuery request, CancellationToken cancellationToken)
	{
		return _playerQueryRepository.GetPlayerIdByIdentityId(request.GameId, request.IdentityId, cancellationToken);
	}
}

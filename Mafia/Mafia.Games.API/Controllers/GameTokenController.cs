using Mafia.Games.API.Extensions;
using Mafia.Games.API.Tokens.GameToken;
using Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayerIdByIdentityId;
using Mafia.Shared.API.Models;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mafia.Games.API.Controllers;

[ApiController]
[Route("api/Games")]
public class GameTokenController : ControllerBase
{
	private readonly IGameTokenManager _tokenManager;
	private readonly IMediator _mediator;

	public GameTokenController(IGameTokenManager tokenManager, IMediator mediator)
	{
		_tokenManager = tokenManager;
		_mediator = mediator;
	}

	[HttpPost("{gameId}/Token")]
	public async Task<ResponseModel<string>> GetToken(Guid gameId)
	{
		var userId = User.GetUserId();
		var playerId = await _mediator.Send(new GetPlayerIdByIdentityIdQuery(gameId, userId));

		if (playerId == Guid.Empty)
			return ResponseModel<string>.Fail(["You are unauthorized"]);

		var token = _tokenManager.GenerateToken(gameId, playerId);
		return ResponseModel<string>.Ok(token);
	}
}

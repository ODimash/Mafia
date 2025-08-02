using Mafia.Games.API.Extensions;
using Mafia.Games.API.Tokens.GameToken;
using Mafia.Games.Application.Handlers.PlayerHandlers.GetPlayerIdByIdentityId;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Mafia.Games.API.Controllers;

[ApiController]
[Route("api/game")]
public class GameTokenController : ControllerBase
{
	private readonly IGameTokenManager _tokenManager;
	private readonly IMediator _mediator;

	public GameTokenController(IGameTokenManager tokenManager, IMediator mediator)
	{
		_tokenManager = tokenManager;
		_mediator = mediator;
	}

	[HttpPost("{gameId}/token")]
	public async Task<ActionResult<string>> GetToken(Guid gameId)
	{
		var userId = User.GetUserId();
		var playerId = await _mediator.Send(new GetPlayerIdByIdentityIdQuery(gameId, userId));

		if (playerId == Guid.Empty)
			return Unauthorized();

		var token = _tokenManager.GenerateToken(gameId, playerId);
		return Ok(token);
	}
}

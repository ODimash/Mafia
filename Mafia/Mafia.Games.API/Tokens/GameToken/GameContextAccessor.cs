using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace Mafia.Games.API.Tokens.GameToken;

public interface IGameContextAccessor
{
	Guid? GameId { get; }
	Guid? PlayerId { get; }
	bool IsPlayer { get; }
}

public class GameContextAccessor : IGameContextAccessor
{
	public Guid? GameId { get; }
	public Guid? PlayerId { get; }
	public bool IsPlayer { get; }

	public GameContextAccessor(IHttpContextAccessor accessor, IGameTokenManager tokenManager)
	{
		var context = accessor.HttpContext;
		if (context == null || !context.Request.Headers.TryGetValue("GameToken", out var token))
		{
			IsPlayer = false;
			return;
		}

		var principal = tokenManager.Validate(token!);

		var gameId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
		var playerId = principal.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Actor)?.Value;

		var gameIdExist = Guid.TryParse(gameId, out var gid);
		var playerIdExist = Guid.TryParse(playerId, out var pid);

		IsPlayer = gameIdExist && playerIdExist;

		if (IsPlayer)
		{
			PlayerId = pid;
			GameId = gid;
		}
	}
}

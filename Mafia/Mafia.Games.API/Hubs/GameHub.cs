using Mafia.Games.API.Tokens.GameToken;
using Microsoft.AspNetCore.SignalR;

namespace Mafia.Games.API.Hubs;

public sealed class GameHub : Hub
{
	private readonly IGameContextAccessor  _gameContext;
	
	public GameHub(IGameContextAccessor gameContextAccessor)
	{
		_gameContext = gameContextAccessor;
	}

	public async override Task OnConnectedAsync()
	{
		Guid gameId = _gameContext.GameId ?? GetGameIdFromQuery();

		if (gameId == Guid.Empty)
			return;
		
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
		
		if (_gameContext.IsPlayer)
			await Groups.AddToGroupAsync(Context.ConnectionId, _gameContext.PlayerId!.Value.ToString());

		await base.OnConnectedAsync();
	}

	private Guid GetGameIdFromQuery()
	{
		var httpContext = Context.GetHttpContext();
		if (httpContext == null)
			return Guid.Empty;
		
		if (!httpContext.Request.Query.TryGetValue("gameId", out var gameIdAsString)) 
			return Guid.Empty;
		
		if (Guid.TryParse(gameIdAsString, out var gameId))
			return gameId;
		
		return Guid.Empty;
	}
}

using Mafia.Games.API.Extensions;
using Mafia.Games.API.Tokens.GameToken;
using Microsoft.AspNetCore.SignalR;
using System.Security.Claims;

namespace Mafia.Games.API.Hubs;

public sealed class GameHub : Hub
{
	private readonly IGameContextAccessor  _gameContextAccessor;
	
	public GameHub(IGameContextAccessor gameContextAccessor)
	{
		_gameContextAccessor = gameContextAccessor;
	}

	public override async Task OnConnectedAsync()
	{
		var httpContext = Context.GetHttpContext();
		if (httpContext == null)
			return;
		
		if (!httpContext.Request.Query.TryGetValue("gameId", out var gameIdAsString)) 
			return;
		
		if (!Guid.TryParse(gameIdAsString, out var gameId))
			return;
		
		await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
		
		if (httpContext.User.Identity == null || !httpContext.User.Identity.IsAuthenticated)
			return;

		var userId = httpContext.User.GetUserId();
		
		
		await base.OnConnectedAsync();
	}
}

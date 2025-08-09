
using Mefia.Shared.Infrastructure.Extensions;
using System.Text.RegularExpressions;

namespace Mafia.Lobby.API.Hubs;
using Microsoft.AspNetCore.SignalR;

public class RoomsHub : Hub
{

	public async override Task OnConnectedAsync()
	{
		var httpContext = Context.GetHttpContext();
		if (httpContext == null)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "Hall");
			await base.OnConnectedAsync();
			return;
		}
		
		var isAuthenticated = httpContext.User?.Identity?.IsAuthenticated == true;
		if (!isAuthenticated)
		{
			await Groups.AddToGroupAsync(Context.ConnectionId, "Hall");
		}
		else
		{
			var hasRoomId = httpContext.Request.Query.TryGetValue("roomId", out var roomId);
			if (hasRoomId)
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, $"room-{roomId.ToString()}");
				await Groups.AddToGroupAsync(Context.ConnectionId, $"player-{Context.User.GetUserId()}");
			}
			else
			{
				await Groups.AddToGroupAsync(Context.ConnectionId, "Hall");
			}
		}
		
		await base.OnConnectedAsync();
	}
	
}

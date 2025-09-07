using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Shared.Infrastructure.Notifiers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Mafia.Lobby.API.Hubs.Notifiers;

public class LobbyNotifier : HubNotifier<LobbyHub>, ILobbyNotifier
{

	public LobbyNotifier(IHubContext<LobbyHub> hubContext, ILogger<HubNotifier<LobbyHub>> logger)
		: base(hubContext, logger)
	{
	}
	public Task NotifyNewRoom(Guid roomId)
	{
		return SendToAll("NewRoom", new { roomId });
	}
	public Task NotifyChangedPrivacy(Guid roomId, bool isPrivate)
	{
		return SendToAll("ChangedPrivacy", new { roomId, isPrivate });
	}
}

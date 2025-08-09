using Mafia.Lobby.Abstraction.Notifiers;
using Mefia.Shared.Infrastructure.Notifiers;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Mafia.Lobby.API.Hubs.Notifiers;

public class RoomNotifier : HubNotifier<RoomsHub>, IRoomNotifier
{

	public RoomNotifier(
		IHubContext<RoomsHub> hubContext, 
		ILogger<HubNotifier<RoomsHub>> logger) 
		: base(hubContext, logger) { }
	
	public Task NotifyNewPlayer(Guid roomId, Guid userId, Guid participantId)
	{
		return SendToGroup($"room-{roomId}", "NewPlayer", new {roomId, userId, participantId});
	}
	public Task NotifyLeavedPlayer(Guid roomId, Guid userId)
	{
		return SendToGroup($"room-{roomId}", "PlayerLeaved", new {roomId, userId});
	}
	public Task NotifyKickedPlayer(Guid roomId, Guid identityId)
	{
		return SendToGroup($"room-{roomId}", "PlayerKicked", new {roomId, identityId});
	}
	public Task NotifyNewRoom(Guid roomId)
	{
		return SendToGroup("Hall", "NewRoom", new {roomId});
	}
	public Task NotifyChangedPrivacy(Guid roomId, bool isPrivate)
	{
		return  SendToGroup("Hall", "ChangedPrivacy", new {roomId, isPrivate});
	}
	public Task NotifyGameStartTime(Guid roomId, DateTime startTime)
	{
		return SendToGroup($"room-{roomId}", "GameStartTime", new {roomId, startTime});
	}
	public Task NotifyGameStartTimeCancelled(Guid roomId)
	{
		return SendToGroup($"room-{roomId}", "GameStartTimeCancelled", new {roomId});
	}
}

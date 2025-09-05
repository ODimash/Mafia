using Mafia.Lobby.Application.Handlers.RoomHandlers.CreateRoom;
using Mafia.Shared.Contracts.DTOs.Lobby;

namespace Mafia.Lobby.API.RequestModels;

public record CreateRoomRequest(
	RoomSettingsDto Settings,
	bool IsPrivate,
	string RoomName)
{
	public CreateRoomCommand ToCommand(Guid ownerId) => new(ownerId, Settings, IsPrivate, RoomName);
}



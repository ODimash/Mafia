using Mafia.Lobby.Application.Handlers.RoomHandlers.JoinRoom;

namespace Mafia.Lobby.API.RequestModels;

public record JoinRoomRequest(string? RoomCode, Guid? RoomId, string? Password)
{
	public JoinRoomCommand ToCommand(Guid userId) => new()
	{
		RoomCode = RoomCode, 
		RoomId = RoomId, 
		Password = Password, 
		UserId = userId
	};
}

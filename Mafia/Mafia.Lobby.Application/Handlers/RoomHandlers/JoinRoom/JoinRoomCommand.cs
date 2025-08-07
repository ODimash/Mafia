using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.JoinRoom;

public class JoinRoomCommand : ICommand<Result>
{
	public string? RoomCode { get; set; }
	public Guid? RoomId { get; set; }
	public string? Password { get; set; }
	
	public Guid UserId { get; set; }
}

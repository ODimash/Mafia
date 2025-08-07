using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.LeaveRoom;

public class LeaveRoomCommand : ICommand<Result>
{
	public Guid RoomId { get; set; }
	public Guid UserId { get; set; }
}

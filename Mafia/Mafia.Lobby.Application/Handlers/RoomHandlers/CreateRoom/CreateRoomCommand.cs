using FluentResults;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Contracts.Models.DTOs.Lobby;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.CreateRoom;

public record CreateRoomCommand(
	Guid OwnerId,
	RoomSettingsDto Settings,
	bool IsPrivate,
	string RoomName) : ICommand<Result<Guid>>
{
	public  Guid OwnerId { get; set; } = OwnerId;
}

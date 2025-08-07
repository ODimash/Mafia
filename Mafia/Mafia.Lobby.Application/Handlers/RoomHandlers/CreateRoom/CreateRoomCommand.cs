using FluentResults;
using Mafia.Lobby.Contracts.DTOs;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.CreateRoom;

public record CreateRoomCommand(
	Guid OwnerId, 
	RoomSettingsDto Settings,
	bool IsPrivate,
	string RoomName) : ICommand<Result<Guid>>;

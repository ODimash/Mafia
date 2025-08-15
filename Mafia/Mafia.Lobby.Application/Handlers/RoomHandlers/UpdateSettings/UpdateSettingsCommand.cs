
using FluentResults;
using Mafia.Shared.Contracts.DTOs.Lobby;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.UpdateSettings;

public class UpdateSettingsCommand : ICommand<Result>
{
	public required Guid RoomId { get; set; }
	public required Guid UserId { get; set; }
	public required RoomSettingsDto  RoomSettings { get; set; }
}

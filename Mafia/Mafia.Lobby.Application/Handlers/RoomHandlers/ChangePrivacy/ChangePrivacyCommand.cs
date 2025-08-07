using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.ChangePrivacy;

public class ChangePrivacyCommand : ICommand<Result>
{
	public Guid RoomId { get; set; }
	public Guid UserId { get; set; }
	public bool IsPrivate { get; set; }
	public string? Password { get; set; }
}

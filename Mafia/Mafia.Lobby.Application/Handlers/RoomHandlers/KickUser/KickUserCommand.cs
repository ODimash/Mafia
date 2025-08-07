using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.KickUser;

public class KickUserCommand : ICommand<Result>
{
	public Guid RoomId { get; set; }
	public Guid UserId { get; set; }
	public Guid OwnerId { get; set; }
}

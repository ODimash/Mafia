using FluentResults;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.Handlers.RoomHandlers.StartGame;

public class StartGameCommand : ICommand<Result<Guid>>
{
	public required Guid RoomId { get; set; }
	public Guid UserId { get; set; }
	public bool IsAutoStart { get; set; } = false;
}

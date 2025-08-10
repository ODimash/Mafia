using Mafia.Games.Contracts.InegrationEvents;
using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Shared.Contracts.Messaging;
using MediatR;

namespace Mafia.Lobby.Application.EventHandlers.OnGameStarted;

public class SendNotificationHandler : IEventHandler<GameStartedEvent>
{
	private readonly IRoomNotifier _roomNotifier;

	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(GameStartedEvent notification, CancellationToken cancellationToken)
	{
		Task[] tasks = new Task[notification.PlayersWithRole.Count];
		int i = 0;
		foreach (var players in notification.PlayersWithRole)
		{
			tasks[i] = _roomNotifier.NotifyStartedGame(players.IdentityId, players.Role);
		}
		
		return Task.WhenAll(tasks);
	}
}

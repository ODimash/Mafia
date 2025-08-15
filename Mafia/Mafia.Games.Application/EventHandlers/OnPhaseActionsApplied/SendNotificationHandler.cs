using Mafia.Games.Abstraction.Notifiers;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Contracts.InegrationEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.EventHandlers.OnPhaseActionsApplied;

public class SendNotificationHandler : IEventHandler<PhaseActionsAppliedEvent>
{
	private readonly IGameNotifier _gameNotifier;

	public SendNotificationHandler(IGameNotifier gameNotifier)
	{
		_gameNotifier = gameNotifier;
	}

	public async Task Handle(PhaseActionsAppliedEvent notification, CancellationToken cancellationToken)
	{
		var diedPlayersId = notification.ActionsEvents
			.OfType<PlayerDiedDomainEvent>()
			.Select(e => e.PlayerId).ToList();

		var checkingEvents = notification.ActionsEvents
			.OfType<CheckedPlayerIsMafiaDomainEvent>()
			.ToList();

		var tasks = new Task[checkingEvents.Count];

		int i = 0;
		foreach (var checkingEvent in checkingEvents)
		{
			tasks[i] = _gameNotifier.NotifySheriffAboutCheckingResult(
				checkingEvent.CheckerId,
				checkingEvent.TargetId,
				checkingEvent.IsMafia);
		}

		await Task.WhenAll(
			_gameNotifier.NotifyDiedPlayers(notification.GameId, diedPlayersId, notification.Phase),
			Task.WhenAll(tasks));

	}
}

using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnGameWillStart;

public class SendNotificationHandler : IDomainEventHandler<GameWillStartDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<GameWillStartDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyGameStartTime(
			notification.DomainEvent.RoomId,
			notification.DomainEvent.StartTime);
	}
}

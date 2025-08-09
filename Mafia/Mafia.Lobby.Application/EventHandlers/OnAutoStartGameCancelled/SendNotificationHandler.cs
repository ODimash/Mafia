using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnAutoStartGameCancelled;

public class SendNotificationHandler : IDomainEventHandler<AutoStartGameCancelledDomainEvent>
{
	private readonly IRoomNotifier  _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<AutoStartGameCancelledDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyGameStartTimeCancelled(notification.DomainEvent.RoomId);	
	}
}

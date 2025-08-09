using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnPrivacyChanged;

public class SendNotificationHandler : IDomainEventHandler<RoomPrivacyChangedDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<RoomPrivacyChangedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyChangedPrivacy(notification.DomainEvent.RoomId, notification.DomainEvent.IsPrivate);
	}
}

using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnPlayerKicked;

public class SendNotificationHandler : IDomainEventHandler<PlayerKickedDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}
	
	public Task Handle(DomainEventNotification<PlayerKickedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyKickedPlayer(notification.DomainEvent.RoomId, notification.DomainEvent.IdentityId);
	}
}

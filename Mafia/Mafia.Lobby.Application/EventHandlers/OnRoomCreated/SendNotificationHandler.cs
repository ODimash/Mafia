using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnRoomCreated;

public class SendNotificationHandler : IDomainEventHandler<RoomCreatedDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<RoomCreatedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyNewRoom(notification.DomainEvent.RoomId);
	}
}

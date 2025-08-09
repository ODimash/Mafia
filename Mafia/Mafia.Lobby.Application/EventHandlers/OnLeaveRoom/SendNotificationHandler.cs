using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnLeaveRoom;

public class SendNotificationHandler : IDomainEventHandler<PlayerLeftRoomDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<PlayerLeftRoomDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyLeavedPlayer(notification.DomainEvent.RoomId, notification.DomainEvent.IdentityId);
	}
}

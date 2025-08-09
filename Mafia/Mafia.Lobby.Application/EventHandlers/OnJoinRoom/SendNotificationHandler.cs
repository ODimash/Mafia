using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnJoinRoom;

public class SendNotificationHandler : IDomainEventHandler<JoinedNewPlayerDomainEvent>
{
	private readonly IRoomNotifier _roomNotifier;
	
	public SendNotificationHandler(IRoomNotifier roomNotifier)
	{
		_roomNotifier = roomNotifier;
	}

	public Task Handle(DomainEventNotification<JoinedNewPlayerDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _roomNotifier.NotifyNewPlayer(
			notification.DomainEvent.RoomId,
			notification.DomainEvent.IdentityId,
			notification.DomainEvent.ParticipantId);
	}
}

using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnRoomCreated;

public class SendNotificationHandler : IDomainEventHandler<RoomCreatedDomainEvent>
{
	private readonly ILobbyNotifier _lobbyNotifier;

	public SendNotificationHandler(ILobbyNotifier lobbyNotifier)
	{
		_lobbyNotifier = lobbyNotifier;
	}

	public Task Handle(DomainEventNotification<RoomCreatedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return notification.DomainEvent.IsPrivate 
			? Task.CompletedTask 
			: _lobbyNotifier.NotifyNewRoom(notification.DomainEvent.RoomId);
	}
}

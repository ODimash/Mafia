using Mafia.Lobby.Abstraction.Notifiers;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Lobby.Application.EventHandlers.OnPrivacyChanged;

public class SendNotificationHandler : IDomainEventHandler<RoomPrivacyChangedDomainEvent>
{
	private readonly ILobbyNotifier _lobbyNotifier;
	
	public SendNotificationHandler(ILobbyNotifier lobbyNotifier)
	{
		_lobbyNotifier = lobbyNotifier;
	}

	public Task Handle(DomainEventNotification<RoomPrivacyChangedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _lobbyNotifier.NotifyChangedPrivacy(notification.DomainEvent.RoomId, notification.DomainEvent.IsPrivate);
	}
}

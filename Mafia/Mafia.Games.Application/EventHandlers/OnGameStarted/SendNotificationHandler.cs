using Mafia.Games.Abstraction.Notifiers;
using Mafia.Games.Contracts.InegrationEvents;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Contracts.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Application.EventHandlers.OnGameStarted;

public class SendNotificationHandler : IDomainEventHandler<GameStartedDomainEvent>
{
	private readonly IGameNotifier _gameNotifier;
	private readonly IMediator _mediator;
	private readonly ILogger<SendNotificationHandler> _logger;

	public SendNotificationHandler(IGameNotifier gameNotifier, IMediator mediator, ILogger<SendNotificationHandler> logger)
	{
		_gameNotifier = gameNotifier;
		_mediator = mediator;
		_logger = logger;
	}


	public Task Handle(DomainEventNotification<GameStartedDomainEvent> notification, CancellationToken cancellationToken)
	{
		_logger.LogInformation("Handling game started");
		
		return Task.WhenAll(
			_gameNotifier.NotifyGameStarted(notification.DomainEvent.GameId),
			_mediator.Publish(new GameStartedEvent(notification.DomainEvent.GameId)));
	}
}

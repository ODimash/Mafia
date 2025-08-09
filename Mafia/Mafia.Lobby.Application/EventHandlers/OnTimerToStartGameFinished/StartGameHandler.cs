using Mafia.Lobby.Abstraction.Repositories;
using Mafia.Lobby.Application.Handlers.RoomHandlers.StartGame;
using Mafia.Lobby.Domain.DomainEvents;
using Mafia.Shared.Contracts.Messaging;
using MediatR;

namespace Mafia.Lobby.Application.EventHandlers.OnTimerToStartGameFinished;

public class StartGameHandler : IDomainEventHandler<TimerToStartGameFinishedDomainEvent>
{
	private readonly IMediator _mediator;
	
	public StartGameHandler(IMediator mediator)
	{
		_mediator = mediator;
	}

	public Task Handle(DomainEventNotification<TimerToStartGameFinishedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _mediator.Send(new StartGameCommand { RoomId = notification.DomainEvent.RoomId, IsAutoStart = true });
	}
}

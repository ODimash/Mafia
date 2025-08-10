using AutoMapper;
using Mafia.Games.Contracts.DTOs;
using Mafia.Games.Contracts.InegrationEvents;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.EventHandlers.OnGameStarted;

public class PublishToEventBusHandler : IDomainEventHandler<GameStartedDomainEvent>
{
	private readonly IEventBus _eventBus;
	private readonly IMapper  _mapper;
	
	public PublishToEventBusHandler(IEventBus eventBus, IMapper mapper)
	{
		_eventBus = eventBus;
		_mapper = mapper;
	}

	public Task Handle(DomainEventNotification<GameStartedDomainEvent> notification, CancellationToken cancellationToken)
	{
		return _eventBus.Publish(new GameStartedEvent(
			notification.DomainEvent.GameId, 
			_mapper.Map<List<PlayerWithRoleDto>>(notification.DomainEvent.Players)));
	}
}

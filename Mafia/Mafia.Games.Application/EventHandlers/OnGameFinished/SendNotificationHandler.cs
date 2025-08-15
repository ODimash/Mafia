using AutoMapper;
using Mafia.Games.Abstraction.Notifiers;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Mafia.Shared.Contracts.InegrationEvents;
using Mafia.Shared.Contracts.Messaging;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Application.EventHandlers.OnGameFinished;

public class SendNotificationHandler : IDomainEventHandler<GameFinishedDomainEvent>
{
	private readonly ILogger<SendNotificationHandler> _logger;
	private readonly IGameCommandRepository _gameCommandRepository;
	private readonly IGameNotifier  _gameNotifier;
	private readonly IMapper _mapper;
	private readonly IMediator  _mediator;
	
	public SendNotificationHandler(
		ILogger<SendNotificationHandler> logger, 
		IGameNotifier gameNotifier, 
		IGameCommandRepository gameCommandRepository, 
		IMapper mapper, 
		IMediator mediator)
	{
		_logger = logger;
		_gameNotifier = gameNotifier;
		_gameCommandRepository = gameCommandRepository;
		_mapper = mapper;
		_mediator = mediator;
	}

	public async Task Handle(DomainEventNotification<GameFinishedDomainEvent> notification, CancellationToken cancellationToken)
	{
		var game = await _gameCommandRepository.GetGameById(notification.DomainEvent.GameId);
		if (game == null)
		{
			_logger.LogError($"Game with id {notification.DomainEvent.GameId} doesn't exist");
			return;
		}

		var gameResult = new GameResult
		{
			GameId = game.Id, 
			PassedDays = game.Day, 
			Players = _mapper.Map<List<PlayerWithRoleDto>>(game.Players), 
			WinnerSide = game.WinnerSide!.Value,
		};

		await _mediator.Publish(new GameFinishedEvent(gameResult));
		await _gameNotifier.NotifyGameEnded(game.Id, gameResult);
	}
}

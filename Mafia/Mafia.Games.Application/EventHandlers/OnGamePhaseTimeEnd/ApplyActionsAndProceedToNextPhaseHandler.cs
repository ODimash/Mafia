using Mafia.Games.Abstraction;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Events;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.InegrationEvents;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel.Services;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Application.EventHandlers.OnGamePhaseTimeEnd;

public class ApplyActionsAndProceedToNextPhaseHandler 
	: IDomainEventHandler<GamePhaseTimeEndDomainEvent>
{
	private readonly IGameCommandRepository  _gameCommandRepository;
	private readonly IGameActionService _gameActionService;
	private readonly IGamePhaseService _gamePhaseService;
	private readonly ILogger<ApplyActionsAndProceedToNextPhaseHandler> _logger;
	private readonly IAlarmService _alarmService;
	private readonly IMediator _mediator;
	
	public ApplyActionsAndProceedToNextPhaseHandler(
		IGameCommandRepository gameCommandRepository, 
		IGameActionService gameActionService, 
		IGamePhaseService gamePhaseService,
		ILogger<ApplyActionsAndProceedToNextPhaseHandler> logger,
		IAlarmService alarmService,
		IMediator mediator)
	{
		_gameCommandRepository = gameCommandRepository;
		_gameActionService = gameActionService;
		_gamePhaseService = gamePhaseService;
		_logger = logger;
		_alarmService = alarmService;
		_mediator = mediator;
	}

	public async Task Handle(DomainEventNotification<GamePhaseTimeEndDomainEvent> notification, CancellationToken cancellationToken)
	{
		var game = await _gameCommandRepository.GetGameById(notification.DomainEvent.GameId,  cancellationToken);
		if (game == null)
		{
			_logger.LogError("No game found with id : {GameId}", notification.DomainEvent.GameId);
			return;
		}
		
		_gameActionService.ApplyPhaseActions(game);
		var playersForActionAtNextPhase = _gameActionService.GetPlayersForActionAtNextPhase(game)
			.Select(x => x.Id)
			.ToList();
		
		var result = _gamePhaseService.TryProceedToNextPhase(game, playersForActionAtNextPhase);
		if (result.IsFailed)
		{
			_logger.LogWarning("Can not proceed to next phase at game {GameId}", game.Id);
			_alarmService.SetAlarm(game.CurrentPhase.EndTime.AddSeconds(10), new GamePhaseTimeEndDomainEvent(game.Id, game.CurrentPhase.Type));
			return;
		}
		
		await _mediator.Publish(new PhaseActionsAppliedEvent(game.Id, game.CurrentPhase.Type, game.DomainEvents.ToList()));
		
		await _gameCommandRepository.Update(game);
	}
}

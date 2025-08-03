using Mafia.Games.Abstraction;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Abstraction.Services;
using Mafia.Games.Domain.Events;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Kernel;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Application.EventHandlers.OnGameStarted;

public class SetPhaseAlarmHandler : IDomainEventHandler<GameStartedDomainEvent>
{
	private readonly IAlarmService  _alarmService;
	private readonly IGameCommandRepository _gameCommandRepository;
	private readonly ILogger<SetPhaseAlarmHandler> _logger;
	
	public SetPhaseAlarmHandler(IAlarmService alarmService, IGameCommandRepository gameCommandRepository, ILogger<SetPhaseAlarmHandler> logger)
	{
		_alarmService = alarmService;
		_gameCommandRepository = gameCommandRepository;
		_logger = logger;
	}

	public async Task Handle(DomainEventNotification<GameStartedDomainEvent> notification, CancellationToken cancellationToken)
	{
		var game = await _gameCommandRepository.GetGameById(notification.DomainEvent.GameId, cancellationToken);
		if (game == null)
		{
			_logger.LogError("Game with ID {GameId} doesn't exist", notification.DomainEvent.GameId);
			return;
		}
		
		_alarmService.SetAlarm(game.CurrentPhase.EndTime, new GamePhaseTimeEndDomainEvent(game.Id, game.CurrentPhase.Type));
	}
}

using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Events;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.Messaging;
using Microsoft.Extensions.Logging;

namespace Mafia.Games.Application.EventHandlers.OnGamePhaseChanged;

public class CheckGameIsFinishedHandler : IDomainEventHandler<GamePhaseChangedDomainEvent>
{
	private readonly IGameCommandRepository  _gameCommandRepository;
	private readonly IGameTerminationService _gameTerminationService;
	private readonly ILogger<CheckGameIsFinishedHandler> _logger;
	
	public CheckGameIsFinishedHandler(
		IGameCommandRepository gameCommandRepository, 
		IGameTerminationService gameTerminationService, 
		ILogger<CheckGameIsFinishedHandler> logger)
	{
		_gameCommandRepository = gameCommandRepository;
		_gameTerminationService = gameTerminationService;
		_logger = logger;
	}

	public async Task Handle(DomainEventNotification<GamePhaseChangedDomainEvent> notification, CancellationToken cancellationToken)
	{
		var game = await _gameCommandRepository.GetGameById(notification.DomainEvent.GameId);
		if (game == null)
		{
			_logger.LogError("Game ID {GameId} does not exist", notification.DomainEvent.GameId);	
			return;
		}
		
		var isGameFinished = _gameTerminationService.TryTerminateGame(game);
		if (!isGameFinished)
			return;
		
		await _gameCommandRepository.Update(game);
	}
}

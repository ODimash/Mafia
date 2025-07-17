
using FluentResults;
using Mafia.Game.Abstraction.Repositories;
using Mafia.Game.Contracts.Commands;
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.ValueObjects;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Game.Application.Handlers.GameHandlers;
public class StartGameHandler : ICommandHandler<StartGameCommand, Result>
{
	private readonly IGameSessionCommandRepository _gameRepository;

	public StartGameHandler(IGameSessionCommandRepository gameRepository)
	{
		_gameRepository = gameRepository;
	}

	public async Task<Result> Handle(StartGameCommand request, CancellationToken cancellationToken)
	{
		var gameSettingsResult = GameSettings.Create(
			request.GameSettings.DayDiscussionDuration,
			request.GameSettings.NightDuration,
			request.GameSettings.VoteDuration,
			request.GameSettings.Roles);

		if (gameSettingsResult.IsFailed)
			return gameSettingsResult.ToResult();

		var gameResult = GameSession.Create(gameSettingsResult.Value, []);
		if (gameResult.IsFailed)
			return gameResult.ToResult();

		await _gameRepository.AddGameAsync(gameResult.Value, cancellationToken);

		return Result.Ok();
	}
}


using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Contracts.Commands;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.GameHandlers;
public class StartGameHandler : ICommandHandler<StartGameCommand, Result>
{
	private readonly IGameCommandRepository _gameRepository;

	public StartGameHandler(IGameCommandRepository gameRepository)
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

		var gameResult = Game.Create(gameSettingsResult.Value, []);
		if (gameResult.IsFailed)
			return gameResult.ToResult();

		await _gameRepository.AddGame(gameResult.Value, cancellationToken);
		return Result.Ok();
	}
}

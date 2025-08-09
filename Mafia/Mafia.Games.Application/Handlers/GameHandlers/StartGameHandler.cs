
using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Contracts.Commands;
using Mafia.Games.Domain.Models;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.GameHandlers;
public class StartGameHandler : ICommandHandler<StartGameCommand, Result<Guid>>
{
	private readonly IGameCommandRepository _gameRepository;
	private readonly IRoleSelectorService  _roleSelectorService;

	public StartGameHandler(IGameCommandRepository gameRepository, IRoleSelectorService roleSelectorService)
	{
		_gameRepository = gameRepository;
		_roleSelectorService = roleSelectorService;
	}

	public async Task<Result<Guid>> Handle(StartGameCommand request, CancellationToken cancellationToken)
	{
		var gameSettingsResult = GameSettings.Create(
			request.GameSettings.DayDiscussionDuration,
			request.GameSettings.NightDuration,
			request.GameSettings.VoteDuration,
			request.GameSettings.Roles);

		if (gameSettingsResult.IsFailed)
			return gameSettingsResult.ToResult();
		
		var selectedRoles = _roleSelectorService.SelectRoles(request.PlayersIdentityId,  gameSettingsResult.Value);
	
		var gameResult = Game.Create(gameSettingsResult.Value, selectedRoles);
		if (gameResult.IsFailed)
			return gameResult.ToResult();

		await _gameRepository.AddGame(gameResult.Value, cancellationToken);
		return Result.Ok(gameResult.Value.Id);
	}
}

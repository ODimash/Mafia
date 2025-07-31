using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.ActionHandlers.PerformAction;

public class PerformActionHandler : ICommandHandler<PerformActionCommand, Result>
{
	private readonly IGameCommandRepository _repository;
	private readonly IGameActionService _gameActionService;

	public PerformActionHandler(IGameCommandRepository repository, IGameActionService gameActionService)
	{
		_repository = repository;
		_gameActionService = gameActionService;
	}

	public async Task<Result> Handle(PerformActionCommand request, CancellationToken cancellationToken)
	{
		var game = await _repository.GetGameById(request.GameId, cancellationToken);
		if (game == null)
			return Result.Fail("Game not found");

		var result = _gameActionService.PerformAction(game, request.ActorId, request.TargetId, request.ActionType);

		if (result.IsSuccess)
			await _repository.Update(game, cancellationToken);

		return result;
	}
}

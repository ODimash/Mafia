using AutoMapper;
using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Contracts.DTOs;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.ActionHandlers.GetActionsToDo;

public class GetActionsToDoHandler : IQueryHandler<GetActionsToDoQuery, Result<PlayerActionsToDoDto>>
{
	private readonly IGameCommandRepository _repository;
	private readonly IMapper _mapper;
	private readonly IGameActionService  _gameActionService;
	
	public GetActionsToDoHandler(
		IGameCommandRepository repository, 
		IMapper mapper, 
		IGameActionService gameActionService)
	{
		_repository = repository;
		_mapper = mapper;
		_gameActionService = gameActionService;
	}

	public async Task<Result<PlayerActionsToDoDto>> Handle(GetActionsToDoQuery request, CancellationToken cancellationToken)
	{
		var game = await _repository.GetGameById(request.GameId, cancellationToken);
		if (game == null)
			return Result.Fail("Game not found");

		var actionsToDoResult = _gameActionService.GetPlayerActionsToDo(game, request.PlayerId);
		if (actionsToDoResult.IsFailed)
		{
			// Возвращаем совершенные действия если игрок уже совершил действия
			var performedActionsResult = _gameActionService.GetPlayerPerformedActionAtThisPhase(game, request.PlayerId);
			if (performedActionsResult.IsFailed)
				return performedActionsResult.ToResult();
			return new PlayerActionsToDoDto()
			{
				IsPerformed = performedActionsResult.IsSuccess, 
				PerformedAction = _mapper.Map<PlayerActionDto>(performedActionsResult.Value), 
			};
		}
		
		// В ином случае возвращаем список действии который могут совершать игрок
		return new PlayerActionsToDoDto() { IsPerformed = false, ActionsToDo = actionsToDoResult.Value };
	}
}

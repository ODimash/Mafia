using FluentResults;
using Mafia.Games.API.Attributes;
using Mafia.Games.API.Extensions;
using Mafia.Games.API.RequestModels;
using Mafia.Games.API.Tokens.GameToken;
using MediatR;
using Mafia.Games.Application.Handlers.ActionHandlers.GetActionsToDo;
using Mafia.Games.Application.Handlers.ActionHandlers.PerformAction;
using Mafia.Shared.API.Models;
using Mafia.Shared.Contracts.Commands;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;

namespace Mafia.Games.API.Controllers;

[Route("api/[controller]")]
[OpenApiRule]
[ApiController]
[GameAuthorize]
public class GamesController : ControllerBase
{
	private IMediator _mediator;
	private IGameContextAccessor _gameContext;

	public GamesController(IMediator mediator, IGameContextAccessor gameContext)
	{
		_mediator = mediator;
		_gameContext = gameContext;
	}

	// [HttpPost("Start")]
	// public async Task<Result<Guid>> StartGame(StartGameCommand request, CancellationToken cancellationToken)
	// {
	//     var result = await _mediator.Send(request, cancellationToken);
	//     return result;
	// }
	
	[HttpPost("Actions/Perform")]
	public async Task<ResponseModel> PerformAction(PerformActionRequest request, CancellationToken cancellationToken)
	{
		var command = request.ToCommand(_gameContext.GetPlayerId(), _gameContext.GetGameId());
		var result = await _mediator.Send(command, cancellationToken);
		return result.ToResponse();
	}

	[HttpGet("Actions")]
	public async Task<ResponseModel<PlayerActionsToDoDto>> GetActionsToDo(CancellationToken cancellationToken)
	{
		var query = new GetActionsToDoQuery(_gameContext.GetGameId(), _gameContext.GetPlayerId());
		var result = await _mediator.Send(query, cancellationToken);
		return result.ToResponse();
	}
}

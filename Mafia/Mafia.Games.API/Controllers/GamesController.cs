
using FluentResults;
using MediatR;
using Mafia.Games.Application.Handlers.ActionHandlers.GetActionsToDo;
using Mafia.Games.Application.Handlers.ActionHandlers.PerformAction;
using Mafia.Shared.Contracts.Commands;
using Mafia.Shared.Contracts.DTOs;
using Mafia.Shared.Contracts.DTOs.Games;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Validations.Rules;

namespace Mafia.Games.API.Controllers;

[Route("api/[controller]")]
[OpenApiRule] 
[ApiController]
public class GamesController : ControllerBase
{
    private IMediator _mediator;
    
    public GamesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("Start")]
    public async Task<Result<Guid>> StartGame(StartGameCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }

    [HttpPost("Actions/Perform")]
    public async Task<Result> PerformAction(PerformActionCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }

    [HttpGet("Actions")]
    public async Task<Result<PlayerActionsToDoDto>> GetActionsToDo(GetActionsToDoQuery request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }
}

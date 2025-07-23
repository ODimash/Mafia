
using FluentResults;
using Mafia.Games.Contracts.Commands;
using MediatR;
using FluentResults.Extensions.AspNetCore;
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
    public async Task<Result> StartGame(StartGameCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }

    [HttpPost("Action")]
    public async Task<Result> PerformAction(PerformActionCommand request, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(request, cancellationToken);
        return result;
    }
}

using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Contracts.Commands;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Games.Application.Handlers.GameHandlers;

public class PerformActionHandler :  ICommandHandler<PerformActionCommand, Result>
{
    private readonly IGameCommandRepository _repository;
        
    public PerformActionHandler(IGameCommandRepository repository)
    {
        _repository = repository;
    }

    public async Task<Result> Handle(PerformActionCommand request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetGameById(request.GameId, cancellationToken);
        if (game == null)
            return Result.Fail("Game not found");

        var result = game.PerformAction(request.ActorId, request.TargetId, request.ActionType);
        if (result.IsSuccess)
            await _repository.Update(game, cancellationToken);

        return Result.Ok();
    }
}
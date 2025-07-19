using FluentResults;
using Mafia.Game.Abstraction.Repositories;
using Mafia.Game.Contracts.Commands;
using Mafia.Shared.Contracts.Messaging;

namespace Mafia.Game.Application.Handlers.GameHandlers
{
    public class PerformActionHandler :  ICommandHandler<PerformActionCommand, Result>
    {
        private readonly IGameSessionCommandRepository _repository;
        
        public PerformActionHandler(IGameSessionCommandRepository repository)
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
}
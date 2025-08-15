using FluentResults;
using Mafia.Games.Abstraction.Repositories;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Contracts.Messaging;
using Mafia.Shared.Contracts.Queries;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Application.Handlers.ChatHandlers;

public class GetPlayerCanTellChatHandler : IQueryHandler<GetPlayerCanTellChatQuery, Result<GameChat>>
{
    private IGameCommandRepository _repository;
    private IGameMessagingService  _gameMessageingService;
    
    public GetPlayerCanTellChatHandler(IGameCommandRepository repository, IGameMessagingService gameMessageingService)
    {
        _repository = repository;
        _gameMessageingService = gameMessageingService;
    }

    public async Task<Result<GameChat>> Handle(GetPlayerCanTellChatQuery request, CancellationToken cancellationToken)
    {
        var game = await _repository.GetGameById(request.GameId);
        if (game == null)
            return Result.Fail("Game not found");
        
        return _gameMessageingService.GetChatHeCanTell(game, request.PlayerId);
    }
}

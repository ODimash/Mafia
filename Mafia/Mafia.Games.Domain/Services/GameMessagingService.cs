using FluentResults;
using Mafia.Games.Domain.Models;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services;

public class GameMessagingService : IGameMessagingService
{
    public Result<GameChat> GetChatHeCanTell(Game game, Guid playerId)
    {
        if (game.IsFinished)
            return Result.Fail("Game is finished");

        var player = game.Players.FirstOrDefault(p => p.Id == playerId);
        if (player == null || player.IsKilled)
            return GameChat.ViewersChat;
        
        if (game.CurrentPhase.Type == PhaseType.Night && player.Role == Role.Mafia)
            return GameChat.MafiaChat;

        return GameChat.ViewersChat;
    }
}

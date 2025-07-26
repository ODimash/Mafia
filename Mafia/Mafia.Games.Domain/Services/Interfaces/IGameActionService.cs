using FluentResults;
using Mafia.Games.Domain.Models;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGameActionService
{
    Result PerformAction(Game game, Guid actorId, Guid targetId, ActionType actionType);
    void ApplyPhaseActions(Game game);
    List<Player> GetPlayersForActionAtNextPhase(Game game);
}

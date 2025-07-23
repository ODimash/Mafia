using FluentResults;
using Mafia.Games.Domain.Models;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGamePhaseService
{
    bool IsCanProcessToNextPhase(Game game);
    Result<GamePhase> TryProcessToNextPhase(Game game, List<Guid> playersIdForAction);
}

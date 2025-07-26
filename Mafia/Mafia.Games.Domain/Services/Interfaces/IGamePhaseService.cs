using FluentResults;
using Mafia.Games.Domain.Models;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGamePhaseService
{
    bool IsCanProceedToNextPhase(Game game);
    Result<GamePhase> TryProceedToNextPhase(Game game, List<Guid> playersIdForAction);
}

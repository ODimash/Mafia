using FluentResults;
using Mafia.Games.Domain.Entities;
using Mafia.Games.Domain.ValueObjects;

namespace Mafia.Games.Domain.Services.Interfaces;

public interface IGamePhaseService
{
    bool IsCanProcessToNextPhase(Game game);
    Result<GamePhase> TryProcessToNextPhase(Game game, List<Guid> playersIdForAction);
}

using FluentResults;
using Mafia.Games.Domain.Entities;
using Mafia.Games.Domain.Services.Interfaces;
using Mafia.Games.Domain.ValueObjects;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Services;

public class GamePhaseService : IGamePhaseService
{
    private IClockService _clockService;
    
    public GamePhaseService(IClockService clockService)
    {
        _clockService = clockService;
    }

    public bool IsCanProcessToNextPhase(Game game) => game.CurrentPhase.IsCanProceessToNextPhase().IsSuccess;
    public Result<GamePhase> TryProcessToNextPhase(Game game, List<Guid> playersIdForAction)
    {
        if (game.IsFinished)
            return Result.Fail("Game is already finished");

        var nextPhaseType = game.CurrentPhase.Type.GetNextPhase();
        var duration = GetPhaseDuration(nextPhaseType, game.Settings);
        var nextEndTime = _clockService.CurrentDateTime.Add(duration);

        return game.CurrentPhase.ProceedToNextPhase(playersIdForAction, nextEndTime);
    }
    
    private TimeSpan GetPhaseDuration(PhaseType phaseType, GameSettings settings) => phaseType switch
    {
        PhaseType.Night => settings.NightDuration,
        PhaseType.DayDiscussion => settings.DayDiscussionDuration,
        PhaseType.DayVoting => settings.VotingDuration,
        _ => throw new ArgumentOutOfRangeException(nameof(phaseType))
    };
    
    
}

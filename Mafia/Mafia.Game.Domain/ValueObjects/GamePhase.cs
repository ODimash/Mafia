
using FluentResults;
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;
using static System.Net.Mime.MediaTypeNames;

namespace Mafia.Game.Domain.ValueObjects;

public class GamePhase : ValueObject
{

    public PhaseType Type { get; }
    public List<Player> PlayersForAction { get; }
    public List<PlayerAction> PerfectActions { get; }
    public DateTime EndTime { get; }

    private GamePhase(PhaseType type, DateTime endTime, List<Player> playersForAction, List<PlayerAction> perfectActions)
    {
        Type = type;
        PlayersForAction = playersForAction;
        PerfectActions = perfectActions;
        EndTime = endTime;
    }

    public static Result<GamePhase> Create(PhaseType type, DateTime endTime, List<Player> playersForAction)
    {
        if (endTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");
        return new GamePhase(type, endTime, playersForAction, []);
    }

    public Result<GamePhase> ProceedToNextPhase(List<Player> playersForAction, DateTime nextEndTime)
    {
        var isPhaseTimeOver = DateTime.UtcNow > EndTime;
        var allActionCompleted = playersForAction.All(p => PerfectActions.Any(a => a.ActorId == p.Id));

        if (!isPhaseTimeOver && Type.IsDiscussion())
            return Result.Fail("The time for discussion is not over");

        if (!isPhaseTimeOver && !allActionCompleted)
            return Result.Fail("The players did not make their choice");

        if (nextEndTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");

        
        return Create(Type.GetNextPhase(), nextEndTime, playersForAction);
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return PlayersForAction;
        yield return EndTime;
    }
}

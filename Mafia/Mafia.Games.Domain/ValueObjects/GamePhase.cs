
using FluentResults;
using Mafia.Games.Domain.Entities;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;
using System.Reflection.Metadata.Ecma335;
using static System.Net.Mime.MediaTypeNames;

namespace Mafia.Games.Domain.ValueObjects;

public class GamePhase : ValueObject
{

    public PhaseType Type { get; }
    public List<Guid> PlayersForAction { get; }
    public List<PlayerAction> PerfectActions { get; }
    public DateTime EndTime { get; }

    private GamePhase(PhaseType type, DateTime endTime, List<Guid> playersForAction, List<PlayerAction> perfectActions)
    {
        Type = type;
        PlayersForAction = playersForAction;
        PerfectActions = perfectActions;
        EndTime = endTime;
    }

    public static Result<GamePhase> Create(PhaseType type, DateTime endTime, List<Guid> playersForAction)
    {
        if (endTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");
        return new GamePhase(type, endTime, playersForAction, []);
    }

    internal Result IsCanProceessToNextPhase()
    {
        var isPhaseTimeOver = DateTime.UtcNow > EndTime;
        var allActionCompleted = PlayersForAction.All(playerId => PerfectActions.Any(a => a.ActorId == playerId));

        if (!isPhaseTimeOver && Type.IsDiscussion())
            return Result.Fail("The time for discussion is not over");

        if (!isPhaseTimeOver && !allActionCompleted)
            return Result.Fail("The players did not make their choice");
        
        return Result.Ok();
    }

    internal Result<GamePhase> ProceedToNextPhase(List<Guid> playersForAction, DateTime nextEndTime)
    {
        var checkResult = IsCanProceessToNextPhase();
        if (checkResult.IsFailed)
            return checkResult;

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

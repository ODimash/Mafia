
using FluentResults;
using Mafia.Game.Domain.Entities;
using Mafia.Game.Domain.Enums;
using Mafia.Shared.Kernel;
using static System.Net.Mime.MediaTypeNames;

namespace Mafia.Game.Domain.ValueObjects;

public class GamePhase : ValueObject
{

    public PhaseType Type { get; private set; }
    public List<Player> PlayersForAction { get; private set; }
    public List<RoleAction> PerfectActions { get; private set; }
    public DateTime EndTime { get; private set; }

    private GamePhase(PhaseType type, DateTime endTime, List<Player> playersForAction, List<RoleAction> perfectActions)
    {
        Type = type;
        PlayersForAction = playersForAction;
        PerfectActions = perfectActions;
        EndTime = endTime;
    }

    public static Result<GamePhase> Create(PhaseType type, DateTime endTime, List<Player> playersForAction, List<RoleAction> perfectActions)
    {
        if (endTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");
        return new GamePhase(type, endTime, playersForAction, perfectActions);
    }

    public Result ProceedToNextPhase(List<Player> playersForAction, DateTime nextEndTime)
    {
        var isPhaseTimeOver = DateTime.UtcNow > EndTime;
        var allActionCompleted = playersForAction.All(p => PerfectActions.Any(a => a.ActorId == p.Id));

        if (!isPhaseTimeOver && Type.IsDiscussion())
            return Result.Fail("The time for discussion is not over");

        if (!isPhaseTimeOver && !allActionCompleted)
            return Result.Fail("The players did not make their choice");

        if (nextEndTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");

        Type = Type.GetNextPhase();
        PlayersForAction = playersForAction;
        PerfectActions.Clear();
        EndTime = nextEndTime;

        return Result.Ok();
    }


    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Type;
        yield return PlayersForAction;
        yield return EndTime;
    }
}

using FluentResults;
using Mafia.Shared.Kernel;
using Mafia.Shared.Kernel.Enums;

namespace Mafia.Games.Domain.Models;

public class GamePhase : ValueObject
{
    private List<PlayerAction> _perfectActions = [];

    public PhaseType Type { get; }
    public List<Guid> PlayersForAction { get; }
    public IReadOnlyList<PlayerAction> PerfectActions => _perfectActions;
    public DateTime EndTime { get; }

    private GamePhase(PhaseType type, DateTime endTime, List<Guid> playersForAction, List<PlayerAction> perfectActions)
    {
        Type = type;
        PlayersForAction = playersForAction;
        _perfectActions = perfectActions;
        EndTime = endTime;
    }

    public static Result<GamePhase> Create(PhaseType type, DateTime endTime, List<Guid> playersForAction)
    {
        if (endTime < DateTime.UtcNow)
            return Result.Fail("Phase end time can not be past time");
        return new GamePhase(type, endTime, playersForAction, []);
    }

    public void AddPerfectAction(PlayerAction action)
    {
        _perfectActions.Add(action);
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
